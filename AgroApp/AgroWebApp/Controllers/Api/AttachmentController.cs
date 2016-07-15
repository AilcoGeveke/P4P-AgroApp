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
    public class AttachmentController : Controller
    {
        /// <summary>
        /// Laat een lijst van alle Attachment zien
        /// </summary>
        /// <returns></returns>

        [HttpGet("getall")]
        public async Task<List<Attachment>> GetAllAttachments()
        {
            string query = "SELECT idAttachment, name, number FROM Attachment WHERE isDeleted=@0";
            List<Attachment> data = new List<Attachment>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false)))
                while (reader.Read())
                    data.Add(new Attachment(idAttachment: reader["IdAttachment"] as int? ?? -1, name: reader["name"] as string, number: reader["number"] as int? ?? -1));
            return data;
        }

        [HttpGet("getallarchived")]
        public async Task<List<Attachment>> GetAllArchivedAttachments()
        {
            string query = "SELECT idAttachment, name, number FROM Attachment WHERE isDeleted=@0";
            List<Attachment> data = new List<Attachment>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true)))
                while (reader.Read())
                    data.Add(new Attachment(idAttachment: reader["IdAttachment"] as int? ?? -1, name: reader["name"] as string, number: reader["number"] as int? ?? -1));
            return data;
        }


        /// <summary>
        /// voegt een Attachment toe
        /// </summary>
        /// <param name="Attachment"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<string> AddAttachment([FromBody]Attachment Attachment)
        {
            if (string.IsNullOrWhiteSpace(Attachment.Name) || Attachment.Number == 0)
                return "Een van de opgegeven velden is leeg";

            string query = "INSERT INTO Attachment (`name`, `number`) VALUES (@0, @1);";
            try
            {
                using (MySqlConnection conn = await DatabaseConnection.GetConnection())
                {
                    await MySqlHelper.ExecuteNonQueryAsync(conn, query,
                      new MySqlParameter("@0", Attachment.Name),
                      new MySqlParameter("@1", Attachment.Number));
                    return "succes";
                }
            }
            catch { return "Er is iets misgegaan, neem contact op met een ontwikkelaar!"; }
        }

        [HttpPost("change")]
        public async Task<bool> EditAttachment([FromBody]Attachment attachment)
        {
            if (GetAttachment(attachment.IdAttachment) == null)
                return false;

            string query = "UPDATE Attachment SET number=@0, name=@1 WHERE IdAttachment=@2";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", attachment.Number),
                new MySqlParameter("@1", attachment.Name),
                new MySqlParameter("@2", attachment.IdAttachment)))
                return reader.RecordsAffected == 1;
        }

        [HttpGet("archive/{id}")]
        public async Task<bool> ArchiveAttachment(int id)
        {
            if (GetAttachment(id) == null)
                return false;

            string query = "UPDATE Attachment SET isDeleted=@0 WHERE idAttachment=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", true),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1;
        }

        [HttpGet("restoreattachment/{id}")]
        public async Task<bool> RestoreAttachment(int id)
        {
            if (GetAttachment(id) == null)
                return false;

            string query = "UPDATE Attachment SET isDeleted=@0 WHERE idAttachment=@1";
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query,
                new MySqlParameter("@0", false),
                new MySqlParameter("@1", id)))
                return reader.RecordsAffected == 1;
        }

        /// <summary>
        /// geeft 1 Attachment weer om deze te bewerken.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                return reader.HasRows ? new Attachment(reader["idAttachment"] as int? ?? -1, reader["number"] as int? ?? -1, reader["name"] as string) : null;
            }
        }
    }
}

