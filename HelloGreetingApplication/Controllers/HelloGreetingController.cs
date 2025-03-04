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
            logger.Info("GET request received for greeting message.");
            logger.Info("GET request processed successfully.");
            return Ok(_greetingBL.GetGreetingBL());
        }

        /// <summary>
        /// Post method to add the new greeting message.
        /// </summary>
        /// <param name="requestModel">Request Model</param>
        /// <returns>Response Model with added greeting message</returns>
        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {
            logger.Info($"POST request received with Key: {requestModel.Key}, Value: {requestModel.Value}");
            logger.Info("POST request processed successfully.");
            return Ok(_greetingBL.AddGreetingBL(requestModel));
        }

        /// <summary>
        /// Put method to change the greeting method.
        /// </summary>
        /// <param name="requestModel">Request Model</param>
        /// <returns>Response Model with updated greeting message</returns>
        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            logger.Info($"PUT request received to update greeting to: {requestModel.Value}");
            logger.Info("PUT request processed successfully.");
            return Ok(_greetingBL.UpdateGreetingBL(requestModel));
        }

        /// <summary>
        /// Patch method to update the single change in greeting message.
        /// </summary>
        /// <param name="newValue">New message value</param>
        /// <returns>Response Model with partially updated greeting message</returns>
        [HttpPatch]
        public IActionResult Patch(string newValue)
        {
            logger.Info($"PATCH request received to update greeting to: {newValue}");
            logger.Info("PATCH request processed successfully.");
            return Ok(_greetingBL.PartialUpdateGreetingBL(newValue));
        }

        /// <summary>
        /// Delete method to delete the greeting message.
        /// </summary>
        /// <returns>Response Model confirming deletion</returns>
        [HttpDelete]
        public IActionResult Delete()
        {
            logger.Info("DELETE request received to remove the greeting message.");
            logger.Info("DELETE request processed successfully.");
            return Ok(_greetingBL.DeleteGreetingBL());
        }
    }
}