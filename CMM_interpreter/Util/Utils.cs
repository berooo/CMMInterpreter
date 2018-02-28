using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMM.Util
{
    class Utils
    {
        // 将字符转化为字符串的方法
        public static String CharToString(char[] array, int index)
        {
            String str = "";
            for (int i = 0; i < index; i++)
            {
                str += array[i];
            }
            return str;
        }
        public static String CharToString(char[] array, int begin, int end)
        {
            String str = "";
            for (int i = begin; i < end; i++)
            {
                str += array[i];
            }
            return str;
        }

        // 判断数组中是否包含某个元素
        public static bool Include<T>(T[] array, T t)
        {
            foreach (T a in array)
            {
                if (t.Equals(a))
                {
                    return true;
                }
            }
            return false;
        }

        // 判断是否为字母
        public static bool IsLetter(char c)
        {
            if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_')
                return true;
            return false;
        }

        // 判断字符是否为数字
        public static bool IsDigit(char c)
        {
            if (c >= '0' && c <= '9')
                return true;
            return false;
        }

        // 判断是否为空格
        public static bool IsWhiteSpace(char c)
        {
            if (c == ' ' || c == '\n' || c == '\r' || c == '\t')
            {
                return true;
            }
            return false;
        }

        // 判断是否为分隔符
        public static bool IsSeparator(char c)
        {
            char[] separators = new char[8] { '[', ']', '{', '}', '(', ')', ';', ',' };
            for (int i = 0; i < separators.Length; i++)
            {
                if (separators[i] == c)
                {
                    return true;
                }
            }
            return false;
        }

        // 判断是否是保留字
        public static bool IsReservedWord(String identifier)
        {
            // 保留字
            String[] reservedwords = new String[] { "if", "while", "else", "read", "write", "int", "real" };
            for (int i = 0; i < reservedwords.Length; i++)
            {
                if (reservedwords[i].Equals(identifier))
                    return true;
            }
            return false;
        }

    }
}
