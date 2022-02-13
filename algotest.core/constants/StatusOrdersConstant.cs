using System;
using System.Collections.Generic;

namespace algotest.core.constants
{
    public class StatusOrdersConstant
    {
        public static Dictionary<int, string> Dict = new Dictionary<int, string>()
        {
            {Pending,  "Pending"},
            {Delivered, "Delivered"}
        };
        public const int Pending = 1;
        public const int Delivered = -1;
    }
}
