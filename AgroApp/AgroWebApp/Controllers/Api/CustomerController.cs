using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using AgroApp.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Api
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        [HttpPost("add")]
        public async Task<bool> AddMachine([FromBody]Machine machine)
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Machine WHERE number=@0";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", machine.Number)))
                    if (reader.HasRows)
                        return false;

                query = "INSERT INTO Machine (name, number, tag, type, status) VALUES (@0, @1, @2, @3, @4)";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", machine.Name),
                    new MySqlParameter("@1", machine.Number),
                    new MySqlParameter("@2", machine.Tag),
                    new MySqlParameter("@3", machine.Type),
                    new MySqlParameter("@4", machine.Status)))
                    return reader.RecordsAffected == 1;
            }
        }

        [HttpGet("getall")]
        public async Task<List<Customer>> GetAllCustomers()
        {
            string query = "SELECT idCustomer, name, address FROM Customer WHERE isDeleted=@0";
            List<Customer> data = new List<Customer>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (reader.Read())
                    data.Add(new Customer(idCustomer: reader.GetInt32(3), name: reader["name"] as string, address: reader.GetString(0)));
            return data;
        }

        [HttpPost("register")]
        public async Task<string> AddCustomer([FromBody]Customer Customer)
        {
            if (string.IsNullOrWhiteSpace(Customer.Name) || string.IsNullOrWhiteSpace(Customer.Address))
                return "Een van de opgegeven velden is leeg";

            string query = "INSERT INTO Customer (`name`, `username`) VALUES (@0, @1,);";
            try
            {
                using (MySqlConnection conn = await DatabaseConnection.GetConnection())
                {
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                      new MySqlParameter("@0", Customer.Name),
                      new MySqlParameter("@1", Customer.Address));
                    return "succes";
                }
            }
            catch { return "Er is iets misgegaan, neem contact op met een ontwikkelaar!"; }

            public static async Task<Customer> GetCustomer(int id)
        {
            if (id < 0)
                return null;

            string query = "SELECT idCustomer, name, address FROM Customer WHERE idCustomer=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", id)))
            {
                await reader.ReadAsync();
                return reader.HasRows ? new Customer(reader["idCustomer"] as int? ?? -1, reader["name"] as string, reader["address"] as string) : null;
            }
        }
    }
    }
}
