using RetailECommercePlatform.Data.ResponseModels.Query.Product;

namespace RetailECommercePlatform.Data.ResponseModels.Query.Cart;

public class GetCartQueryResponse
{
    public List<ProductForCartModel> Products { get; set; }
    public decimal TotalPrice { get; set; }
}

public class ProductForCartModel : GetProductQueryResponse
{
    public int Quantity { get; set; }
}