using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
   public interface ITest
    {
        void Test();
        int TestInt();

        long TestString(string str);

        TestCls GetTestCls(string str);
    }
}
