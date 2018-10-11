using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonClass;
namespace ClsLibTest
{
    [SrvAttribute("Test")]
   public interface ITest
    {
        void Test();
        int TestInt();

        [SrvAttribute("JINYU")]
        long TestString(string str);

        TestCls GetTestCls(string str);
    }
}
