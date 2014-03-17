//================================================================================
//  FileName: ConvertEx.cs
//  Desc: 类型转换扩展集合。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-10-27
//================================================================================
//  Change History
//================================================================================
//  Date  Author  Description
//  ----    ------  -----------------
// 2009-11-01   yangyong    添加字符串处理部分。
//================================================================================
//  Copyright (C) 2004-2009 Jeason Young Corporation
//================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
namespace iPower.Utility
{
    /// <summary>
    /// 类型转换扩展集合。
    /// </summary>
    public static class ConvertEx
    {
        #region StringCollection To Arrary
        /// <summary>
        /// 字符串集合转换成字符串数组。
        /// </summary>
        /// <param name="collection">需要转化为字符串数组的字符串集合。</param>
        /// <returns>转化后的字符串数组，如果传入集合为null则返回0长度的字符数组。</returns>
        public static string[] ToStringArray(StringCollection collection)
        {
            if (collection == null)
            {
                return new string[0];
            }
            string[] array = new string[collection.Count];
            collection.CopyTo(array, 0);
            return array;
        }
        /// <summary>
        /// 字符串数组转换成字符串集合。 
        /// </summary>
        /// <param name="value">需要转化为字符串集合的数组。</param>
        /// <returns>转化后的字符串集合，如果传入数组为null则返回空集合。</returns>
        public static StringCollection ToStringCollection(params string[] value)
        {
            StringCollection strings = new StringCollection();
            if (value != null)
            {
                strings.AddRange(value);
            }
            return strings;
        }
        #endregion

        #region 日期
        /// <summary>
        /// 日期转化为yyyyMMddHHmmss格式的字符串。 
        /// </summary>
        /// <param name="date">需要转化的日期。</param>
        /// <returns>转化后的字符串。</returns>
        public static string ToStringEx(DateTime date)
        {
            return date.ToString("yyyyMMddHHmmss");
        }
        /// <summary>
        /// 将日期转换为yyyy年MM月dd日格式。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToShortDateCN(DateTime dt)
        {
            return dt.ToString("yyyy年MM月dd日", CultureInfo.CurrentCulture); 
        }
        /// <summary>
        /// 将日期转换为yyyy年MM月dd日HH时mm分ss秒格式。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToLongDateCN(DateTime dt)
        {
            return dt.ToString("yyyy年MM月dd日HH时mm分ss秒", CultureInfo.CurrentCulture);
        }
        #endregion

