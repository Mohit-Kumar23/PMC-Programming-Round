using InventorySystem.Interfaces;
using InventorySystem.Models;
using InventorySystem.Models.BLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace InventorySystem.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IBaseRepository _baseRepository;

        private readonly int MINIMUM_EXPENSIVE_PRICE = 100;
        private readonly int MIN_NUMBER_RECORDS = 10;

        public InventoryService(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async Task<Byte[]> GetExpensiveItems(int noOfRecordsReq, int minExpensivePrice)
        {
            byte[] docBytes = null;
            if (noOfRecordsReq == null || noOfRecordsReq == 0)
            {
                noOfRecordsReq = MIN_NUMBER_RECORDS;
            }
            if (minExpensivePrice == null || minExpensivePrice == 0)
            {
                minExpensivePrice = MINIMUM_EXPENSIVE_PRICE;
            }

            List<InventoryEntityModel> itemRecords = await _baseRepository.GetExpensiveItems(noOfRecordsReq, minExpensivePrice);
            var itemReport = new List<ItemReport>();

            if (itemRecords != null)
            {
                foreach (var item in itemRecords)
                {
                    ItemReport obj = new ItemReport()
                    {
                        ItemID = item.ItemID,
                        Name = item.Name,
                        Price = item.Price,
                        Category = item.Category
                    };
                    itemReport.Add(obj);
                }

                docBytes = ConvertDataRecordToCsv<ItemReport>(itemReport);
            }
            return docBytes;
        }

        public async Task<CategorySummary> GetItemsSummaryByCategory(string category)
        {
            List<InventoryEntityModel> itemsRecord = await _baseRepository.GetItemsByCategory(category);
            CategorySummary result = new CategorySummary();
            result.CategoryName = category;

            if (itemsRecord != null)
            {
                var itemsSummary = new List<ItemSummary>();

                foreach(var item in itemsRecord)
                {
                    ItemSummary obj = new ItemSummary
                    {
                        ItemID = item.ItemID,
                        Name = item.Name,
                        Price = item.Price,
                        Stock = item.Stock
                    };

                    itemsSummary.Add(obj);
                }

                result.ItemSummaryList = itemsSummary.OrderBy(x => x.Price);
            }
            return result;
        }

        public async Task<bool> IsAnyItemExist()
        {
            return await _baseRepository.Exist();
        }

        public async Task<bool> IsAnyItemExistByCategory(string category)
        {
            return await _baseRepository.ExistByCategory(category);
        }

        public async Task UpdateItemsInventory(List<ItemModel> itemsModel)
        {
            if (itemsModel.Any(x => x.ItemID != null && x.ItemID != 0))
            {
                List<ItemModel> itemsToBeUpdated = itemsModel.FindAll(x => x.ItemID != null && x.ItemID != 0);
                await _baseRepository.Update<ItemModel>(itemsToBeUpdated);
            }

            if (itemsModel.Any(x => x.ItemID == null || x.ItemID == 0))
            {
                List<ItemModel> itemsToBeAdded = itemsModel.FindAll(x => x.ItemID == null || x.ItemID == 0);
                await _baseRepository.Insert<ItemModel>(itemsToBeAdded);
            }
        }

        private byte[] ConvertDataRecordToCsv<T>(List<T> data)
        {
            var stringBuilder = new StringBuilder();
            var properties = typeof(T).GetProperties();

            stringBuilder.AppendLine(String.Join(",", properties.Select(x => x.Name)));

            foreach (var record in data)
            {
                var values = properties.Select(p => p.GetValue(record)?.ToString());
                stringBuilder.AppendLine(String.Join(",", values));
            }

            return Encoding.UTF8.GetBytes(stringBuilder.ToString());
        }
    }
}