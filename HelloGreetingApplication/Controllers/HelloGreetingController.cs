using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            logger.Info("GET request received for greeting message.");
            logger.Info("GET request processed successfully.");
            return Ok(_greetingBL.GetGreetingBL());
        }



        /// <summary>
        /// Get method to retrieve a user by ID.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>Response Model with user details</returns>
        [Authorize]
        [HttpGet("user/{id}")]
        public IActionResult GetUserById(int id)
        {
            logger.Info($"GET request received for user with ID: {id}");
            if (id == null)
            {
                logger.Warn("User ID is null. Returning NotFound response.");
                return NotFound(new ResponseModel<string> { Success = false, Message = "User not found" });
            }
            var response = _greetingBL.GetUserById(id);
            logger.Info("GET request for user processed successfully.");
            return Ok(response);
        }


        /// <summary>
        /// Get method to retrieve all users.
        /// </summary>
        /// <returns>Response Model with list of users</returns>
        [Authorize]
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            logger.Info("GET request received to fetch all users.");
            var response = _greetingBL.GetAllUsers();
            logger.Info("GET request for all users processed successfully.");
            return Ok(response);
        }


        /// <summary>
        /// Post method to add the new greeting message.
        /// </summary>
        /// <param name="requestModel">Request Model</param>
        /// <returns>Response Model with added greeting message</returns>

        [Authorize]
        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {
            logger.Info($"POST request received with Key: {requestModel.FirstName} , Value:  {requestModel.LastName}");
            logger.Info("POST request processed successfully.");
            _greetingBL.GetGreetingBL(requestModel.FirstName, requestModel.LastName);
            return Ok(_greetingBL.AddGreetingBL(requestModel));
        }

        /// <summary>
        /// Put method to change the greeting method.
        /// </summary>
        /// <param name="requestModel">Request Model</param>
        /// <returns>Response Model with updated greeting message</returns>
        /// 
        [Authorize]
        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            logger.Info($"PUT request received to update greeting to: {requestModel.LastName}");
            logger.Info("PUT request processed successfully.");
            return Ok(_greetingBL.UpdateGreetingBL(requestModel));
        }

        /// <summary>
        /// Patch method to update the single change in greeting message.
        /// </summary>
        /// <param name="requestModel">New message value will be update</param>
        /// <returns>Response Model with partially updated greeting message</returns>
        /// 
        [Authorize]
        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
        {
            logger.Info($"PATCH request received to update greeting to: {requestModel.Message}");
            logger.Info("PATCH request processed successfully.");
            return Ok(_greetingBL.PartialUpdateGreetingBL(requestModel));
        }

        /// <summary>
        /// Delete method to delete the greeting message.
        /// </summary>
        /// <param name="requestModel">to delete message from repo</param>
        /// <returns>Response Model confirming deletion</returns>
        /// 

        [Authorize]
        [HttpDelete]
        public IActionResult Delete(RequestModel requestModel)
        {
            logger.Info("DELETE request received to remove the greeting message.");
            logger.Info("DELETE request processed successfully.");
            return Ok(_greetingBL.DeleteGreetingBL(requestModel));
        }
    }
}
