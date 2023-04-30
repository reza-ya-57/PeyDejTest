using Microsoft.EntityFrameworkCore;

namespace PeyDej.Services.Pagination;
public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }

    public PaginatedList()
    {
        
    }
    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize) : this()
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        PageSize = pageSize;
        this.AddRange(items);
        SelectedFruits = new List<string>();
    }

    public bool HasPreviousPage => PageIndex > 1;

    public IList<string> SelectedFruits { get; set; }
    public bool HasNextPage => PageIndex < TotalPages;
    public int PageSize { get; set; }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}
