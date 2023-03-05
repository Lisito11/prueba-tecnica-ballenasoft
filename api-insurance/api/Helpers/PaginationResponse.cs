using System;
namespace api.Helpers
{
    public class PaginationResponse<T> : ResponseBase<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public PaginationResponse(T data, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.Succeeded = true;
            this.Error = null;
        }

        public PaginationResponse()
        {

        }

        public static PaginationResponse<List<T>> CreatePaginationReponse(List<T> pagedData, PaginationFilter validFilter, int totalRecords)
        {
            var response = new PaginationResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            response.TotalPages = roundedTotalPages;
            response.TotalRecords = totalRecords;
            return response;
        }



    }
}

