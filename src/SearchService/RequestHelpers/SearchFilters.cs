using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchService.RequestHelpers
{
    public class SearchFilters
    {
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set;} = 4;
        public string Seller  { get; set; }
        public string Winner {set; get;}
        public string OrderBy { get; set; }
        public string FilterBy  { get; set; }
    }
}