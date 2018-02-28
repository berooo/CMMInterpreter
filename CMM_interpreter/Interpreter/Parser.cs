using System;
using System.Collections.Generic;
using CMM.Model;

namespace CMM.Interpreter
{
    // 语法分析类
    class Parser
    {
        // 基本字段
        private List<Token> tokenList; // 词法分析结果
        private List<ExceptionMessage> errorList = new List<ExceptionMessage>(); // 错误列表
        private Token currentToken; // 当前 token
        private Token lastToken; // 上一个 token
        private int index; // 当前token的索引
        private int level = 1; // 当前层
        private SymbolTable symbolTable = new SymbolTable(); // 符号表

        // 构造方法
        public Parser(String sourcecode)
        {
            Lexer lexer = new Lexer(sourcecode);
            lexer.LexicalAnalyse();

            this.tokenList = lexer.GetTokenList();
            if (tokenList.Count > 0)
            {
                currentToken = tokenList[0];
                lastToken = tokenList[0];
                index = 0;
            }
        }

        // 程序入口
        public SyntaxTreeNode Parse()
        {
            return Program();
        }

        // 程序
        private SyntaxTreeNode Program()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("Program");
            node.ChildTreeNodeList.Add(StatementList());
            return node;
        }
        // 语句集合
        private SyntaxTreeNode StatementList()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("StatementList");
            while (true)
            {
                String typeTemp = currentToken.Name;
                if (typeTemp.Equals("{"))
                {
                    SyntaxTreeNode nodeTemp = Block();
                    if (nodeTemp != null)
                        node.ChildTreeNodeList.Add(nodeTemp);
                    else
                        break;          
                }
                else
                {
                    SyntaxTreeNode nodeTemp = Statement();
                    if (nodeTemp != null)
                        node.ChildTreeNodeList.Add(nodeTemp);
                    else
                        break;
                }
                // 分析完毕
                if (currentToken == null)
                    break;
            }
            return node;
        }
        // 单个语句
        private SyntaxTreeNode Statement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("Statement");
            String currentTokenType = currentToken.Type;
            switch(currentToken.Type)
            {
                case "VarialbeDeclarationWord": node = VariableDeclarationStatement();break;
                case "Identifier": node = AssignmentStatement();break;
                case "If": node = IfStatement();break;
                case "While": node = WhileStatement();break;
                case "Read": node = ReadStatement();break;
                case "Write": node = WriteStatement();break;
                default:
                    errorList.Add(new ExceptionMessage("Invalid statement", currentToken.Line));
                    return null;
            }
            return node;
        }
        
        // if 语句
        private SyntaxTreeNode IfStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("IfStatement");
            node.ChildTreeNodeList.Add( new SyntaxTreeNode(currentToken.Type, currentToken.Name));
            if (GetNextToken())
            {
                errorList.Add(new ExceptionMessage("\"(\" is required after if", lastToken.Line));
                return null;
            }
            if (currentToken.Name.Equals("("))
            {
                node.ChildTreeNodeList.Add( new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                    errorList.Add(new ExceptionMessage("Condition expression is required afer \"(\"", lastToken.Line));
                    return null;
                }
            }
            else
            {
                errorList.Add(new ExceptionMessage("\"(\" is required after if", currentToken.Line));
                return null;
            }

            // 条件表达式
            SyntaxTreeNode nodeTemp = ConditionExpression();
            if (nodeTemp != null)
                node.ChildTreeNodeList.Add(nodeTemp);
            else
                return null;
            if (currentToken.Name.Equals(")"))
            {
                node.ChildTreeNodeList.Add( new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                    errorList.Add(new ExceptionMessage("\"{\" is required after\")\"", lastToken.Line));
                    return null;
                }
            }
            else
            {
                errorList.Add(new ExceptionMessage("\")\" is quired after " + lastToken.Name,currentToken.Line));
                return null;
            }
            if (currentToken.Name.Equals("{"))
            {
                nodeTemp = Block();
                if (nodeTemp != null)
                    node.ChildTreeNodeList.Add(nodeTemp);
                else
                    return null;
            }
            else
            {

                errorList.Add(new ExceptionMessage("\"{\" is required.", currentToken.Line));
                return null;
            }
            if (currentToken == null)
            {

            }
            else if (currentToken.Name.Equals("else"))
            {
                node.ChildTreeNodeList.Add( new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                   
                    errorList.Add(new ExceptionMessage("\"{\" is required", lastToken.Line));
                    return null;
                }
                if (currentToken.Name.Equals("{"))
                {
                    nodeTemp = Block();
                    if (nodeTemp != null)
                        node.ChildTreeNodeList.Add(nodeTemp);
                    else
                        return null;
                }
                else
                {
                    errorList.Add(new ExceptionMessage("\"{\" is required", currentToken.Line));
                    return null;
                }
            }
            return node;
        }
        // while 语句
        private SyntaxTreeNode WhileStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("WhileStatement");
            node.ChildTreeNodeList.Add( new SyntaxTreeNode(currentToken.Type, currentToken.Name));
            if (GetNextToken())
            {
                // while后(被期待
                errorList.Add(new ExceptionMessage("\"(\" is required after while", lastToken.Line));
                return null;
            }
            if (currentToken.Name.Equals("("))
            {
                node.ChildTreeNodeList.Add( new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                    errorList.Add(new ExceptionMessage("Condition expression is required after \"(\"", lastToken.Line));
                    return null;
                }
            }
            else
            {
                errorList.Add(new ExceptionMessage("\"(\" is required after while", lastToken.Line));
                return null;
            }

            node.ChildTreeNodeList.Add(ConditionExpression());
            if (currentToken.Name.Equals(")"))
            {
                node.ChildTreeNodeList.Add( new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                    errorList.Add(new ExceptionMessage("\"{\" is required after \")\"", lastToken.Line));
                    return null;
                }
            }
            else
            {
                errorList.Add(new ExceptionMessage("\")\" is required after " + lastToken.Name, currentToken.Line));
                return null;
            }
            if (currentToken.Name.Equals("{"))
            {
                SyntaxTreeNode nodeTemp = Block();
                if (nodeTemp != null)
                {
                    node.ChildTreeNodeList.Add(nodeTemp);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                errorList.Add(new ExceptionMessage("\"{\" is required after " + lastToken.Name, currentToken.Line));
                return null;
            }

            return node;
        }
        // read 语句
        private SyntaxTreeNode ReadStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("ReadStatement");
            int length = -1;

            String type = "", exType = "";
            node.ChildTreeNodeList.Add( new SyntaxTreeNode(currentToken.Type, currentToken.Name));
            if (GetNextToken())
            {
                errorList.Add(new ExceptionMessage("\"(\" is required after read", lastToken.Line));
                return null;
            }
            if (currentToken.Name.Equals("("))
            {
                node.ChildTreeNodeList.Add( new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                    errorList.Add(new ExceptionMessage("Identifier is required after \"(\"", lastToken.Line));
                    return null;
                }
            }
            else
            {
                errorList.Add(new ExceptionMessage("\"(\" is required after read", currentToken.Line));
                return null;
            }
            if (currentToken.Type.Equals("Identifier"))
            {
                Symbol symbol = symbolTable.GetSymbol(currentToken.Name, level);
                if (symbol == null)
                {
                    errorList.Add(new ExceptionMessage(currentToken.Name + " is not declared", lastToken.Line));
                    return null;
                }
                length = symbol.Length;
                type = symbol.Type;

                node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                    errorList.Add(new ExceptionMessage("\",\" or \";\" is required after identifier", lastToken.Line));
                    return null;
                }
            }
            else
            {
                errorList.Add(new ExceptionMessage("Identifier is required after \"(\"", currentToken.Line));
                return null;
            }
            if (currentToken.Name.Equals("["))
            {
                node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (length == -1)
                {
                    errorList.Add(new ExceptionMessage("Array " + lastToken.Name + " is not declared", lastToken.Line));
                    return null;
                }
                if (type.Equals("int") || type.Equals("real"))
                {
                    errorList.Add(new ExceptionMessage("Array Variable " + lastToken.Name + " is not declared", lastToken.Line));
                    return null;
                }
                if (GetNextToken())
                {
                    errorList.Add(new ExceptionMessage( "Interger is required after " + lastToken.Name, lastToken.Line));
                    return null;
                }
                SyntaxTreeNode nodeTemp = Expression();

                if (nodeTemp != null)
                {
                    exType = nodeTemp.exType;
                    if (exType.Equals("real"))
                    {
                        errorList.Add(new ExceptionMessage("Array index must be no-negative interger", currentToken.Line));
                        return null;
                    }
                    node.ChildTreeNodeList.Add(nodeTemp);
                    if (currentToken.Name.Equals("]"))
                    {
                        node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                        if (GetNextToken())
                        {
                            errorList.Add(new ExceptionMessage("Separator or operator is required after expression",lastToken.Line));
                            return null;
                        }
                    }
                    else
                    {
                        errorList.Add(new ExceptionMessage("后\"]\" is required after " + lastToken.Name, lastToken.Line));
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else if (length != -1)
            {
                errorList.Add(new ExceptionMessage(lastToken.Name + " is not declared", lastToken.Line));
                return null;
            }
            if (currentToken.Name.Equals(")"))
            {
                node.ChildTreeNodeList.Add( new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                    errorList.Add(new ExceptionMessage("\";\" is required after \")\"", lastToken.Line));
                    return null;
                }
            }
            else
            {
                errorList.Add(new ExceptionMessage("后\")\" is required after " + lastToken.Name,currentToken.Line));
                return null;
            }
            if (currentToken.Name.Equals(";"))
            {
                node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                
                }
            }
            else
            {
                errorList.Add(new ExceptionMessage("\";\" is required after \")\"", currentToken.Line));
                return null;
            }
            return node;
        }
        // write 语句
        private SyntaxTreeNode WriteStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("WriteStatement");
            node.ChildTreeNodeList.Add( new SyntaxTreeNode(currentToken.Type, currentToken.Name));
            if (GetNextToken())
            {
                errorList.Add(new ExceptionMessage("\"(\" is required after write", lastToken.Line));
                return null;
            }
            if (currentToken.Name.Equals("("))
            {
                node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                    errorList.Add(new ExceptionMessage("\"(\" is required after expression", lastToken.Line));
                    return null;
                }
            }
            else
            {
                errorList.Add(new ExceptionMessage("\")\" is required after " + lastToken.Name,currentToken.Line));
                return null;
            }
            SyntaxTreeNode nodeTemp = Expression();
            if (nodeTemp != null)
            {
                node.ChildTreeNodeList.Add(nodeTemp);
            }
            else
            {
                return null;
            }
            if (currentToken.Name.Equals(")"))
            {
                node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                    errorList.Add(new ExceptionMessage("\";\" is requred after \")\"", lastToken.Line));
                    return null;
                }
            }
            else
            {
                errorList.Add(new ExceptionMessage("\")\" is required after " + lastToken.Name, currentToken.Line));
                return null;
            }
            if (currentToken.Name.Equals(";"))
            {
                node.ChildTreeNodeList.Add( new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                    
                }
            }
            else
            {
                errorList.Add(new ExceptionMessage("\";\" is required after \")\"", currentToken.Line));
                return null;
            }
            return node;
        }
        // 赋值语句
        private SyntaxTreeNode AssignmentStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("AssignmentStatement");
            String name = currentToken.Name, type = "real", exType = "int";
            int length = -1;
            Symbol symbol = symbolTable.GetSymbol(currentToken.Name, level);
            if (symbol == null)
            {
                errorList.Add(new ExceptionMessage( currentToken.Name + " is not declared", currentToken.Line));
                return null;
            }
            else if (symbol.Type.Equals("arr_int") || symbol.Type.Equals("int"))
            {
                type = "int";
            }
            if (symbol.Length != -1)
            {
                length = symbol.Length;
            }

            node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
            if (GetNextToken())
            {
                errorList.Add(new ExceptionMessage("\"=\" is required after " + lastToken.Name, lastToken.Line));
                return null;
            }

            // 数组赋值
            if (currentToken.Name.Equals("["))   
            {
                node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (length == -1)
                {
                    errorList.Add(new ExceptionMessage("Array " + lastToken.Name + " is not declared", lastToken.Line));
                    return null;
                }
                if (symbol.Type.Equals("int") || symbol.Type.Equals("real"))
                {
                    errorList.Add(new ExceptionMessage("Array variable " + lastToken.Name + " is not declared", lastToken.Line));
                    return null;
                }
                if (GetNextToken())
                {
                    errorList.Add(new ExceptionMessage("Positive interger is required after " + lastToken.Name, lastToken.Line));
                    return null;
                }
                SyntaxTreeNode cursorTemp = Expression();

                if (cursorTemp != null)
                {
                    exType = cursorTemp.exType;
                    if (exType.Equals("real"))
                    {
                        errorList.Add(new ExceptionMessage("Array index must be positive integer", currentToken.Line));
                        return null;
                    }
                    node.ChildTreeNodeList.Add(cursorTemp);
                    if (currentToken.Name.Equals("]"))
                    {
                        node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                        if (GetNextToken())
                        {
                            errorList.Add(new ExceptionMessage("Separator or operator is requried", lastToken.Line));
                            return null;
                        }
                    }
                    else
                    {
                        // ]被期待
                        errorList.Add(new ExceptionMessage("\"]\" is required after " + lastToken.Name, lastToken.Line));
                        return null;
                    }
                }
                else
                    return null;

            }
            else if (length != -1)
            {
                errorList.Add(new ExceptionMessage(lastToken.Name + "is not declared", lastToken.Line));
                return null;
            }

            if (currentToken.Name.Equals("="))
            {
                node.ChildTreeNodeList.Add( new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                    errorList.Add(new ExceptionMessage("Expression is required after \"=\"", lastToken.Line));
                    return null;
                }
            }
            else
            {
                errorList.Add(new ExceptionMessage("\"=\" is required after " + lastToken.Name + ".",currentToken.Line));
                return null;
            }
            SyntaxTreeNode nodeTemp = Expression();

            if (nodeTemp != null)
            {
                exType = nodeTemp.exType;
                if (exType.Equals("real") && type.Equals("int"))
                {
                    errorList.Add(new ExceptionMessage("Type is not matched", currentToken.Line));
                    return null;
                }
                node.ChildTreeNodeList.Add(nodeTemp);
            }
            else
            {
                return null;
            }
            if (currentToken.Name.Equals(";"))
            {
                node.ChildTreeNodeList.Add( new SyntaxTreeNode(currentToken.Type, currentToken.Name));
            }
            else
            {
                errorList.Add(new ExceptionMessage("\";\" is required after "  + lastToken.Name,currentToken.Line));
                return null;
            }
            if (GetNextToken())
            {
            }
            return node;
        }
        // 变量声明语句
        private SyntaxTreeNode VariableDeclarationStatement()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("VariableDeclarationStatement");

            SyntaxTreeNode nodeTemp = VariableDeclaration();
            if (nodeTemp != null)
            {
                node.ChildTreeNodeList.Add(nodeTemp);
            }
            else
            {
                return null;
            }
            return node;
        }
        private SyntaxTreeNode VariableDeclaration()
        {
            String type = "", name = "", exType = "real";
            SyntaxTreeNode node = new SyntaxTreeNode("VariableDeclaration");
            node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
            if (GetNextToken())
            {
                errorList.Add(new ExceptionMessage("Identifier is required after " + lastToken.Name, lastToken.Line));
                return null;
            }
            if (!currentToken.Type.Equals("Identifier"))
            {
                // 变量声明中：保留字后面跟的不是标识符，错误
                errorList.Add(new ExceptionMessage("Identifier is required after " + lastToken.Name, currentToken.Line));
                return null;
            }

            while (true)
            {
                type = this.lastToken.Name;
                name = this.currentToken.Name;
                exType = type;

                String typeTemp = currentToken.Type;
                if (typeTemp.Equals("Identifier"))
                {
                    node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                    if (GetNextToken())
                    {
                        errorList.Add(new ExceptionMessage("\";\" is requried after " + lastToken.Name, lastToken.Line));
                        return null;
                    }
                    Symbol flag = symbolTable.GetSymbol(lastToken.Name, level);
                   
                    if (flag != null && flag.Level == level)
                    {
                        errorList.Add(new ExceptionMessage( lastToken.Name + " is declared repeatedly", lastToken.Line));
                        return null;
                    }
                    if (currentToken.Name.Equals("["))
                    {
                        node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                        if (GetNextToken())
                        {
                            errorList.Add(new ExceptionMessage("Positive interger is required after " + lastToken.Name, lastToken.Line));
                            return null;
                        }
                        SyntaxTreeNode nodeTemp = Expression();

                        if (nodeTemp != null)
                        {
                            exType = nodeTemp.exType;
                            if (exType.Equals("real"))
                            {
                                errorList.Add(new ExceptionMessage("Array index must be positive interger", currentToken.Line));
                                return null;
                            }
                            node.ChildTreeNodeList.Add(nodeTemp);
                            if (currentToken.Name.Equals("]"))
                            {
                                node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                                if (GetNextToken())
                                {
                                    errorList.Add(new ExceptionMessage("Separetor or operator is requried", lastToken.Line));
                                    return null;
                                }
                            }
                            else
                            {
                                errorList.Add(new ExceptionMessage("后\"]\" is requried after " + lastToken.Name, lastToken.Line));
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                        symbolTable.Add(new Symbol(name, "arr_" + type, level, 0, lastToken.Line));
                    }
                    else
                    {
                        symbolTable.Add(new Symbol(name, type, "0", level, lastToken.Line));
                    }
                }
                String valueTemp = currentToken.Name;

                if (valueTemp.Equals(";"))
                {
                    node.ChildTreeNodeList.Add(
                            new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                    if (GetNextToken())
                    {
                    }
                    break;
                }
                else if (valueTemp.Equals("="))
                {
                    if (lastToken.Name.Equals("]"))
                    {
                        // 数组必须先声明后定义
                        errorList.Add(new ExceptionMessage("\";\" is required after " + lastToken.Name, lastToken.Line));
                        return null;
                    }
                    node.ChildTreeNodeList.Add(
                            new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                    if (GetNextToken())
                    {
                        // "="后赋值语句被期待
                        errorList.Add(new ExceptionMessage("Assignment is required after " + lastToken.Name, lastToken.Line));
                        return null;
                    }
                    SyntaxTreeNode nodeTemp = Expression();
                    if (nodeTemp != null)
                    {
                        exType = nodeTemp.exType;
                        if (exType.Equals("real") && type.Equals("int"))
                        {
                            errorList.Add(new ExceptionMessage("Type does not match", currentToken.Line));
                            return null;
                        }
                        node.ChildTreeNodeList.Add(nodeTemp);
                    }
                    else
                    {
                        return null;
                    }
                    if (currentToken.Name.Equals(","))
                    {
                        node.ChildTreeNodeList.Add(
                                new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                        if (GetNextToken())
                        {
                            errorList.Add(new ExceptionMessage("Indentifier is required after \",\"", lastToken
                                    .Line));
                            return null;
                        }
                    }
                    else if (currentToken.Name.Equals(";"))
                    {
                        node.ChildTreeNodeList.Add(
                                new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                        if (GetNextToken())
                        {
                            
                        }
                        break;
                    }
                    else
                    {
                        errorList.Add(new ExceptionMessage("\";\" or \",\" is reuqired after expression", currentToken.Line));
                        return null;
                    }
                }
                else if (valueTemp.Equals(","))
                {
                    node.ChildTreeNodeList.Add(
                            new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                    if (GetNextToken())
                    {
                        errorList.Add(new ExceptionMessage("Indetifier is required after \",\"", lastToken.Line));
                        return null;
                    }
                }
                else
                {
                    errorList.Add(new ExceptionMessage("\";\" or \",\" is required after " +  lastToken.Name, currentToken
                            .Line));
                    return null;
                }
            }
            return node;
        }

        // 语句块
        public SyntaxTreeNode Block()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("Block");
            node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
            level++;
            if (GetNextToken())
            {
                errorList.Add(new ExceptionMessage("\"}\" or statemtent is required", lastToken.Line));
                return null;
            }
            while (true)
            {
                if (currentToken.Name.Equals("{"))
                {
                    SyntaxTreeNode nodeTemp = Block();
                    if (nodeTemp != null)
                    {
                        node.ChildTreeNodeList.Add(nodeTemp);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (!currentToken.Name.Equals("}"))
                {
                    SyntaxTreeNode nodeTemp = Statement();
                    if (nodeTemp != null)
                    {
                        node.ChildTreeNodeList.Add(nodeTemp);
                    }
                    else
                    {
                        return null;
                    }
                }
                if (currentToken == null)
                {
                    errorList.Add(new ExceptionMessage("\"}\" is required", lastToken.Line));
                    return null;
                }
                else if (currentToken.Name.Equals("}"))
                {

                    level--;
                    node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                    GetNextToken();
                    break;
                }

            }
            return node;
        }

        // 条件表达式
        public SyntaxTreeNode ConditionExpression()
        {
            SyntaxTreeNode node = new SyntaxTreeNode("Condition");
            SyntaxTreeNode nodeTemp = Expression();
            if (nodeTemp != null)
            {
                node.ChildTreeNodeList.Add(nodeTemp);
            }
            else
            {
                return null;
            }
            if (currentToken.Type.Equals("Comparison"))
            {
                node.ChildTreeNodeList.Add(
                        new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                    errorList.Add(new ExceptionMessage("Expression is required after comparison", lastToken.Line));
                    return null;
                }
            }
            else
            {
                errorList.Add(new ExceptionMessage("Comparison is required after expression", currentToken.Line));
                return null;
            }
            nodeTemp = Expression();
            if (nodeTemp != null)
            {
                node.ChildTreeNodeList.Add(nodeTemp);
            }
            else
            {
                return null;
            }

            return node;
        }

        // 表达式
        public SyntaxTreeNode Expression()
        {
            String exType = "int";
            SyntaxTreeNode node = new SyntaxTreeNode("Expression");
            if (currentToken.Name.Equals("-"))
            {
                currentToken = new Token("Integer", "0", currentToken.Line, true);
                index--;
            }
            SyntaxTreeNode nodeTemp = Term();
            if (nodeTemp != null)
            {
                node.ChildTreeNodeList.Add(nodeTemp);
                if (nodeTemp.exType.Equals("real"))
                {
                    exType = "real";
                }
            }
            else
            {
                return null;
            }
            while (true)
            {
                if (currentToken.Name.Equals("+")
                        || currentToken.Name.Equals("-"))
                {
                    node.ChildTreeNodeList.Add(
                            new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                    if (GetNextToken())
                    {

                    }
                }
                else
                {
                    break;
                }
                nodeTemp = Term();
                if (nodeTemp != null)
                {
                    node.ChildTreeNodeList.Add(nodeTemp);
                    if (nodeTemp.exType.Equals("real"))
                    {
                        exType = "real";
                    }
                }
                else
                {
                    return null;
                }
            }
            node.exType = exType;
            return node;
        }

        // 项
        private SyntaxTreeNode Term()
        {
            String exType = "int";
            SyntaxTreeNode node = new SyntaxTreeNode("Term");
            SyntaxTreeNode nodeTemp = Factor();
            if (nodeTemp != null)
            {
                node.ChildTreeNodeList.Add(nodeTemp);
                if (nodeTemp.exType.Equals("real"))
                {
                    exType = "real";
                }
            }
            else
            {
                return null;
            }
            while (true)
            {
                if (currentToken.Name.Equals("*")
                        || currentToken.Name.Equals("/"))
                {
                    node.ChildTreeNodeList.Add(
                            new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                    if (GetNextToken())
                    {

                    }
                }
                else
                {
                    break;
                }
                nodeTemp = Factor();
                if (nodeTemp != null)
                {
                    node.ChildTreeNodeList.Add(nodeTemp);
                    if (nodeTemp.exType.Equals("real"))
                    {
                        exType = "real";
                    }
                }
                else
                {
                    return null;
                }
            }
            node.exType = exType;
            return node;
        }

        // 因子
        private SyntaxTreeNode Factor()
        {
            String exType = "int", type = "";
            int length = -1;
            SyntaxTreeNode node = new SyntaxTreeNode("Factor");
            if (currentToken.Type.Equals("Identifier")
               || currentToken.Type.Equals("Real")
               || currentToken.Type.Equals("Integer"))
            {
                if (currentToken.Type.Equals("Identifier"))
                {
                    Symbol flag = symbolTable.GetSymbol(currentToken.Name, level);
                    if (flag == null)
                    { 
                        errorList.Add(new ExceptionMessage(currentToken.Name + " is not declared", lastToken.Line));
                        return null;
                    }
                    else
                    {
                        type = flag.Type;
                        length = flag.Length;
                        if (flag.Type.Equals("real") || flag.Type.Equals("arr_real"))
                        {
                            exType = "real";
                        }
                    }
                }
                else if (currentToken.Type.Equals("Real"))
                {
                    exType = "real";
                }
                node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                {
                    errorList.Add(new ExceptionMessage("Separetor or operator is required after " + lastToken.Name, lastToken.Line));
                    return null;
                }
                if (currentToken.Name.Equals("["))
                {
                    node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                    if (length == -1)
                     {
                        errorList.Add(new ExceptionMessage("Array " + lastToken.Name + " is not declared", lastToken.Line));
                        return null;
                    }
                    if (type.Equals("int") || type.Equals("real"))
                    {
                        errorList.Add(new ExceptionMessage("Array Variable " + lastToken.Name + " is not decalared", lastToken.Line));
                        return null;
                    }
                    if (GetNextToken())
                    {
                        errorList.Add(new ExceptionMessage("Positive interger is required after " + lastToken.Name + ".", lastToken.Line));
                        return null;
                    }
                    SyntaxTreeNode nodeTemp = Expression();
                    
                    if (nodeTemp != null)
                    {
                        exType = nodeTemp.exType;
                        if (exType.Equals("real"))
                        {
                            errorList.Add(new ExceptionMessage("Array index must be positive interger", currentToken.Line));
                            return null;
                        }
                        node.ChildTreeNodeList.Add(nodeTemp);
                        if (currentToken.Name.Equals("]"))
                        {
                            node.ChildTreeNodeList.Add(new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                            if (GetNextToken())
                            {
                                errorList.Add(new ExceptionMessage("Separetor or operator is required after expression", lastToken.Line));
                                return null;
                            }
                        }
                        else
                        {
                            errorList.Add(new ExceptionMessage("\"]\" is required after " + lastToken.Name, lastToken.Line));
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (length != -1)
                {
                    errorList.Add(new ExceptionMessage(lastToken.Name + " is not declared", lastToken.Line));
                    return null;
                }

            }
            else if (currentToken.Name.Equals("("))
            {
                node.ChildTreeNodeList.Add(
                        new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                if (GetNextToken())
                { 
                    errorList.Add(new ExceptionMessage("Expression is required after " + lastToken.Name,lastToken.Line));
                    return null;
                }
                SyntaxTreeNode nodeTemp = Expression();
                
                if (nodeTemp != null)
                {
                    exType = nodeTemp.exType;
                    node.ChildTreeNodeList.Add(nodeTemp);
                }
                else
                {
                    return null;
                }
                if (currentToken.Name.Equals(")"))
                {
                    node.ChildTreeNodeList.Add(
                            new SyntaxTreeNode(currentToken.Type, currentToken.Name));
                    if (GetNextToken())
                    {
                    }
                }
                else
                {
                    errorList.Add(new ExceptionMessage("\")\" is required after " + lastToken.Name,currentToken.Line));
                    return null;

                }
            }
            else
            { 
                errorList.Add(new ExceptionMessage("Expression is required after " + lastToken.Name, currentToken.Line));
                return null;
            }
            node.exType = exType;
            return node;
        }

        // 获取下一个Token
        private bool GetNextToken()
        {
            if (index != tokenList.Count - 1)
            {
                lastToken = currentToken;
                index++;
                currentToken = tokenList[index];
                return false;
            }
            else
            {
                lastToken = currentToken;
                currentToken = null;
                index = tokenList.Count - 1;
                return true;
            }
        }

        public List<ExceptionMessage> GetErrorList()
        {
            return this.errorList;
        }

    }
}
