using InventorySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Interfaces
{
    public interface IBaseRepository 
    {
        Task Insert<T> (List<T> entities);

        Task Update<T>(List<T> entities);

        Task<Boolean> Exist();

        Task<Boolean> ExistByCategory(string category);

        Task<List<InventoryEntityModel>> GetItemsByCategory(string category);

        Task<List<InventoryEntityModel>> GetExpensiveItems(int number, int price);
    }
}
