using InventorySystem.Models.BLModels;
using InventorySystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Interfaces
{
    public interface IInventoryService
    {
        Task UpdateItemsInventory(List<ItemModel> itemsModel);

        Task<Boolean> IsAnyItemExist();

        Task<Boolean> IsAnyItemExistByCategory(String category);

        Task<CategorySummary> GetItemsSummaryByCategory(String category);

        Task<Byte[]> GetExpensiveItems(int noOfRecordsReq, int minExpensivePrice);
    }
}
