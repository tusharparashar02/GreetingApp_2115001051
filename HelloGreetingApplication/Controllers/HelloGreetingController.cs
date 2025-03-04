using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;

namespace HelloGreetingApplication.Controllers
{
    /// <summary>
    /// API Controller for HelloGreeting
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Get method to return the greeting message
        /// </summary>
        /// <returns> Hello World!</returns>
        [HttpGet]
        public IActionResult Get()
        {
            _logger.Info("GET request received at HelloGreetingController");
            var responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Hello to Greeting App API Endpoint",
                Data = "Hello World!"
            };
            return Ok(responseModel);
        }

        /// <summary>
        /// Post method to add a new greeting message
        /// </summary>
        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {
            _logger.Info("POST request received with Key: {0}, Value: {1}", requestModel.key, requestModel.value);
            var responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Request received successfully",
                Data = $"Key: {requestModel.key}, Value: {requestModel.value}"
            };
            return Ok(responseModel);
        }

        /// <summary>
        /// Put method to update the greeting message
        /// </summary>
        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            _logger.Info("PUT request received with Key: {0}, Value: {1}", requestModel.key, requestModel.value);
            var responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting message updated successfully",
                Data = $"Updated Key: {requestModel.key}, Updated Value: {requestModel.value}"
            };
            return Ok(responseModel);
        }

        /// <summary>
        /// Patch method to partially update the greeting message
        /// </summary>
        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
        {
            _logger.Info("PATCH request received with Key: {0}, Value: {1}", requestModel.key, requestModel.value);
            var responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting message partially updated successfully",
                Data = $"Updated Key: {requestModel.key}, Updated Value: {requestModel.value}"
            };
            return Ok(responseModel);
        }

        /// <summary>
        /// Delete method to remove a greeting message
        /// </summary>
        [HttpDelete]
        public IActionResult Delete(RequestModel requestModel)
        {
            _logger.Info("DELETE request received with Key: {0}", requestModel.key);
            var responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Greeting message deleted successfully",
                Data = $"Deleted Key: {requestModel.key}"
            };
            return Ok(responseModel);
        }
    }
}
