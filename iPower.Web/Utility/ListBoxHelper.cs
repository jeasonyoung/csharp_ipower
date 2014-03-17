//================================================================================
//  FileName: ListBoxHelper.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/21
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iPower.Web.UI;
namespace iPower.Web.Utility
{
    /// <summary>
    /// 列表框的处理帮助类。
    /// </summary>
    public static class ListBoxHelper
    {
        /// <summary>
        /// 获取源的所有数据。
        /// </summary>
        /// <param name="source">源<see cref="ListBox"/>。</param>
        /// <param name="text">获取的文本数组。</param>
        /// <param name="values">获取的值数组。</param>
        public static void GetAll(ListBox source, out string[] text, out string[] values)
        {
            text = values = null;
            if (source != null && source.Items.Count > 0)
            {
                int len = source.Items.Count;
                text = new string[len];
                values = new string[len];

                for (int i = 0; i < len; i++)
                {
                    ListItem item = source.Items[i];
                    if (item != null)
                    {
                        text[i] = item.Text;
                        values[i] = item.Value;
                    }
                }
            }
        }
        ///// <summary>
        ///// 获取源的所有数据。
        ///// </summary>
        ///// <param name="source">源<see cref="ListBoxEx"/>。</param>
        ///// <param name="text">获取的文本数组。</param>
        ///// <param name="values">获取的值数组。</param>
        //public static void GetAll(ListBoxEx source, out string[] text, out string[] values)
        //{
        //    text = values = null;
        //    if (source != null && source.Items.Count > 0)
        //    {
        //        int len = source.Items.Count;
        //        text = new string[len];
        //        values = new string[len];

