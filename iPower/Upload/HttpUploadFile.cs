//================================================================================
//  FileName: HttpUploadFile.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2013-4-27
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
using System.Net;
namespace iPower.Upload
{
    /// <summary>
    /// 通过HTTP向Web服务器页面上传附件。
    /// </summary>
    public class HttpUploadFile : IDisposable
    {
        #region 成员变量，构造函数。
        private const double CONST_BYTESTOM = 1048576.0;//1024*1024
        /// <summary>
        /// 构造函数。
        /// </summary>
        public HttpUploadFile()
            : this(null)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="url"></param>
        public HttpUploadFile(string url)
        {
            this.URL = url;
            this.Timeout = 300 * 1000;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置上传目标的URL地址。
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 设置获取响应的超时时间(秒)。
        /// </summary>
        public int Timeout { private get; set; }
        #endregion

        #region 事件。
        /// <summary>
        /// 上传进度事件。
        /// </summary>
        public event UploadProgressEventHandler UploadProgress;
        /// <summary>
        /// 触发上传进度事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUploadProgress(ProgressEventArgs e)
        {
            UploadProgressEventHandler handler = this.UploadProgress;
            if (handler != null)
            {
                handler(e);
            }
        }
        /// <summary>
        /// 上传反馈事件。
        /// </summary>
        public event CallbackEventHandler UploadCallback;
        /// <summary>
        /// 触发上传反馈事件。
        /// </summary>
        /// <param name="e"></param>
        protected virtual bool OnUploadCallback(CallbackEventArgs e)
        {
            CallbackEventHandler handler = this.UploadCallback;
            if (handler != null)
            {
               return handler(e);
            }
            return false;
        }
        #endregion

        /// <summary>
        /// 将本地上传到指定的服务器（HTTP）方式。
        /// </summary>
        /// <param name="filePath">本地文件路径。</param>
        /// <returns></returns>
        public bool Upload(string filePath)
        {
            return this.Upload(filePath, null);
        }
        /// <summary>
        /// 将本地上传到指定的服务器（HTTP）方式。
        /// </summary>
        /// <param name="filePath">本地文件路径。</param>
        /// <param name="saveName">上传后的文件名</param>
        /// <returns>上传结果。</returns>
        public bool Upload(string filePath, string saveName)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentNullException("文件不存在！" + filePath);
            }
            if (string.IsNullOrEmpty(saveName))
            {
                saveName = Path.GetFileName(filePath);
            }
            bool result = false;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                result = this.Upload(fs, saveName);
            }
            return result;
        }
        /// <summary>
        /// 将数据流上传到指定的服务器（HTTP）方式。
        /// </summary>
        /// <param name="input">上传文件流。</param>
        /// <param name="saveName">文件上传后的名称</param>
        /// <returns>结果，成功为true,失败返回0</returns>
        public bool Upload(Stream input, string saveName)
        {
            bool result = false;
            if (input == null)
            {
                throw new ArgumentNullException("fs");
            }
            if (string.IsNullOrEmpty(this.URL))
            {
                throw new ArgumentNullException("未设置文件上传到的服务器地址(URL)！");
            }
            string strBoundary = this.BuildBoundary();
            byte[] boundaryBytes = this.BuildBoundary(strBoundary);
            byte[] postHeaderBytes = this.BuildPostHeader(strBoundary, saveName);
            //根据URI创建HttpWebRequest对象。
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(this.URL));
            httpReq.Method = "POST";
            //对发送方数据不使用缓存。
            httpReq.AllowWriteStreamBuffering = false;
            httpReq.Timeout = this.Timeout;
            httpReq.ContentType = "multipart/form-data; boundary=" + strBoundary;
            long fileLength = input.Length;
            long length = fileLength + postHeaderBytes.Length + boundaryBytes.Length;
            httpReq.ContentLength = length;
            try
            {
                byte[] buffer = new byte[4096];
                using (BinaryReader reader = new BinaryReader(input))
                {
                    using (Stream post = httpReq.GetRequestStream())
                    {
                        DateTime startTime = DateTime.Now;
                        long offset = 0; int count = 0;
                        ProgressEventArgs args = null;
                        //发送请求头部消息。
                        post.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                        //上传数据。
                        while ((count = reader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            post.Write(buffer, 0, count);
                            offset += count;
                            args = new ProgressEventArgs();
                            args.Value = (int)(offset * (int.MaxValue / length));
                            args.Time = (DateTime.Now - startTime).TotalSeconds;
                            args.TimeText = string.Format("已用时:{0:F2}秒", args.Time);
                            if (args.Time > 0.01)
                            {
                                args.SpeedText = "平均速度:" + (offset / 1024 / args.Time).ToString("0.00") + "KB/秒";
                            }
                            else
                            {
                                args.SpeedText = "正在连接...";
                            }
                            args.Status = string.Format("{0:F2}%,{1:F2}M/{2:F2}M", (offset * 100.0 / length), (offset / CONST_BYTESTOM), (fileLength / CONST_BYTESTOM));
                            this.OnUploadProgress(args);
                        }
                        //添加尾部时间戳。
                        post.Write(boundaryBytes, 0, boundaryBytes.Length);
                        post.Close();
                    }
                    reader.Close();
                }
                //获取服务器响应。
                using (WebResponse resp = httpReq.GetResponse())
                {
                    result = this.OnUploadCallback(new CallbackEventArgs(resp.GetResponseStream()));
                }
            }
            finally
            {
                input.Close();
            }
            return result;
        }

        #region 辅助函数。
        /// <summary>
        /// 构建时间戳字符串。
        /// </summary>
        /// <returns></returns>
        protected virtual string BuildBoundary()
        {
            return "----------" + DateTime.Now.Ticks.ToString("x");
        }
        /// <summary>
        /// 构建时间戳。
        /// </summary>
        /// <returns></returns>
        protected virtual byte[] BuildBoundary(string boundary)
        {
            return Encoding.ASCII.GetBytes("/r/n--" + boundary + "/r/n");
        }
        /// <summary>
        /// 构建请求头部信息。
        /// </summary>
        /// <param name="boundary">时间戳字符串。</param>
        /// <param name="saveName">保存文件名。</param>
        /// <returns></returns>
        protected virtual byte[] BuildPostHeader(string boundary, string saveName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("--");
            sb.Append(boundary);
            sb.Append("/r/n");
            sb.Append("Content-Disposition: form-data; name=\"");
            sb.Append("file");
            sb.Append("\"; filename=\"");
            sb.Append(saveName);
            sb.Append("\"");
            sb.Append("/r/n");
            sb.Append("Content-Type: ");
            sb.Append("application/octet-stream");
            sb.Append("/r/n");
            sb.Append("/r/n");
            return Encoding.UTF8.GetBytes(sb.ToString());
        }
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            
        }
        #endregion
    }
}