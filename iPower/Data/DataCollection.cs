//================================================================================
//  FileName: IDataCollection.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/4/8
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
using System.Data;
using System.Reflection;

using System.ComponentModel;
namespace iPower.Data
{
    /// <summary>
    /// 数据集合。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class DataCollection<T> : ICollection<T>, IComparer<T>, IListSource
        where T : new()
    {
        #region 成员变量，构造函数。
        List<T> list;
        static BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public DataCollection()
        {
            this.list = new List<T>();
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取数据。
        /// </summary>
        protected List<T> Items
        {
            get
            {
                this.list.Sort(this);
                return this.list;
            }
        }
        /// <summary>
        /// 获取或设置数据项。
        /// </summary>
        /// <param name="index">从零开始的索引。</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return this.list[index];
            }
            set
            {
                this.list[index] = value;
            }
        }
        #endregion

        #region 函数。
        /// <summary>
        /// 根据<see cref="DataTable"/>数据源填充数据。
        /// </summary>
        /// <param name="dataSource">数据源。</param>
        public void InitAssignment(DataTable dataSource)
        {
            this.Clear();
            if (dataSource != null)
            {
                T item = default(T);
                foreach (DataRow row in dataSource.Rows)
                {
                    item = this.Assignment(row);
                    if (item != null)
                        this.Add(item);
                }
            }
        }
        /// <summary>
        /// 根据<see cref="DataTable"/>数据源填充数据。
        /// </summary>
        /// <param name="dataSource">数据源。</param>
        public void InitAssignment(IEnumerator dataSource)
        {
            this.Clear();
            if (dataSource != null)
            {
                T item = default(T);
                while (dataSource.MoveNext())
                {
                    item = this.Assignment(dataSource.Current);
                    if (item != null)
                        this.Add(item);
                }
            }
        }
        /// <summary>
        /// 给数据项赋值。
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected virtual T Assignment(DataRow row)
        {
            T item = default(T);
            if (row != null && row.ItemArray.Length > 0)
            {
                item = new T();
                Type t = typeof(T);
                object obj = null;
                foreach (PropertyInfo info in t.GetProperties(bindingFlags))
                {
                    obj = row[info.Name];
                    if (obj != null)
                        info.SetValue(item, this.ConvertToPropertyType(obj, info.PropertyType), null);
                }
            }
            return item;
        }
        /// <summary>
        /// 给数据项赋值。
        /// </summary>
        /// <param name="dataSouce"></param>
        /// <returns></returns>
        protected virtual T Assignment(object dataSouce)
        {
            T item = default(T);
            if (dataSouce != null)
            {
                item = new T();
                Type t = typeof(T);
                object obj = null;
                PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(dataSouce);
                if (pdc != null && pdc.Count > 0)
                {
                    PropertyDescriptor pd = null;
                    foreach (PropertyInfo info in t.GetProperties(bindingFlags))
                    {
                        pd = pdc.Find(info.Name, true);
                        if (pd != null)
                        {
                            obj = pd.GetValue(dataSouce);
                            if (obj != null)
                                info.SetValue(item, this.ConvertToPropertyType(obj, info.PropertyType), null);
                        }
                    }
                }
            }
            return item;
        }
        /// <summary>
        /// 数据类型转换。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual object ConvertToPropertyType(object value, Type type)
        {
            if (type.BaseType == typeof(Enum))
            {
                if (value == null || value == DBNull.Value)
                    return Enum.ToObject(type, 0);
                return Enum.ToObject(type, Convert.ToInt32(value));
            }
            if (type == typeof(GUIDEx))
            {
                return new GUIDEx(value);
            }

            if (type == typeof(Guid))
            {
                if (value == DBNull.Value || value == null)
                    return Guid.Empty;
                return new Guid(Convert.ToString(value));
            }
            if (value == DBNull.Value)
                return null;
            if (type == typeof(string))
            {
                if (value == null)
                    return value;
                return value.ToString();
            }

            try
            {
                return Convert.ChangeType(value, type);
            }
            catch { }
            return value;
        }
        #endregion

        #region ICollection<T> 成员
        /// <summary>
        /// 添加数据。。
        /// </summary>
        /// <param name="item">数据项。</param>
        public virtual void Add(T item)
        {
            if (item != null && !this.Contains(item))
                this.list.Add(item);
        }
        /// <summary>
        /// 清空数据。
        /// </summary>
        public void Clear()
        {
            this.list.Clear();
        }
        /// <summary>
        /// 是否存在相同数据。
        /// </summary>
        /// <param name="item">数据项。</param>
        /// <returns></returns>
        public virtual bool Contains(T item)
        {
            if (item != null)
                return this.list.Contains(item);
            return false;
        }
        /// <summary>
        /// 复制数据项到数据。
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// 获取数据集合数量。
        /// </summary>
        public int Count
        {
            get { return this.list.Count; }
        }
        /// <summary>
        /// 获取是否只读。
        /// </summary>
        public virtual bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// 移除数据项。
        /// </summary>
        /// <param name="item">数据项。</param>
        /// <returns></returns>
        public virtual bool Remove(T item)
        {
            if (item != null)
                return this.list.Remove(item);
            return false;
        }
        #endregion

        #region IEnumerable<T> 成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            this.list.Sort(this);
            return this.list.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            this.list.Sort(this);
            foreach (T t in this.list)
                yield return t;
        }

        #endregion

        #region IComparer<T> 成员
        /// <summary>
        /// 排序函数。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public virtual int Compare(T x, T y)
        {
            return 0;
        }

        #endregion

        #region IListSource 成员
        /// <summary>
        /// 
        /// </summary>
        bool IListSource.ContainsListCollection
        {
            get { return false; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList IListSource.GetList()
        {
            return this.Items;
        }

        #endregion
    }
}
