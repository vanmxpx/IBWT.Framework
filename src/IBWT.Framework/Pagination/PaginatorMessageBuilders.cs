using System;
using System.Collections.Generic;
using System.Text;


namespace IBWT.Framework.Pagination
{
    public delegate string PaginatorMessageBuilder<T>(T[] data, int startIndex, int itemsPerPage);

    public static class PaginatorMessageBuilders
    {
        public static string DefaultListBuilder<T>(T[] data, int startIndex, int itemsPerPage)
        {
            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < data.Length && i < itemsPerPage; i++)
            {
                sb.Append($"{i + startIndex + 1}. {data[i].ToString()}{Environment.NewLine}");
            }
            
            return sb.ToString();
        }
    }
}