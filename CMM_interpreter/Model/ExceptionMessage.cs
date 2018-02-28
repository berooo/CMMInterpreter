using System;

namespace CMM.Model
{
    // 异常错误类，用来保存保存信息，以显示到信息提示框
    class ExceptionMessage
    {
        // 保存错误行号
        public int LineNo { get; set; }
        // 保存错误信息
        public String Message { get; set; }
        // 构造函数
        public ExceptionMessage(String detail, int lineNo)
        {
            this.Message = detail;
            this.LineNo = lineNo;
        }
    }
}
