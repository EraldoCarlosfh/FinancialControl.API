namespace FinancialControl.API.Api.Controllers
{
    public class PagedSearchViewResult
    {
        public object? SearchResult { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public int TotalRecords { get; set; }
    }
}
