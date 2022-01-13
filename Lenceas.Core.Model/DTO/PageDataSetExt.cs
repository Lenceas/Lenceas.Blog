namespace Lenceas.Core.Model
{
    public static class PageDataSetExt
    {
        public static PageViewModel<T> AsPageViewModel<T>(this IList<T> viewModelList, int pageIndex, int pageSize)
        {
            return new PageViewModel<T>()
            {
                ViewModelList = viewModelList,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecords = viewModelList.Count,
                TotalPages = (int)Math.Ceiling(viewModelList.Count / (double)pageSize),
                HasPreviousPage = pageIndex > 1,
                HasNextPage = pageIndex < (int)Math.Ceiling(viewModelList.Count / (double)pageSize)
            };
        }

        public static PageViewModel<T> AsPageViewModel<T, N>(this PageDataSet<N> pageEntity, Func<IList<N>, IList<T>> func)
        {
            return new PageViewModel<T>()
            {
                ViewModelList = func(pageEntity),
                PageIndex = pageEntity.PageIndex,
                PageSize = pageEntity.PageSize,
                TotalRecords = (int)pageEntity.TotalRecords,
                TotalPages = pageEntity.TotalPages,
                HasPreviousPage = pageEntity.HasPreviousPage,
                HasNextPage = pageEntity.HasNextPage
            };
        }
    }
}
