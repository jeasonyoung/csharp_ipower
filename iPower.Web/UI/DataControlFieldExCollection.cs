//================================================================================
//  FileName: DataControlFieldExCollection.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/7
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

using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Design;
namespace iPower.Web.UI
{
    /// <summary>
    /// 表示数据绑定控件使用的<see cref="DataControlFieldEx"/>对象的集合。
    /// </summary>
    public sealed class DataControlFieldExCollection : StateManagedCollection
    {
        #region 成员变量，构造函数。
        static readonly Type[] knownTypes = new Type[] { typeof(BoundFieldEx), 
                                                         //typeof(ButtonField), 
                                                         typeof(CheckBoxFieldEx), 
                                                         //typeof(CommandField), 
                                                         //typeof(HyperLinkField), 
                                                        // typeof(ImageField), 
                                                         typeof(TemplateFieldEx) };
        /// <summary>
        /// 构造函数。
        /// </summary>
        public DataControlFieldExCollection()
        {
        }
        #endregion

        #region 事件处理。
        /// <summary>
        /// 发生在集合中的字段更改时事件。
        /// </summary>
        public EventHandler FieldsChanged;
        void OnFieldsChanged()
        {
            EventHandler handler = this.FieldsChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        void OnFieldChanged(object sender, EventArgs e)
        {
            this.OnFieldsChanged();
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 索引。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public DataControlFieldEx this[int index]
        {
            get
            {
                return (this[index] as DataControlFieldEx);
            }
        }
        #endregion

        /// <summary>
        /// 将指定的 <see cref="DataControlFieldEx"/> 对象追加到集合的结尾。
        /// </summary>
        /// <param name="field"></param>
        public void Add(DataControlFieldEx field)
        {
            ((IList)this).Add(field);
        }
        /// <summary>
        /// 创建当前集合的副本。
        /// </summary>
        /// <returns></returns>
        public DataControlFieldExCollection CloneFields()
        {
            DataControlFieldExCollection fields = new DataControlFieldExCollection();
            foreach (DataControlFieldEx field in this)
            {
                fields.Add(field.CloneField());
            }
            return fields;
        }
        /// <summary>
        /// 集合中是否包含对象。
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool Contains(DataControlFieldEx field)
        {
            return ((IList)this).Contains(field);
        }
        /// <summary>
        /// 复制。
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(DataControlFieldEx[] array, int index)
        {
            this.CopyTo(array, index);
        }
        /// <summary>
        /// 确定集合中特定 <see cref="DataControlFieldEx"/> 对象的索引。
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public int IndexOf(DataControlFieldEx field)
        {
            return ((IList)this).IndexOf(field);
        }
        /// <summary>
        /// 将指定的 <see cref="DataControlFieldEx"/> 对象插入<see cref="DataControlFieldExCollection"/> 集合中指定的索引位置。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="field"></param>
        public void Insert(int index, DataControlFieldEx field)
        {
            ((IList)this).Insert(index, field);
        }
        /// <summary>
        /// 从 <see cref="DataControlFieldExCollection"/> 集合中移除指定的<see cref="DataControlFieldEx"/>  对象。
        /// </summary>
        /// <param name="field"></param>
        public void Remove(DataControlFieldEx field)
        {
            ((IList)this).Remove(field);
        }
        /// <summary>
        /// 从 <see cref="DataControlFieldExCollection"/> 集合中移除位于指定索引位置的 <see cref="DataControlFieldEx"/> 对象。
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            ((IList)this).RemoveAt(index);
        }
        #region 重载。
        /// <summary>
        /// 在派生类中重写时，创建实现 IStateManager 的类的实例。创建的对象的类型依赖于由 GetKnownTypes 方法返回的集合的指定成员。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override object CreateKnownType(int index)
        {
            switch (index)
            {
                case 0:
                    return new BoundFieldEx();

                //case 1:
                //    return new ButtonField();

                case 2:
                    return new CheckBoxFieldEx();

                //case 3:
                //    return new CommandField();

                //case 4:
                //    return new HyperLinkField();

                //case 5:
                //    return new ImageField();

                case 6:
                    return new TemplateFieldEx();
            }
            throw new ArgumentOutOfRangeException("DataControlFieldExCollection_InvalidTypeIndex");
        }
        /// <summary>
        /// 在派生类中重写时，获取 StateManagedCollection 集合可以包含的 IStateManager 类型的数组。 
        /// </summary>
        /// <returns></returns>
        protected override Type[] GetKnownTypes()
        {
            return knownTypes;
        }
        /// <summary>
        /// 在 Clear 方法完成从集合中移除所有项之后执行额外的工作。
        /// </summary>
        protected override void OnClearComplete()
        {
            this.OnFieldsChanged();
        }
        /// <summary>
        /// 当在派生类中重写时，在 Insert(Int32, Object) 或 Add(Object) 方法向集合中添加项之后执行额外的工作。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected override void OnInsertComplete(int index, object value)
        {
            DataControlFieldEx field = value as DataControlFieldEx;
            if (field != null)
                field.FieldChanged += new EventHandler(this.OnFieldChanged);
            this.OnFieldsChanged();
        }
        /// <summary>
        /// 在派生类中重写时，在 Remove(Object) 或 RemoveAt(Int32) 方法从集合中移除指定项之后执行额外的工作。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        protected override void OnRemoveComplete(int index, object value)
        {
            DataControlFieldEx field = value as DataControlFieldEx;
            if (field != null)
                field.FieldChanged -= new EventHandler(this.OnFieldChanged);
            this.OnFieldsChanged();
        }
        /// <summary>
        /// 当在派生类中重写时，验证 <see cref="StateManagedCollection"/> 集合的元素。
        /// </summary>
        /// <param name="value"></param>
        protected override void OnValidate(object value)
        {
            base.OnValidate(value);
            if (!(value is DataControlFieldEx))
                throw new ArgumentException("DataControlFieldExCollection_InvalidType");
        }
        /// <summary>
        /// 当在派生类中重写时，指示由集合包含的 object 将其整个状态记录到视图状态，而不是仅仅记录更改信息。
        /// </summary>
        /// <param name="o"></param>
        protected override void SetDirtyObject(object o)
        {
            ((DataControlFieldEx)o).SetDirty();
        }
        #endregion
 
    }
}