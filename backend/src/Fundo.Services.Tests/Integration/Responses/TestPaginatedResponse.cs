using System.Collections.Generic;

namespace Fundo.Services.Tests.Integration.Responses
{
    internal class TestPaginatedResponse<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalItems { get; set; }
        public int TotalPages { get; set; }

        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
        public bool IsEmpty => TotalItems == 0;
    }
}