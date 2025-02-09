namespace curdAngularWebAPI.Model
{
    public class ProductItem
    {
        public int ProductItemID { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public Boolean Status { get; set; }


    }

    public class ProductResponse
    {
        public int APIStatus { get; set; }
        public string Message { get; set; }
        public List<ProductItem> ProductItems { get; set; } = new List<ProductItem>();

    }
}
