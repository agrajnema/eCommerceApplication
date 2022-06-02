using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceWebApp.Models.Customer;
using eCommerceWebApp.RestClients;
using Microsoft.AspNetCore.Mvc;
using eCommerceWebApp.Commands;
using eCommerceWebApp.Mappers;
using Microsoft.AspNetCore.Http;

namespace eCommerceWebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerManagementApiClient _customerManagementApiClient;

        public CustomerController(ICustomerManagementApiClient customerManagementApiClient)
        {
            _customerManagementApiClient = customerManagementApiClient;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
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

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] AuthenticateCustomer customer)
        {
            if (ModelState.IsValid)
            {
                var authenticatedCustomer = await _customerManagementApiClient.AuthenticateCustomer(customer);
                HttpContext.Session.SetString("JWTToken", authenticatedCustomer.Token);
                if (!string.IsNullOrEmpty(authenticatedCustomer?.Token))                    
                    HttpContext.Session.SetString("LoggedInUserFirstName", authenticatedCustomer.FirstName);
                
                return RedirectToAction("","Home");
            }
            else
            {
                return View("Login", customer);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWTToken");
            HttpContext.Session.Remove("LoggedInUserFirstName");
            BaseApiClient.JWTToken = string.Empty;
            return RedirectToAction("Login");
        }
    }
}