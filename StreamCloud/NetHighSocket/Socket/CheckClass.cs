using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
* 命名空间: NStStreamCloud 
* 类 名： Class1
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace NetHighSocket
{
    /// <summary>
    /// 功能描述    ：Class1  
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/8 5:51:37 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/8 5:51:37 
    /// </summary>
   public static class CheckClass
    {
        public static bool CheckInterface(this Type type,Type generic)
        {
            var implementedInterfaces = type.GetInterfaces();
            foreach (var interfaceType in implementedInterfaces)
            {
                if (false == interfaceType.IsGenericType) { continue; }
                var genericType = interfaceType.GetGenericTypeDefinition();
                if (genericType == generic)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断接口
        /// </summary>
        /// <param name="type"></param>
        /// <param name="generic"></param>
        /// <returns></returns>
        public static bool HasImplementedRawGeneric(this Type type, Type generic)
        {
            // 遍历类型实现的所有接口，判断是否存在某个接口是泛型，且是参数中指定的原始泛型的实例。
            return type.GetInterfaces().Any(x => generic == (x.IsGenericType ? x.GetGenericTypeDefinition() : x));
        }

        /// <summary>
        /// 判断类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="generic"></param>
        /// <returns></returns>
        public static bool IsSubClassOfRawGeneric(this Type type,Type generic)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (generic == null) throw new ArgumentNullException(nameof(generic));

            while (type != null && type != typeof(object))
            {
              var  isTheRawGenericType = IsTheRawGenericType(type);
                if (isTheRawGenericType) return true;
                type = type.BaseType;
            }

            return false;

            bool IsTheRawGenericType(Type test)
                => generic == (test.IsGenericType ? test.GetGenericTypeDefinition() : test);
        }

        /// <summary>
        /// 接口和类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="generic"></param>
        /// <returns></returns>
        public static bool IsImplementedRawGeneric(this Type type,Type generic)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (generic == null) throw new ArgumentNullException(nameof(generic));

            // 测试接口。
            var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
            if (isTheRawGenericType) return true;

            // 测试类型。
            while (type != null && type != typeof(object))
            {
                isTheRawGenericType = IsTheRawGenericType(type);
                if (isTheRawGenericType) return true;
                type = type.BaseType;
            }

            // 没有找到任何匹配的接口或类型。
            return false;

            // 测试某个类型是否是指定的原始接口。
            bool IsTheRawGenericType(Type test)
                => generic == (test.IsGenericType ? test.GetGenericTypeDefinition() : test);
        } 
    }
}
