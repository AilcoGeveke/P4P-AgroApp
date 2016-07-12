using AgroApp.Models;
using Microsoft.AspNet.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Api
{
    [Route("api/[controller]")]
    public class WorkOrderController : Controller
    {
        // GET: api/values
        [HttpGet("getWorkType")]
        public async Task<IEnumerable<string>> GetWorkType()
        {
            string query = "SELECT WorkType FROM WorkType";
            List<string> data = new List<string>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query))
                while (await reader.ReadAsync())
                    data.Add(reader.GetString(0));
            return data;
        }

        //Machines
        [HttpGet("getmachine")]
        private async Task<Machine> _GetMachine(int id)
        {
            return await GetMachine(id);
        }

        public static async Task<Machine> GetMachine(int id)
        {
            if (id < 0)
                return null;

            string query = "SELECT idMachine, type, number, name, tag, status FROM Machine WHERE idMachine=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", id)))
            {
                await reader.ReadAsync();
                return reader.HasRows ? new Machine(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetString(5)) : null;
            }
        }

        [HttpGet("getmachines")]
        public async Task<string> GetMachines()
        {
            string query = "SELECT name, number, tag, idMachine FROM Machine WHERE isDeleted=@0";
            List<Machine> data = new List<Machine>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (reader.Read())
                    data.Add(new Machine(idMachine: reader.GetInt32(3), tag: reader.GetString(2), number: reader.GetInt32(1), name: reader.GetString(0)));
            return JsonConvert.SerializeObject(data);
        }

        [HttpGet("getarchiefmachine")]
        public async Task<string> GetArchiefMachines()
        {
            string query = "SELECT name, number, tag, idMachine FROM Machine WHERE isDeleted=@0";
            List<Machine> data = new List<Machine>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true)))
                while (reader.Read())
                    data.Add(new Machine(idMachine: reader.GetInt32(3), tag: reader.GetString(2), number: reader.GetInt32(1), name: reader.GetString(0)));
            return JsonConvert.SerializeObject(data);
        }

        [HttpGet("addmachine/{name}/{number}/{tag}/{type}")]
        public async Task<bool> AddMachine(string name, string type, int number = 0, string tag = "")
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Machine WHERE number=@0";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", number)))
                    if (reader.HasRows)
                        return false;

                query = "INSERT INTO Machine (name, number, tag, type, status) VALUES (@0, @1, @2, @3, @4)";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", name),
                    new MySqlParameter("@1", number),
                    new MySqlParameter("@2", tag),
                    new MySqlParameter("@3", type),
                    new MySqlParameter("@4", "")))
                    return reader.RecordsAffected == 1;
            }
        }

        [HttpGet("editmachine/{idmachine}/{name}/{number}/{tag}/{type}")]
        public async Task<bool> EditMachine(int id, string name, string type = "Kranen", int number = 0, string tag = "")
        {
            if (GetMachine(id) == null)
                return false;

            string query = "UPDATE Machine SET name=@0, number=@1, tag=@2, type=@3 WHERE idMachine=@4";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", name),
                new MySqlParameter("@1", number),
                new MySqlParameter("@2", tag),
                new MySqlParameter("@3", type),
                new MySqlParameter("@4", idMachine)))
                return reader.RecordsAffected == 1;
        }

        [HttpGet("deletemachine/{id}")]
        public async Task<bool> DeleteMachine(int id)
        {
            if (GetMachine(id) == null)
                return false;

            string query = "UPDATE Machine SET isDeleted=@0 WHERE idMachine=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1; ;
        }

        [HttpGet("machine/terughalen/{id}")]
        public async Task<bool> ReAddMachine(int id)
        {
            if (GetMachine(id) == null)
                return false;

            string query = "UPDATE Machine SET isDeleted=@0 WHERE idMachine=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1; ;
        }

        //Attachment
        public static async Task<Attachment> GetAttachment(int id)
        {
            if (id < 0)
                return null;

            string query = "SELECT idAttachment, name, number FROM Attachment WHERE idAttachment=@0";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", id)))
            {
                await reader.ReadAsync();
                return reader.HasRows ? new Attachment(idAttachment: reader.GetInt32(0), name: reader.GetString(1), number: reader.GetInt32(2)) : null;
            }
        }

        [HttpGet("getAttachmentken")]
        public async Task<string> GetAttachmentken()
        {
            string query = "SELECT idAttachment, number, name, isDeleted FROM Attachment WHERE isDeleted = @0";
            List<Attachment> data = new List<Attachment>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (reader.Read())
                    data.Add(new Attachment(idAttachment: reader.GetInt32(0), number: reader.GetInt32(1), name: reader.GetString(2)));
            return JsonConvert.SerializeObject(data);
        }

        [HttpGet("getarchiefAttachmentken")]
        public async Task<string> GetArchiefAttachmentken()
        {
            string query = "SELECT idAttachment, number, name, isDeleted FROM Attachment WHERE isDeleted = @0";
            List<Attachment> data = new List<Attachment>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true)))
                while (reader.Read())
                    data.Add(new Attachment(idAttachment: reader.GetInt32(0), number: reader.GetInt32(1), name: reader.GetString(2)));
            return JsonConvert.SerializeObject(data);
        }

        [HttpGet("addAttachment/{name}/{number}")]
        public async Task<bool> AddAttachment(string name, int number = 0)
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Attachment WHERE name=@0 AND number=@1";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", name),
                    new MySqlParameter("@1", number)))
                    if (reader.HasRows)
                        return false;

                query = "INSERT INTO Attachment (name, number) VALUES (@0, @1)";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", name),
                    new MySqlParameter("@1", number)))
                    return reader.RecordsAffected == 1;
            }
        }

        [HttpGet("editAttachment/{id}/{name}/{number}")]
        public async Task<bool> EditAttachment(int id, string name, string number)
        {
            if (GetAttachment(id) == null)
                return false;

            string query = "UPDATE Attachment SET name=@0, number=@1 WHERE idAttachment=@2";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", name),
                new MySqlParameter("@1", number),
                new MySqlParameter("@2", id)))
                return reader.RecordsAffected == 1;
        }

        [HttpGet("deleteAttachment/{id}")]
        public async Task<bool> DeleteAttachment(int id)
        {
            if (GetAttachment(id) == null)
                return false;

            string query = "UPDATE Attachment SET isDeleted=@0 WHERE idAttachment=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1; ;
        }

        [HttpGet("Attachment/terughalen/{id}")]
        public async Task<bool> ReAddAttachment(int id)
        {
            if (GetAttachment(id) == null)
                return false;

            string query = "UPDATE Attachment SET isDeleted=@0 WHERE idAttachment=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1; ;
        }

        //Customers
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
                return reader.HasRows ? new Customer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)) : null;
            }
        }

        [HttpGet("getCustomers")]
        public async Task<string> GetCustomers()
        {
            string query = "SELECT * FROM Customer WHERE isDeleted=@0";
            List<Customer> data = new List<Customer>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (reader.Read())
                    data.Add(new Customer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
            return JsonConvert.SerializeObject(data);

        }

        [HttpGet("getarchiefCustomers")]
        public async Task<string> GetArchiefCustomers()
        {
            string query = "SELECT * FROM Customer WHERE isDeleted=@0";
            List<Customer> data = new List<Customer>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true)))
                while (reader.Read())
                    data.Add(new Customer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
            return JsonConvert.SerializeObject(data);

        }

        [HttpGet("addCustomer/{name}/{address}")]
        public async Task<bool> AddCustomer(string name, string address)
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Customer WHERE name=@0 AND address=@1";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", name),
                    new MySqlParameter("@1", address)))
                    if (reader.HasRows)
                        return false;

                query = "INSERT INTO Customer (name, address) VALUES (@0, @1)";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", name),
                    new MySqlParameter("@1", address)))
                    return reader.RecordsAffected == 1;
            }
        }

        [HttpGet("editCustomer/{id}/{name}/{address}")]
        public async Task<bool> EditCustomer(int id, string name, string address)
        {
            if (GetCustomer(id) == null)
                return false;

            string query = "UPDATE Customer SET name=@0, address=@1 WHERE idCustomer=@2";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", name),
                new MySqlParameter("@1", address),
                new MySqlParameter("@2", id)))
                return reader.RecordsAffected == 1;
        }

        [HttpGet("deleteCustomer/{id}")]
        public async Task<bool> DeleteCustomer(int id)
        {
            bool isDeleted = true;
            if (GetCustomer(id) == null)
                return false;

            string query = "UPDATE Customer SET isDeleted=@0 WHERE idCustomer=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", isDeleted),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1; ;
        }

        [HttpGet("Customer/terughalen/{id}")]
        public async Task<bool> ReAddCustomer(int id)
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

        //Assignment
        [HttpGet("addAssignment")]
        public async Task<bool> AddAssignment()
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                string query = "SELECT * FROM Machine WHERE number=@0";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query))

                    if (reader.HasRows)
                        return false;

                query = "INSERT INTO Assignment (location, description, idCustomer) VALUES (@0, @1, @2, @3, @4)";

                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@4", "")))
                    return reader.RecordsAffected == 1;
            }
            throw new NotImplementedException();
        }

        // GET: /admin/WorkOrdercheck
        [HttpGet("admin/WorkOrder/controleren")]
        public IActionResult Assignment()
        {
            return View("../admin/WorkOrderCheck");
        }

        [HttpGet("getdata")]
        public async Task<string> GetData()
        {
            string query = "SELECT Assignment.location, Customer.name FROM Assignment JOIN EmployeeAssignment ON Assignment.idAssignment = EmployeeAssignment.idAssignment JOIN Customer ON Assignment.idCustomer = Customer.idCustomer";
            List<Customer> data = new List<Customer>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (reader.Read())
                    data.Add(new Customer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
            return JsonConvert.SerializeObject(data);

        }

        // WorkOrdernen
        [HttpPost("toevoegen")]
        public async Task<bool> Toevoegen([FromBody] WorkOrder WorkOrder)
        {
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            {
                int WorkOrderId;
                string query = "INSERT INTO WorkOrder "
                             + "SET WorkOrder.startTime = @0, WorkOrder.endTime = @1, "
                             + "WorkOrder.totalTime = @2, WorkOrder.pauseTime = @3, WorkOrder.date = @4, "
                             + "WorkOrder.verbruikteMaterialen = @5, WorkOrder.description = @6, "
                             + "WorkOrder.WorkType = @7, " 
                             + "WorkOrder.idEmployeeAssignment = @8;"
                             + "SELECT LAST_INSERT_ID();";
                using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                    new MySqlParameter("@0", WorkOrder.startTime),
                    new MySqlParameter("@1", WorkOrder.endTime),
                    new MySqlParameter("@2", WorkOrder.totalTime),
                    new MySqlParameter("@3", WorkOrder.pauseTime),
                    new MySqlParameter("@4", WorkOrder.date),
                    new MySqlParameter("@5", WorkOrder.usedMaterials),
                    new MySqlParameter("@6", WorkOrder.description),
                    new MySqlParameter("@7", WorkOrder.WorkType),
                    new MySqlParameter("@8", WorkOrder.IdEmployeeAssignment)))
                {
                    await reader.ReadAsync();
                    WorkOrderId = reader.GetInt32(0);
                }

                query = "INSERT INTO WorkOrderMachines "
                        + "SET WorkOrderMachines.idWorkOrder = @1, "
                        + "WorkOrderMachines.idMachine = @0; ";
                foreach (Machine machine in WorkOrder.Machines ?? Enumerable.Empty<Machine>())
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                        new MySqlParameter("@0", machine.IdMachine),
                        new MySqlParameter("@1", WorkOrderId));

                query = "INSERT INTO WorkOrderAttachment "
                    + "SET WorkOrderAttachment.idWorkOrder = @1, "
                    + "WorkOrderAttachment.idAttachment = @0";
                foreach (Attachment attachment in WorkOrder.Attachments ?? Enumerable.Empty<Attachment>())
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                        new MySqlParameter("@0", Attachment.IdAttachment),
                        new MySqlParameter("@1", WorkOrderId));

                query = "INSERT INTO Cargo "
                    + "SET Cargo.type = @0, Cargo.fullLoad = @1, Cargo.netLoad = @2, "
                    + "Cargo.direction = @3, Cargo.idWorkOrder = @4";
                foreach (Cargo Cargo in WorkOrder.Cargos ?? Enumerable.Empty<Cargo>())
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                        new MySqlParameter("@0", Cargo.Type),
                        new MySqlParameter("@1", Cargo.fullLoad),
                        new MySqlParameter("@2", Cargo.netLoad),
                        new MySqlParameter("@3", Cargo.direction),
                        new MySqlParameter("@4", WorkOrderId));

                return true;
            }
        }

        [HttpGet("deleteall")]
        public async Task<bool> DeleteAllData()
        {
            string query = "SET FOREIGN_KEY_CHECKS = 0; "
                    + "TRUNCATE TABLE Cargo; "
                    + "TRUNCATE TABLE RoadPlate ; "
                    + "TRUNCATE TABLE EmployeeAssignment; "
                    + "TRUNCATE TABLE Assignment; "
                    + "TRUNCATE TABLE WorkOrderAttachment; "
                    + "TRUNCATE TABLE WorkOrderMachines; "
                    + "TRUNCATE TABLE WorkOrder; "
                    + "SET FOREIGN_KEY_CHECKS = 1;";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query))
                return reader.RecordsAffected >= 1; ;
        }

        //[HttpGet("getcollegakeuze")]
        //public async Task<IEnumerable<string>> GetCollegaKeuze()
        //{
        //    using (MySqlConnection con = DatabaseConnection.GetConnection())
        //    {
        //        con.Open();
        //        string query = "SELECT name FROM Employee";
        //        using (MySqlCommand cmd = new MySqlCommand(query, con))
        //        {
        //            List<string> data = new List<string>();
        //            DbDataReader reader = await cmd.ExecuteReaderAsync();
        //            while (reader.Read())
        //                data.Add(reader.GetString(0));
        //            return data;
        //        }
        //    }
        //}
    }
}
