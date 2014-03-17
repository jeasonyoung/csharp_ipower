//================================================================================
//  FileName: HashCrypto.cs
//  Desc:关于Hash的一些实用方法。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-4
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
using System.Security.Cryptography;
using System.IO;
using iPower.Utility;
namespace iPower.Cryptography
{
    /// <summary>
    /// 关于Hash的一些实用方法。
    /// </summary>
    public static class HashCrypto
    {
        /// <summary>
        /// Hash算法。
        /// </summary>
        /// <param name="input">被Hash的字节数组。</param>
        /// <param name="hashFormat">Hash算法："md5"、"sha1"。</param>
        /// <returns>Hash结果字节数组。</returns>
        /// <remarks>
        /// 当参数<paramref name="hashFormat">不为"md5"、"sha1"时，返回<c>null</c>。</paramref>
        /// </remarks>
        public static byte[] Hash(byte[] input, string hashFormat)
        {
            HashAlgorithm algorithm = null;
            if (string.Compare(hashFormat, "sha1", true) == 0)
            {
                algorithm = SHA1.Create();
            }
            else if (string.Compare(hashFormat, "md5", true) == 0)
            {
                algorithm = MD5.Create();
            }
            byte[] result = null;
            if (algorithm != null)
            {
                result = algorithm.ComputeHash(input);
            }
            return result;
        }
        /// <summary>
        /// Hash算法。
        /// </summary>
        /// <param name="input">被Hash的字节流。</param>
        /// <param name="hashFormat">Hash算法："md5"、"sha1"。</param>
        /// <returns>Hash结果字节数组。</returns>
        /// <remarks>
        /// 当参数<paramref name="hashFormat">不为"md5"、"sha1"时，返回<c>null</c>。</paramref>
        /// </remarks>
        public static byte[] Hash(Stream input, string hashFormat)
        {
            HashAlgorithm algorithm = null;
            if (string.Compare(hashFormat, "sha1", true) == 0)
            {
                algorithm = SHA1.Create();
            }
            else if (string.Compare(hashFormat, "md5", true) == 0)
            {
                algorithm = MD5.Create();
            }
            byte[] result = null;
            if (algorithm != null)
            {
                result = algorithm.ComputeHash(input);
            }
            return result;
        }
        /// <summary>
        /// Hash算法。
        /// </summary>
        /// <param name="data">源数据。</param>
        /// <param name="hashFormat">Hash算法："md5"、"sha1"。</param>
        /// <returns>Hash数据。</returns>
        public static string Hash(string data, string hashFormat)
        {
            if (!string.IsNullOrEmpty(data))
            {
                byte[] input = Encoding.UTF8.GetBytes(data);
                if (input != null)
                {
                    byte[] output = Hash(input, hashFormat);
                    if (output != null)
                    {
                        return HexParser.ToHexString(output);
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Hash文件。
        /// </summary>
        /// <param name="fileName">被Hash的文件（包括路径）。</param>
        /// <param name="hashFormat">Hash算法："md5"、"sha1"。</param>
        /// <returns>Hash结果字符串。</returns>
        /// <remarks>
        /// 当参数<paramref name="hashFormat">不为"md5"、"sha1"时，返回<c>null</c>。</paramref>
        /// </remarks>
        public static string HashFile(string fileName, string hashFormat)
        {
            byte[] hashBytes = HashFileReturnRawData(fileName, hashFormat);
            if (hashBytes == null)
            {
                return null;
            }
            else
            {
                return HexParser.ToHexString(hashBytes);
            }
        }
        /// <summary>
        /// Hash数据流。
        /// </summary>
        /// <param name="fileStream">数据流。</param>
        /// <param name="hashFormat">Hash算法："md5"、"sha1"。</param>
        /// <returns>Hash结果字符串。</returns>
        /// <remarks>
        /// 当参数<paramref name="hashFormat">不为"md5"、"sha1"时，返回<c>null</c>。</paramref>
        /// </remarks>
        public static string HashFile(Stream fileStream, string hashFormat)
        {
            if (fileStream != null)
            {
                byte[] buf = Hash(fileStream, hashFormat);
                if (buf != null)
                {
                    return HexParser.ToHexString(buf);
                }
            }
            return null;
        }

        /// <summary>
        /// Hash文件。
        /// </summary>
        /// <param name="fileName">被Hash的文件（包括路径）。</param>
        /// <param name="hashFormat">Hash算法："md5"、"sha1"。</param>
        /// <returns>Hash结果。</returns>
        /// <remarks>
        /// 当参数<paramref name="hashFormat">不为"md5"、"sha1"时，返回<c>null</c>。</paramref>
        /// </remarks>
        public static byte[] HashFileReturnRawData(string fileName, string hashFormat)
        {
            using (FileStream fs = File.OpenRead(fileName))
            {
                return Hash(fs, hashFormat);
            }
        }
    }
}
