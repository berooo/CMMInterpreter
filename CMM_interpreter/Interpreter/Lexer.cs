using System;
using System.Collections.Generic;
using System.Text;
using CMM.Model;
using CMM.Util;

namespace CMM.Interpreter
{
    class Lexer
    {
        // 词法分析结果
        public List<Token> tokenList = new List<Token>();
        // 错误列表
        public List<ExceptionMessage> errorlist = new List<ExceptionMessage>();

        // 保留字
        private static String[] reservedwords = new String[] { "if", "while", "else", "read", "write", "int", "real" };
        // 基本运算符
        private static Char[] operators = new Char[] { '+', '-', '*', '/', '=', '<', '>' };
        // 分隔符
        private static Char[] seperators = new Char[] { '(', ')', '{', '}', '[', ']', ';', ',' };
        // 比较运算符
        private static String[] compare = new String[] { "<", ">", "<>", "==" };
        // 是否是注释
        private bool isAnnotation = false;

        // 源代码
        private String source = "";
        private String result = "";

        // 构造函数
        public Lexer(String source)
        {
            this.source = source;
        }

        // 获取词法分析结果
        public List<Token> GetTokenList()
        {
            return this.tokenList;
        }

        // 开始分析
        public String LexicalAnalyse()
        {
            // 去掉源代码首尾的空格并以每行为一个单位进行分割，这样可以方便记录行号
            String[] sourceTemp = this.source.Trim().Split('\n');
            StringBuilder resultString = new StringBuilder();

            // 逐行分析
            for (int i = 0; i < sourceTemp.Length; i++)
            {
                resultString.Append(i + 1 + ": " + sourceTemp[i] + "\n");
                Analyse(sourceTemp[i], resultString, i + 1);
                resultString.Append("\n");
            }
            this.result = resultString.ToString();
            return this.result;
        }

