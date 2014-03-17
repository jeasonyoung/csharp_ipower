//================================================================================
//  FileName: HexParser.cs
//  Desc:16进制转换。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2010/4/12
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
using System.Globalization;
namespace iPower.Utility
{
    /// <summary>
    /// 16进制转换。
    /// </summary>
    public static class HexParser
    {
        /// <summary>
        /// 将16进制转换为Byte数组。
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static byte[] Parse(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException("token");

            int index = 0, hexLen = token.Length;
            if ((hexLen >= 2) && (token[0] == '0') && (token[1] == 'x' || token[1] == 'X'))
            {
                hexLen -= 2;
                index = 2;
            }
            if ((hexLen % 2) != 0)
                throw new ArgumentNullException("token", "无效的16进制字符串格式");

            byte[] buffer = null;
            bool flag = false;
            if (hexLen >= 3 && token[index + 2] == ' ')
            {
                buffer = new byte[(hexLen / 3) + 1];
                flag = true;
            }
            else
                buffer = new byte[hexLen / 2];

            string strHex = string.Empty;
            for (int i = 0; i < buffer.Length; i++)
            {
                strHex = token.Substring(index + (flag ? 3 : 2) * i, i == buffer.Length - 1 ? 2 : (flag ? 3 : 2)).Trim();
                buffer[i] = byte.Parse(strHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            return buffer;
        }
        /// <summary>
        /// 将Byte数组转换为16进制字符串。
        /// </summary>
        /// <param name="tokenBytes"></param>
        /// <returns></returns>
        public static string ToHexString(byte[] tokenBytes)
        {
            if (tokenBytes == null)
                throw new ArgumentNullException("tokenBytes");
            StringBuilder builder = new StringBuilder(tokenBytes.Length * 2);
            for (int i = 0; i < tokenBytes.Length; i++)
                builder.Append(tokenBytes[i].ToString("x2", CultureInfo.InvariantCulture));
            return builder.ToString();
        }
    }
}
