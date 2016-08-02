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

        [HttpGet("getall")]
        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }
    }
}
