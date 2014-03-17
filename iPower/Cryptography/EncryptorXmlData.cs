//================================================================================
//  FileName: EncryptorXmlData.cs
//  Desc:加密Xml数据。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-5
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

using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

namespace iPower.Cryptography
{
    /// <summary>
    /// 加密Xml数据。
    /// </summary>
    public static class EncryptorXmlData
    {
        #region 成员变量，构造函数。
        static SymmetricAlgorithm keyAlgorithm;
        /// <summary>
        /// 静态构造函数。
        /// </summary>
        static EncryptorXmlData()
        {
            keyAlgorithm = (SymmetricAlgorithm)(new RijndaelManaged());
            keyAlgorithm.Key = new byte[] { 0x01, 0x02, 0x03, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x04, 0x05, 0x06 };
            keyAlgorithm.IV = new byte[] { 0x08, 0x09, 0x10, 0x01, 0x02, 0x03, 0x04, 0x05, 0x13, 0x14, 0x15, 0x16, 0x06, 0x07, 0x11, 0x12 };
        }
        #endregion

        /// <summary>
        /// 加密Xml元素
        /// </summary>
        /// <param name="doc">XmlDocument</param>
        /// <param name="elmentName">要加密的元素名称</param>
        /// <param name="isContent">加密元素内容为True,加密整个元素为False</param>
        public static void EncryptorXmlElement(XmlDocument doc, string elmentName, bool isContent)
        {
            if (doc == null)
                throw new ArgumentNullException("doc");

            EncryptedXml encXml = new EncryptedXml(doc);
            encXml.AddKeyNameMapping("session", keyAlgorithm);

            XmlElement encElement = null;
            EncryptedData encData = null;
            if (string.IsNullOrEmpty(elmentName))
            {
                encElement = doc.DocumentElement;
                encData = encXml.Encrypt(encElement, "session");
                EncryptedXml.ReplaceElement(encElement, encData, isContent);
            }
            else
            {
                XmlNodeList nodeList = doc.GetElementsByTagName(elmentName);
                if (nodeList != null && nodeList.Count > 0)
                {
                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        encElement = nodeList[i] as XmlElement;
                        if (encElement != null)
                        {
                            encData = encXml.Encrypt(encElement, "session");
                            EncryptedXml.ReplaceElement(encElement, encData, isContent);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 加密Xml元素
        /// </summary>
        /// <param name="doc">XmlDocument</param>
        /// <param name="elmentName">要加密的元素名称</param>
        public static void EncryptorXmlElement(XmlDocument doc, string elmentName)
        {
            EncryptorXmlElement(doc, elmentName, false);
        }
        /// <summary>
        /// 加密Xml。
        /// </summary>
        /// <param name="doc"></param>
        public static void EncryptorXml(XmlDocument doc)
        {
            EncryptorXmlElement(doc, null, false);
        }
        /// <summary>
        /// 解密Xml元素
        /// </summary>
        /// <param name="doc">带密文的XmlDocument文档</param>
        public static void DecryptorXmlElement(XmlDocument doc)
        {
            if (doc != null)
            {
                EncryptedXml encXml = new EncryptedXml(doc);
                encXml.AddKeyNameMapping("session", keyAlgorithm);
                encXml.DecryptDocument();
            }
        }
        /// <summary>
        /// 解密Xml。
        /// </summary>
        /// <param name="doc">带密文的XmlDocument文档</param>
        public static void DecryptorXml(XmlDocument doc)
        {
            DecryptorXmlElement(doc);
        }
    }
}
