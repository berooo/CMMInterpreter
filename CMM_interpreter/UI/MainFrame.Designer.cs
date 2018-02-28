namespace CMM_interpreter.UI
{
    partial class MainFrame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrame));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.保存文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.新建文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.运行ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.使用说明ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.关于我们ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.p_line = new System.Windows.Forms.Panel();
            this.source = new System.Windows.Forms.RichTextBox();
            this.output = new System.Windows.Forms.RadioButton();
            this.lexical = new System.Windows.Forms.RadioButton();
            this.grammar = new System.Windows.Forms.RadioButton();
            this.middlecode = new System.Windows.Forms.RadioButton();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.运行ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(834, 32);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开文件ToolStripMenuItem,
            this.toolStripSeparator1,
            this.保存文件ToolStripMenuItem,
            this.toolStripSeparator2,
            this.新建文件ToolStripMenuItem,
            this.toolStripSeparator3});
            this.文件ToolStripMenuItem.Image = global::CMM_interpreter.Properties.Resources.菜单;
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(78, 28);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 打开文件ToolStripMenuItem
            // 
            this.打开文件ToolStripMenuItem.Name = "打开文件ToolStripMenuItem";
            this.打开文件ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.打开文件ToolStripMenuItem.Text = "打开文件";
            this.打开文件ToolStripMenuItem.Click += new System.EventHandler(this.打开文件ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(161, 6);
            // 
            // 保存文件ToolStripMenuItem
            // 
            this.保存文件ToolStripMenuItem.Name = "保存文件ToolStripMenuItem";
            this.保存文件ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.保存文件ToolStripMenuItem.Text = "保存文件";
            this.保存文件ToolStripMenuItem.Click += new System.EventHandler(this.保存文件ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(161, 6);
            // 
            // 新建文件ToolStripMenuItem
            // 
            this.新建文件ToolStripMenuItem.Name = "新建文件ToolStripMenuItem";
            this.新建文件ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.新建文件ToolStripMenuItem.Text = "新建文件";
            this.新建文件ToolStripMenuItem.Click += new System.EventHandler(this.新建文件ToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(161, 6);
            // 
            // 运行ToolStripMenuItem
            // 
            this.运行ToolStripMenuItem.Image = global::CMM_interpreter.Properties.Resources.启动;
            this.运行ToolStripMenuItem.Name = "运行ToolStripMenuItem";
            this.运行ToolStripMenuItem.Size = new System.Drawing.Size(78, 28);
            this.运行ToolStripMenuItem.Text = "运行";
            this.运行ToolStripMenuItem.Click += new System.EventHandler(this.运行ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.使用说明ToolStripMenuItem,
            this.toolStripSeparator4,
            this.关于我们ToolStripMenuItem});
            this.帮助ToolStripMenuItem.Image = global::CMM_interpreter.Properties.Resources.help;
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(78, 28);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 使用说明ToolStripMenuItem
            // 
            this.使用说明ToolStripMenuItem.Name = "使用说明ToolStripMenuItem";
            this.使用说明ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.使用说明ToolStripMenuItem.Text = "使用说明";
            this.使用说明ToolStripMenuItem.Click += new System.EventHandler(this.使用说明ToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(161, 6);
            // 
            // 关于我们ToolStripMenuItem
            // 
            this.关于我们ToolStripMenuItem.Name = "关于我们ToolStripMenuItem";
            this.关于我们ToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.关于我们ToolStripMenuItem.Text = "关于我们";
            this.关于我们ToolStripMenuItem.Click += new System.EventHandler(this.关于我们ToolStripMenuItem_Click);
            // 
            // p_line
            // 
            this.p_line.Location = new System.Drawing.Point(0, 79);
            this.p_line.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.p_line.Name = "p_line";
            this.p_line.Size = new System.Drawing.Size(39, 324);
            this.p_line.TabIndex = 1;
            this.p_line.Paint += new System.Windows.Forms.PaintEventHandler(this.p_line_Paint);
            // 
            // source
            // 
            this.source.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.source.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.source.Location = new System.Drawing.Point(46, 79);
            this.source.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.source.Name = "source";
            this.source.Size = new System.Drawing.Size(741, 323);
            this.source.TabIndex = 0;
            this.source.Text = "";
            this.source.TextChanged += new System.EventHandler(this.source_TextChanged);
            // 
            // output
            // 
            this.output.AutoSize = true;
            this.output.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.output.Location = new System.Drawing.Point(75, 418);
            this.output.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(73, 29);
            this.output.TabIndex = 2;
            this.output.TabStop = true;
            this.output.Text = "输出";
            this.output.UseVisualStyleBackColor = true;
            this.output.CheckedChanged += new System.EventHandler(this.output_CheckedChanged);
            // 
            // lexical
            // 
            this.lexical.AutoSize = true;
            this.lexical.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lexical.Location = new System.Drawing.Point(244, 418);
            this.lexical.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lexical.Name = "lexical";
            this.lexical.Size = new System.Drawing.Size(109, 29);
            this.lexical.TabIndex = 3;
            this.lexical.TabStop = true;
            this.lexical.Text = "词法分析";
            this.lexical.UseVisualStyleBackColor = true;
            this.lexical.CheckedChanged += new System.EventHandler(this.lexical_CheckedChanged);
            // 
            // grammar
            // 
            this.grammar.AutoSize = true;
            this.grammar.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grammar.Location = new System.Drawing.Point(440, 418);
            this.grammar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grammar.Name = "grammar";
            this.grammar.Size = new System.Drawing.Size(109, 29);
            this.grammar.TabIndex = 4;
            this.grammar.TabStop = true;
            this.grammar.Text = "语法分析";
            this.grammar.UseVisualStyleBackColor = true;
            this.grammar.CheckedChanged += new System.EventHandler(this.grammar_CheckedChanged);
            // 
            // middlecode
            // 
            this.middlecode.AutoSize = true;
            this.middlecode.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.middlecode.Location = new System.Drawing.Point(628, 418);
            this.middlecode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.middlecode.Name = "middlecode";
            this.middlecode.Size = new System.Drawing.Size(109, 29);
            this.middlecode.TabIndex = 5;
            this.middlecode.TabStop = true;
            this.middlecode.Text = "中间代码";
            this.middlecode.UseVisualStyleBackColor = true;
            this.middlecode.CheckedChanged += new System.EventHandler(this.middlecode_CheckedChanged);
            // 
            // richTextBox2
            // 
            this.richTextBox2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox2.Location = new System.Drawing.Point(46, 463);
            this.richTextBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(741, 255);
            this.richTextBox2.TabIndex = 6;
            this.richTextBox2.Text = "";
            this.richTextBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.richTextBox2_KeyPress);
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(46, 463);
            this.listView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(741, 255);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.SmallIcon;
            // 
            // treeView1
            // 
            this.treeView1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.Location = new System.Drawing.Point(46, 463);
            this.treeView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(741, 255);
            this.treeView1.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 18);
            this.label1.TabIndex = 10;
            // 
            // MainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(834, 748);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.middlecode);
            this.Controls.Add(this.grammar);
            this.Controls.Add(this.lexical);
            this.Controls.Add(this.output);
            this.Controls.Add(this.source);
            this.Controls.Add(this.p_line);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainFrame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CMM_interepreter";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 保存文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 新建文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem 运行ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 使用说明ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem 关于我们ToolStripMenuItem;
        private System.Windows.Forms.Panel p_line;
        private System.Windows.Forms.RichTextBox source;
        private System.Windows.Forms.RadioButton output;
        private System.Windows.Forms.RadioButton lexical;
        private System.Windows.Forms.RadioButton grammar;
        private System.Windows.Forms.RadioButton middlecode;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label1;
    }
}