using InventorySystem.Interfaces;
using InventorySystem.Models;
using InventorySystem.Models.BLModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace InventorySystem.Repository
{
    public class BaseRepository : IBaseRepository
    { 
        private MySqlConnection connection;
        public void Connect()
        {
            try
            {
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;
                connection = new MySqlConnection(connectionString);
                connection.Open();
            }
            catch
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }

        public async Task<bool> Exist()
        {
            var bResult = false;
            Connect();
            try 
            {
                var query = "SELECT EXISTS (SELECT 1 FROM InventoryTable WHERE ItemID > 0) AS RowExists;";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    var result = cmd.ExecuteScalar();
                    bResult = Convert.ToBoolean(result);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Dispose();
            }

            return bResult;
        }

        public async Task<bool> ExistByCategory(string category)
        {
            var bResult = false;
            Connect();
            try
            {
                var query = $"SELECT EXISTS (SELECT 1 FROM InventoryTable WHERE Category = \'{category}\') AS RowExists;";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    var result = cmd.ExecuteScalar();
                    bResult = Convert.ToBoolean(result);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Dispose();
            }

            return bResult;
        }

        public async Task<List<InventoryEntityModel>> GetExpensiveItems(int number, int price)
        {
            List<InventoryEntityModel> result = null;
            try
            {
                Connect();
                var query = $"SELECT ItemID, Name, Category, Price, Stock FROM InventoryTable WHERE Price >= {price} LIMIT {number}";
                var dataTable = new DataTable();

                using (var cmd = new MySqlCommand(query, connection))
                {
                    var dataReader = cmd.ExecuteReader();
                    dataTable.Load(dataReader);
                    dataReader.Close();
                }
                if (dataTable != null)
                {
                    var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(dataTable);
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InventoryEntityModel>>(jsonResult);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Dispose();
            }

            return result;
        }

        public async Task<List<InventoryEntityModel>> GetItemsByCategory(string category)
        {
            List<InventoryEntityModel> result = null;
            try
            {
                Connect();
                var query = $"SELECT ItemID, Name, Category, Price, Stock FROM InventoryTable WHERE Category = \'{category}\'";
                var dataTable = new DataTable();
                
                using (var cmd = new MySqlCommand(query, connection))
                {
                    var dataReader = cmd.ExecuteReader();
                    dataTable.Load(dataReader);
                    dataReader.Close();
                }
                if (dataTable != null)
                {
                    var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(dataTable);
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InventoryEntityModel>>(jsonResult);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Dispose();
            }

            return result;
        }

        public async Task Insert<T>(List<T> entities)
        {
            try
            {
                Connect();

                if (entities is List<ItemModel>)
                {
                    var itemsList = entities as List<ItemModel>;
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = "INSERT INTO InventoryTable (Name,Category,Price,Stock) VALUES (@Name,@Category,@Price,@Stock)";
                            using (var cmd = new MySqlCommand(query, connection, transaction))
                            {
                                foreach (var item in itemsList)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@Name", item.Name);
                                    cmd.Parameters.AddWithValue("@Category", item.Category);
                                    cmd.Parameters.AddWithValue("@Price", item.Price);
                                    cmd.Parameters.AddWithValue("@Stock", item.Stock);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Dispose();
                            throw;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        public async Task Update<T>(List<T> entities)
        {
            try
            {
                Connect();

                if (entities is List<ItemModel>)
                {
                    var itemsList = entities as List<ItemModel>;
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            var query = "UPDATE InventoryTable SET Stock = @Stock WHERE ItemID = @ItemID";
                            using (var cmd = new MySqlCommand(query, connection, transaction))
                            {
                                foreach (var item in itemsList)
                                {
                                    cmd.Parameters.Clear();
                                    cmd.Parameters.AddWithValue("@ItemID", item.ItemID);
                                    cmd.Parameters.AddWithValue("@Stock", item.Stock);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Dispose();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Dispose();
            }
        }
    }
}