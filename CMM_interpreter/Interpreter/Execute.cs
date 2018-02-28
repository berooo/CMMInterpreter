using System;
using System.Collections.Generic;
using System.Linq;
using CMM.Model;

using System.Windows.Forms;
using CMM_interpreter.UI;

namespace CMM.Interpreter
{   
    class Execute 
    {
        
       
        private SymbolTable symbolTable = new SymbolTable(); // 符号表
        public List<ExceptionMessage> errList = new List<ExceptionMessage>(); // 错误列表
        private List<FourCode> fourCodeList; // 四元式集合

        public String readVar = "";
        private String result = "";
        int level = 0;

        public RichTextBox resultBox;

        public MainFrame mf;
    


        public Execute(List<FourCode> quaterList, MainFrame m, RichTextBox rtb)
        {
            this.fourCodeList = quaterList;
            mf = m;
            resultBox = rtb;
        }

        public List<ExceptionMessage> getErrList(){
            return this.errList;
        }

        // 执行入口
        public int ExecuteCode(int cursor)
        {
            for (int i = cursor; i < fourCodeList.Count; i++)
            {
                if (errList.Count != 0)
                {
                    return -2;
                }
                level = fourCodeList[i].Level;
                FourCode quater = fourCodeList[i];
                switch (quater.Operation)
                {
                    case FourCode.DECLARATIONINT: 
                        this.DeclarationInt(quater.SecondArgument, quater.FirstArgument);
                        break;
                    case FourCode.DECLATATIONREAL:
                        this.DeclarationReal(quater.SecondArgument, quater.FirstArgument);
                        break;
                    case FourCode.ASIGN:
                        this.Asign(quater.SecondArgument, quater.FirstArgument);
                        break;
                    case FourCode.READ:
                        mf.setRead(true);
                        this.PrintRes("Please enter a number：");
                        readVar = quater.Result;
                        return i+1;
                    case FourCode.WRITE:
                        this.Write(quater.Result);
                        break;
                    case FourCode.ADD:
                        this.Cal(quater.FirstArgument, quater.SecondArgument, FourCode.ADD);
                        break;
                    case FourCode.SUB:
                        this.Cal(quater.FirstArgument, quater.SecondArgument, FourCode.SUB);
                        break;
                    case FourCode.MUL:
                        this.Cal(quater.FirstArgument, quater.SecondArgument, FourCode.MUL);
                        break;
                    case FourCode.DIV:
                        this.Cal(quater.FirstArgument, quater.SecondArgument,FourCode.DIV);
                        break;
                    case FourCode.EQUAL:
                        this.Cal(quater.FirstArgument, quater.SecondArgument, FourCode.EQUAL);
                        break;
                    case FourCode.NOTEQUAL:
                        this.Cal(quater.FirstArgument, quater.SecondArgument, FourCode.NOTEQUAL);
                        break;
                    case FourCode.LARGER:
                        this.Cal(quater.FirstArgument, quater.SecondArgument, FourCode.LARGER);
                        break;
                    case FourCode.SMALLER:
                        this.Cal(quater.FirstArgument, quater.SecondArgument, FourCode.SMALLER);
                        break;
                    case FourCode.IFFALSEJUMP:
                        int address = this.IfFalseJump(quater.FirstArgument);
                        if (address != -1)
                        {
                            i = address;
                        }
                        break;
                    case FourCode.JUMP:
                        i = this.Jump(quater.Result);
                        break;
     
                    default:
                        break;
                }
            }
            if (errList.Count != 0)
            {
                return -2;
            }
            return -1;
        }

        // int类型变量声明
        private void DeclarationInt(String name, String value)
        {
            int legth = -1;
            legth = IsArray(name);

            // 数组操作说明
            if (!name.Equals(GetVariableName(name)))
            {
                if (legth <= 0)
                {
                    errList.Add(new ExceptionMessage("Array index are out of bounds", 2));
                    return;
                }
                else
                {
                    name = GetVariableName(name);
                    symbolTable.Add(new Symbol(name, "int", level, legth));
                }

            }
            else
            {
                value = GetValue(value);
                symbolTable.Add(new Symbol(name, "int", value, level));
            }
        }
        // real类型变量声明
        private void DeclarationReal(String name, String value)
        {
            int length = -1;
            length = IsArray(name);

            if (!name.Equals(GetVariableName(name)))
            {
                if (length <= 0)
                {
                    errList.Add(new ExceptionMessage("Array index are out of bounds", 2));
                    return;
                }
                else
                {
                    name = GetVariableName(name);
                    symbolTable.Add(new Symbol(name, "real", level, length));
                }
            }
            else
            {
                value = GetValue(value);
                symbolTable.Add(new Symbol(name, "real", value, level));
            }
        }



