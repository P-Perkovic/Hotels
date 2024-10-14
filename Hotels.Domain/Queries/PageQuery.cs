using System;
using System.Collections.Generic;
using System.Text;

namespace Hotels.Domain.Queries
{
    public class PageQuery
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public bool Validate()
        {
            return PageSize < 1;
        }
    }
}
