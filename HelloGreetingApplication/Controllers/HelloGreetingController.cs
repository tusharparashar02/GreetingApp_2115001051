using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using BusinessLayer.Interface;

namespace HelloGreetingApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private readonly IGreetingBL _greetingBL;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public HelloGreetingController(IGreetingBL greetingBL)
        {
            _greetingBL = greetingBL;
        }

        /// <summary>
        /// Get method to get the greeting message.
        /// </summary>
        /// <returns>Response Model with greeting message</returns>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                logger.Info("GET request received for greeting message.");
                var result = _greetingBL.GetGreetingBL();
                logger.Info("GET request processed successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in GET request.");
                throw;
            }
        }

        /// <summary>
        /// Get method to retrieve a user by ID.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Response Model with user details</returns>
        [HttpGet("user/{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                logger.Info($"GET request received for user with ID: {id}");

                if (id <= 0)
                {
                    logger.Warn("Invalid User ID provided.");
                    return BadRequest(new { Success = false, Message = "Invalid User ID" });
                }

                var response = _greetingBL.GetUserById(id);

                if (response == null)
                {
                    logger.Warn($"User with ID {id} not found.");
                    return NotFound(new { Success = false, Message = "User not found" });
                }

                logger.Info("GET request for user processed successfully.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error in GET request for user with ID: {id}");
                throw;
            }
        }

        /// <summary>
        /// Get method to retrieve all users.
        /// </summary>
        /// <returns>Response Model with list of users</returns>
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            try
            {
                logger.Info("GET request received to fetch all users.");
                var response = _greetingBL.GetAllUsers();
                logger.Info("GET request for all users processed successfully.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in GET request for all users.");
                throw;
            }
        }

        /// <summary>
        /// Post method to add a new greeting message.
        /// </summary>
        /// <param name="requestModel">Request Model</param>
        /// <returns>Response Model with added greeting message</returns>
        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {
            try
            {
                logger.Info($"POST request received with Key: {requestModel.FirstName} , Value: {requestModel.LastName}");

                if (string.IsNullOrWhiteSpace(requestModel.FirstName) || string.IsNullOrWhiteSpace(requestModel.LastName))
                {
                    logger.Warn("Invalid data in POST request.");
                    return BadRequest(new { Success = false, Message = "First Name and Last Name are required." });
                }

                var result = _greetingBL.AddGreetingBL(requestModel);
                logger.Info("POST request processed successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in POST request.");
                throw;
            }
        }

        /// <summary>
        /// Put method to update the greeting message.
        /// </summary>
        /// <param name="requestModel">Request Model</param>
        /// <returns>Response Model with updated greeting message</returns>
        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            try
            {
                logger.Info($"PUT request received to update greeting to: {requestModel.LastName}");

                if (requestModel == null)
                {
                    logger.Warn("Invalid PUT request data.");
                    return BadRequest(new { Success = false, Message = "Invalid data." });
                }

                var result = _greetingBL.UpdateGreetingBL(requestModel);
                logger.Info("PUT request processed successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in PUT request.");
                throw;
            }
        }

        /// <summary>
        /// Patch method to update a single change in the greeting message.
        /// </summary>
        /// <param name="requestModel">New message value to update</param>
        /// <returns>Response Model with partially updated greeting message</returns>
        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
        {
            try
            {
                logger.Info($"PATCH request received to update greeting to: {requestModel.Message}");

                if (string.IsNullOrWhiteSpace(requestModel.Message))
                {
                    logger.Warn("Invalid PATCH request data.");
                    return BadRequest(new { Success = false, Message = "Message cannot be empty." });
                }

                var result = _greetingBL.PartialUpdateGreetingBL(requestModel);
                logger.Info("PATCH request processed successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in PATCH request.");
                throw;
            }
        }

        /// <summary>
        /// Delete method to remove the greeting message.
        /// </summary>
        /// <param name="requestModel">Request Model to delete message</param>
        /// <returns>Response Model confirming deletion</returns>
        [HttpDelete]
        public IActionResult Delete(RequestModel requestModel)
        {
            try
            {
                logger.Info("DELETE request received to remove the greeting message.");

                if (requestModel == null)
                {
                    logger.Warn("Invalid DELETE request data.");
                    return BadRequest(new { Success = false, Message = "Invalid request data." });
                }

                var result = _greetingBL.DeleteGreetingBL(requestModel);
                logger.Info("DELETE request processed successfully.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error in DELETE request.");
                throw;
            }
        }
    }
}