        #region NameValueCollection To Xml
        /// <summary>
        /// 将经过Base64编码的，携带名称值信息的Xml文档转变为名称值集合。
        /// </summary>
        /// <param name="strBase64xml">携带名称值信息的Xml文档的Base64编码。</param>
        /// <returns>转化后的名称值对信息。</returns>
        public static NameValueCollection Base64StringToNameValueCollection(string strBase64xml)
        {
            if (string.IsNullOrEmpty(strBase64xml))
            {
                return new NameValueCollection();
            }
            try
            {
                NameValueCollection values = new NameValueCollection();
                string xml = Encoding.Unicode.GetString(Convert.FromBase64String(strBase64xml));
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                XmlNode documentElement = document.DocumentElement;
                if (documentElement.Name.Equals("items"))
                {
                    foreach (XmlNode node2 in documentElement.ChildNodes)
                    {
                        if ((node2.Name.ToUpper() == "I") && ((node2.Attributes["k"] != null) && (node2.Attributes["v"] != null)))
                        {
                            values.Add(node2.Attributes["k"].Value, node2.Attributes["v"].Value);
                        }
                    }
                }
                return values;
            }
            catch
            {
                return new NameValueCollection();
            }
        }
        /// <summary>
        /// 将名称值对转化为Xml文档并进行Base64的编码。
        /// </summary>
        /// <param name="values">需要进行转换的名称值对集合。</param>
        /// <returns>转化为Xml文档并且进行Base64编码的字符串。</returns>
        public static string NameValueCollectionToBase64String(NameValueCollection values)
        {
            if ((values == null) || (values.Count == 0))
            {
                return string.Empty;
            }
            try
            {
                XmlDocument document = new XmlDocument();
                XmlNode node2 = document.CreateElement("items");
                foreach (string str in values.Keys)
                {
                    XmlNode newChild = document.CreateNode(XmlNodeType.Element, "i", document.NamespaceURI);
                    XmlAttribute node = document.CreateAttribute("k");
                    node.Value = str;
                    newChild.Attributes.Append(node);
                    node = document.CreateAttribute("v");
                    node.Value = values[str];
                    newChild.Attributes.Append(node);
                    node2.AppendChild(newChild);
                }
                string outerXml = node2.OuterXml;
                return Convert.ToBase64String(Encoding.Unicode.GetBytes(outerXml));
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region 字符串处理。
        /// <summary>
        /// 把指定的字符串按照需要的字节长度截取，并在结尾附加指定个数的半个省略号的数量。
        /// </summary>
        /// <param name="inputString">需要截取的字符串。</param>
        /// <param name="len">保留的长度（字节）。</param>
        /// <param name="num">需要在结尾附加半个省略号的数量。</param>
        /// <returns>截取后的字符串。</returns>
        public static string CutString(string inputString, int len, int num)
        {
            if (!string.IsNullOrEmpty(inputString) && (inputString.Length > len))
            {
                inputString = inputString.Substring(0, len) + new string('.', num * 3);
            }
            return inputString;
        }
        /// <summary>
        /// 将指定字符，按照串联的方式重复一定次数。 
        /// </summary>
        /// <param name="c">要重复的字符。</param>
        /// <param name="count">重复的次数。</param>
        /// <returns>字符c重复count次后的串联字符串。</returns>
        public static string ReplicateString(char c, int count)
        {
            return new string(c, count);
        }
        /// <summary>
        /// 将指定字符串，按照串联的方式重复一定次数。 
        /// </summary>
        /// <param name="value">要重复的字符串。</param>
        /// <param name="count">重复的次数。</param>
        /// <returns>字符串value重复count次后的串联字符串。</returns>
        public static string ReplicateString(string value, int count)
        {
            return string.Join(value, new string[count + 1]);
        }
        #endregion

        /// <summary>
        /// 获取文件后缀。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <returns>后缀名。</returns>
        public static string GetExtension(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
                return Path.GetExtension(fileName);
            return string.Empty;
        }

        //#region Hex Util

        //static readonly char[] hexValues = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        //static int ConvertHexDigit(char val)
        //{
        //    if (val <= '9')
        //    {
        //        return (val - '0');
        //    }
        //    if (val >= 'a')
        //    {
        //        return ((val - 'a') + '\n');
        //    }
        //    return ((val - 'A') + '\n');
        //}

        ///// <summary>
        ///// 把字节数组转换成16进制表示的字符串
        ///// </summary>
        ///// <param name="sArray">字节数组</param>
        ///// <returns>16进制表示的字符串</returns>
        //public static string ToHexString(byte[] sArray)
        //{
        //    if (sArray == null)
        //    {
        //        return null;
        //    }
        //    char[] chArray = new char[sArray.Length * 2];
        //    int i = 0;
        //    int k = 0;
        //    while (i < sArray.Length)
        //    {
        //        int t = (sArray[i] & 240) >> 4;
        //        chArray[k++] = hexValues[t];
        //        t = sArray[i] & 15;
        //        chArray[k++] = hexValues[t];
        //        i++;
        //    }
        //    return new string(chArray);
        //}

        ///// <summary>
        ///// 把16进制表示的字符串转换成字节数组
        ///// </summary>
        ///// <param name="hexString">进制表示的字符串</param>
        ///// <returns>字节数组</returns>
        //public static byte[] FromHexString(string hexString)
        //{
        //    byte[] buffer;
        //    if (hexString == null)
        //    {
        //        throw new ArgumentNullException("hexString");
        //    }
        //    bool flag = false;
        //    int i = 0;
        //    int hexLength = hexString.Length;
        //    if (((hexLength >= 2) && (hexString[0] == '0')) && ((hexString[1] == 'x') || (hexString[1] == 'X')))
        //    {
        //        hexLength = hexString.Length - 2;
        //        i = 2;
        //    }
        //    if (((hexLength % 2) != 0) && ((hexLength % 3) != 2))
        //    {
        //        throw new ArgumentException("无效的16进制字符串格式");
        //    }
        //    if ((hexLength >= 3) && (hexString[i + 2] == ' '))
        //    {
        //        flag = true;
        //        buffer = new byte[(hexLength / 3) + 1];
        //    }
        //    else
        //    {
        //        buffer = new byte[hexLength / 2];
        //    }
        //    for (int k = 0; i < hexString.Length; k++)
        //    {
        //        int h = ConvertHexDigit(hexString[i]);
        //        int l = ConvertHexDigit(hexString[i + 1]);
        //        buffer[k] = (byte)(l | (h << 4));
        //        if (flag)
        //        {
        //            i++;
        //        }
        //        i += 2;
        //    }
        //    return buffer;
        //}

        //#endregion Hex Util
    }
}
