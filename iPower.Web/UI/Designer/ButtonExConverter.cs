//================================================================================
//  FileName: ButtonExConverter.cs
//  Desc: ButtonEx�ؼ�������ת������
//
//  Called by
//
//  Auth:���£�jeason1914@gmail.com��
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
	/// ButtonEx�ؼ�������ת������
	/// </summary>
	public class ButtonExConverter : TypeConverter
	{
		#region ���캯��
		/// <summary>
		/// ���캯����
		/// </summary>
		public ButtonExConverter()
		{
		}
		#endregion

        #region ���غ�����
        /// <summary>
		/// ʹ��ָ���������ķ��ش˶����Ƿ�֧�ֿ��Դ��б���ѡȡ�ı�׼ֵ����
		/// </summary>
		/// <param name="context">ITypeDescriptorContext�����ṩ��ʽ�����ġ�</param>
		/// <returns>���Ӧ���� GetStandardValues �����Ҷ���֧�ֵ�һ�鹫��ֵ����Ϊ true������Ϊ false��</returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>
		/// �����ʽ������һ���ṩʱ�����ش�����ת����������ڵ��������͵ı�׼ֵ���ϡ�
		/// </summary>
		/// <param name="context">�ṩ��ʽ�����ĵ� ITypeDescriptorContext����������ȡ�йش��е��ô�ת�����Ļ����ĸ�����Ϣ���˲����������Կ���Ϊ�����á�</param>
		/// <returns>������׼��Чֵ���� TypeConverter.StandardValuesCollection������������Ͳ�֧�ֱ�׼ֵ������Ϊ�����á�</returns>
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
		/// ���ظ�ת�����Ƿ����ʹ��ָ���������Ľ��������͵Ķ���ת��Ϊ��ת���������͡�
		/// </summary>
		/// <param name="context">ITypeDescriptorContext�����ṩ��ʽ�����ġ�</param>
		/// <param name="sourceType">��ʾҪת�������͡�</param>
		/// <returns>�����ת�����ܹ�ִ��ת������Ϊ true������Ϊ false��</returns>
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
		/// ʹ��ָ���������ĺ���������Ϣ�������Ķ���ת��Ϊ��ת���������͡�
		/// </summary>
		/// <param name="context">ITypeDescriptorContext�����ṩ��ʽ�����ġ�</param>
		/// <param name="culture">������ǰ�����Ե� CultureInfo��</param>
		/// <param name="value">Ҫת���� Object��</param>
		/// <returns>Object����ʾ��ת����ֵ��</returns>
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
