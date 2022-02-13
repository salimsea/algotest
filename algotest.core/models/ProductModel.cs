using System;
namespace algotest.core.models
{
    public class ProductModelDefault
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }
    public class ProductModel : ProductModelDefault
    {
        public int Id { get; set; }
        public string CreatedAt{ get; set; }
    }
    public class ProductAddModel : ProductModelDefault { }
    public class ProductUpdateModel : ProductModelDefault
    {
        public int Id { get; set; }
    }


}
