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

    public class PostAPI
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int mobileNumber { get; set; }
        public string address { get; set; }
        public string gender { get; set; }
    }

    public class PostAPIResponse
    {
        public int postApiID { get; set; }
        public string message { get; set; }
        public bool status { get; set; }
    }

    public class GetApiResponse
    {
        public int postApiID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int mobileNumber { get; set; }
        public string address { get; set; }
        public string gender { get; set; }
    }

    public class MasterResponse
    {
        public string message { get; set; }
        public bool status { get; set; }

        public int totalRecord { get; set; }

        public List<GetApiResponse>? ProductItems { get; set; }
    }
}
