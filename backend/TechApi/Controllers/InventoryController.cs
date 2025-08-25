using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Text.Json.Serialization;
using TechApi.Models;



namespace TechApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        [HttpPost]
        public ActionResult SaveInventoryData(Inventory InventoryDto )
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString= "Server=DESKTOP-QO97POE\\SQLEXPRESS;Database=techDb;Trusted_Connection=True;MultipleActiveResultSets=true;"
            };
            SqlCommand command = new SqlCommand
            {
               CommandText= "sp_SaveInventoryData",
               CommandType=System.Data.CommandType.StoredProcedure,
               Connection = connection
            };
            command.Parameters.AddWithValue("@ProductId",InventoryDto.ProductId);
            command.Parameters.AddWithValue("@ProductName", InventoryDto.ProductName);
            command.Parameters.AddWithValue("@StockAvailable", InventoryDto.StockAvailable);
            command.Parameters.AddWithValue("@ReorderStock", InventoryDto.ReorderStock);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return Ok("Inventory Data Saved!");
        }



        [HttpGet]
        public ActionResult GetInventoryData()
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = "Server=DESKTOP-QO97POE\\SQLEXPRESS;Database=techDb;Trusted_Connection=True;MultipleActiveResultSets=true;"
            };
            SqlCommand command = new SqlCommand
            {
                CommandText = "sp_GetInventoryData",
                CommandType = System.Data.CommandType.StoredProcedure,
                Connection = connection
            };
           
            connection.Open();

            List<InventoryDto> response = new List<InventoryDto>();

            using (SqlDataReader sqlDataReader = command.ExecuteReader())
            {
                while (sqlDataReader.Read())
                {
                    InventoryDto inventoryDto = new InventoryDto();
                    inventoryDto.ProductId = Convert.ToInt32(sqlDataReader["ProductId"]);
                    inventoryDto.ProductName = Convert.ToString(sqlDataReader["ProductName"]);
                    inventoryDto.StockAvailable = Convert.ToInt32(sqlDataReader["StockAvailable"]);
                    inventoryDto.ReorderStock = Convert.ToInt32(sqlDataReader["ReorderStock"]);

                    response.Add(inventoryDto);
                }
            }
            command.ExecuteReader();
            connection.Close();
            return Ok(JsonConvert.SerializeObject(response));
        }


        [HttpDelete]
        public ActionResult DeleteInventoryData(int ProductId)
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = "Server=DESKTOP-QO97POE\\SQLEXPRESS;Database=techDb;Trusted_Connection=True;MultipleActiveResultSets=true;"
            };
            SqlCommand command = new SqlCommand
            {
                CommandText = "sp_DeleteInventoryDetails",
                CommandType = System.Data.CommandType.StoredProcedure,
                Connection = connection
            };
            command.Parameters.AddWithValue("@ProductId", ProductId);
            
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return Ok();
        }
    }
}
