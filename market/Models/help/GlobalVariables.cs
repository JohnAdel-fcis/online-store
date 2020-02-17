using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace market.Models.help
{
    public class GlobalVariables
    {
        public GlobalVariables()
        {
            counter = 0;
        }

        public static User LoginUser { get; set; }
        public static Basket MyBasket { get; set; }
        

        public static int counter { get; set; }
        public static bool admin { get; set; }
    }
}