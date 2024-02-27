namespace my_books.Data.Paging
{
    //This willbe the Gerneric class, you can use throwout the system
    public class PaginatedList<T> : List<T>
    {
        //Define page index and total pages

        public int PageIndex { get; private set; }

        public int TotalPages { get; private set; }

        //Create a constructor

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex; //setting the page index

            TotalPages = (int)Math.Ceiling(count / (double)pageSize); // based upo the number of items and page size, thats how you get the total pages

            this .AddRange(items);
        }
        
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
        
       public static PaginatedList<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

    }
}
   