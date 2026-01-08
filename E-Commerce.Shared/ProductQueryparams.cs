namespace E_Commerce.Shared;

public class ProductQueryparams
{
    public int? BrandId { get; set; }
    public int? TypeId { get; set; }
    public int? SubTypeId { get; set; }
    public string? Search { get; set; }
    public ProductSortingOptions Sort { get; set; }
    
    private int _pageIndex = 1;

    public int PageIndex
    {
        get =>  _pageIndex; 
        set =>  _pageIndex = (value <= 0 ) ? 1 : value;
    }

    private const int DefaultPageSize = 8;
    private const int MaxPageSize = 10;
    
    private int _pageSize = DefaultPageSize;
    
    public int PageSize
    {
        get => _pageSize;

        set
        {
            if (value <= 0)
                _pageSize = DefaultPageSize;
            else if (value > MaxPageSize)
                _pageSize = MaxPageSize;
            else
                _pageSize = value;
        }
    }

}