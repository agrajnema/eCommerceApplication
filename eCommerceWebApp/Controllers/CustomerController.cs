using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceWebApp.Models.Customer;
using eCommerceWebApp.RestClients;
using Microsoft.AspNetCore.Mvc;
using eCommerceWebApp.Commands;
using eCommerceWebApp.Mappers;

namespace eCommerceWebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerManagementApiClient _customerManagementApiClient;

        public CustomerController(ICustomerManagementApiClient customerManagementApiClient)
        {
            _customerManagementApiClient = customerManagementApiClient;
        }
        [HttpPost]
        public IActionResult Register([FromForm] RegisterCustomer customer)
        {
            if (ModelState.IsValid)
            {
                var registerCustomerCommand = customer.MapToRegisterCustomer();
                _customerManagementApiClient.RegisterCustomer(registerCustomerCommand);
                return View();
            }
            else
            {
                return View("Register", customer);
            }
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}