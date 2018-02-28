using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMM_interpreter.UI
{
    public partial class AboutUs : Form
    {
        public AboutUs()
        {
            InitializeComponent();
        }

        private void AboutUs_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "组员：\n"+
                 "豆宛荣 20153025800115 词法分析+PPT\n" +
                "石宝荣 2015302580090 语法分析+UI界面\n" +
                "韩晓峰 2015302580046 语义分析+中间代码\n"+
                "洪程之 2015302580047 解释执行+说明文档";
        }

        
    }
}
