//================================================================================
//  FileName: DataSourceHelper.cs
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

using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace iPower.Web.UI
{
    /// <summary>
    /// 帮助工具类。
    /// </summary>
    public static class DataSourceHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="dataMember"></param>
        /// <returns></returns>
        public static IEnumerable GetResolvedDataSource(object dataSource, string dataMember)
        {
            if (dataSource != null)
            {
                IListSource source = dataSource as IListSource;
                if (source != null)
                {
                    IList list = source.GetList();
                    if (!source.ContainsListCollection)
                        return list;
                    if ((list != null) && (list is ITypedList))
                    {
                        PropertyDescriptorCollection itemProperties = ((ITypedList)list).GetItemProperties(new PropertyDescriptor[0]);
                        if ((itemProperties == null) || (itemProperties.Count == 0))
                            throw new HttpException("ListSource_Without_DataMembers");

                        PropertyDescriptor descriptor = null;
                        if (string.IsNullOrEmpty(dataMember))
                            descriptor = itemProperties[0];
                        else
                            descriptor = itemProperties.Find(dataMember, true);
                        if (descriptor != null)
                        {
                            object component = list[0];
                            object obj = descriptor.GetValue(component);
                            if ((obj != null) && (obj is IEnumerable))
                                return (IEnumerable)obj;
                        }
                        throw new HttpException("ListSource_Missing_DataMember");
                    }
                }

                if (dataSource is IEnumerable)
                    return (IEnumerable)dataSource;
            }
            return null;
        }
    }
}
