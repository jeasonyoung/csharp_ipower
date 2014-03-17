//================================================================================
//  FileName: UploadViewDataItem.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/19
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using System.Web.UI;
using iPower;
using iPower.Utility;
using iPower.Cryptography;
using iPower.Data;
namespace iPower.Web.Upload
{
    /// <summary>
    /// 上传数据项。
    /// </summary>
    public class UploadViewDataItem: IStateManager
    {
        #region 成员变量，构造函数。
        bool bTrackingViewState;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public UploadViewDataItem()
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置文件ID。
        /// </summary>
        public GUIDEx FileID
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置文件名称。
        /// </summary>
        public string FileName
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置扩展名。
        /// </summary>
        public string Extension
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置文件大小。
        /// </summary>
        public double Size
        {
            get;
            set;
        }
        #endregion

        #region IStateManager 成员
        /// <summary>
        /// 指示服务器控件是否正在跟踪其视图状态更改。
        /// </summary>
        public bool IsTrackingViewState
        {
            get { return this.bTrackingViewState; }
        }
        /// <summary>
        /// 加载服务器控件以前保存的控件视图状态。
        /// </summary>
        /// <param name="state"></param>
        public void LoadViewState(object state)
        {
            if (state != null)
            {
                object[] obj = (object[])state;
                if (obj[0] != null)
                    this.FileID = new GUIDEx(obj[0]);
                if (obj[1] != null)
                    this.FileName = obj[1] as string;
                if (obj[2] != null)
                    this.Extension = obj[2] as string;
                if (obj[3] != null)
                    this.Size = Convert.ToDouble(obj[3]);
            }
        }
        /// <summary>
        /// 将服务器控件的视图状态更改保存到 <see cref="Object"/>。
        /// </summary>
        /// <returns></returns>
        public object SaveViewState()
        {
            return new object[] {
                                    this.FileID,
                                    this.FileName,
                                    this.Extension,
                                    this.Size
                                };
        }
        /// <summary>
        /// 指示服务器控件跟踪其视图状态更改。
        /// </summary>
        public void TrackViewState()
        {
            this.bTrackingViewState = true;
        }

        #endregion
    }
    /// <summary>
    /// 上传数据项集合。
    /// </summary>
    public class UploadViewDataItemCollection : DataCollection<UploadViewDataItem>, IStateManager
    {
        #region 成员变量，构造函数。
        bool bTrackingViewState;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public UploadViewDataItemCollection()
        {
        }
        #endregion

        #region 索引。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileID"></param>
        /// <returns></returns>
        public UploadViewDataItem this[GUIDEx fileID]
        {
            get
            {
                if (fileID.IsValid)
                {
                    UploadViewDataItem item = this.Items.Find(new Predicate<UploadViewDataItem>(delegate(UploadViewDataItem data)
                    {
                        return fileID.IsValid && (data.FileID == fileID);
                    }));
                    return item;
                }
                return null;
            }
        }
        #endregion

        #region IStateManager 成员
        /// <summary>
        ///  指示服务器控件是否正在跟踪其视图状态更改。
        /// </summary>
        public bool IsTrackingViewState
        {
            get { return this.bTrackingViewState; }
        }
        /// <summary>
        ///  加载服务器控件以前保存的控件视图状态。
        /// </summary>
        /// <param name="state"></param>
        public void LoadViewState(object state)
        {
            ArrayList list = state as ArrayList;
            if (list != null && list.Count > 0)
            {
                this.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    object obj = list[i];
                    if (obj != null)
                    {
                        UploadViewDataItem item = new UploadViewDataItem();
                        item.LoadViewState(obj);
                        this.Add(item);
                    }
                }
            }
        }
        /// <summary>
        /// 将服务器控件的视图状态更改保存到 <see cref="Object"/>。
        /// </summary>
        /// <returns></returns>
        public object SaveViewState()
        {
            ArrayList list = new ArrayList(this.Count);
            foreach (UploadViewDataItem item in this.Items)
            {
                list.Add(item.SaveViewState());
            }
            return list;
        }
        /// <summary>
        /// 指示服务器控件跟踪其视图状态更改。
        /// </summary>
        public void TrackViewState()
        {
            this.bTrackingViewState = true;
        }

        #endregion
    }

    /// <summary>
    /// 数据项数据。
    /// </summary>
    public class UploadViewDataItemRaw : UploadViewDataItem
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public UploadViewDataItemRaw()
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="item"></param>
        /// <param name="contentType"></param>
        /// <param name="raw"></param>
        internal UploadViewDataItemRaw(UploadViewDataItem item, string contentType, byte[] raw)
        {
            if (item != null)
            {
                this.FileID = item.FileID;
                this.FileName = item.FileName;
                this.Extension = item.Extension;
                this.Size = item.Size;
            }
            this.ContentType = contentType;
            if (raw != null)
                this.FileRaw = raw;
        }
        #endregion
        /// <summary>
        /// 获取或设置MIME内容类型。
        /// </summary>
        public string ContentType
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置文件数据。
        /// </summary>
        public byte[] FileRaw
        {
            get;
            internal set;
        }

        /// <summary>
        /// 获取数据校验码。
        /// </summary>
        public string CheckCode
        {
            get
            {
                byte[] data = this.FileRaw;
                if (data != null)
                    return HexParser.ToHexString(HashCrypto.Hash(data, "md5"));
                return string.Empty;
            }
        }
    }
}
