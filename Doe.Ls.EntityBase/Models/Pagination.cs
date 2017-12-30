using System.Collections.Generic;

namespace Doe.Ls.EntityBase.Models
{
    public class Pagination
    {
        public List<PaginationItem> PaginationItems { get; private set; }
        public int PageSize { get; set; }

        public int OriginalStart { get; }

        public int OriginalLength { get; private set; }

        private Pagination(int pageSize, int originalStart, int originalLength):this()
        {
            PageSize = pageSize;
            OriginalLength = originalLength;
            OriginalStart = originalStart;
        }
        private Pagination()
        {
            
        }
        public static Pagination ConstructPagination(int pageSize, int originalStart, int originalLength)
        {
            var pagination=new Pagination(pageSize, originalStart, originalLength);

            var noOfPages = originalLength / pageSize + (originalLength % pageSize > 0 ? 1 : 0);
            
            var items=new List<PaginationItem>();
            for (var i = 1; i <= noOfPages; i++)
            {
                var start= originalStart + (i - 1) * pageSize;

                var length= (i) * pageSize <= originalLength ? pageSize : originalLength % pageSize;

                items.Add(new PaginationItem(start,length,pagination));
            }



            pagination.PaginationItems = items;
            return pagination;
        }
    }
}
