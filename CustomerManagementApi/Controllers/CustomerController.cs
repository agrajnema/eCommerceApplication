using CustomerManagementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerManagementApi.Model;

namespace CustomerManagementApi.Controllers
{
    [Authorize]
    [Route("/api/[controller]")]
    public class CustomerController: ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterCustomerModel model)
        {
            try
            {
                var customer = _mapper.Map<Customer>(model);
                _customerService.Create(customer, model.Password);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateCustomerModel model)
        {
            var customer = _customerService.Authenticate(model.EmailAddress, model.Password);

            if (customer == null)
                return BadRequest("User name or password is incorrect");


        }

        [HttpGet("/customer")]
        public IActionResult GetAll()
        {
            var customers = _customerService.GetAll();
            var customerModelList = _mapper.Map<IEnumerable<CustomerModel>>(customers);
            return Ok(customerModelList);
        }

        [HttpGet("/customer/{id}")]
        public IActionResult GetById(int id)
        {
            var customer = _customerService.GetById(id);
            var customerModel = _mapper.Map<CustomerModel>(customer);
            return Ok(customerModel);
        }
    }
}
