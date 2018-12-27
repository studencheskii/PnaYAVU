using System;
using System.Runtime.Remoting;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections;

namespace Lab2
{
    internal class MyString : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            return Str.GetEnumerator();
        }

        public Char[] Str;

        public MyString()
        {
            this.Str = null;
        }

        public MyString(Char Symbol, Int32 Count)
        {
            this.Str = new char[Count];
            for (int i = 0; i < Count; i++)
            {
                this.Str[i] = Symbol;
            }
        }

        unsafe public MyString(Char* Str)
        {
            int Count = 0;
            for (int i = 0; Str[i] != '\0'; i++)
            {
                Count++;
            }
            for (int i = 0; i < Count; i++)
            {
                this.Str[i] = Str[i];
            }
        }

        public MyString(Char[] Str)
        {
            this.Str = new char[Str.Length];
            for (int i = 0; i < Str.Length; i++)
            {
                this.Str[i] = Str[i];
            }
        }

        public MyString(Char[] Str, Int32 StartIndex, Int32 Size)
        {
            this.Str = new char[StartIndex + Size];
            for (int i = 0; i < StartIndex; i++)
            {
                this.Str[i] = ' ';
            }
            for (int i = StartIndex; i < Size; i++)
            {
                this.Str[i] = Str[i];
            }
        }

        public MyString(string Str)
        {
            this.Str = new char[Str.Length];
            for(int i = 0; i < Str.Length; i++)
            {
                this.Str[i] = Str[i];
            }
        }

        public char this[int index]
        {
            get
            {
                return this.Str[index];
            }
            set
            {
                this.Str[index] = value;
            }
        }

        public static MyString operator +(MyString s1, MyString s2)
        {
            char[] NewStr = new char[s1.Length + s2.Length];
            int count = 0;
            foreach (var ch in (char[])s1)
            {
                NewStr[count++] = ch;
            }
            foreach(var ch in (char[])s2)
            {
                NewStr[count++] = ch;
            }

            return new MyString(NewStr);
        }

        public static bool operator true(MyString MyStr)
        {
            return MyStr.Str != null && MyStr.Length > 0;
        }

        public static bool operator false(MyString MyStr)
        {
            return MyStr.Str == null || MyStr.IsEmpty;
        }

        public static bool operator ==(MyString Left, MyString Right)
        {
            if(CompareLength(Left,Right) == 0)
            {
                for(int i = 0; i < Left.Length; i++)
                {
                   if(Left.Str[i] != Right.Str[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public static bool operator !=(MyString Left, MyString Right)
        {
            if(CompareLength(Left,Right) == 0)
            {
                for (int i = 0; i < Left.Length; i++)
                {
                    if (Left.Str[i] == Right.Str[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool operator >(MyString Left, MyString Right)
        {
            if (CompareLength(Left, Right) != 1)
            {
                for (int i = 0; i < Left.Length; i++)
                {
                    if (Left.Str[i] <= Right.Str[i])
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Right.Length; i++)
                {
                    if (Left.Str[i] <= Right.Str[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool operator <(MyString Left, MyString Right)
        {
            if (CompareLength(Left, Right) != -1)
            {
                for (int i = 0; i < Right.Length; i++)
                {
                    if (Left.Str[i] >= Right.Str[i])
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Left.Length; i++)
                {
                    if (Left.Str[i] >= Right.Str[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static int Compare(MyString Left, MyString Right) => (Left == Right ? 0 : Left > Right ? 1 : -1);

        public int CompareTo(MyString myString) => (this == myString ? 0 : this > myString ? 1 : -1);

        public int CompareTo(object value)
        {
            var myString = value as MyString;
            if (myString == null)
                throw new ArgumentNullException();
            return (this == myString ? 0 : this > myString ? 1 : -1);
        }

        private static int CompareLength(MyString Left, MyString Right) => (Left.Length == Right.Length ? 0 : Left.Length > Right.Length ? 1 : -1);

        public override int GetHashCode() => base.GetHashCode();

        public int Length => this.Str.Length;

        public bool IsEmpty => this.Length == 0;

        public static bool IsNullOrEmpty(MyString Str)
        {
            return Str == null || Str.IsEmpty;
        }

        public override bool Equals(object obj)
        {
            return (obj is MyString) ? this == (MyString)obj : false;
        }


        public bool Contains(MyString myString)
        {
            bool result = true;
            if (myString == null || myString.IsEmpty)
                result = false;
            if (result)
            {
                for (int i = 0, j = 0; i < this.Length && j < myString.Length; i++)
                {
                    if (j == myString.Length)
                        result = true;
                    else
                    {
                        if (myString.Str[j] != this.Str[i])
                            j = 0;
                        else
                            j++;
                    }
                }
            }
            return result;
        }

        public bool Contains(string myString)
        {
            bool result = true;
            if (myString == null || myString.Length == 0)
                result = false;
            if (result)
            {
                for (int i = 0, j = 0; i < this.Length && j < myString.Length; i++)
                {
                    if (j == myString.Length)
                        result = true;
                    else
                    {
                        if (myString[j] != this.Str[i])
                            j = 0;
                        else
                            j++;
                    }
                }
            }
            return result;
        }

        public int IndexOf(char c)
        {
            for (int i = 0; i < this.Length; i++)
            {
                if (this[i] == c)
                    return i;
            }
            return -1;
        }
        public int IndexOf(char c, int pos)
        {
            int len = this.Length;
            if(pos < len && pos >= 0)
            {
                for (int i = pos; i < len; i++)
                {
                    if(this.Str[i] == c)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        public int IndexOf(MyString myString)
        {
            int result = -1;
            for (int i = 0, j = 0; i < this.Length && j <= myString.Length; i++)
            {
                if (j == myString.Length)
                    result = i - j;
                else
                {
                    if (myString.Str[j] != this.Str[i])
                        j = 0;
                    else
                        j++;
                }
            }
            return result;
        }
        public int IndexOf(MyString myString, int pos)
        {
            int len = this.Length;
            int result = -1;
            if(pos < len && pos >= 0)
            {
                for (int i = pos, j = 0; i < len && j <= myString.Length; i++)
                {
                    if (j == myString.Length)
                        result = i - j;
                    else
                    {
                        if (myString.Str[j] != this.Str[i])
                            j = 0;
                        else
                            j++;
                    }
                }
            }
            return result;
        }

        public MyString SubString(int pos)
        {
            int len = this.Length;
            if(pos < len && pos >= 0)
            {
                char[] newstr = new char[len - pos];
                for (int i = pos, j = 0; i < len; i++, j++)
                {
                    newstr[j] = this.Str[i];
                }
                return new MyString(newstr);
            }
            return new MyString();
        }

        public MyString SubString(int StartIndex, int size)
        {
            int len = this.Length;
            if (StartIndex < len && StartIndex >= 0 && size >= 0)
            {
                if (len < size + StartIndex)
                    size = len;
                char[] newstr = new char[size];
                for (int i = StartIndex, j = 0; i < StartIndex + size; i++, j++)
                {
                    newstr[j] = this.Str[i];
                }
                return new MyString(newstr);
            }
            return new MyString();
        }

        public static MyString Join(string sep, MyString[] arr)
        {
            char[] result;
            int count = 0;
            char[] separator = new char[sep.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                count += arr[i].Length;
                if (i < arr.Length - 1)
                    count += separator.Length;
            }
            result = new char[count];
            int k = 0;
            foreach (var elem in arr)
            {
                foreach(char ch in elem)
                {
                    result[k++] = ch;
                }
                if (elem != arr[arr.Length - 1])
                {
                    foreach (char ch in separator)
                    {
                        result[k++] = ch;
                    }
                }
            }
            return new MyString(result);
        }

        public MyString Trim(char ch = ' ')
        {
            char[] result;
            int start = 0, finish = this.Length;
            if(this[0] == ch)
            {
                while (this[start] == ch)
                    start++;
            }
            else if(this[finish] == ch)
            {
                while (this[finish] == ch)
                    finish--;
            }
            if (start == 0 && finish == this.Length)
                return this;
            else
            {
                result = new char[finish - start + 1];
                for (int i = 0; start < finish; i++, start++)
                {
                    result[i] = this[start];
                }
            }
            return new MyString(result);
        }

        public static MyString Concat(MyString str1, MyString str2)
        {
            char[] result = new char[str1.Length + str2.Length];
            int count = 0;
            foreach (var ch in (char[])str1)
            {
                result[count++] = ch;
            }
            foreach (var ch in (char[])str2)
            {
                result[count++] = ch;
            }
            return new MyString(result);
        }

        public bool StartsWith(string Str)
        {
            if (Str.Length > this.Length)
                return false;
            for(int i = 0; i < Str.Length; i++)
            {
                if (this[i] != Str[i])
                    return false;
            }
            return true;
        }

        public bool StartsWith(MyString Str)
        {
            for (int i = 0; i < Str.Length; i++)
            {
                if (this[i] != Str[i])
                    return false;
            }
            return true;
        }

        public static implicit operator string(MyString Str)
        {
            return new string(Str.Str);
        }

        public static implicit operator char[](MyString Str)
        {
            return Str.Str;
        }

        /*
        public static implicit operator char(MyString Str)
        {
            if (Str.Length == 1)
                return Str.Str[0];
            return '\0';
        }
        */

        public static explicit operator int(MyString Str)
        {
            int result = 0;
            int exp = 1;
            for (int i = 0; i < Str.Length; i++)
            {
                if (Str.Str[i] >= '0' && Str.Str[i] <= '9')
                {     
                    result = result * exp + (int)Str.Str[i];
                    exp *= 10;
                }
            }
            return result;
        }

    }


    class MainClass
    {
        public static void Main(string[] args)
        {
            MyString Str1 = new MyString();

            MyString Str2 = new MyString('S', 7);

            MyString Str3 = new MyString(new char[] { 'H', 'e', 'l', 'l', 'o' });

            MyString Str4 = new MyString(new char[] { 'w', 'o', 'r', 'l', 'd'});

            MyString Str5 = new MyString(new char[] { 'N', 'e' });

            MyString Str6 = new MyString(new char[] { 'g', 'a', 't', 'i', 'v' },3,1);

            MyString Str7 = new MyString("Bez dna");


            MyString Str8 = Str3 + Str4;

            MyString StrA = new MyString("Lol");

            MyString StrB = new MyString("Lol");

            MyString StrC = new MyString("oLolo");

            MyString[] StrMass = new MyString[]
            {
                Str3,
                Str4,
                Str5,
                Str6
            };

            MyString StrForTrim = new MyString("  a a_a  ");

            if (Str1)
            {
                Console.WriteLine("Строка Str1 не пустая");
            }
            else
            {
                Console.WriteLine("Строка Str1 пустая или равна null");
            }

            if (StrA == StrB)
            {
                Console.WriteLine("StrA == StrB");
            }

            if (StrA != StrC)
            {
                Console.WriteLine("StrA != StrC");
            }

            if (Str3 > Str4)
                Console.WriteLine("Str3 > Str4");
            else
                Console.WriteLine("Str3 < Str4");

            Console.WriteLine("MyString.Compare(Stra,StrB): " + MyString.Compare(StrA, StrB));

            Console.WriteLine("MyString.Compare(Str3,Str4): " + MyString.Compare(Str3, Str4));

            Console.WriteLine("StrA.CompareTo(StrB): " + StrA.CompareTo(StrB));

            Console.WriteLine("Str3.CompareTo(Str4): " + Str3.CompareTo(Str4));

            //Console.WriteLine("MyString.CompareLength(Str5,Str6): " + MyString.CompareLength(Str5, Str6));

            //Console.WriteLine("Str2.Length: " + Str2.Length);

            //Console.WriteLine("MyString.IsNullOrEmpty(Str1): " +  MyString.IsNullOrEmpty(Str1));

            Console.WriteLine("Str5.Equals(new string()): " + Str5.Equals(new string('c',2)));

            Console.WriteLine("Str5.Equals(Str6): " + Str5.Equals(Str6));

            Console.WriteLine("StrA.Equals(StrB): " + StrA.Equals(StrB));

            Console.WriteLine("Str7.Contains(\"Bez\"): " + Str7.Contains("Bez"));

            Console.WriteLine("Str4.IndexOf('r'): " + Str4.IndexOf('r'));


            Console.WriteLine("Str4.IndexOf('r',3): " + Str4.IndexOf('r', 3));

            Console.WriteLine("StrC.IndexOf(StrB): " + StrC.IndexOf(StrB));

            Console.WriteLine("StrC.IndexOf(StrB,2): " + StrC.IndexOf(StrB,2));

            Console.WriteLine("Str3.SubString(2): " + Str3.SubString(2));

            Console.WriteLine("Str3.SubString(2,1): " + Str3.SubString(2, 1));

            Console.WriteLine("MyString.Join(\",\", StrMass\"): " + MyString.Join(",", StrMass));

            Console.WriteLine("StrForTrim.Trim(): " + StrForTrim.Trim());

            Console.WriteLine("MyString.Concat(Str3,Str4): " + MyString.Concat(Str3, Str4));

            Console.WriteLine("Str3.StartsWith(\"He\"): " + Str3.StartsWith("He"));

            Console.ReadKey();
        }
    }
}
