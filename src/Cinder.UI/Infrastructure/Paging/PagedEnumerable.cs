using System.Collections.Generic;

namespace Cinder.UI.Infrastructure.Paging
{
    public class PagedEnumerable<T> : IPage<T>
    {
        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
