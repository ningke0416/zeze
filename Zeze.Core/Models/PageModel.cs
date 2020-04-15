using System.Collections.Generic;

namespace Zeze.Core.Models
{
    public class PageModel<T>
    {
        public int TotalCount { get; set; } = 0;
        public List<T> data { get; set; }
    }
}
