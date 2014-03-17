//================================================================================
//  FileName: FileStorage.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/22
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
using System.IO;
using System.IO.Compression;
using System.Data;
using System.Data.SqlClient;

using iPower;
using iPower.Utility;
using iPower.Cryptography;
namespace iPower.FileStorage.SQLServer
{
    /// <summary>
    /// 数据存储。
    /// </summary>
    internal class FileDataStorage
    {
        #region 成员变量，构造函数。
        SqlConnection connection = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="config"></param>
        public FileDataStorage(IStorageConfig config)
        {
            this.connection = new SqlConnection(config.StorageSource);
        }
        #endregion

        /// <summary>
        /// 上传文件。
        /// </summary>
        /// <param name="fileID">文件ID。</param>
        /// <param name="fileName">文件名称。</param>
        /// <param name="contentType">MIME内容类型。</param>
        /// <param name="content">文件内容。</param>
        /// <returns>成功返回True，失败False。</returns>
        public bool Upload(GUIDEx fileID, string fileName,string contentType, byte[] content)
        {
            bool result = false;
            try
            {
                if (fileID.IsValid && content != null)
                {
                    string strCheckCode = HexParser.ToHexString(HashCrypto.Hash(content, "md5"));
                    long len = content.Length;
                    float size = (float)(len / (float)1024 / (float)1024);
                    const string sql = "spPlatformFileStorage";// @FileID,@FileName,@FileSize,@Suffix,@CheckCode,@FileContent
                    SqlCommand cmd = new SqlCommand(sql, this.connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@FileID", SqlDbType.NVarChar);
                    cmd.Parameters[0].Value = fileID.Value;

                    cmd.Parameters.Add("@FileName", SqlDbType.NVarChar);
                    cmd.Parameters[1].Value = fileName;

                    cmd.Parameters.Add("@ContentType", SqlDbType.NVarChar);
                    cmd.Parameters[2].Value = contentType;

                    cmd.Parameters.Add("@FileSize", SqlDbType.Float);
                    cmd.Parameters[3].Value = size;

                    cmd.Parameters.Add("@Suffix", SqlDbType.NVarChar);
                    cmd.Parameters[4].Value = string.IsNullOrEmpty(fileName) ? string.Empty : Path.GetExtension(fileName);

                    cmd.Parameters.Add("@CheckCode", SqlDbType.NVarChar);
                    cmd.Parameters[5].Value = strCheckCode;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (GZipStream zipStream = new GZipStream(ms, CompressionMode.Compress))
                        {
                            zipStream.Write(content, 0, content.Length);
                            zipStream.Close();
                        }
                        cmd.Parameters.Add("@FileContent", SqlDbType.Image);
                        cmd.Parameters[6].Value = ms.ToArray();
                        ms.Close();
                    }

                    this.connection.Open();
                    result = cmd.ExecuteNonQuery() > 0;
                    this.connection.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (this.connection != null && (this.connection.State != ConnectionState.Closed))
                    this.connection.Close();
            }
            return result;
        }
        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="fileID">文件ID。</param>
        /// <returns>成功返回true,失败返回false。</returns>
        public bool DeleteFile(GUIDEx fileID)
        {
            bool result = false;
            try
            {
                const string sql = "exec spPlatformDeleteFileStorage '{0}'";
                if (fileID.IsValid)
                {
                    SqlCommand cmd = new SqlCommand(string.Format(sql, fileID), this.connection);
                    this.connection.Open();
                    result = cmd.ExecuteNonQuery() > 0;
                    this.connection.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (this.connection != null && (this.connection.State != ConnectionState.Closed))
                    this.connection.Close();
            }
            return result;
        }
        /// <summary>
        /// 下载文件。
        /// </summary>
        /// <param name="fileID">文件ID。</param>
        /// <param name="fullFileName">文件名称(包括后缀名)。</param>
        /// <param name="contentType">MIME内容类型。</param>
        /// <returns>文件数据流。</returns>
        public byte[] Download(GUIDEx fileID, out string fullFileName, out string contentType)
        {
            try
            {
                fullFileName = contentType = null;
                const string sql = "exec spPlatformFileStorageDownload '{0}'";
                if (fileID.IsValid)
                {
                    SqlCommand cmd = new SqlCommand(string.Format(sql, fileID), this.connection);
                    byte[] buf = null;
                    this.connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fullFileName = Convert.ToString(reader[0]);
                            contentType = Convert.ToString(reader[1]);
                            buf = (byte[])reader[2];
                            break;
                        }
                        reader.Close();
                    }
                    this.connection.Close();

                    if (buf != null)
                    {
                        byte[] data = null;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (GZipStream zipStream = new GZipStream(new MemoryStream(buf), CompressionMode.Decompress))
                            {
                                byte[] buffer = new byte[128];
                                int len = 0;
                                while ((len = zipStream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    ms.Write(buffer, 0, len);
                                }
                                zipStream.Close();
                            }
                            data = ms.ToArray();
                            ms.Close();
                        }
                        return data;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (this.connection != null && (this.connection.State != ConnectionState.Closed))
                    this.connection.Close();
            }
            return null;
        }
    }
}
