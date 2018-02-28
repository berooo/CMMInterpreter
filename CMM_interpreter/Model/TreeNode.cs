using System;
using System.Collections.Generic;

namespace CMM.Model
{   
    // 语法树节点类
    class SyntaxTreeNode
    {
        public String Type { get; set; }
        public String Name { get; set; }
        public String exType { get; set; }
        public List<SyntaxTreeNode> ChildTreeNodeList { get; set; } // 子节点链表

        public SyntaxTreeNode(String type)
        {
            this.Type = type;
            this.ChildTreeNodeList = new List<SyntaxTreeNode>();
        }

        public SyntaxTreeNode(String type, String name)
        {
            this.Type = type;
            this.Name = name;
            this.ChildTreeNodeList = new List<SyntaxTreeNode>();
        }

    }   
}
