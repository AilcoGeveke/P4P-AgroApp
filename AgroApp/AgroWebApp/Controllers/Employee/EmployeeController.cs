﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AgroApp.Controllers.Api;
using AgroApp.Models;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AgroApp.Controllers.Employee
{
    public class EmployeeController : Controller
    {
        /// <summary>
        /// Returns main page of the Employee
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("werknemer")]
        public IActionResult EmployeeView()
        {
            return View("Employee");
        }

        [HttpGet("werknemer/opdrachten")]
        public IActionResult EmployeeAssignmentView()
        {
            return View("OverviewEmployeeAssignment");
        }


    }
}