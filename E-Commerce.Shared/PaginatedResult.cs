namespace E_Commerce.Shared;

public class PaginatedResult<TEntity>
{
    public PaginatedResult(IEnumerable<TEntity> data, int count, int pageSize, int pageIndex)
    {
        Data = data;
        Count = count;
        PageSize = pageSize;
        PageIndex = pageIndex;
    }
    
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Count { get; set; }
    public IEnumerable<TEntity> Data { get; set; }
    
}