        //        for (int i = 0; i < len; i++)
        //        {
        //            ListItem item = source.Items[i];
        //            if (item != null)
        //            {
        //                text[i] = item.Text;
        //                values[i] = item.Value;
        //            }
        //        }
        //    }
        //}
        /// <summary>
        /// 获取源的选中数据。
        /// </summary>
        /// <param name="source">源<see cref="ListBox"/>。</param>
        /// <param name="text">获取的文本数组。</param>
        /// <param name="values">获取的值数组。</param>
        public static void GetSelected(ListBox source, out string[] text, out string[] values)
        {
            text = values = null;
            if (source != null && source.Items.Count > 0)
            {
                List<ListItem> list = new List<ListItem>();
                foreach (ListItem item in source.Items)
                {
                    if (item.Selected)
                        list.Add(item);
                }
                int len = list.Count;
                if (len > 0)
                {
                    text = new string[len];
                    values = new string[len];
                    for (int i = 0; i < len; i++)
                    {
                        ListItem item = list[i];
                        if (item != null)
                        {
                            text[i] = item.Text;
                            values[i] = item.Value;
                        }
                    }
                }
            }
        }
        ///// <summary>
        ///// 获取源的选中数据。
        ///// </summary>
        ///// <param name="source">源<see cref="ListBoxEx"/>。</param>
        ///// <param name="text">获取的文本数组。</param>
        ///// <param name="values">获取的值数组。</param>
        //public static void GetSelected(ListBoxEx source, out string[] text, out string[] values)
        //{
        //    text = values = null;
        //    if (source != null && source.Items.Count > 0)
        //    {
        //        List<ListItem> list = new List<ListItem>();
        //        foreach (ListItem item in source.Items)
        //        {
        //            if (item.Selected)
        //                list.Add(item);
        //        }
        //        int len = list.Count;
        //        if (len > 0)
        //        {
        //            text = new string[len];
        //            values = new string[len];
        //            for (int i = 0; i < len; i++)
        //            {
        //                ListItem item = list[i];
        //                if (item != null)
        //                {
        //                    text[i] = item.Text;
        //                    values[i] = item.Value;
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 判断<see cref="ListBox"/>里是否存在指定的值。
        /// </summary>
        /// <param name="listBox">要判断<see cref="ListBox"/>。</param>
        /// <param name="itemValue">要检查的值。</param>
        /// <returns>是否存在。</returns>
        public static bool IsExistedValueInListBox(ListBox listBox, string itemValue)
        {
            if (listBox != null && !string.IsNullOrEmpty(itemValue))
            {
                foreach (ListItem item in listBox.Items)
                {
                    if (string.Equals(item.Value, itemValue, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }
            return false;
        }
        ///// <summary>
        ///// 判断<see cref="ListBoxEx"/>里是否存在指定的值。
        ///// </summary>
        ///// <param name="listBox">要判断<see cref="ListBoxEx"/>。</param>
        ///// <param name="itemValue">要检查的值。</param>
        ///// <returns>是否存在。</returns>
        //public static bool IsExistedValueInListBox(ListBoxEx listBox, string itemValue)
        //{
        //    if (listBox != null && !string.IsNullOrEmpty(itemValue))
        //    {
        //        foreach (ListItem item in listBox.Items)
        //        {
        //            if (string.Equals(item.Value, itemValue, StringComparison.InvariantCultureIgnoreCase))
        //                return true;
        //        }
        //    }
        //    return false;
        //}
        /// <summary>
        /// 将源<see cref="ListBox"/> 里的所有的项移到目标<see cref="ListBox"/>里。
        /// </summary>
        /// <param name="source">源<see cref="ListBox"/>。</param>
        /// <param name="dest">目标<see cref="ListBox"/>。</param>
        public static void AddAll(ListBox source, ListBox dest)
        {
            if (source != null && dest != null)
            {
                foreach (ListItem item in source.Items)
                {
                    if (!dest.Items.Contains(item))
                        dest.Items.Add(item);
                }
                source.Items.Clear();
            }
        }
        ///// <summary>
        ///// 将源<see cref="ListBoxEx"/> 里的所有的项移到目标<see cref="ListBoxEx"/>里。
        ///// </summary>
        ///// <param name="source">源<see cref="ListBoxEx"/>。</param>
        ///// <param name="dest">目标<see cref="ListBoxEx"/>。</param>
        //public static void AddAll(ListBoxEx source, ListBoxEx dest)
        //{
        //    if (source != null && dest != null)
        //    {
        //        foreach (ListItem item in source.Items)
        //        {
        //            if (!dest.Items.Contains(item))
        //                dest.Items.Add(item);
        //        }
        //        source.Items.Clear();
        //    }
        //}
        /// <summary>
        /// 将源<see cref="ListBox"/>里的所有选择的项移到目标<see cref="ListBox"/>里。
        /// </summary>
        /// <param name="source">源<see cref="ListBox"/>。</param>
        /// <param name="dest">目标<see cref="ListBox"/>。</param>
        public static void AddSelected(ListBox source, ListBox dest)
        {
            if (source != null && dest != null)
            {
                List<ListItem> list = new List<ListItem>();
                foreach (ListItem item in source.Items)
                {
                    if (item.Selected)
                    {
                        list.Add(item);
                        if (!dest.Items.Contains(item))
                            dest.Items.Add(item);
                    }
                }
                //移除选中项
                foreach (ListItem item in list)
                {
                    source.Items.Remove(item);
                }
            }
        }
        ///// <summary>
        ///// 将源<see cref="ListBoxEx"/>里的所有选择的项移到目标<see cref="ListBoxEx"/>里。
        ///// </summary>
        ///// <param name="source">源<see cref="ListBoxEx"/>。</param>
        ///// <param name="dest">目标<see cref="ListBoxEx"/>。</param>
        //public static void AddSelected(ListBoxEx source, ListBoxEx dest)
        //{
        //    if (source != null && dest != null)
        //    {
        //        List<ListItem> list = new List<ListItem>();
        //        foreach (ListItem item in source.Items)
        //        {
        //            if (item.Selected)
        //            {
        //                list.Add(item);
        //                if (!dest.Items.Contains(item))
        //                    dest.Items.Add(item);
        //            }
        //        }
        //        //移除选中项
        //        foreach (ListItem item in list)
        //        {
        //            source.Items.Remove(item);
        //        }
        //    }
        //}
        /// <summary>
        /// 将目标<see cref="ListBox"/>里的所有选择的项移到源<see cref="ListBox"/>里。
        /// </summary>
        /// <param name="source">源<see cref="ListBox"/>。</param>
        /// <param name="dest">目标<see cref="ListBox"/>。</param>
        public static void RemoveSelected(ListBox source, ListBox dest)
        {
            if (source != null && dest != null)
            {
                List<ListItem> list = new List<ListItem>();
                foreach (ListItem item in dest.Items)
                {
                    if (item.Selected)
                    {
                        list.Add(item);
                        if (!source.Items.Contains(item))
                            source.Items.Add(item);
                    }
                }
                //移除选中项
                foreach (ListItem item in list)
                {
                    dest.Items.Remove(item);
                }
            }
        }
        ///// <summary>
        ///// 将目标<see cref="ListBoxEx"/>里的所有选择的项移到源<see cref="ListBoxEx"/>里。
        ///// </summary>
        ///// <param name="source">源<see cref="ListBoxEx"/>。</param>
        ///// <param name="dest">目标<see cref="ListBoxEx"/>。</param>
        //public static void RemoveSelected(ListBoxEx source, ListBoxEx dest)
        //{
        //    if (source != null && dest != null)
        //    {
        //        List<ListItem> list = new List<ListItem>();
        //        foreach (ListItem item in dest.Items)
        //        {
        //            if (item.Selected)
        //            {
        //                list.Add(item);
        //                if (!source.Items.Contains(item))
        //                    source.Items.Add(item);
        //            }
        //        }
        //        //移除选中项
        //        foreach (ListItem item in list)
        //        {
        //            dest.Items.Remove(item);
        //        }
        //    }
        //}
        
        /// <summary>
		/// 将目标<see cref="ListBox"/> 里的所有项移到源<see cref="ListBox"/> 里。
		/// </summary>
        /// <param name="source">源<see cref="ListBox"/></param>
        /// <param name="dest">目标<see cref="ListBox"/></param>
        public static void RemoveAll(ListBox source, ListBox dest)
        {
            if (source != null && dest != null)
            {
                foreach (ListItem item in dest.Items)
                {
                    if (!source.Items.Contains(item))
                        source.Items.Add(item);
                }
                //移除
                dest.Items.Clear();
            }
        }
        ///// <summary>
        ///// 将目标<see cref="ListBoxEx"/> 里的所有项移到源<see cref="ListBoxEx"/> 里。
        ///// </summary>
        ///// <param name="source">源<see cref="ListBoxEx"/></param>
        ///// <param name="dest">目标<see cref="ListBoxEx"/></param>
        //public static void RemoveAll(ListBoxEx source, ListBoxEx dest)
        //{
        //    if (source != null && dest != null)
        //    {
        //        foreach (ListItem item in dest.Items)
        //        {
        //            if (!source.Items.Contains(item))
        //                source.Items.Add(item);
        //        }
        //        //移除
        //        dest.Items.Clear();
        //    }
        //}
        /// <summary>
        /// 初始化列表。
        /// </summary>
        /// <param name="listBox"></param>
        /// <param name="values"></param>
        public static void InitValue(ListBox listBox, NameValueCollection values)
        {
            if (listBox != null)
            {
                listBox.Items.Clear();
                foreach (string key in values.Keys)
                {
                    listBox.Items.Add(new ListItem(values[key], key));
                }
            }
        }
        ///// <summary>
        ///// 初始化列表。
        ///// </summary>
        ///// <param name="listBox"></param>
        ///// <param name="values"></param>
        //public static void InitValue(ListBoxEx listBox, NameValueCollection values)
        //{
        //    if (listBox != null)
        //    {
        //        listBox.Items.Clear();
        //        foreach (string key in values.Keys)
        //        {
        //            listBox.Items.Add(new ListItem(values[key], key));
        //        }
        //    }
        //}
        /// <summary>
        /// 将列表的项向上移。
        /// </summary>
        /// <param name="listBox">列表框对象。</param>
        public static void MoveUp(ListBox listBox)
        {
            if (listBox != null)
            {
                if (listBox.SelectionMode == ListSelectionMode.Multiple || listBox.Items.Count < 2 || listBox.SelectedIndex == -1)
                    return;

                ListItem selectedItem = listBox.SelectedItem, prevItem = null;
                if (listBox.SelectedIndex == 0)
                    prevItem = listBox.Items[listBox.Items.Count - 1];
                else
                    prevItem = listBox.Items[listBox.SelectedIndex - 1];

                string value1 = selectedItem.Value, text1 = selectedItem.Text;
                string value2 = prevItem.Value, text2 = prevItem.Text;

                selectedItem.Text = text2;
                selectedItem.Value = value2;

                prevItem.Text = text1;
                prevItem.Value = value1;

                if (listBox.SelectedIndex > 0)
                    listBox.SelectedIndex += -1;
                else
                    listBox.SelectedIndex = listBox.Items.Count - 1;
            }
        }
        ///// <summary>
        ///// 将列表的项向上移。
        ///// </summary>
        ///// <param name="listBox">列表框对象。</param>
        //public static void MoveUp(ListBoxEx listBox)
        //{
        //    if (listBox != null)
        //    {
        //        if (listBox.SelectionMode == ListSelectionMode.Multiple || listBox.Items.Count < 2 || listBox.SelectedIndex == -1)
        //            return;

        //        ListItem selectedItem = listBox.SelectedItem, prevItem = null;
        //        if (listBox.SelectedIndex == 0)
        //            prevItem = listBox.Items[listBox.Items.Count - 1];
        //        else
        //            prevItem = listBox.Items[listBox.SelectedIndex - 1];

        //        string value1 = selectedItem.Value, text1 = selectedItem.Text;
        //        string value2 = prevItem.Value, text2 = prevItem.Text;

        //        selectedItem.Text = text2;
        //        selectedItem.Value = value2;

        //        prevItem.Text = text1;
        //        prevItem.Value = value1;

        //        if (listBox.SelectedIndex > 0)
        //            listBox.SelectedIndex += -1;
        //        else
        //            listBox.SelectedIndex = listBox.Items.Count - 1;
        //    }
        //}
        /// <summary>
        /// 将列表框的项向下移。
        /// </summary>
        /// <param name="listBox">列表框对象。</param>
        public static void MoveDown(ListBox listBox)
        {
            if (listBox != null)
            {
                if (listBox.SelectionMode == ListSelectionMode.Multiple || listBox.Items.Count < 2 || listBox.SelectedIndex == -1)
                    return;

                ListItem selectedItem = listBox.SelectedItem, lastItem = null;
                if (listBox.SelectedIndex == listBox.Items.Count - 1)
                    lastItem = listBox.Items[0];
                else
                    lastItem = listBox.Items[listBox.SelectedIndex + 1];

                string value1 = selectedItem.Value, text1 = selectedItem.Text;
                string value2 = lastItem.Value, text2 = lastItem.Text;

                selectedItem.Text = text2;
                selectedItem.Value = value2;

                lastItem.Text = text1;
                lastItem.Value = value1;

                if (listBox.SelectedIndex < listBox.Items.Count - 1)
                    listBox.SelectedIndex += 1;
                else
                    listBox.SelectedIndex = 0;
            }
        }
        ///// <summary>
        ///// 将列表框的项向下移。
        ///// </summary>
        ///// <param name="listBox">列表框对象。</param>
        //public static void MoveDown(ListBoxEx listBox)
        //{
        //    if (listBox != null)
        //    {
        //        if (listBox.SelectionMode == ListSelectionMode.Multiple || listBox.Items.Count < 2 || listBox.SelectedIndex == -1)
        //            return;

        //        ListItem selectedItem = listBox.SelectedItem, lastItem = null;
        //        if (listBox.SelectedIndex == listBox.Items.Count - 1)
        //            lastItem = listBox.Items[0];
        //        else
        //            lastItem = listBox.Items[listBox.SelectedIndex + 1];

        //        string value1 = selectedItem.Value, text1 = selectedItem.Text;
        //        string value2 = lastItem.Value, text2 = lastItem.Text;

        //        selectedItem.Text = text2;
        //        selectedItem.Value = value2;

        //        lastItem.Text = text1;
        //        lastItem.Value = value1;

        //        if (listBox.SelectedIndex < listBox.Items.Count - 1)
        //            listBox.SelectedIndex += 1;
        //        else
        //            listBox.SelectedIndex = 0;
        //    }
        //}
    }
}
