using System;

namespace CMM.Model
{
    // 符号表因子类
    class Symbol
    {
        public String Name { get; set; }
        public String Type { get; set; }
        public String Value { get; set; }
        public int Level { get; set; }

        public String[] ArrayValue { get; set; }
        public int Length { get; set; }

        public int LineNo;

        // 构造函数
        // 1.1 基本变量的符号表因子构造函数（不记录行号）
        public Symbol(String name, String type, String value, int level)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
            this.Level = level;

            this.ArrayValue = null;
            this.Length = -1;
        }

        // 1.2基本变量的符号表因子构造函数（记录行号）
        public Symbol(String name, String type, String value, int level, int lineNo)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
            this.Level = level;

            this.ArrayValue = null;
            this.Length = -1;

            this.LineNo = lineNo;
        }


        // 2.1 数组变量的符号表因子构造函数（不记录行号）
        public Symbol(String name, String type, int level, int length)
        {
            this.Name = name;
            this.Type = type;
            this.Value = null;
            this.Level = level;

            // 初始值均为0
            ArrayValue = new String[length];
            for (int i = 0; i < ArrayValue.Length; i++)
            {
                ArrayValue[i] = "0";
            }

            this.Length = length;
        }

        // 2.2 数组变量的符号表因子构造函数（记录行号）
        public Symbol(String name, String type, int level, int length, int lineNo)
        {
            this.Name = name;
            this.Type = type;
            this.Value = null;
            this.Level = level;

            // 初始值均为0
            ArrayValue = new String[length];
            for (int i = 0; i < ArrayValue.Length; i++)
            {
                ArrayValue[i] = "0";
            }

            this.Length = length;

            this.LineNo = lineNo;
        }
    }
}