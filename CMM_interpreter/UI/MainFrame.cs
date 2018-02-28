using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CMM.Model;
using CMM.Interpreter;
using System.IO;

namespace CMM_interpreter.UI
{
    public partial class MainFrame : Form
    {
        Highlight hl;
        Lexer lexer;   //词法
        Parser parser;  //语法
        List<ExceptionMessage> lexicalErrorList; //词法错误列表
        List<ExceptionMessage> syntaxErrorList;   //语法错误列表
        List<ExceptionMessage> exectutionErrorList;   // 执行错误列表
        List<Token> lw;
        SyntaxTreeNode start;
        CodeGenerator gen;   //中间代码
        List<FourCode> fl = new List<FourCode>();
        Boolean flag = false;
        Boolean read = false;
        public String temp;
        public bool tempp = false;
        int cursor = 0;
        String sourcecode;     //源程序
        String input = "";
        bool isPress = true; // 输入时候判断
        Execute exe = null;
        //新建文件使用的变量
        int pagenum = 0;
        List<SourceCode> tablist;
        int currentpage;

        //删除使用
        int tempindex = 0;

        public MainFrame()
        {
            InitializeComponent();
            hl = new Highlight(this);
            tablist = new List<SourceCode>();
            SourceCode table = new SourceCode();
            tablist.Add(table);
        }

        public void setRead(bool a)
        {
            read = a;
        }
        private void p_line_Paint(object sender, PaintEventArgs e)
        {
            showLineNo();
        }
        private void source_TextChanged(object sender, EventArgs e)
        {
            p_line.Invalidate();
            showHighLight();
        }
        private void source_VScroll(object sender, EventArgs e)
        {
            p_line.Invalidate();
        }

        //================行号显示==================
        private void showLineNo()
        {
            //获取当前源代码的坐标信息
            Point p = new Point(0, 0);
            int srcFirstIndex = source.GetCharIndexFromPosition(p);
            int srcFirstLine = source.GetLineFromCharIndex(srcFirstIndex);
            Point srcFirstPos = source.GetPositionFromCharIndex(srcFirstIndex);
            //
            p.Y += source.Height;
            p.X += source.Width;
            //
            int srcLastIndex = source.GetCharIndexFromPosition(p);
            int srcLastLine = source.GetLineFromCharIndex(srcLastIndex);
            Point srcLastPos = source.GetPositionFromCharIndex(srcLastIndex);
            //准备画图
            Graphics g = this.p_line.CreateGraphics();
            Font font = new Font(source.Font, source.Font.Style);
            SolidBrush brush = new SolidBrush(Color.Black);
            //画图开始
            //刷新画布
            Rectangle rect = p_line.ClientRectangle;
            brush.Color = p_line.BackColor;
            g.FillRectangle(brush, 0, 0, p_line.Width, p_line.Height);
            brush.Color = Color.Chocolate;//画行号的画笔
            //绘制行号
            int lineSpace = 0;//每行的高度
            if (srcFirstLine != srcLastLine)
            {
                lineSpace = (srcLastPos.Y - srcFirstPos.Y) / (srcLastLine - srcFirstLine);
            }
            else
            {
                lineSpace = Convert.ToInt32(source.Font.Size);
            }
            //行号的左上角坐标
            int brushX = p_line.ClientRectangle.Width - Convert.ToInt32(font.Size * 3);
            int brushY = srcLastPos.Y + Convert.ToInt32(font.Size * 0.21f);
            for (int i = srcLastLine; i >= srcFirstLine; i--)
            {
                String number = (i + 1).ToString();
                brushX = p_line.ClientRectangle.Width - Convert.ToInt32(font.Size * number.Length) - 3;
                g.DrawString(number, font, brush, brushX, brushY);//左对齐数字
                brushY -= lineSpace;
            }
            //释放资源
            g.Dispose();
            font.Dispose();
            brush.Dispose();
        }

       
        //===================高亮辅助=========================

