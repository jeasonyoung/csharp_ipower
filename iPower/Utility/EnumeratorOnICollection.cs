//================================================================================
//  FileName: EnumeratorOnICollection.cs
//  Desc: 将ICollection转化为IEnumerator。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-12-31
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
using System.Text;

namespace iPower.Utility
{
    /// <summary>
    ///  将ICollection转化为IEnumerator。
    /// </summary>
    public class EnumeratorOnICollection : IEnumerator
    {
        #region 成员变量，构造函数。
        ICollection collection;
        IEnumerator collectionEnum;
        int index, indexBounds, startIndex;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="collection">collection。</param>
        /// <param name="startIndex">开始索引。</param>
        /// <param name="count">个数。</param>
        public EnumeratorOnICollection(ICollection collection, int startIndex, int count)
        {
            Guard.ArgumentNotNull("collection", collection, true);
            this.collection = collection;
            this.startIndex = startIndex;
            this.index = -1;
            this.indexBounds = startIndex + count;
            if (this.indexBounds > collection.Count)
                this.indexBounds = collection.Count;
        }
        #endregion

        #region IEnumerator 成员
        /// <summary>
        /// 获取当前值。
        /// </summary>
        public object Current
        {
            get
            {
                return this.collectionEnum.Current;
            }
        }
        /// <summary>
        /// 下一个值。
        /// </summary>
        /// <returns>存在为True，否则为False。</returns>
        public bool MoveNext()
        {
            if (this.collectionEnum == null)
            {
                this.collectionEnum = this.collection.GetEnumerator();
                for (int i = 0; i < this.startIndex; i++)
                    this.collectionEnum.MoveNext();
            }
            this.collectionEnum.MoveNext();
            this.index++;
            return this.startIndex + this.index < this.indexBounds;
        }
        /// <summary>
        /// 复位。
        /// </summary>
        public void Reset()
        {
            this.collectionEnum = null;
            this.index = -1;
        }

        #endregion
    }
}