        // 私有方法，对每一行进行分析
        private void Analyse(String sourceTemp, StringBuilder resultString, int row)
        {
            int state = 0;
            sourceTemp = sourceTemp.Trim();

            char[] charArray = sourceTemp.ToCharArray();
            for (int index = 0; charArray.Length != 0; index++)
            {
                // 分析注释
                while (isAnnotation && sourceTemp.Length != 0)
                {
                    // 多行注释结尾
                    if (sourceTemp[0] == '*' && sourceTemp[1] == '/')
                    {
                        isAnnotation = false;
                        // 过滤注释部分
                        sourceTemp = sourceTemp.Substring(2);
                        charArray = sourceTemp.ToCharArray();
                    }
                    else
                    {
                        // 逐个字符进行判断
                        sourceTemp = sourceTemp.Substring(1);
                    }

                }

                // 在过滤注释之后，判断源代码长度
                if (sourceTemp.Length == 0)
                    break;

                // 根据状态进行判断
                switch (state)
                {

                    case 0:
                        {
                            index = 0;
                            char ch = charArray[index];

                            // 过滤空格
                            while (ch == ' ')
                            {
                                sourceTemp = sourceTemp.Substring(1);
                                charArray = sourceTemp.ToCharArray();
                                ch = charArray[index];
                            }


                            if (ch == '/')
                            {
                                if (index + 1 == charArray.Length)
                                {
                                }
                                else if (charArray[index + 1] == '/')
                                {
                                    // 单行注释，直接结束
                                    charArray = "".ToCharArray();
                                    break;
                                }
                                else if (charArray[index + 1] == '*')
                                {
                                    // 多行注释
                                    this.isAnnotation = true;
                                    sourceTemp = sourceTemp.Substring(index + 2);
                                    charArray = sourceTemp.ToCharArray();
                                    state = 0;
                                    index = 0;
                                    break;
                                }
                            }

                            
                            if (Char.IsLetter(ch) || ch == '_')
                            {
                                // 判断是否是字母或者_
                                state = 1;
                            }
                            else if (Char.IsDigit(ch))
                            {
                                // 判断是否是数字
                                state = 2;
                            }
                            else if (Utils.Include(Lexer.operators, ch))
                            {
                                // 操作符判断
                                state = 3;
                            }
                            else if (Utils.Include(Lexer.seperators, ch))
                            {
                                String s = Utils.CharToString(charArray, index + 1);
                                switch (ch)
                                {
                                    case ',':
                                        tokenList.Add(new Token("Comma", ch + "", row, true));
                                        break;
                                    case ';':
                                        tokenList.Add(new Token("Semicolon", ch + "", row, true));
                                        break;
                                    case '(':
                                        tokenList.Add(new Token("LeftTransparentheses", ch + "", row, true));
                                        break;
                                    case ')':
                                        tokenList.Add(new Token("RightTransparentheses", ch + "", row, true));
                                        break;
                                    case '[':
                                        tokenList.Add(new Token("ArrayLeftDelimiter", ch + "", row, true));
                                        break;
                                    case ']':
                                        tokenList.Add(new Token("ArrayRightDelimiter", ch + "", row, true));
                                        break;
                                    case '{':
                                        tokenList.Add(new Token("LeftBrace", ch + "", row, true));
                                        break;
                                    case '}':
                                        tokenList.Add(new Token("RightBrace", ch + "", row, true));
                                        break;
                                }
                                resultString.Append(s + "：Separator\n");
                                sourceTemp = sourceTemp.Substring(index + 1);
                                charArray = sourceTemp.ToCharArray();
                                state = 0;
                                index = 0;
                            }

                            else
                            {
                                tokenList.Add(new Token("Unrecognized word symbol", ch + "", row, false));
                                resultString.Append(ch + "：Unrecognized word symbol.\n");

                                sourceTemp = sourceTemp.Substring(index + 1);
                                charArray = sourceTemp.ToCharArray();
                                state = 0;
                                index = 0;
                            }
                            break;
                        }
                    // 标识符的判断
                    case 1:
                        {
                            char ch = ' ';
                            if (index < charArray.Length)
                                ch = charArray[index];
                            // 字母、数字、下划线
                            if (Char.IsLetter(ch) || ch == '_' || Char.IsDigit(ch))
                            {
                                state = 1;
                            }
                            else
                            {
                                String str = Utils.CharToString(charArray, index);
                                if (Utils.Include(reservedwords, str))
                                {
                                    resultString.Append(str + "：Reserved words.\n");
                                    switch (str)
                                    {
                                        case "int":
                                        case "real":
                                            tokenList.Add(new Token("VarialbeDeclarationWord", str, row, true));
                                            break;
                                        case "read":
                                            tokenList.Add(new Token("Read", str, row, true));
                                            break;
                                        case "write":
                                            tokenList.Add(new Token("Write", str, row, true));
                                            break;
                                        case "if":
                                            tokenList.Add(new Token("If", str, row, true));
                                            break;
                                        case "else":
                                            tokenList.Add(new Token("Else", str, row, true));
                                            break;
                                        case "while":
                                            tokenList.Add(new Token("While", str, row, true));
                                            break;
                                    } 
                                }
                                else if (charArray[index - 1] == '_')
                                {
                                    tokenList.Add(new Token("The identifier can not end with underline", str, row, false));
                                    resultString.Append(str + "：The identifier can not end with underline\n");
                                }
                                else
                                {
                                    tokenList.Add(new Token("Identifier", str, row, true));
                                    resultString.Append(str + "：Identifier\n");
                                }
                                sourceTemp = sourceTemp.Substring(index);
                                charArray = sourceTemp.ToCharArray();
                                state = 0;
                                index = 0;
                            }
                            break;
                        }
                    // 数字的判断
                    case 2:
                        {
                            char ch = ' ';
                            if (index < charArray.Length)
                                ch = charArray[index];
                            if (Char.IsDigit(ch) || ch == '.')
                            {
                                state = 2;
                            }
                            else
                            {
                                String str = Utils.CharToString(charArray, index);
                                try
                                {
                                    double dou = Double.Parse(str);
                                    resultString.Append(str + "：Constant\n");
                                    int i = (int)dou;
                                    if (dou == i)
                                        tokenList.Add(new Token("Integer", str, row, true));
                                    else
                                        tokenList.Add(new Token("Real", str, row, true));
                                }
                                catch (Exception e)
                                {
                                    tokenList.Add(new Token("The number format is wrong", str, row, false));
                                    resultString.Append(str + "：The number format is wrong\n");
                                }
                                sourceTemp = sourceTemp.Substring(index);
                                charArray = sourceTemp.ToCharArray();
                                state = 0;
                                index = 0;
                            }
                            break;
                        }
                    // 操作符的判断
                    case 3:
                        {
                            char ch = ' ';
                            if (index < charArray.Length)
                                ch = charArray[index];
                            if (ch == '>' || ch == '=')
                            {
                                state = 3;
                            }
                            else
                            {
                                String str = Utils.CharToString(charArray, index);
                                if (str.Equals("="))
                                {
                                    resultString.Append(str + "：Assignment operator\n");
                                    tokenList.Add(new Token("Assignment", str, row, true));
                                }
                                else if (str.Length == 1 && !str.Equals("<")
                                      && !str.Equals(">"))
                                {
                                    resultString.Append(str + "：Arithmetic operator\n");
                                    tokenList.Add(new Token("Arithmetic", str, row, true));
                                }
                                else if (Utils.Include(Lexer.compare, str))
                                {
                                    resultString.Append(str + "：Comparison operator\n");
                                    tokenList.Add(new Token("Comparison", str, row, true));
                                }
                                else
                                {
                                    tokenList.Add(new Token("Invalid operator", str, row, false));
                                    resultString.Append(str + "：Invalid operator\n");
                                }
                                sourceTemp = sourceTemp.Substring(index);
                                charArray = sourceTemp.ToCharArray();
                                state = 0;
                                index = 0;
                            }
                            break;
                        }
                }
            }
        }

        // 将TokenList中的不正确的token放到ErrorList中
        public void ChangeErrorList()
        {
            foreach (Token token in tokenList)
            {
                if (token.IsTrue == false)
                {
                    errorlist.Add(new ExceptionMessage(token.Type + "：" + token.Name, token.Line));
                }
            }
        }
       
    }
}
