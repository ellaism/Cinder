using System.Collections.Generic;

namespace Cinder.UI.Infrastructure.Paging
{
    public interface IPage<T>
    {
        int Page { get; set; }
        int Size { get; set; }
        int Total { get; set; }
        IEnumerable<T> Items { get; set; }
    }
}
