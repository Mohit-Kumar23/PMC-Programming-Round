using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventorySystem.Models.BLModels
{
    public class ItemReport
    {
        public int ItemID { get; set; }

        public String Name { get; set; }

        public double Price { get; set; }

        public String Category { get; set; }
    }
}