        public void showHighLight()
        {
            // 判断语法高亮
            hl.BeginPaint(source);
            List<String> list = hl.getMatchWords(source.Text);
            int currentIndex = source.SelectionStart, i = 0;
            int j = 0;
            foreach (String item in list)
            {
                j++;
                if (hl.Keywords.Contains(item.Trim()))
                {
                    if (item.Trim() == "//" || item.Trim() == "/*" || item.Trim() == "*/")
                    {
                        hl.mySelect(source, i, item, Color.Green, true);
                    }
                    else
                    {
                        hl.mySelect(source, i, item, Color.Orange, true);
                    }
                }
                else
                {
                    hl.mySelect(source, i, item, Color.Black, false);
                }
                i += item.Length;
            }
            source.Select(currentIndex, 0);
            hl.EndPaint(source);
        }

        //======================词法分析=====================
        private void ShowLexer()
        {
            sourcecode = source.Text;
            lexer = new Lexer(sourcecode);
            List<Token> lw = new List<Token>();
            lexer.LexicalAnalyse();
            lexer.ChangeErrorList();// 把错误的token调价到errorList
            lexicalErrorList = lexer.errorlist;
            
        }

        //======================语法/语义分析+错误处理=====================
        private void ShowParser()
        {
            //语法分析
            parser = new Parser(sourcecode);
            start = parser.Parse();

            //获取错误列表
            syntaxErrorList = new List<ExceptionMessage>();
            syntaxErrorList = parser.GetErrorList();
           
        }

        // 解析语法树，并保存到C#控件TreeNode中，然后在面板上以TreeView进行显示。
        private void ShowTreeNode(SyntaxTreeNode node, TreeNode root, String space)
        {
            String typeTemp = node.Type;
            TreeNode no;
            if (typeTemp.Equals("Program") || typeTemp.Equals("StmtList")
                    || typeTemp.Equals("Statement")
                    || typeTemp.Equals("VariableDeclaration")
                    || typeTemp.Equals("Block") || typeTemp.Equals("Condition")
                    || typeTemp.Equals("Expression") || typeTemp.Equals("Term")
                    || typeTemp.Equals("Factor"))
            {
                no = new TreeNode(node.Type);
                root.Nodes.Add(no);
            }
            else
            {
                no = new TreeNode(space + node.Type + ":  " + node.Name);
                root.Nodes.Add(no);
            }

            if (node.ChildTreeNodeList != null)
            {
                foreach (SyntaxTreeNode subNode in node.ChildTreeNodeList)
                {
                    ShowTreeNode(subNode, no, space);
                }
            }
        }

        // ================中间代码生成=========================
        private void ShowGen()
        {
            if (lexicalErrorList.Count == 0 && syntaxErrorList.Count == 0) //判断上是否存在词法、语法错误
            {
                gen = new CodeGenerator(sourcecode);

                fl = gen.GetFourCodeList();
            }
            else
            {
                richTextBox2.Text = "There was an error, the middle code generation failed.";
            }
        }
        // ================解释执行=======================
        private void ShowExcute()
        {
            listView1.Clear();
            if (lexicalErrorList.Count == 0 && syntaxErrorList.Count == 0) // 判断语法、词法是否存在错误
            {
                if (exe == null)
                {
                    treeView1.Hide();
                    listView1.Hide();
                    richTextBox2.Clear();
                    exe = new Execute(fl, this, richTextBox2);
                    richTextBox2.Show();
                }
                   
                if (cursor != -1)
                    cursor = exe.ExecuteCode(cursor);
                if (cursor == -1)
                {
                    exe = null;
                    cursor = 0;
                }
                if (cursor == -2)
                {
                    richTextBox2.Hide();
                    treeView1.Hide();
                    exectutionErrorList = exe.getErrList();
                    foreach (ExceptionMessage e in exectutionErrorList)
                    {
                        ListViewItem Item = new ListViewItem(e.Message + " at line " + e.LineNo + ".");
                        Item.ForeColor = Color.Red;
                        Item.Font = new Font("微软雅黑", 10);
                        listView1.Items.Add(Item);
                    }
                    listView1.Show();
                    exe = null;
                    cursor = 0;
                }

            }
            else if (lexicalErrorList.Count != 0)
            {
                richTextBox2.Hide();
                treeView1.Hide();
                //词法错误
                foreach (ExceptionMessage er in lexicalErrorList)
                {
                    ListViewItem Item = new ListViewItem(er.Message + " at line " + er.LineNo + ".");
                    Item.ForeColor = Color.Red;
                    Item.Font = new Font("微软雅黑", 10);
                    listView1.Items.Add(Item);
                   
                }
                listView1.Show();
            }else if (syntaxErrorList.Count != 0)
            {
                richTextBox2.Hide();
                treeView1.Hide();
                foreach (ExceptionMessage er in syntaxErrorList)
                {
                    ListViewItem Item = new ListViewItem(er.Message + " at line " + er.LineNo + ".");
                    Item.ForeColor = Color.Red;
                    Item.Font = new Font("微软雅黑", 10);
                    listView1.Items.Add(Item);
                }
                listView1.Show();
            }
        }

