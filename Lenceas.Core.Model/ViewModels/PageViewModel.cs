namespace Lenceas.Core.Model
{
    public class PageViewModel<T>
    {
        public IList<T> ViewModelList { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }
    }

    public class PageViewModel<T1, T2> : PageViewModel<T1>
    {
        public T2 Data { get; set; }
    }
}
