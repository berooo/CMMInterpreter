using System;
using System.Collections.Generic;


namespace CMM.Model
{
    // 符号表
    class SymbolTable
    {
        private List<Symbol> symbolList = new List<Symbol>();


        // 向符号表中添加一个符号表因子
        public void Add(Symbol symbol)
        {
            symbolList.Add(symbol);
        }

        // 删除某层所有符号表因子
        private void RemoveSymbol(int level)
        {
            for (int i = 0; i < symbolList.Count; i++)
            {
                if (symbolList[i].Level == level)
                {
                    symbolList.RemoveAt(i);
                }
            }
        }

        // 获取该层外及该层离得最近的名字为name的变量
        public Symbol GetSymbol(String name, int level)
        {
            Symbol temp = null;
            int minLevel = 0;

            foreach (Symbol symbol in symbolList)
            {
                if (symbol.Name == name)
                {
                    if (temp != null && level >= symbol.Level)
                    {
                        int levelDif = level - symbol.Level;
                        if (levelDif < minLevel)
                        {
                            minLevel = levelDif;
                            temp = symbol;
                        }
                    }
                    else if (temp == null && level >= symbol.Level)
                    {
                        temp = symbol;
                        minLevel = level - symbol.Level;
                    }
                }
            }

            return temp;
        }
    }
}