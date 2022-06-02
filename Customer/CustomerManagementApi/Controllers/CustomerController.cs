using CustomerManagementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerManagementApi.Model;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using CustomerManagementApi.Commands;
using CustomerManagementApi.Mapper;
using InfrastructureLibrary;

namespace CustomerManagementApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IMessagePublisher _rabbitMQPublisher;

        public CustomerController(ICustomerService customerService, IMapper mapper, IConfiguration configuration, IMessagePublisher rabbitMQPublisher)
        {
            _customerService = customerService;
            _mapper = mapper;
            _configuration = configuration;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerCommand registerCustomerCommand)
        {
            try
            {
                var customer = registerCustomerCommand.MapCustomerCommandToCustomer();
                await _customerService.Create(customer, registerCustomerCommand.Password);

                //Publish to RabbitMQ
                var customerRegisteredEvent = registerCustomerCommand.MapCustomerCommandToEvent();
                await _rabbitMQPublisher.PublishMessageAsync(customerRegisteredEvent.MessageType, customerRegisteredEvent,"");
                Console.WriteLine("Message Published successfully");
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateCustomerModel model)
        {
            var customer = await _customerService.Authenticate(model.EmailAddress, model.Password);

            if (customer == null)
                return BadRequest("User name or password is incorrect");

            var tokenHandler = new JwtSecurityTokenHandler();
            var keySection = _configuration.GetSection("Settings");
            var key = keySection["SecretKey"];
            var signingKey = Encoding.ASCII.GetBytes(key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, customer.CustomerId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
                {
                    CustomerId = customer.CustomerId,
                    EmailAddress = customer.EmailAddress,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Token = tokenString
                }
            );
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
