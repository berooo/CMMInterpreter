using System;

namespace CMM.Model
{
    // 单词类
    class Token
    {

        // 保留字
        public const String IF = "If";
        public const String ELSE = "Else";
        public const String WHILE = "While";
        public const String READ = "Read";
        public const String WRITE = "Write";
        public const String INTEGER = "Integer";
        public const String REAL = "Real";
        // 分隔符
        public const String COMMA = ",";
        public const String SEMICOLON = ";";
        public const String LEFTTRANSPARENTHESES = "(";
        public const String RIGHTTRANSPARENTHESES = ")";
        public const String LEFTBRACKET = "[";
        public const String RIGHTBRACKET = "]";
        public const String LEFTBRACE = "{";
        public const String RIGHTBRACE = "}";
        // 运算符
        public const String LARGER = ">";
        public const String SMALLER = "<";
        public const String EQUAL = "==";
        public const String NOTEQUAL = "<>";
        public const String ADD = "+";
        public const String SUB = "-";
        public const String MUL = "*";
        public const String DIV = "/";


        // 运算符
        public const String ASSIGNMENT = "Assignment";
        public const String ARITHMETIC = "Arithmetic";
        public const String COMPARISON = "Comparison";

        public const String VarialbeDeclaration = "VarialbeDeclarationWord";
        public const String Identifier = "Identifier";
        // 基本属性
        public String Name { get; set; }
        public String Type { get; set; }
        public int Line { get; set; }
        public bool IsTrue { get; set; }

        // 构造方法
        public Token(String type, String name, int line, bool isTrue)
        {
            this.Name = name;
            this.Type = type;
            this.Line = line;
            this.IsTrue = isTrue;
        }


        public String GetString()
        {
            String s = "";
            if (Type.Equals(VarialbeDeclaration) || Type.Equals(READ) || Type.Equals(WRITE) || Type.Equals(WHILE) || Type.Equals(IF) || Type.Equals(ELSE))
            {
                s = "ReservedWord：" + Name;
            }
            else if (Type.Equals(Identifier))
            {
                s = "Identifier：" + Name;
            }
            else if (Type.Equals(INTEGER))
            {
                s = "Integer：" + Name;
            }
            else if (Type.Equals(REAL))
            {
                s = "Real：" + Name;
            }
            else if (Type.Equals(ASSIGNMENT) || Type.Equals(ARITHMETIC) || Type.Equals(COMPARISON))
            {
                s = "Operator：" + Name;
            }
            else
            {
                s = "Separator：" + Name;
            }
            return s;
        }

    }

}