using InventorySystem.Interfaces;
using InventorySystem.Models.BLModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace InventorySystem.Controllers
{
    public class InventoryController : ApiController
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IHttpActionResult> UpdateInventory([FromBody] List<ItemModel> itemsInventory)
        {
            if (itemsInventory != null && itemsInventory.Count > 0)
            {
                try
                {
                    await _inventoryService.UpdateItemsInventory(itemsInventory);
                    return Ok<String>("Items Updated");
                }
                catch(Exception ex)
                {
                    return InternalServerError(ex);
                }                
            }
            else 
            {
                return Ok<String>("No Items Found to Update");
            }            
        }

        [HttpGet]
        [Route("Expensive/Top/{number}/Price/{minPrice}")]
        public async Task<IHttpActionResult> GetExpensiveItemsReport(int number, int minPrice)
        {
            if (await _inventoryService.IsAnyItemExist())
            {
                var result = await _inventoryService.GetExpensiveItems(number, minPrice);
               
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(result)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "ItemReport.csv"
                };

                return ResponseMessage(response);
            }
            else 
            {
                return BadRequest("No Items exist in your Database");
            }
        }

        private object File(byte[] result, string v1, string v2)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("Summary/{category}")]
        public async Task<IHttpActionResult> GetCategorySummary(String category)
        {
            if (!String.IsNullOrEmpty(category))
            {
                if (await _inventoryService.IsAnyItemExistByCategory(category))
                {
                    var result = await _inventoryService.GetItemsSummaryByCategory(category);
                    return Ok(result);
                }
                else 
                {
                    return Ok($"No Items Exist under {category} Category");
                }
            }
            else
            {
                return BadRequest("Category is not Specified");
            }
        }
    }
}