        /**
         * 通过字符串截取获取名称
         * 如果是a[5]样子的数组形式，则返回a
         * 如果是普通变量返回变量名
         * */
        private String GetVariableName(String name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                if (name[i] == '[')
                {
                    name = name.Substring(0, i);
                    break;
                }
            }
            return name;
        }

        // 获取名为name的token的值
        private String GetValue(String name)
        {
            String value = "";
            if (isDigit(name))
            {
                value = name;
            }
            else if (name.Equals("result"))
            {
                value = this.result;
            }
            else
            {
                //获取数组下标
                int index = IsArray(name);
                String arr_name = GetVariableName(name);
                Symbol symbol = symbolTable.GetSymbol(arr_name,level);
                if (!name.Equals(arr_name))
                {
                    if (index < 0 || index > symbol.Length - 1)
                    {
                        errList.Add(new ExceptionMessage("Array index are out of bounds", 2));
                        return null;
                    }
                    else
                    {
                        value = symbol.ArrayValue[index];
                    }
                    
                }
                else
                {
                    value = symbol.Value;
                }
            }
            return value;
        }

        /**
         * 判断字符串是否为数字
         * 
         **/
        private bool isDigit(String name)
        {
            try
            {
                Double.Parse(name);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /**
         * 判断一个标识符是否为数组
         * 
         * 返回数组下标
         * */
        private int IsArray(String name)
        {
            char[] nameArr = name.ToCharArray();
            int count = nameArr.Count();
            int cursor = 0;
            String nameTemp = "";           
            if (count > 3 && nameArr[count - 1] == ']')
            {
                bool start = false;
                for (int i = 0; i < count - 1; i++)
                {                    
                    if (start)
                    {
                        nameTemp += name[i];
                    }
                    if (nameArr[i] == '[')
                    {
                        start = true;
                    }                    
                }
                cursor = int.Parse(GetValue(nameTemp));
                return cursor;            
            }
            
            return -1;
        }

        // 有条件跳转Jump
        private int IfFalseJump(String cursor)
        {
            int address = int.Parse(cursor) - 1;
            if (result.Equals("True"))
            {
                return -1;
            }
            return address;
        }
        // 无条件跳转Jump
        private int Jump(String cursor)
        {
            int address = int.Parse(cursor) - 1;
            return address;
        }
        // 赋值语句
        private void Asign(String name, String value)
        {
            int cursor = -1;
            cursor = IsArray(name);

            String arrayName = GetVariableName(name);
            Symbol symbol = symbolTable.GetSymbol(arrayName, level);
            value = GetValue(value);

            if (!name.Equals(arrayName))
            {
                if (cursor < 0 || cursor >= symbol.Length)
                {
                    errList.Add(new ExceptionMessage("Array index are out of bounds", 2));
                    return;
                }
                symbol.ArrayValue[cursor] = value;
            }
            else
            {
                symbol.Value = value;
            }
        }
        // read语句
        public void Read(String name, String value)
        {
            String input = value; ; //输入值 
            if (!isDigit(input))
            {
               //输入的值有误，请重新输入
            }
            String array_name = GetVariableName(name);
            Symbol symbol = symbolTable.GetSymbol(array_name, level);
            int length = IsArray(name);                      
            if (!name.Equals(array_name))
            {
                if(length<0 || length>=symbol.Length)
                {
                    errList.Add(new ExceptionMessage("Array index are out of bounds", 2));
                    return;
                }
            }
            if (length != -1)
            {
                symbol.ArrayValue[length] = input;
            }
            else
            {
                symbol.Value = input;
            }
        }
        // write语句
        private void Write(String name)
        {
            String value = "";
            value = GetValue(name);
            if (value == null)
                return;
            PrintRes(value); 
        }
        // 根据操作符的类型及进行不同的计算
        private void Cal(String op1, String op2, String operation)
        {
            op1 = GetValue(op1);
            op2 = GetValue(op2);
            if (op1 == null || op2 == null)
                return;
            double a = Double.Parse(op1);
            double b = Double.Parse(op2);

            switch(operation)
            {
                case FourCode.ADD: result = (b + a).ToString();break;
                case FourCode.SUB: result = (b - a).ToString(); break;
                case FourCode.MUL: result = (b * a).ToString(); break;
                case FourCode.DIV:
                    if (a == 0)
                    {
                        errList.Add(new ExceptionMessage("Can not divide by zero", 2));
                        return;
                    }
                    result = (b / a).ToString();
                    break;
                case FourCode.NOTEQUAL: result = (b != a).ToString(); break;
                case FourCode.EQUAL: result = (b == a).ToString(); break;
                case FourCode.LARGER: result = (b > a).ToString(); break;
                case FourCode.SMALLER: result = (b < a).ToString(); break;
            }
        }

        // 打印输出
        public void PrintRes(String str)
        {
            resultBox.AppendText(str + "\n");
        }
    }
}
