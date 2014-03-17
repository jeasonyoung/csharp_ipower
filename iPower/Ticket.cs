//================================================================================
//  FileName: Ticket.cs
//  Desc:票据类。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2010-1-25
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

using System.IO;
using System.IO.Compression;

using iPower.Utility;
using iPower.Cryptography;
namespace iPower
{
    /// <summary>
    /// 票据类。
    /// </summary>
    [Serializable]
    public class Ticket
    {
        #region 成员变量，构造函数。
        double ver;
        string al, userData;
        string token;
        DateTime issueDate,expiration;

        static Regex ticketRegex = new Regex(@"^VER=(?<Ver>[0-9]+(\.[0-9]+)?),AL=(?<AL>[A-Za-z]+([1|5]{1})),TOKEN=(?<Token>[0-9|A-F]+),USERDATD=(?<userData>([0-9|A-F]+)?),ISSUEDATE=(?<IssueDate>[0-9|A-F]+),EXPIRED=(?<Expired>[0-9|A-F]+),SIGN=(?<Sign>[0-9|A-F]+)$", 
                            RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// 构造函数。
        /// </summary>
        public Ticket()
            : this(1.0, "sha1", GUIDEx.New, DateTime.Now, DateTime.MaxValue, string.Empty)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="ticket">票据串。</param>
        public Ticket(string ticket)
            : this()
        {
            Ticket obj = Ticket.CreateTicket(ticket);
            if (obj != null)
            {
                this.ver = obj.ver;
                this.al = obj.al;

                this.token = obj.token;

                this.issueDate = obj.issueDate;
                this.expiration = obj.expiration;

                this.userData = obj.userData;
            }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="token">令牌串。</param>
        /// <param name="timeout">过期时间（秒）。</param>
        public Ticket(string token, int timeout)
            : this()
        {
            this.token = token;
            this.expiration = DateTime.Now.AddSeconds((double)timeout);
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="version">版本。</param>
        /// <param name="al">算法标识(md5,sha1)。</param>
        /// <param name="token">令牌。</param>
        /// <param name="issueDate">票据发布日期时间。</param>
        /// <param name="expiration">票据过期日期时间。</param>
        /// <param name="userData">用户数据。</param>
        public Ticket(double version, string al, string token, DateTime issueDate, DateTime expiration, string userData)
        {
            this.ver = version;
            this.al = al;
            this.token = token;
            this.issueDate = issueDate;
            this.expiration = expiration;
            this.userData = userData;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置版本号。
        /// </summary>
        public virtual double Ver
        {
            get { return this.ver; }
            set { this.ver = value; }
        }
        /// <summary>
        /// 获取或设置算法标识。
        /// </summary>
        public virtual string AL
        {
            get { return this.al; }
            set { this.al = value; }
        }
        /// <summary>
        /// 获取或设置令牌。
        /// </summary>
        public virtual string Token
        {
            get { return this.token; }
            set { this.token = value; }
        }
        /// <summary>
        /// 获取或设置用户数据。
        /// </summary>
        public virtual string UserData
        {
            get { return this.userData; }
            set { this.userData = value; }
        }
        /// <summary>
        /// 获取或设置最初发出验证票证时的本地日期和时间。
        /// </summary>
        public DateTime IssueDate
        {
            get { return this.issueDate; }
            set { this.issueDate = value; }
        }
        /// <summary>
        /// 获取或设置验证票证过期时的本地日期和时间。
        /// </summary>
        public DateTime Expiration
        {
            get { return this.expiration; }
            set { this.expiration = value; }
        }
        /// <summary>
        /// 获取散列值。
        /// </summary>
        public string HexSign
        {
            get
            {
                byte[] src = this.HashCrytoSign(string.Format("{0}-{1}-{2}-{3}-{4:yyyyMMddHHmmss}-{5:yyyyMMddHHmmss}", 
                    this.ver, 
                    this.al, 
                    this.token, 
                    this.userData, 
                    this.issueDate,
                    this.expiration));
                if (src != null)
                {
                    return HexParser.ToHexString(src);
                }
                return null;
            }
        }
        /// <summary>
        /// 判断票据是否有效。
        /// </summary>
        /// <returns></returns>
        public bool HasValid
        {
            get
            {
                return this.expiration > DateTime.Now;
            }
        }
        #endregion

        #region 辅助函数。
        byte[] HashCrytoSign(string input)
        {
            byte[] result = null;
            if (!string.IsNullOrEmpty(input))
            {
                byte[] b = this.StringToByteArray(input);
                result = HashCrypto.Hash(b, this.AL);
            }
            return result;
        }

        byte[] StringToByteArray(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            return Encoding.Default.GetBytes(str);
        }
        string StringToHex(string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;
            byte[] b = this.StringToByteArray(s);
            return HexParser.ToHexString(b);
        }
        string ByteArrayToString(byte[] byt)
        {
            if (byt == null)
                return null;
            return Encoding.Default.GetString(byt);
        }
        static string HexToString(string hexStr)
        {
            if (string.IsNullOrEmpty(hexStr))
                return null;
            byte[] b = HexParser.Parse(hexStr);
            return Encoding.Default.GetString(b);
        }
        #endregion
        
        #region 函数。
        /// <summary>
        /// 测试字符串是否为票据。
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static bool TestTicket(string ticket)
        {
            return (CreateTicket(ticket) is Ticket);
        }
        /// <summary>
        /// 测试字符串是否满足Ticket格式。
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static bool TestTicketFormat(string ticket)
        {
            return Ticket.ticketRegex.IsMatch(ticket);
        }
        /// <summary>
        /// 测试十六进制字符串是否为票据。
        /// </summary>
        /// <param name="hexTicket"></param>
        /// <returns></returns>
        public static bool TestTicketHex(string hexTicket)
        {
            return (CreateTicketByHex(hexTicket) is Ticket);
        }
        /// <summary>
        /// 根据票据字符串创建票据对象。
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static Ticket CreateTicket(string ticket)
        {
            Ticket ticketObj = null;
            if (Ticket.TestTicketFormat(ticket))
            {
                Match m = Ticket.ticketRegex.Match(ticket);
                if (m.Success)
                {
                    ticketObj = new Ticket();
                    ticketObj.ver = double.Parse(m.Groups["Ver"].Value);
                    ticketObj.al = m.Groups["AL"].Value;
                    ticketObj.token = HexToString(m.Groups["Token"].Value);
                    ticketObj.userData = HexToString(m.Groups["userData"].Value);
                    ticketObj.issueDate = DateTime.Parse(HexToString(m.Groups["IssueDate"].Value));
                    ticketObj.expiration = DateTime.Parse(HexToString(m.Groups["Expired"].Value));

                    string strSign = m.Groups["Sign"].Value;
                    if (!string.Equals(strSign, ticketObj.HexSign))
                    {
                        throw new ArgumentException("该票据已被篡改！");
                    }
                }
            }
            else
                throw new ArgumentException("票据格式不正确！");
            return ticketObj;
        }
        /// <summary>
        /// 根据票据的十六进制字符串创建票据对象。
        /// </summary>
        /// <param name="hexTicket"></param>
        /// <returns></returns>
        public static Ticket CreateTicketByHex(string hexTicket)
        {
            if (string.IsNullOrEmpty(hexTicket))
                return null;
            string str = HexToString(hexTicket);
            return CreateTicket(str);
        }
        /// <summary>
        /// 票据的十六进制字符串。
        /// </summary>
        /// <returns></returns>
        public string ToHexString()
        {
            string str = this.ToString();
            return this.StringToHex(str);
        }
        /// <summary>
        /// 转化为压缩后的Base64数据格式。
        /// </summary>
        /// <returns></returns>
        public string ToCompressBase64String()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] source = Encoding.UTF8.GetBytes(this.ToString());
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress))
                {
                    zip.Write(source, 0, source.Length);
                    zip.Flush();
                    zip.Close();
                }

                byte[] result = ms.ToArray();
                ms.Flush();
                ms.Close();

                return Convert.ToBase64String(result, 0, result.Length);
            }
        }
        /// <summary>
        /// 将压缩后的Base64解压转换为票据串。
        /// </summary>
        /// <param name="compressBase64String"></param>
        /// <returns></returns>
        public static string DeCompressBase64StringToTicket(string compressBase64String)
        {
            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrEmpty(compressBase64String))
            {
                byte[] source = Convert.FromBase64String(compressBase64String);
                using (GZipStream zip = new GZipStream(new MemoryStream(source), CompressionMode.Decompress))
                {
                    byte[] buffer = new byte[1024];
                    int size = 0;
                    while ((size = zip.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        result.Append(Encoding.UTF8.GetString(buffer, 0, size));
                    }
                    zip.Flush();
                    zip.Close();
                }
            }
            return result.ToString();
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("VER={0},AL={1},TOKEN={2},USERDATD={3},ISSUEDATE={4},EXPIRED={5},SIGN={6}",
                 this.ver, 
                 this.al, 
                 this.StringToHex(this.token), 
                 this.StringToHex(this.userData), 
                 this.StringToHex(string.Format("{0:yyyy-MM-dd HH:mm:ss}",this.issueDate)),
                 this.StringToHex(string.Format("{0:yyyy-MM-dd HH:mm:ss}",this.expiration)),
                 this.HexSign);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            Ticket ticket = obj as Ticket;
            if (ticket != null)
            {
                return string.Equals(this.HexSign, ticket.HexSign);
            }
            return base.Equals(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

    }
}
