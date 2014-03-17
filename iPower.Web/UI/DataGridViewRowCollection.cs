//================================================================================
//  FileName: DataGridViewRowCollection.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/4
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

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
namespace iPower.Web.UI
{
    /// <summary>
    /// 表示 <see cref="DataGridView"/> 控件中的 <see cref="DataGridViewRow"/> 对象的集合。
    /// </summary>
    public class DataGridViewRowCollection : ICollection, IEnumerable
    {
        #region 成员变量，构造函数。
        ArrayList rows;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="rows"></param>
        public DataGridViewRowCollection(ArrayList rows)
        {
            this.rows = rows;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataGridViewRow this[int index]
        {
            get
            {
                return this.rows[index] as DataGridViewRow;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(DataGridViewRow[] array, int index)
        {
            ((ICollection)this).CopyTo(array, index);
        }

        #region ICollection 成员

        void ICollection.CopyTo(Array array, int index)
        {
            IEnumerator enumerator = this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                array.SetValue(enumerator.Current, index++);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return this.rows.Count; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }
        /// <summary>
        /// 
        /// </summary>
        public object SyncRoot
        {
            get { return this; }
        }

        #endregion

        #region IEnumerable 成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return this.rows.GetEnumerator();
        }

        #endregion
    }
}
