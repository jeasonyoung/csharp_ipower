//================================================================================
//  FileName: SortHelper.cs
//  Desc:
//
//  Called by
//
//  Auth:杨勇（jeason1914@gmail.com）
//  Date: 2013-4-26
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
using System.Text;

namespace iPower.Utility
{
    /// <summary>
    /// 快速排序。
    /// </summary>
    public static class SortHelper
    {
        /// <summary>
        /// 快速排序
        /// </summary>
        /// <typeparam name="T">排序类型</typeparam>
        /// <param name="sources">排序源数组</param>
        /// <param name="comparison">判定条件</param>
        public static void QuickSort<T>(List<T> sources, Comparison<T> comparison)
        {
            if (sources == null || sources.Count == 0 || sources.Count == 1)
            {
                return;
            }
            QuickSort<T>(sources, 0, sources.Count - 1, comparison);
        }
        /// <summary>
        /// 快速排序
        /// </summary>
        /// <typeparam name="T">排序类型</typeparam>
        /// <param name="sources">排序源数组</param>
        /// <param name="low">低位</param>
        /// <param name="high">高位</param>
        /// <param name="comparison">判定条件</param>
        private static void QuickSort<T>(List<T> sources, int low, int high, Comparison<T> comparison)
        {
            if (low < high)
            {
                int leftIndex = low;
                int rightIndex = high;

                T item = sources[leftIndex];
                T temp;

                while (leftIndex < rightIndex)
                {
                    while (leftIndex < rightIndex && comparison(sources[leftIndex], item) < 1) { leftIndex++; }
                    while (leftIndex < rightIndex && comparison(sources[rightIndex], item) == 1) { rightIndex--; }

                    if (leftIndex < rightIndex)
                    {
                        temp = sources[leftIndex];
                        sources[leftIndex] = sources[rightIndex];
                        sources[rightIndex] = temp;
                    }
                }

                temp = item;

                int position = comparison(temp, sources[rightIndex]) == -1 ? rightIndex - 1 : rightIndex;
                sources[low] = sources[position];
                sources[position] = temp;

                QuickSort<T>(sources, low, position - 1, comparison);
                QuickSort<T>(sources, position + 1, high, comparison);
            }
        }

        /// <summary>
        /// 移动指点成员到队列最后
        /// </summary>
        /// <typeparam name="TItem">成员类型</typeparam>
        /// <param name="items">成员数组</param>
        /// <param name="moveIndex">移动下标</param>
        public static void MoveItemToEnd<TItem>(List<TItem> items, int moveIndex)
        {
            if (items != null && items.Count > 0 && moveIndex < items.Count - 1)
            {
                TItem item = items[moveIndex];

                if (items.Contains(item))
                {
                    items.Remove(item);
                    items.Add(item);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="items"></param>
        /// <param name="moveIndex"></param>
        /// <param name="toIndex"></param>
        public static void MoveItemToIndex<TItem>(List<TItem> items, int moveIndex, int toIndex)
        {
            if (toIndex == moveIndex)
            {
                return;
            }
            if (items != null && items.Count > 0 && moveIndex >= 0 && moveIndex < items.Count && toIndex >= 0 && toIndex < items.Count)
            {
                if (toIndex > moveIndex)
                {
                    items.Insert(toIndex + 1, items[moveIndex]);
                    items.RemoveAt(moveIndex);
                }
                else if (toIndex < moveIndex)
                {
                    items.Insert(toIndex, items[moveIndex]);
                    items.RemoveAt(moveIndex + 1);
                }
            }
        }

    }
}