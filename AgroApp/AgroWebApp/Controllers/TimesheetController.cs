using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgroApp.Controllers
{

		
		        [HttpGet("GetTimeSheet")]
        public async Task<IEnumerable<string>> GetTimeSheet()
        {

			query = "SELECT TimesheetPart.startTime, TimesheetPart.endTime, TimesheetPart.totalTime, 
			TimesheetPart.description, Customer.name, Assignment.location, Employee.name
			FROM TimesheetPart 
			JOIN EmployeeAssignment 
			ON TimesheetPart.idEmployeeAssignment = EmployeeAssignment.idEmployeeAssignment 
			JOIN Employee 
			ON Employee.idEmployee = EmployeeAssignment.idEmployee 
			JOIN Assignment 
			ON Assignment.idAssignment = EmployeeAssignment.idAssignment
			JOIN Customer 
			ON Customer.idCustomer = Assignment.idCustomer 
			JOIN CoWorker 
			ON CoWorker.idTimesheetPart = TimesheetPart.idTimesheetPart
			WHERE assignment.date = @0 AND Employee.idEmployee = @1;

			SELECT Machine.name, Machine.number FROM Machine
			JOIN workordermachine
			ON workordermachine.idMachine = workordermachine.idMachine
			JOIN timesheetpart
			ON workordermachine.idTimesheetPart = timesheetpart.idTimesheetPart
			WHERE timesheetpart.idTimesheetPart = @3";
			
			List<string> data = new List<string>();
            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query))
                while (await reader.ReadAsync())
                    data.Add(reader.GetString(0));
            return data;
        }
    
}
