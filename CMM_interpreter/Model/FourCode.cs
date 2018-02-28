using System;
namespace CMM.Model
{
    //四元式类
    class FourCode
    {

        // 基本属性
        public String Operation { get; set; }
        public String FirstArgument { get; set; }
        public String SecondArgument { get; set; }
        public String Result { get; set; }
        public int Level { get; set; }

        // 构造函数
        public FourCode(String operation, String firstArgument, 
            String secondArgument, String result, int level)
        {
            this.Operation = operation;
            this.FirstArgument = firstArgument;
            this.SecondArgument = secondArgument;
            this.Result = result;
            this.Level = level;
        }

        public const String EMPTY = "NULL";

        public const String READ = "read";
        public const String WRITE = "write";
        public const String LARGER = ">";
        public const String SMALLER = "<";

        public const String EQUAL = "==";
        public const String NOTEQUAL = "<>";
        public const String ASIGN = "asign";
        public const String SUB = "sub";
        public const String ADD = "add";
        public const String MUL = "mul";
        public const String DIV = "div";

        public const String IFFALSEJUMP = "ifFalseJump";
        public const String JUMP= "jump";
        public const String DECLARATIONINT = "declareInt";
        public const String DECLATATIONREAL= "declareReal";
        public const String TEMP = "temp";

        public const String FINISH = "finish";

    }
}