        private void output_CheckedChanged(object sender, EventArgs e)
        {
            if (source.Text != "")
            {
                if (output.Checked == true)
            {
                ShowExcute();
            }
           }
           
        }

        private void lexical_CheckedChanged(object sender, EventArgs e)
        {
            if (source.Text != "")
            {
                treeView1.Hide();
                listView1.Hide();
                listView1.Clear();
                richTextBox2.Hide();
                richTextBox2.Clear();

                lw = new List<Token>();
                if (lexicalErrorList.Count != 0)
                {
                    foreach (ExceptionMessage er in lexicalErrorList)
                    {

                        ListViewItem Item = new ListViewItem(er.Message + " at line " + er.LineNo + ".");
                        Item.ForeColor = Color.Red;
                        Item.Font = new Font("微软雅黑", 10);
                        listView1.Items.Add(Item);

                    }
                    listView1.Show();
                }
                else
                {
                    lw = lexer.GetTokenList();
                    String[] sourceTemp = this.sourcecode.Trim().Split('\n');
                    for (int i = 0; i < sourceTemp.Length; i++)
                    {
                        richTextBox2.AppendText("Line " + (i + 1) + ":    " + sourceTemp[i] + "\n");
                        foreach (Token w in lw)
                        {
                            if (w.Line == i + 1)
                            {
                                richTextBox2.AppendText( w.GetString() + "   ");
                            }
                        }
                        richTextBox2.AppendText("\n");
                    }
                    richTextBox2.Show();
                }
            }
        }

        private void grammar_CheckedChanged(object sender, EventArgs e)
        {
            if (source.Text != "")
            {
                richTextBox2.Hide();
                listView1.Hide();
                treeView1.Nodes.Clear();
                if (lexicalErrorList.Count == 0)
                {
                    if (syntaxErrorList.Count != 0)//判断是否存在语法错误
                    {
                        foreach (ExceptionMessage er in syntaxErrorList)
                        {
                            ListViewItem Item = new ListViewItem(er.Message + " at line " + er.LineNo + ".");
                            Item.ForeColor = Color.Red;
                            Item.Font = new Font("微软雅黑", 10);
                            listView1.Items.Add(Item);
                        }

                        //=========存在错误、语法树创建失败
                        TreeNode tree = new TreeNode("There is syntax or semantic error, syntax tree creation failed!");
                        treeView1.Nodes.Add(tree);
                    }
                    else
                    {
                        TreeNode tree = new TreeNode("Syntax  Tree");
                        treeView1.Nodes.Add(tree);
                        ShowTreeNode(start, tree, "");

                    }
                }
                else
                {
                    //=========存在词法错误、语法树创建失败======================
                    TreeNode tree = new TreeNode("There is a lexical error, syntax tree creation failed!");
                    treeView1.Nodes.Add(tree);
                }
                treeView1.Show();
            }
        }
        private void middlecode_CheckedChanged(object sender, EventArgs e)
        {
            if (source.Text != "")
            {
                treeView1.Hide();
                listView1.Hide();
                richTextBox2.Clear();
                if (flag)
                {
                    if (lexicalErrorList.Count == 0 && syntaxErrorList.Count == 0) //判断上是否存在词法、语法错误
                    {
                        gen = new CodeGenerator(sourcecode);

                        fl = gen.GetFourCodeList();
                        int i = 0;
                        foreach (FourCode f in fl)
                        {
                            richTextBox2.AppendText(i + ". （" + f.Operation + "，" + f.FirstArgument + "，"
                            + f.SecondArgument + "，" + f.Result + "）\n");
                            i++;
                        }
                    }
                    else
                    {
                        richTextBox2.Text = "There was an error, the middle code generation failed.";
                    }
                }

                richTextBox2.Show();
            }
        }
        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //打开对话框
            OpenFileDialog fileExplorer = new OpenFileDialog();
            fileExplorer.Filter = "*.cmm | *.cmm";//筛选cmm文件

