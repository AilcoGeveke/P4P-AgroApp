using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AWA.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AWA.Controllers.Api
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private AgroContext _context;

        public CustomerController(AgroContext context)
        {
            _context = context;
        }

        #region Api Methods

        [HttpGet("getall")]
        [HttpGet("getall/{archived}")]
        public List<Customer> GetAllCustomers(bool archived = false)
        {
            return _context.Customers.Where(x => x.IsArchived == archived).ToList();
        }

        [HttpPost("add")]
        public object AddCustomer([FromBody]Customer customer)
        {
            return AddCustomer(_context, customer); ;
        }

        [HttpPost("edit")]
        public object EditCustomer([FromBody]Customer customer)
        {
            return EditCustomer(_context, customer);
        }

        #endregion

        #region Static Methods

        public static Customer GetCustomer(AgroContext context, int id)
        {
            return context.Customers.First(x => x.CustomerId == id);
        }

        public static object AddCustomer(AgroContext context, Customer customer)
        {
            context.Customers.Add(customer);
            context.SaveChanges();
            return true;
        }

        public static object EditCustomer(AgroContext context, Customer customer)
        {
            Customer c = GetCustomer(context, customer.CustomerId);
            c.Name = customer.Name;
            c.Address = customer.Address;
            context.SaveChanges();
            return true;
        }

        #endregion
    }
}
