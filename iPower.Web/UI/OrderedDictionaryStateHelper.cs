//================================================================================
//  FileName: OrderedDictionaryStateHelper.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2011/3/9
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
namespace iPower.Web.UI
{
    /// <summary>
    /// 
    /// </summary>
    internal static class OrderedDictionaryStateHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="state"></param>
        public static void LoadViewState(IOrderedDictionary dictionary, ArrayList state)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            if (state == null)
                throw new ArgumentNullException("state");
            if (state != null)
            {
                for (int i = 0; i < state.Count; i++)
                {
                    Pair pair = (Pair)state[i];
                    dictionary.Add(pair.First, pair.Second);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static ArrayList SaveViewState(IOrderedDictionary dictionary)
        {
            if (dictionary == null)
                 throw new ArgumentNullException("dictionary");
             ArrayList list = new ArrayList(dictionary.Count);
            foreach (DictionaryEntry entry in dictionary)
            {
                list.Add(new Pair(entry.Key, entry.Value));
            }
            return list;
        }
    }


}
