using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using Microsoft.Extensions.Logging;
namespace HelloGreetingApplication.Controllers
{
    /// <summary>
    /// class provinding  API for Hellogreeting
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private readonly ILogger<HelloGreetingController> _logger;

        public HelloGreetingController(ILogger<HelloGreetingController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Get method to get the greeting message
        /// </summary>
        /// <returns> Hello, World!</returns>
        [HttpGet]
        public IActionResult Get(){
            _logger.LogInformation("GET request received at HelloGreetingController");
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Hello to Greeting App Api EndPoint";
            responseModel.Data = "Hello World!";
            return Ok(responseModel);
        }
        /// <summary>
        /// Post method to add the new greeting message
        /// </summary>
        [HttpPost]
        public IActionResult Post(RequestModel requestModel) {
            _logger.LogInformation("POST request received with Key: {Key}, Value: {Value}", requestModel.key, requestModel.value);
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Request Received successfully";
            responseModel.Data = $"Key: {requestModel.key}, Value: {requestModel.value}";
            return Ok(responseModel);
        }
        /// <summary>
        /// put method to change the greeting message
        /// </summary>
        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            _logger.LogInformation("PUT request received with Key: {Key}, Value: {Value}", requestModel.key, requestModel.value);
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Greeting message updated successfully";
            responseModel.Data = $"Updated Key: {requestModel.key}, Updated Value: {requestModel.value}";
            return Ok(responseModel);
        }
        /// <summary>
        /// Patch method update the single change in greeting method
        /// </summary>
        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
        {
            _logger.LogInformation("PATCH request received with Key: {Key}, Value: {Value}", requestModel.key, requestModel.value);
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Greeting message partially updated successfully";
            responseModel.Data = $"Updated Key: {requestModel.key}, Updated Value: {requestModel.value}";
            return Ok(responseModel);
        }
        /// <summary>
        /// delete method to delete the greeting method
        /// </summary>
        [HttpDelete]
        public IActionResult Delete(RequestModel requestModel)
        {
            _logger.LogInformation("DELETE request received with Key: {Key}", requestModel.key);
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Greeting message deleted successfully";
            responseModel.Data = $"Deleted Key: {requestModel.key}";
            return Ok(responseModel);
        }

    }
}
