using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

/**
* 命名空间: CommonClass 
* 类 名： CommonExtend
* 版本 ：v1.0
* Copyright (c) year 
*/

namespace CommonClass
{
    /// <summary>
    /// 功能描述    ：CommonExtend  提供扩展方法的类
    /// 创 建 者    ：jinyu
    /// 创建日期    ：2018/10/11 19:38:03 
    /// 最后修改者  ：jinyu
    /// 最后修改日期：2018/10/11 19:38:03 
    /// </summary>
   public static class CommonExtend
    {
        /// <summary>
        /// 枚举描述
        /// </summary>
        private static Dictionary<string, string> dicEnum = new Dictionary<string, string>();
        /// <summary>
        /// 枚举描述特性获取信息
        /// </summary>
        /// <param name="value">枚举</param>
        /// <param name="isNameInstend">没有特性时是否直接使用字段名称</param>
        /// <returns></returns>
        public static string EnumDescription(this Enum value,bool isNameInstend=false)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }
            string description = "";
            if (dicEnum.TryGetValue(type.FullName+"_"+name,out description))
            {
                return description;
            }
            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null && isNameInstend == true)
            {
                return name;
            }
            //
            if(attribute!=null)
            {
                dicEnum[type.FullName + "_" + name] = attribute.Description;
            }
            return attribute == null ? null : attribute.Description;
        }
    }
}
