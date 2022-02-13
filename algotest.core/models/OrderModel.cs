using System;
using System.Collections.Generic;

namespace algotest.core.models
{
    public class OrderAddModel
    {
        public List<OrderProductModel> Products { get; set; }
    }
    public class OrderProductModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
