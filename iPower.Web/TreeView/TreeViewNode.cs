//================================================================================
//  FileName: TreeViewNode.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/23
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
namespace iPower.Web.TreeView
{
    /// <summary>
    /// 菜单节点。
    /// </summary>
    public class TreeViewNode : IStateManager
    {
        #region 成员变量，构造函数。
        string text, value, clickAction, hrefUrl, orderNo;
        TreeViewNode parent;
        bool bChecked, bExpand, isTrackingViewState;
        TreeViewNodeCollection childs = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="parent">父节点。</param>
        /// <param name="text">节点文本。</param>
        /// <param name="value">节点值。</param>
        /// <param name="orderNo">排序数据。</param>
        public TreeViewNode(TreeViewNode parent, string text, string value, string orderNo)
        {
            this.parent = parent;
            this.text = text;
            this.value = value;
            this.orderNo = orderNo;

            this.childs = new TreeViewNodeCollection(this);
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="parent">父节点。</param>
        /// <param name="text">节点文本。</param>
        /// <param name="value">节点值。</param>
        public TreeViewNode(TreeViewNode parent, string text, string value)
            : this(parent, text, value, text)
        {

        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="parent">父节点。</param>
        public TreeViewNode(TreeViewNode parent)
            : this(parent, string.Empty, string.Empty)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="text">节点文本。</param>
        /// <param name="value">节点值。</param>
        public TreeViewNode(string text, string value)
            : this(null, text, value)
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public TreeViewNode()
            : this(string.Empty, string.Empty)
        {
        }
        #endregion

        #region 事件处理。
        /// <summary>
        /// 展开或收缩事件。
        /// </summary>
        internal EventHandler<TreeViewNodeClickEventArgs> ChangedExpand;
        /// <summary>
        /// 触发事件。
        /// </summary>
        protected void OnChangedExpand()
        {
            EventHandler<TreeViewNodeClickEventArgs> handler = this.ChangedExpand;
            if (handler != null)
                handler(this, new TreeViewNodeClickEventArgs(this));
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置父节点。
        /// </summary>
        public TreeViewNode Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }
        /// <summary>
        /// 获取或设置节点文本。
        /// </summary>
        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }
        /// <summary>
        /// 获取或设置节点值。
        /// </summary>
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        /// <summary>
        /// 获取或设置排序。
        /// </summary>
        public string OrderNo
        {
            get { return this.orderNo; }
            set { this.orderNo = value; }
        }
        /// <summary>
        /// 获取或设置单击数据。
        /// </summary>
        public string ClickAction
        {
            get { return this.clickAction; }
            set { this.clickAction = value; }
        }
        /// <summary>
        /// 获取或设置链接URL。
        /// </summary>
        public string HrefURL
        {
            get { return this.hrefUrl; }
            set { this.hrefUrl = value; }
        }
        /// <summary>
        /// 获取或设置是否选中。
        /// </summary>
        public bool Checked
        {
            get { return this.bChecked; }
            set { this.bChecked = value; }
        }
        /// <summary>
        /// 获取或设置是否展开。
        /// </summary>
        public bool Expand
        {
            get { return this.bExpand; }
            set
            {
                if (this.bExpand != value)
                {
                    this.bExpand = value;
                    this.OnChangedExpand();
                }
            }
        }
        /// <summary>
        /// 获取子节点。
        /// </summary>
        public TreeViewNodeCollection Childs
        {
            get { return this.childs; }
        }
        #endregion

        /// <summary>
        /// 设置是否展开。
        /// </summary>
        /// <param name="expand"></param>
        internal void SetExpand(bool expand)
        {
            this.bExpand = expand;
        }

        #region IStateManager 成员
        /// <summary>
        /// 
        /// </summary>
        public bool IsTrackingViewState
        {
            get { return this.isTrackingViewState; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void LoadViewState(object state)
        {
            object[] objArray = (object[])state;
            if (objArray != null)
            {
                if (objArray[0] != null)
                    this.text = (string)objArray[0];
                if (objArray[1] != null)
                    this.value = (string)objArray[1];
                if (objArray[2] != null)
                    this.orderNo = (string)objArray[2];
                if (objArray[3] != null)
                    this.clickAction = (string)objArray[3];
                if (objArray[4] != null)
                    this.hrefUrl = (string)objArray[4];
                if (objArray[5] != null)
                    this.bChecked = (bool)objArray[5];
                if (objArray[6] != null)
                    this.bExpand = (bool)objArray[6];
                if (objArray[7] != null)
                    ((IStateManager)this.Childs).LoadViewState(objArray[7]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object SaveViewState()
        {
            return new object[] { 
                this.text,
                this.value,
                this.orderNo,
                this.clickAction,
                this.hrefUrl,
                this.bChecked,
                this.bExpand,
                this.childs.SaveViewState()
            };
        }
        /// <summary>
        /// 
        /// </summary>
        public void TrackViewState()
        {
            this.isTrackingViewState = true;
            if (this.childs != null)
                ((IStateManager)this.childs).TrackViewState();
        }

        #endregion
    }
    /// <summary>
    /// 菜单节点集合。
    /// </summary>
    public class TreeViewNodeCollection : ICollection<TreeViewNode>, IStateManager
    {
        #region 成员变量，构造函数。
        List<TreeViewNode> list = null;
        TreeViewNode parent = null;
        bool isTrackingViewState;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public TreeViewNodeCollection()
        {
            this.list = new List<TreeViewNode>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        internal TreeViewNodeCollection(TreeViewNode parent)
            : this()
        {
            this.parent = parent;
        }
        #endregion

        #region 索引。
        /// <summary>
        /// 获取或设置节点。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TreeViewNode this[int index]
        {
            get
            {
                return this.list[index];
            }
            set
            {
                TreeViewNode node = value;
                if (node != null && !this.list.Contains(node))
                {
                    if (this.parent != null)
                        node.Parent = this.parent;
                    this.list[index] = node;
                }
            }
        }
        /// <summary>
        /// 获取节点。
        /// </summary>
        /// <param name="value">节点值。</param>
        /// <returns></returns>
        public TreeViewNode this[string value]
        {
            get
            {
                TreeViewNode node = null;
                if (!string.IsNullOrEmpty(value) && this.list.Count > 0)
                {
                    foreach (TreeViewNode item in this.list)
                    {
                        node = this.FindTreeViewNode(item, value);
                        if (node != null)
                            break;
                    }
                }
                return node;
            }
        }
        #endregion

        #region 辅助函数。
        TreeViewNode FindTreeViewNode(TreeViewNode node, string value)
        {
            if (node != null)
            {
                if (node.Value == value)
                    return node;
                foreach (TreeViewNode item in node.Childs)
                {
                    TreeViewNode t = this.FindTreeViewNode(item, value);
                    if (t != null)
                        return t;
                }
            }
            return null;
        }
        #endregion

        #region ICollection<TreeViewNode> 成员
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(TreeViewNode item)
        {
            if (item != null)
            {
                if (this.parent != null)
                    item.Parent = parent;
                this.list.Add(item);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            this.list.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(TreeViewNode item)
        {
            return this.list.Contains(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(TreeViewNode[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return this.list.Count; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(TreeViewNode item)
        {
            return this.list.Remove(item);
        }

        #endregion

        #region IEnumerable<TreeViewNode> 成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TreeViewNode> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (TreeViewNode node in this.list)
                yield return node;
        }

        #endregion

        #region IStateManager 成员
        /// <summary>
        /// 
        /// </summary>
        public bool IsTrackingViewState
        {
            get { return this.isTrackingViewState; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void LoadViewState(object state)
        {
            object[] objArray = (object[])state;
            if (objArray != null)
            {
                this.Clear();
                foreach (object obj in objArray)
                {
                    TreeViewNode node = new TreeViewNode();
                    ((IStateManager)node).LoadViewState(obj);
                    this.Add(node);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object SaveViewState()
        {
            object[] objArray = new object[this.Count];
            if (this.Count > 0)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    objArray[i] = ((IStateManager)this[i]).SaveViewState();
                }
            }
            return objArray;
        }
        /// <summary>
        /// 
        /// </summary>
        public void TrackViewState()
        {
            this.isTrackingViewState = true;
            for (int i = 0; i < this.Count; i++)
                ((IStateManager)this[i]).TrackViewState();
        }

        #endregion
    }
}
