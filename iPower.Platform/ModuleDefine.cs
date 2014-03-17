//================================================================================
//  FileName:ModuleDefine.cs
//  Desc:
//
//  Called by
//
//  Auth:JeasonYoung
//  Date:2010-12-10 09:50:14
//================================================================================
//  Change History
//================================================================================
//  Date  Author  Description
// ----  ------  -----------
//
//================================================================================
//  Copyright (C) 2009-2010 Jeason Young Corporation
//================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Xml.Serialization;
namespace iPower.Platform
{
    /// <summary>
    ///  菜单模块工厂类。
    /// </summary>
    [Serializable]
    [XmlRoot("Jeason")]
    public class ModuleDefineFactory
    {
        #region 成员变量，构造函数。
        ModuleSystemDefineCollection collection = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ModuleDefineFactory()
        {
            this.Ver = "1.0";
            this.collection = new ModuleSystemDefineCollection();
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="ver">版本。</param>
        /// <param name="system">系统菜单模块定义集合。</param>
        public ModuleDefineFactory(string ver, ModuleSystemDefineCollection system)
            : this()
        {
            if (string.IsNullOrEmpty(ver))
                ver = "1.0";
            this.Ver = ver;
            this.System = system;
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置版本。
        /// </summary>
        [XmlAttribute("version")]
        public string Ver { get; set; }

        /// <summary>
        /// 获取或设置系统菜单模块定义集合。
        /// </summary>
        [XmlElement("System")]
        public ModuleSystemDefineCollection System
        {
            get { return this.collection; }
            set
            {
                ModuleSystemDefineCollection v = value;
                if (v != null && v.Count > 0)
                {
                    foreach (ModuleSystemDefine d in v)
                    {
                        if (!this.collection.Contains(d))
                            this.collection.Add(d);
                    }
                }
            }
        }
        #endregion

        #region 序列化函数。
        /// <summary>
        /// 序列化当前对象。
        /// </summary>
        /// <param name="output">输出流。</param>
        public void Serializer(Stream output)
        {
            if (output != null)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ModuleDefineFactory));
                serializer.Serialize(output, this);
            }
        }
        /// <summary>
        /// 反序列化。
        /// </summary>
        /// <param name="input">数据输入流。</param>
        /// <returns> 菜单模块工厂类。</returns>
        public static ModuleDefineFactory DeSerializer(Stream input)
        {
            ModuleDefineFactory f = null;
            if (input != null)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ModuleDefineFactory));
                f = serializer.Deserialize(input) as ModuleDefineFactory;
            }
            return f;
        }
        #endregion
    }
    /// <summary>
    /// 系统菜单模块定义集合。
    /// </summary>
    [Serializable]
    public class ModuleSystemDefineCollection : Collection<ModuleSystemDefine>
    {
        #region 成员变量，构造函数。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ModuleSystemDefineCollection()
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="define">系统菜单模块定义。</param>
        public ModuleSystemDefineCollection(ModuleSystemDefine define)
            : this()
        {
            this.Add(define);
        }
        #endregion

        #region 索引。
        /// <summary>
        /// 获取系统菜单模块定义。
        /// </summary>
        /// <param name="systemID">系统ID。</param>
        /// <returns>系统菜单模块定义。</returns>
        public ModuleSystemDefine this[string systemID]
        {
            get
            {
                ModuleSystemDefine result = null;
                if (string.IsNullOrEmpty(systemID))
                    return result;

                foreach (ModuleSystemDefine d in this.Items)
                {
                    if (string.Equals(d.SystemID, systemID, StringComparison.InvariantCulture | StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = d;
                        break;
                    }
                }
                return result;
            }
        }
        #endregion
    }
    /// <summary>
    /// 系统菜单模块定义。
    /// </summary>
    [Serializable]
    [XmlRoot("System")]
    public class ModuleSystemDefine
    {
        #region 成员变量，构造函数。
        ModuleDefineCollection collection = null;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ModuleSystemDefine()
        {
            this.collection = new ModuleDefineCollection(null);
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="systemID">系统ID。</param>
        /// <param name="systemSign">系统标识。</param>
        /// <param name="systemName">系统名称。</param>
        /// <param name="systemDescription">系统描述。</param>
        /// <param name="modules">菜单模块集合。</param>
        public ModuleSystemDefine(string systemID, string systemSign, string systemName, string systemDescription, ModuleDefineCollection modules)
            : this()
        {
            this.SystemID = systemID;
            this.SystemSign = systemSign;
            this.SystemName = systemName;
            this.SystemDescription = systemDescription;

            this.Modules = modules;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="systemID">系统ID。</param>
        /// <param name="systemSign">系统标识。</param>
        /// <param name="systemName">系统名称。</param>
        /// <param name="systemDescription">系统描述。</param>
        public ModuleSystemDefine(string systemID, string systemSign, string systemName, string systemDescription)
            : this(systemID, systemSign, systemName, systemDescription, null)
        {
        }
        #endregion

        #region 属性。
        /// <summary>
        /// 获取或设置系统ID。
        /// </summary>
        [XmlAttribute("SystemID")]
        public string SystemID { get; set; }
        /// <summary>
        /// 获取或设置系统标识。
        /// </summary>
        [XmlAttribute("SystemSign")]
        public string SystemSign { get; set; }
        /// <summary>
        /// 获取或设置系统名称。
        /// </summary>
        [XmlAttribute("SystemName")]
        public string SystemName { get; set; }
        /// <summary>
        /// 获取或设置系统描述。
        /// </summary>
        [XmlAttribute("SystemDescription")]
        public string SystemDescription { get; set; }

        /// <summary>
        /// 获取或设置菜单模块集合。
        /// </summary>
        [XmlElement("Module")]
        public ModuleDefineCollection Modules
        {
            get { return this.collection; }
            set
            {
                ModuleDefineCollection v = value;
                if (v != null && v.Count > 0)
                {
                    foreach (ModuleDefine d in v)
                    {
                        if (!this.collection.Contains(d))
                            this.collection.Add(d);
                    }
                }
            }
        }
        #endregion
    }
    /// <summary>
    /// 菜单模块定义集合。
    /// </summary>
    [Serializable]
    public class ModuleDefineCollection : Collection<ModuleDefine>, IComparer<ModuleDefine>
    {
        #region 成员变量，构造函数。
        ModuleDefine parent;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ModuleDefineCollection()
        {
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="parent">父菜单模块。</param>
        internal ModuleDefineCollection(ModuleDefine parent)
            : this()
        {
            this.parent = parent;
        }
        #endregion

        #region 索引。
        /// <summary>
        /// 获取菜单模块。
        /// </summary>
        /// <param name="moduleID">菜单模块ID。</param>
        /// <returns>菜单模块。</returns>
        public ModuleDefine this[string moduleID]
        {
            get
            {
                ModuleDefine result = null;
                if (string.IsNullOrEmpty(moduleID))
                    return result;

                ModuleDefine[] array = new ModuleDefine[this.Count];
                this.CopyTo(array, 0);
                Array.Sort<ModuleDefine>(array, this);
                foreach (ModuleDefine d in array)
                {
                    if (string.Equals(d.ModuleID, moduleID, StringComparison.InvariantCulture | StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = d;
                        break;
                    }
                    else if (d.Modules.Count > 0)
                    {
                        result = d.Modules[moduleID];
                        if (result != null)
                            break;
                    }
                }
                return result;
            }
        }
        #endregion

        #region 重载。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void InsertItem(int index, ModuleDefine item)
        {
            if (item != null)
                item.Parent = this.parent;
            base.InsertItem(index, item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected override void SetItem(int index, ModuleDefine item)
        {
            if (item != null)
                item.Parent = this.parent;
            base.SetItem(index, item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new IEnumerator GetEnumerator()
        {
            ModuleDefine[] array = new ModuleDefine[this.Count];
            this.CopyTo(array, 0);
            Array.Sort<ModuleDefine>(array, this);
            return array.GetEnumerator();
        }
        #endregion


        #region IComparer<ModuleDefine> 成员
        /// <summary>
        /// 排序比较。
        /// </summary>
        /// <param name="x">菜单模块。</param>
        /// <param name="y">菜单模块。</param>
        /// <returns></returns>
        public int Compare(ModuleDefine x, ModuleDefine y)
        {
            if (x == null && y == null)
                return 0;
            else if (x == null)
                return -1;
            else if (y == null)
                return 1;
            else
                return (x.OrderNo - y.OrderNo);
        }

        #endregion
    }
    /// <summary>
    /// 菜单模块定义。
    /// </summary>
    [Serializable]
    [XmlRoot("Module")]
    public class ModuleDefine
    {
        #region 成员变量，构造函数。
        ModuleDefineCollection collection;
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ModuleDefine()
        {
            this.collection = new ModuleDefineCollection(this);
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="moduleID">菜单模块ID。</param>
        /// <param name="moduleName">菜单模块名称。</param>
        /// <param name="moduleUri">菜单模块Uri。</param>
        /// <param name="orderNo">菜单模块排序。</param>
        /// <param name="modules">子菜单模块集合。</param>
        public ModuleDefine(string moduleID, string moduleName, string moduleUri, int orderNo, ModuleDefineCollection modules)
            : this()
        {
            this.ModuleID = moduleID;
            this.ModuleName = moduleName;
            this.ModuleUri = moduleUri;
            this.OrderNo = orderNo;

            this.Modules = modules;
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="moduleID">菜单模块ID。</param>
        /// <param name="moduleName">菜单模块名称。</param>
        /// <param name="moduleUri">菜单模块Uri。</param>
        /// <param name="orderNo">菜单模块排序。</param>
        public ModuleDefine(string moduleID, string moduleName, string moduleUri, int orderNo)
            : this(moduleID, moduleName, moduleUri, orderNo, null)
        {
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置父菜单模块。
        /// </summary>
        [XmlIgnore]
        public ModuleDefine Parent { get; set; }
        /// <summary>
        /// 获取或设置菜单模块ID。
        /// </summary>
        [XmlAttribute("ModuleID")]
        public string ModuleID { get; set; }
        /// <summary>
        /// 获取或设置菜单模块名称。
        /// </summary>
        [XmlAttribute("ModuleName")]
        public string ModuleName { get; set; }
        /// <summary>
        /// 获取或设置菜单模块Uri。
        /// </summary>
        [XmlAttribute("ModuleUri")]
        public string ModuleUri { get; set; }
        /// <summary>
        /// 获取或设置菜单模块排序。
        /// </summary>
        [XmlAttribute("OrderNo")]
        public int OrderNo { get; set; }
        /// <summary>
        /// 获取或设置子菜单模块集合。
        /// </summary>
        [XmlElement("Module")]
        public ModuleDefineCollection Modules
        {
            get { return this.collection; }
            set
            {
                ModuleDefineCollection v = value;
                if (v != null && v.Count > 0)
                {
                    foreach (ModuleDefine d in v)
                    {
                        if (!this.collection.Contains(d))
                            this.collection.Add(d);
                    }
                }
            }
        }
        #endregion
    }
}
