using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Middleware.GlobalExceptionHandler;

namespace HelloGreetingApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private readonly IGreetingBL _greetingBL;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //static int userId;

        public HelloGreetingController(IGreetingBL greetingBL)
        {
            _greetingBL = greetingBL;
        }

        [Authorize]
        [HttpGet("user/{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                logger.Info($"GET request received for user with ID: {id}");
                var response = _greetingBL.GetGreetingById(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in GetUserById");
                return BadRequest(ExceptionHandler.CreateErrorResponse(ex));
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                logger.Info("GET request received to fetch all users.");
                int userId = GetUserId();
                var response = _greetingBL.GetAllGreetings(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in GetAllUsers");
                return BadRequest(ExceptionHandler.CreateErrorResponse(ex));
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] RequestModel requestModel)
        {
            try
            {
                int userId = GetUserId();
                logger.Info($"POST request received to add greeting: {requestModel.FirstName} {requestModel.LastName}");
                var response = _greetingBL.AddGreetingBL(requestModel, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in Post");
                return BadRequest(ExceptionHandler.CreateErrorResponse(ex));
            }
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody] RequestModel requestModel)
        {
            try
            {
                int userId = GetUserId();
                logger.Info($"PUT request received to update greeting: {requestModel.LastName}");
                var response = _greetingBL.UpdateGreetingBL(requestModel, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in Put");
                return BadRequest(ExceptionHandler.CreateErrorResponse(ex));
            }
        }

        [Authorize]
        [HttpPatch]
        public IActionResult Patch([FromBody] RequestModel requestModel)
        {
            try
            {
                int userId = GetUserId();
                logger.Info($"PATCH request received to update greeting: {requestModel.Message}");
                var response = _greetingBL.PartialUpdateGreetingBL(requestModel, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in Patch");
                return BadRequest(ExceptionHandler.CreateErrorResponse(ex));
            }
        }

        [Authorize]
        [HttpDelete]
        public IActionResult Delete([FromBody] RequestModel requestModel)
        {
            try
            {
                int userId = GetUserId();
                logger.Info("DELETE request received to remove the greeting message.");
                var response = _greetingBL.DeleteGreetingBL(requestModel, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in Delete");
                return BadRequest(ExceptionHandler.CreateErrorResponse(ex));
            }
        }

        private int GetUserId()
        {
            try
            {

                var userIdClaims = User.Claims
                    .Where(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                    .Select(c => c.Value)
                    .ToList();

                if (!userIdClaims.Any())
                {
                    throw new Exception("User ID claim is missing in the JWT.");
                }

                foreach (var claim in userIdClaims)
                {
                    if (int.TryParse(claim, out int userId))
                    {
                        //Console.WriteLine($"Extracted Valid User ID: {userId}");
                        return userId;
                    }
                }
                throw new FormatException($"No valid integer User ID found in claims: {string.Join(", ", userIdClaims)}");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in GetUserId");
                throw;
            }
        }
    }
}
