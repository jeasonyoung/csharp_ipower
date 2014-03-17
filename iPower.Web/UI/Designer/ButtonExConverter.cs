//================================================================================
//  FileName: ButtonExConverter.cs
//  Desc: ButtonEx控件的类型转换器。
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2009-11-17
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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Globalization;

namespace iPower.Web.UI.Designer
{
	/// <summary>
	/// ButtonEx控件的类型转换器。
	/// </summary>
	public class ButtonExConverter : TypeConverter
	{
		#region 构造函数
		/// <summary>
		/// 构造函数。
		/// </summary>
		public ButtonExConverter()
		{
		}
		#endregion

        #region 重载函数。
        /// <summary>
		/// 使用指定的上下文返回此对象是否支持可以从列表中选取的标准值集。
		/// </summary>
		/// <param name="context">ITypeDescriptorContext，它提供格式上下文。</param>
		/// <returns>如果应调用 GetStandardValues 来查找对象支持的一组公共值，则为 true；否则，为 false。</returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// 当与格式上下文一起提供时，返回此类型转换器设计用于的数据类型的标准值集合。
		/// </summary>
		/// <param name="context">提供格式上下文的 ITypeDescriptorContext，可用来提取有关从中调用此转换器的环境的附加信息。此参数或其属性可以为空引用。</param>
		/// <returns>包含标准有效值集的 TypeConverter.StandardValuesCollection；如果数据类型不支持标准值集，则为空引用。</returns>
		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			System.Collections.ArrayList list=new System.Collections.ArrayList();

			foreach(System.ComponentModel.IComponent componet in context.Container.Components)
			{
				if(componet is  ButtonEx)
				{
                    list.Add(((ButtonEx)componet).ID);
				}
			}

			list.Sort();

			return new StandardValuesCollection(list.ToArray());
		}

		/// <summary>
		/// 返回该转换器是否可以使用指定的上下文将给定类型的对象转换为此转换器的类型。
		/// </summary>
		/// <param name="context">ITypeDescriptorContext，它提供格式上下文。</param>
		/// <param name="sourceType">表示要转换的类型。</param>
		/// <returns>如果该转换器能够执行转换，则为 true；否则为 false。</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context,Type sourceType)
		{
			if(sourceType.ToString()=="System.String")
			{
				return true;
			}
			else
			{
				return base.CanConvertFrom(context, sourceType);
			}
		}

		/// <summary>
		/// 使用指定的上下文和区域性信息将给定的对象转换为此转换器的类型。
		/// </summary>
		/// <param name="context">ITypeDescriptorContext，它提供格式上下文。</param>
		/// <param name="culture">用作当前区域性的 CultureInfo。</param>
		/// <param name="value">要转换的 Object。</param>
		/// <returns>Object，表示已转换的值。</returns>
		public override object ConvertFrom(ITypeDescriptorContext context,CultureInfo culture,object value)
		{
			if(value.GetType().ToString()=="System.String")
			{
				return value;
			}
			else
			{
				return base.ConvertFrom(context, culture, value);
			}
        }
        #endregion
    }
}
