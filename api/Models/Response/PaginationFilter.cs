namespace api.Models.Response
{
    public class PaginationFilter
    {
        public string Search { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }
        public int TotalRegisters { get; set; }
        public bool Calendario { get; set; } = false;
        public DateTime DtEvento { get; set; }
        public PaginationFilter()
        {
        }
        public PaginationFilter(string search, int pageNumber, int pageSize, int totalRegisters = 0)
        {
            this.Search = search;
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
            this.TotalRegisters = totalRegisters;
        }
    }
}