            if (fileExplorer.ShowDialog() == DialogResult.OK)
            {
                String fileName = fileExplorer.FileName;//文件名
                int ii = 0;
                foreach (SourceCode de in tablist)
                {
                    if (((SourceCode)de).filename.Equals(fileName))
                    {
                        return;
                    }
                    ii++;
                }

                if (fileExplorer.FileName == "") return;//如果没有文件名
                if (source.Text != "")//如果这个sourceCode没有文字就替代
                {
                    pagenum++;//增加最大标签行数
                    SourceCode table = new SourceCode();
                    table.filename = fileName;
                    table.code = source.Text;
                    tablist.Add(table);
                    //新建标签页

                    TabPage tabPage = new TabPage(fileName.Substring(fileName.LastIndexOf('\\') + 1));//筛选文件名称
                    //写入
                    if (fileExplorer.FileName == "") return;
                    StreamReader sr = new StreamReader(fileExplorer.FileName, Encoding.Default);
                    String sourceLine = null, readString = "";
                    while ((sourceLine = sr.ReadLine()) != null)
                    {
                        readString += sourceLine + "\r\n";
                    }
                    source.Text = readString;
                    label1.Text = fileName;
                    sr.Close();
                }
                else
                {
                    SourceCode table = new SourceCode();
                    table.filename = fileName;
                    //下面是吧文件写进去~

                    StreamReader sr = new StreamReader(fileExplorer.FileName, Encoding.Default);
                    String sourceLine = null, readString = "";
                    while ((sourceLine = sr.ReadLine()) != null)
                    {
                        readString += sourceLine + "\r\n";
                    }
                    source.Text = readString;
                    label1.Text = fileName;
                    table.code = source.Text;
                    sr.Close();
                }
            }
            currentpage = pagenum;//存上次的标签页数
        }

        private void 保存文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();//保存函数窗口
            sfd.Filter = "*.cmm|*.cmm";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName))//打开文本域
                {

                    sw.Write(source.Text);

                    sw.Close();
                    MessageBoxButtons messButton = MessageBoxButtons.OK;
                    DialogResult dr = MessageBox.Show("Saved successfully", "Confirm", messButton, MessageBoxIcon.Information);
                }
            }
        }

        private void 运行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        
                listView1.Items.Clear();
                richTextBox2.Clear();
                treeView1.Nodes.Clear();
                flag = true;
                if (source.Text != "")
                {
                    ShowLexer();
                    ShowParser();
                    ShowGen();
                    if (output.Checked == true)
                    {
                        ShowExcute();
                    }
                    else
                    {
                        output.Select();
                    }

                }
                else
                {
                    MessageBoxButtons messButton = MessageBoxButtons.OK;
                    DialogResult dr = MessageBox.Show("Source code is blank, please enter source code.", "Error", messButton, MessageBoxIcon.Error);
                }
        }
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

        private void richTextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (read)
            {
                if (e.KeyChar == '\n' || e.KeyChar == '\r')
                {
                    isPress = false;
                    if (!this.isDigit(input))
                    {
                        this.richTextBox2.AppendText("Not a number, please re-enter!" + "\n");
                        input = "";
                    }
                    else if (cursor != -1)
                    {
                        exe.Read(exe.readVar, input);
                        this.ShowExcute();
                        read = false;
                    }
                    input = "";
                }
                else
                {
                    if (e.KeyChar == '\b')
                    {
                        if (input.Length != 0)
                            input = input.Substring(0, input.Length - 1);
                    }
                    else
                    {
                        input += e.KeyChar.ToString();
                    }
                }

                //将光标置于最下方
                richTextBox2.SelectionStart = richTextBox2.Text.Length;
            }
        }
        public String getInput()
        {
            return input;
        }

        private void 新建文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text="";
            source.Clear();
            output.Checked = false;
            lexical.Checked = false;
            grammar.Checked = false;
            middlecode.Checked = false;
            richTextBox2.Clear();
            lexicalErrorList.Clear();
            syntaxErrorList.Clear();

        }

        private void 关于我们ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutUs a = new AboutUs();
            a.ShowDialog();
        }

        private void 使用说明ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UsingDetails u = new UsingDetails();
            u.ShowDialog();
        }

    }
}
