//================================================================================
//  FileName: GUIDEx.cs
//  Desc:扩展的Guid数据类型。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-10-29
//================================================================================
//  Change History
//================================================================================
//  Date  Author  Description
//  ----    ------  -----------------
//
//================================================================================
//  Copyright (C) 2004-2009 Jeason Young Corporation
//================================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
namespace iPower
{
    /// <summary>
    /// 扩展的Guid数据类型。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct GUIDEx : IComparable, IFormattable, IComparable<GUIDEx>, IEquatable<GUIDEx>, IXmlSerializable
    {
        #region 成员变量、构造函数。
        string strValue;
        static Regex regex, regex2;
        static GUIDEx gEmpty, gNull;
       
        /// <summary>
        /// 静态构造函数。
        /// </summary>
        static GUIDEx()
        {
            regex = new Regex("^[a-fA-F0-9]{32}$", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            regex2 = new Regex("^[a-zA-Z_0-9]{1,32}$", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            gEmpty = new GUIDEx(Guid.Empty);
            gNull = new GUIDEx(null);
        }

        /// <summary>
        /// 构造函数。
        /// <param name="value">值字符串。</param>
        /// </summary>
        public GUIDEx(string value)
        {
            this.strValue = value;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="value">值对象。</param>
        public GUIDEx(object value)
            : this(value == null ? null : value.ToString())
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="id">值Guid。</param>
        public GUIDEx(Guid id)
        {
            this.strValue = id.ToString("N");
        }

        /// <summary>
        /// 私有序列化构造函数。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        GUIDEx(SerializationInfo info, StreamingContext context)
        {
            this.strValue = info.GetString("value");
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取值。
        /// </summary>
        [XmlIgnore]
        public string Value
        {
            get { return this.strValue; }
        }

        /// <summary>
        /// 获取是否为Guid。
        /// </summary>
        [XmlIgnore]
        public bool IsGuid
        {
            get { return regex.IsMatch(this.Value); }
        }

        /// <summary>
        /// 获取是否为GUIDEx。
        /// </summary>
        [XmlIgnore]
        public bool IsGUIDEx
        {
            get { return regex2.IsMatch(this.Value); }
        }

        /// <summary>
        /// 获取是否为Null。
        /// </summary>
        [XmlIgnore]
        public bool IsNull
        {
            get { return string.IsNullOrEmpty(this.strValue); }
        }

        /// <summary>
        /// 获取是否正确的GUIDEx数据类型。
        /// </summary>
        [XmlIgnore]
        public bool IsValid
        {
            get { return !string.IsNullOrEmpty(this.Value); }
        }

        /// <summary>
        /// 获取空的GUIDEx。
        /// </summary>
        [XmlIgnore]
        public static GUIDEx Empty
        {
            get { return gEmpty; }
        }

        /// <summary>
        /// 获取新的GUIDEx。
        /// </summary>
        [XmlIgnore]
        public static GUIDEx New
        {
            get { return new GUIDEx(Guid.NewGuid()); }
        }

        /// <summary>
        /// 获取为Null的GUIDEx。
        /// </summary>
        [XmlIgnore]
        public static GUIDEx Null
        {
            get { return gNull; }
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return this.Value;
        }

        /// <summary>
        /// 比较。 
        /// </summary>
        /// <param name="obj">GUIDEx对象。</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this.CompareTo(obj) == 0;
        }

        /// <summary>
        /// 获取HashCode。 
        /// </summary>
        /// <returns>返回int<see cref="int"/>。</returns>
        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(this.Value))
                return -1;
            return this.Value.GetHashCode();
        }
        #endregion

        #region 操作符重载。
        /// <summary>
        /// 比较运算符。
        /// </summary>
        /// <param name="a">GUIDEx<see cref="GUIDEx"/>对象。</param>
        /// <param name="b">GUIDEx<see cref="GUIDEx"/>对象。</param>
        /// <returns>值一致返回true，否则返回false。</returns>
        public static bool operator ==(GUIDEx a, GUIDEx b)
        {
            return a.CompareTo(b) == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a">GUIDEx<see cref="GUIDEx"/>对象。</param>
        /// <param name="b">GUIDEx<see cref="GUIDEx"/>对象。</param>
        /// <returns>值不一致返回true，否则返回false。</returns>
        public static bool operator !=(GUIDEx a, GUIDEx b)
        {
            return a.CompareTo(b) != 0;
        }
        #endregion

        #region 隐式转换
        /// <summary>
        /// 隐私转换为Guid。
        /// </summary>
        /// <param name="a">GUIDEx<see cref="GUIDEx"/>对象。</param>
        /// <returns>返回Guid<see cref=" Guid"/>。</returns>
        public static implicit operator Guid(GUIDEx a)
        {
            return new Guid(a.Value);
        }
        /// <summary>
        /// 隐私转换为string.
        /// </summary>
        /// <param name="a">GUIDEx<see cref="GUIDEx"/>对象。</param>
        /// <returns>返回String<see cref="string"/>。</returns>
        public static implicit operator string(GUIDEx a)
        {
            return a.Value;
        }
        /// <summary>
        /// 隐式转换为GUIDEx。
        /// </summary>
        /// <param name="a">string<see cref="string"/>对象。</param>
        /// <returns>返回GUIDEx<see cref="GUIDEx"/>对象。</returns>
        public static implicit operator GUIDEx(string a)
        {
            if (!string.IsNullOrEmpty(a) && a.Length > 32)
                a = a.Substring(0, 32);
            return new GUIDEx(a);
        }
        #endregion

        #region IComparable 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if ((obj != null) && (obj.GetType() != typeof(GUIDEx)))
                throw new ArgumentException("对象不是GUIDEx类型。");
            GUIDEx gx = (GUIDEx)obj;
            return string.Compare(this.Value, gx.Value, true);
        }

        #endregion

        #region IFormattable 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(this.Value))
                return null;
            return this.Value.ToString(formatProvider);
        }

        #endregion

        #region IComparable<GUIDEx> 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(GUIDEx other)
        {
            return string.Compare(this.Value, other.Value, true);
        }

        #endregion

        #region IEquatable<GUIDEx> 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(GUIDEx other)
        {
            return this.CompareTo(other) == 0;
        }

        #endregion

        #region IXmlSerializable 成员
        
        /// <summary>
        /// 此方法是保留方法，请不要使用。
        /// </summary>
        /// <returns></returns>
        public XmlSchema GetSchema()
        {
            return null;
        }
        /// <summary>
        /// 从对象的 XML 表示形式生成该对象。
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            this.strValue = reader.ToString();
        }
        /// <summary>
        /// 将对象转换为其 XML 表示形式。
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(this.Value);
        }

        #endregion
    }
}
