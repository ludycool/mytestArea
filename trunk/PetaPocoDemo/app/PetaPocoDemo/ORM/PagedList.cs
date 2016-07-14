/*
 ASP.NET MvcPager control
 Copyright:2009-2010 葛劲
 Source code released under Ms-PL license
 */

using System;
using System.Collections.Generic;

namespace PetaPocoDemo.ORM
{
    public class PagedList<T> : List<T>
    {
        public PagedList(IList<T> items,int pageIndex,int pageSize)
        {
            PageSize = pageSize;
            TotalItemCount = items.Count;
            TotalPageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
            CurrentPageIndex = pageIndex;
            StartRecordIndex=(CurrentPageIndex - 1) * PageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : TotalItemCount;
            for (int i = StartRecordIndex-1; i < EndRecordIndex;i++ )
            {
                Add(items[i]);
            }
        }

        public PagedList(List<T> items, int pageIndex, int pageSize, long totalItemCount)
        {

            AddRange(items);
            TotalItemCount = totalItemCount;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
            StartRecordIndex = (pageIndex - 1) * pageSize + 1;
            EndRecordIndex = TotalItemCount > pageIndex * pageSize ? pageIndex * pageSize : totalItemCount;
        }

        public int CurrentPageIndex { get; set; }
        public int PageSize { get; set; }
        public long TotalItemCount { get; set; }
        public int TotalPageCount{get; private set;}
        public int StartRecordIndex{get; private set;}
        public long EndRecordIndex { get; private set; }
    }
}
