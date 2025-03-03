using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using System;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloGreetingController : ControllerBase
    {
        private readonly IGreetingBL _greetingBL;
        private readonly ILogger<HelloGreetingController> _logger;

        public HelloGreetingController(IGreetingBL greetingBL, ILogger<HelloGreetingController> logger)
        {
            _greetingBL = greetingBL;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Executing GET request in HelloGreetingController");
            var greetingResult = _greetingBL.GetGreeting();
            var data = new { Greeting = greetingResult, Date = DateTime.Now };

            var response = new ResponseModel<object>
            {
                Success = true,
                Message = "Request successful",
                Data = data
            };
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Post([FromBody] RequestModel request)
        {
            _logger.LogInformation("Executing POST request with data: {FirstName} {LastName}", request.FirstName, request.LastName);
            var data = new { Greeting = $"Hello {request.FirstName} {request.LastName}", Email = request.Email, ReceivedAt = DateTime.Now };

            var response = new ResponseModel<object>
            {
                Success = true,
                Message = "Greeting created",
                Data = data
            };
            return Ok(response);
        }

        [HttpPut]
        public IActionResult Put([FromBody] RequestModel request)
        {
            _logger.LogInformation("Executing PUT request with data: {FirstName} {LastName}", request.FirstName, request.LastName);
            var data = new { FullName = $"{request.FirstName} {request.LastName}", Email = request.Email, UpdatedAt = DateTime.Now };

            var response = new ResponseModel<object>
            {
                Success = true,
                Message = "Greeting updated",
                Data = data
            };
            return Ok(response);
        }

        [HttpPatch]
        public IActionResult Patch([FromBody] RequestModel request)
        {
            _logger.LogInformation("Executing PATCH request with optional updates.");
            var data = new
            {
                UpdatedFields = new
                {
                    FirstName = string.IsNullOrEmpty(request.FirstName) ? "Not updated" : request.FirstName,
                    LastName = string.IsNullOrEmpty(request.LastName) ? "Not updated" : request.LastName,
                    Email = string.IsNullOrEmpty(request.Email) ? "Not updated" : request.Email
                },
                UpdatedAt = DateTime.Now
            };

            var response = new ResponseModel<object>
            {
                Success = true,
                Message = "Greeting partially updated",
                Data = data
            };
            return Ok(response);
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            _logger.LogInformation("Executing DELETE request in HelloGreetingController");
            var data = new { DeletedAt = DateTime.Now };

            var response = new ResponseModel<object>
            {
                Success = true,
                Message = "Greeting deleted",
                Data = data
            };
            return Ok(response);
        }
    }
}
