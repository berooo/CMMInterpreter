using System;
using System.Collections.Generic;

using CMM.Model;

namespace CMM.Interpreter
{
    class CodeGenerator
    {

        public String sourcecode;
        private List<FourCode> fourCodeList = new List<FourCode>(); // 四元式集合 
        private SyntaxTreeNode root; // 根节点

        private int level = 1;
        private bool isArray = false;

        Stack<string> argumentStack = new Stack<string>(); // 操作数栈
        Stack<string> operationStack = new Stack<string>();// 操作符栈

        public CodeGenerator(String sourcecode)
        {
            this.sourcecode = sourcecode;
            Parser parser = new Parser(sourcecode);
            root = parser.Parse();
        }

        public List<FourCode> GetFourCodeList()
        {
            GenerateIntermediateCode(root);
            fourCodeList.Add(new FourCode(FourCode.FINISH, FourCode.EMPTY, FourCode.EMPTY, FourCode.EMPTY, 1));
            return fourCodeList;
        }
        
        // 生成中间代码，也就是获取四元式集合
        private void GenerateIntermediateCode(SyntaxTreeNode node)
        {
            string t1 = "";
            string t2 = "";
            String op = "";
            switch (node.Type)
            {
                //case "Statement":
                case "ReadStatement":
                    for (int i = 0; i < node.ChildTreeNodeList.Count; i++)
                    {
                        GenerateIntermediateCode(node.ChildTreeNodeList[i]);
                    }
                    t1 = argumentStack.Pop();
                    fourCodeList.Add(new FourCode(FourCode.READ, FourCode.EMPTY, FourCode.EMPTY, t1, level));
                    break;
                case "WriteStatement":
                    for (int i = 0; i < node.ChildTreeNodeList.Count; i++)
                    {
                        GenerateIntermediateCode(node.ChildTreeNodeList[i]);
                    }
                    t1 = argumentStack.Pop();
                    fourCodeList.Add(new FourCode(FourCode.WRITE, FourCode.EMPTY, FourCode.EMPTY, t1, level));
                    break;
                case "WhileStatement":
                    for (int i = 0; i < node.ChildTreeNodeList.Count && node.ChildTreeNodeList[i].Name != ")"; i++)
                    {
                        GenerateIntermediateCode(node.ChildTreeNodeList[i]);
                    }
                    t1 = argumentStack.Pop();
                    fourCodeList.Add(new FourCode(FourCode.IFFALSEJUMP, FourCode.EMPTY, FourCode.EMPTY, "result", level));
                    int whileIF_F = fourCodeList.Count - 1;
                    GenerateIntermediateCode(node.ChildTreeNodeList[4]);
                    fourCodeList.Add(new FourCode(FourCode.JUMP, FourCode.EMPTY, FourCode.EMPTY, (whileIF_F-1) + "", level));
                    int whileGoto = fourCodeList.Count;
                    fourCodeList[whileIF_F].FirstArgument = whileGoto + "";
                    //跳出while语句
                    GenerateIntermediateCode(node.ChildTreeNodeList[node.ChildTreeNodeList.Count - 1].ChildTreeNodeList[node.ChildTreeNodeList[node.ChildTreeNodeList.Count - 1].ChildTreeNodeList.Count - 1]);
                    break;
                case "IfStatement":
                    for (int i = 0; i < node.ChildTreeNodeList.Count && node.ChildTreeNodeList[i].Name != ")"; i++)
                    {
                        GenerateIntermediateCode(node.ChildTreeNodeList[i]);
                    }
                    t1 = argumentStack.Pop();
                    fourCodeList.Add(new FourCode(FourCode.IFFALSEJUMP, FourCode.EMPTY, FourCode.EMPTY, "result", level));
                    int ifJump = fourCodeList.Count - 1;// 记录jump指令的位置
                    GenerateIntermediateCode(node.ChildTreeNodeList[4]);
                    fourCodeList.Add(new FourCode(FourCode.JUMP, FourCode.EMPTY, FourCode.EMPTY, FourCode.EMPTY, level));
                    int ifGoto = fourCodeList.Count;
                    fourCodeList[ifJump].FirstArgument = ifGoto + "";
                    ifJump = ifGoto - 1;
                    if (node.ChildTreeNodeList.Count > 5)
                    {
                        GenerateIntermediateCode(node.ChildTreeNodeList[6]);
                        fourCodeList[ifJump].Result = fourCodeList.Count + "";
                    }
                    else
                    {
                        fourCodeList[ifJump].Result = fourCodeList.Count + "";
                        GenerateIntermediateCode(node.ChildTreeNodeList[4].ChildTreeNodeList[node.ChildTreeNodeList[4].ChildTreeNodeList.Count - 1]);
                    }
                    break;
                case "AssignmentStatement":
                    for (int i = 0; i < node.ChildTreeNodeList.Count; i++)
                    {
                        GenerateIntermediateCode(node.ChildTreeNodeList[i]);
                    }
                    t1 = argumentStack.Pop();
                    t2 = argumentStack.Pop();
                    op = operationStack.Pop();
                    fourCodeList.Add(new FourCode(FourCode.ASIGN, t1, t2, FourCode.EMPTY, level));
                    break;
                case "VariableDeclarationStatement":
                    switch (GetDeclarationType(node))
                    {
                        //==========Integer声明语句==========
                        case "DeclareInt":
                            for (int i = 0; i < node.ChildTreeNodeList.Count; i++)
                            {
                                GenerateIntermediateCode(node.ChildTreeNodeList[i]);
                            }

                            int opNum = operationStack.Count;
                            for (int i = 0; i <= opNum; i++)
                            {
                                //获取op、t1、t2
                                if (operationStack.Count > 0)
                                    op = operationStack.Pop();
                                else
                                    op =  FourCode.EMPTY;

                                if (argumentStack.Count > 0)
                                    t1 = argumentStack.Pop();
                                else
                                    t1 =  FourCode.EMPTY;

                                if (argumentStack.Count > 0 && t1 != "separator")
                                    t2 = argumentStack.Pop();
                                else
                                    t2 =  FourCode.EMPTY;

                                //判断后添加
                                if (t2 == "separator" && op == "separator" )
                                {
                                    fourCodeList.Add(new FourCode(FourCode.DECLARATIONINT, "0", t1, FourCode.EMPTY, level));
                                }
                                else if (op == "=")
                                {
                                    //声明并赋值
                                    fourCodeList.Add(new FourCode(FourCode.DECLARATIONINT, t1, t2, FourCode.EMPTY, level));
                                }
                                else if (op ==  FourCode.EMPTY && t1 !=  FourCode.EMPTY && t2 ==  FourCode.EMPTY)
                                {
                                    fourCodeList.Add(new FourCode(FourCode.DECLARATIONINT, "0", t1, FourCode.EMPTY, level));
                                }
                            }
                            break;

                        //==========Real声明语句==========
                        case "DeclareReal":
                            for (int i = 0; i < node.ChildTreeNodeList.Count; i++)
                            {
                                GenerateIntermediateCode(node.ChildTreeNodeList[i]);
                            }

                            opNum = operationStack.Count;
                            for (int i = 0; i <= opNum; i++)
                            {
                                //获取op、t1、t2
                                if (operationStack.Count > 0)
                                    op = operationStack.Pop();
                                else
                                    op =  FourCode.EMPTY;

                                if (argumentStack.Count > 0)
                                    t1 = argumentStack.Pop();
                                else
                                    t1 =  FourCode.EMPTY;

                                if (argumentStack.Count > 0 && t1 != "separator")
                                    t2 = argumentStack.Pop();
                                else
                                    t2 =  FourCode.EMPTY;

                                //添加
                                if (t2 == "separator" && op == "separator" && t2 !=  FourCode.EMPTY)
                                {
                                    fourCodeList.Add(new FourCode(FourCode.DECLATATIONREAL, "0", t1, FourCode.EMPTY, level));
                                }
                                else if (op == "=")
                                {
                                    // 声明并赋值
                                    fourCodeList.Add(new FourCode(FourCode.DECLATATIONREAL, t1, t2, FourCode.EMPTY, level));
                                }
                                else if (op ==  FourCode.EMPTY && t1 !=  FourCode.EMPTY && t2 ==  FourCode.EMPTY)
                                {
                                    fourCodeList.Add(new FourCode(FourCode.DECLATATIONREAL, "0", t1, FourCode.EMPTY, level));
                                }
                            }
                            break;
                    }
                    break;

                //==========块==========
                case "Block":
                    level++;
                    for (int i = 0; i < node.ChildTreeNodeList.Count; i++)
                    {
                        GenerateIntermediateCode(node.ChildTreeNodeList[i]);
                    }
                    level--;
                    break;

                //==========条件表达式==========
                case "Condition":
                    for (int i = 0; i < node.ChildTreeNodeList.Count; i++)
                    {
                        GenerateIntermediateCode(node.ChildTreeNodeList[i]);
                    }
                    t1 = argumentStack.Pop();
                    t2 = argumentStack.Pop();
                    op = operationStack.Pop();
                    try
                    {
                        if (op == "==")
                        {
                            fourCodeList.Add(new FourCode(FourCode.EQUAL, t1, t2, "result", level));
                            argumentStack.Push("result");
                        }
                        else if (op == "<>")
                        {
                            fourCodeList.Add(new FourCode(FourCode.NOTEQUAL, t1, t2, "result", level));
                            argumentStack.Push("result");
                        }
                        else if (op == ">")
                        {
                            fourCodeList.Add(new FourCode(FourCode.LARGER, t1, t2, "result", level));
                            argumentStack.Push("result");
                        }
                        else if (op == "<")
                        {
                            fourCodeList.Add(new FourCode(FourCode.SMALLER, t1, t2, "result", level));
                            argumentStack.Push("result");
                        }
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Conversion error.");
                    }

                    break;

                //==========算术表达式==========
                case "Expression":
                    for (int i = 0; i < node.ChildTreeNodeList.Count; i++)
                    {
                        GenerateIntermediateCode(node.ChildTreeNodeList[i]);
                    }
                    if (node.ChildTreeNodeList.Count == 1)
                    {
                        //
                    }
                    else
                    {
                        for (int i = 0; i < ((node.ChildTreeNodeList.Count - 1) / 2); i++)
                        {
                            t1 = argumentStack.Pop();
                            t2 = argumentStack.Pop();
                            op = operationStack.Pop();
                            if (op == "+")
                            {
                                fourCodeList.Add(new FourCode(FourCode.ADD, t1, t2, "result", level));
                                argumentStack.Push("result");
                            }
                            else if (op == "-")
                            {
                                fourCodeList.Add(new FourCode(FourCode.SUB, t1, t2, "result", level));
                                argumentStack.Push("result");
                            }
                        }

                    }
                    if (isArray)
                    {
                        t1 = argumentStack.Pop();
                        t2 = argumentStack.Pop();
                        t2 = t2 + "[" + t1 + "]";
                        argumentStack.Push(t2);
                        isArray = false;
                    }
                    break;

                //==========项==========
                case "Term":
                    for (int i = 0; i < node.ChildTreeNodeList.Count; i++)
                    {
                        GenerateIntermediateCode(node.ChildTreeNodeList[i]);
                    }
                    if (node.ChildTreeNodeList.Count == 1)
                    {
                        //
                    }
                    else
                    {
                        for (int i = 0; i < ((node.ChildTreeNodeList.Count - 1) / 2); i++)
                        {
                            t1 = argumentStack.Pop();
                            t2 = argumentStack.Pop();
                            op = operationStack.Pop();
                            if (op == "*")
                            {
                                fourCodeList.Add(new FourCode(FourCode.MUL, t1, t2, "result", level));
                                argumentStack.Push("result");
                            }
                            else if (op == "/")
                            {
                                fourCodeList.Add(new FourCode(FourCode.DIV, t1, t2, "result", level));
                                argumentStack.Push("result");
                            }
                        }
                    }
                    break;

                //==========因子==========
                case "Factor":
                    for (int i = 0; i < node.ChildTreeNodeList.Count; i++)
                    {
                        GenerateIntermediateCode(node.ChildTreeNodeList[i]);
                    }
                    break;

                //==========算术符==========
                case "Arithmetic":
                    operationStack.Push(node.Name);
                    break;

                //==========关系运算符==========
                case "Comparison":
                    operationStack.Push(node.Name);
                    break;

                //==========标识符==========
                case "Identifier":
                    argumentStack.Push(node.Name);              
                    break;

                //==========int==========
                case "Integer":
                    argumentStack.Push(node.Name); 
                    break;

                //==========real==========
                case "Real":
                    argumentStack.Push(node.Name);
                    break;


                //==========分隔符==========
                case "Comma":
                    argumentStack.Push("separator");
                    operationStack.Push("separator");
                    break;

                //==========等号==========
                case "Assignment":
                    operationStack.Push("=");
                    break;

                //==========数组==========
                case "ArrayLeftDelimiter":
                    this.isArray = true;
                    break;

                //==========其他标记忽略==========
                default:
                    for (int i = 0; i < node.ChildTreeNodeList.Count; i++)
                    {
                        GenerateIntermediateCode(node.ChildTreeNodeList[i]);
                    }
                    break;
            }
        }
       
        //获得变量声明语句类型，也就是声明int还是声明real
        private String GetDeclarationType(SyntaxTreeNode node)
        {
            List<SyntaxTreeNode> list = node.ChildTreeNodeList;
            if (list[0].ChildTreeNodeList[0].Name == "int")
            {
                return "DeclareInt";
            }
            else if (list[0].ChildTreeNodeList[0].Name == "real")
            {
                return "DeclareReal";
            }
            else
                return "";
        }
    }
}
