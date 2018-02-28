using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;

namespace CMM_interpreter.UI
{
    class Highlight
    {
        private List<String> keywords = null;
        MainFrame mf = null;
        public Highlight(MainFrame f)
        {
            mf = f;
            keywords = new List<string>();

            //需特殊处理的关键字
            keywords.Add("read");
            keywords.Add("write");
            keywords.Add("int");
            keywords.Add("real");
            keywords.Add("if");
            keywords.Add("else");
            keywords.Add("while");
            keywords.Add("//");
            keywords.Add("/*");
            keywords.Add("*/");
        }

        public List<String> Keywords
        {
            get { return keywords; }
            set { keywords = value; }
        }
        //将字符流分为可能为关键字的字母，其他
        public List<String> getMatchWords(String str)
        {
            List<String> resultList = new List<string>();
            StringReader reader = new StringReader(str);
            StringBuilder builder = new StringBuilder(), notBuilder = new StringBuilder();
            int c = -1;
            while ((c = reader.Read()) != -1)
            {
                if(char.IsLetter((char)c)|| char.IsDigit((char)c))
                {
                    if (notBuilder.Length > 0)
                    {
                        resultList.Add(notBuilder.ToString());
                        notBuilder = new StringBuilder();
                    }
                    builder.Append((char)c);
                }

                else
                {
                    if (builder.Length > 0)
                    {
                        resultList.Add(builder.ToString());
                        builder = new StringBuilder();
                    }
                    notBuilder.Append((char)c);
                }
            }
            if (builder.Length > 0)
            {
                resultList.Add(builder.ToString());
            }

            if (notBuilder.Length > 0)
            {
                resultList.Add(notBuilder.ToString());
            }


            return resultList;
        }

        //改变i之后string s的字体
        public void mySelect(RichTextBox tb, int i, string s, Color c, bool font)
        {
            //BeginPaint(tb);
            tb.Select(i, s.Length);
            tb.SelectionColor = c;
            //是否改变字体
            if (font)
                tb.SelectionFont = new Font("微软雅黑", tb.Font.Size, (FontStyle.Bold));
            else
                tb.SelectionFont = new Font("微软雅黑", tb.Font.Size, (FontStyle.Regular));
            //以下是把光标放到原来位置，并把光标后输入的文字重置
            tb.Select(i, 0);
            //EndPaint(tb);
            tb.SelectionFont = new Font("微软雅黑", tb.Font.Size, (FontStyle.Regular));
            tb.SelectionColor = Color.Black;
        }

        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;

        //停止控件的重绘 
        public void BeginPaint(RichTextBox rb)
        {
            SendMessage(rb.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
        }

        //允许控件重绘. 
        public void EndPaint(RichTextBox rb)
        {
            SendMessage(rb.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            rb.Refresh();
        }
    }
}
