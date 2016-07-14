//using MySql.Data.MySqlClient;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace AgroApp.Controllers
//{


//		        [HttpGet("GetTimeSheet")]
//        public async Task<IEnumerable<string>> GetTimeSheet()
//        {

//			query = "SELECT Timesheet.startTime, Timesheet.endTime, Timesheet.totalTime, 
//			Timesheet.description, Customer.name, Assignment.location, Employee.name
//			FROM Timesheet 
//			JOIN EmployeeAssignment 
//			ON Timesheet.idEmployeeAssignment = EmployeeAssignment.idEmployeeAssignment 
//			JOIN Employee 
//			ON Employee.idEmployee = EmployeeAssignment.idEmployee 
//			JOIN Assignment 
//			ON Assignment.idAssignment = EmployeeAssignment.idAssignment
//			JOIN Customer 
//			ON Customer.idCustomer = Assignment.idCustomer 
//			JOIN CoWorker 
//			ON CoWorker.idTimesheet = Timesheet.idTimesheet
//			WHERE assignment.date = @0 AND Employee.idEmployee = @1;

//			SELECT Machine.name, Machine.number FROM Machine
//			JOIN Timesheetmachine
//			ON Timesheetmachine.idMachine = Timesheetmachine.idMachine
//			JOIN Timesheet
//			ON Timesheetmachine.idTimesheet = Timesheet.idTimesheet
//			WHERE Timesheet.idTimesheet = @3";

//			List<string> data = new List<string>();
//            using (MySqlConnection conn = await DatabaseConnection.GetConnection())
//            using (MySqlDataReader reader = await MySqlHelper.ExecuteReaderAsync(conn, query))
//                while (await reader.ReadAsync())
//                    data.Add(reader.GetString(0));
//            return data;
//        }

//}
