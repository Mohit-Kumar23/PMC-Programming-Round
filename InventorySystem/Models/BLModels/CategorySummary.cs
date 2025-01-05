using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventorySystem.Models.BLModels
{
    public class CategorySummary
    {
        public String CategoryName { get; set; }

        public IEnumerable<ItemSummary> ItemSummaryList { get; set; }
    }

    public class ItemSummary
    {
        public int ItemID { get; set; }

        public String Name { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }
    }
}