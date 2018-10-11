using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

/**
* 命名空间: ConsoleApp3 
* 类 名： TestInvoke
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace ConsoleApp3
{
    /// <summary>
    /// 功能描述    ：TestInvoke  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/10 2:29:52 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/10 2:29:52 
    /// </summary>
    public class TestInvoke : ITest
    {
        public TestCls GetTest(string str)
        {
            SrvRquest rquest = new SrvRquest();
            rquest.SrvMethod = "GetTest";
            rquest.SrvParam = new List<SrvParam>();
            SrvParam srvParam = new SrvParam();
            srvParam.ParamName = "str";
            srvParam.ParamObj = str;
            rquest.SrvParam.Add(srvParam);
            return null;

        }

        public void Test()
        {
            throw new NotImplementedException();
        }

        public string TestCls(TestCls cls)
        {
            throw new NotImplementedException();
        }

        public int TestInt()
        {
            throw new NotImplementedException();
        }

        public string TestString(string str)
        {
            throw new NotImplementedException();
        }
    }
}
