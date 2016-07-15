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
        /// <summary>
        /// Laat een lijst van alle customers zien
        /// </summary>
        /// <returns></returns>

        [HttpGet("getall")]
        public async Task<List<Customer>> GetAllCustomers()
        {
            string query = "SELECT idCustomer, name, address FROM Customer WHERE isDeleted=@0";
            List<Customer> data = new List<Customer>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (reader.Read())
                    data.Add(new Customer(idCustomer: reader["IdCustomer"] as int? ?? -1, name: reader["name"] as string, address: reader["Address"] as string));
            return data;
        }

        [HttpGet("getallarchived")]
        public async Task<List<Customer>> GetAllArchivedCustomers()
        {
            string query = "SELECT idCustomer, name, address FROM Customer WHERE isDeleted=@0";
            List<Customer> data = new List<Customer>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true)))
                while (reader.Read())
                    data.Add(new Customer(idCustomer: reader["IdCustomer"] as int? ?? -1, name: reader["name"] as string, address: reader["Address"] as string));
            return data;
        }


        /// <summary>
        /// voegt een customer toe
        /// </summary>
        /// <param name="Customer"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<string> AddCustomer([FromBody]Customer Customer)
        {
            if (string.IsNullOrWhiteSpace(Customer.Name) || string.IsNullOrWhiteSpace(Customer.Address))
                return "Een van de opgegeven velden is leeg";

            string query = "INSERT INTO Customer (`name`, `address`) VALUES (@0, @1);";
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
        }

        [HttpPost("change")]
        public async Task<bool> EditCustomer([FromBody]Customer Customer)
        {
            if (GetCustomer(Customer.IdCustomer) == null)
                return false;

            string query = "UPDATE Customer SET Name=@0, Address=@1 WHERE IdCustomer=@2";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", Customer.Name),
                new MySqlParameter("@1", Customer.Address),
                new MySqlParameter("@2", Customer.IdCustomer)))
                return reader.RecordsAffected == 1;
        }


        /// <summary>
        /// geeft 1 customer weer om deze te bewerken.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        [HttpGet("archive/{id}")]
        public async Task<bool> ArchiveCustomer(int id)
        {
            if (GetCustomer(id) == null)
                return false;

            string query = "UPDATE Customer SET isDeleted=@0 WHERE idCustomer=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1;
        }

        [HttpGet("restorecustomer/{id}")]
        public async Task<bool> RestoreCustomer(int id)
        {
            if (GetCustomer(id) == null)
                return false;

            string query = "UPDATE Customer SET isDeleted=@0 WHERE idCustomer=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1;
        }
    }
}

