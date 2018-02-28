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
    public partial class UsingDetails : Form
    {
        public UsingDetails()
        {
            InitializeComponent();
        }

        private void UsingDetails_Load(object sender, EventArgs e)
        {

            richTextBox1.Text = "运行环境：.NET Framework 4.0及以上\n"+
                "操作简介：直接双击打开CMMAnalysis.exe文件，或者在Visual Studio中运行项目。点击文件->打开，可以找到.cmm文件，或者在解释器上方窗口输入源代码，点击运行即可解释执行程序。在下方四个选项中可以选择查看程序运行的输出、程序的词法分析结果、语法树和中间代码。窗口中输入的源代码可以通过文件->保存，保存完成的代码文件。";
        }
    }
}
