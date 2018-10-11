using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp3
{
  public  interface ITest
    {
        void Test();
        int TestInt();
        string TestString(string str);

        string TestCls(TestCls cls);

        TestCls GetTest(string str);
    }
}
