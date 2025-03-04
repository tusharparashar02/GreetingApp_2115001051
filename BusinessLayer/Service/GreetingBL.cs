using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using ModelLayer.Model;
using NLog;

namespace BusinessLayer.Service
{
    /// <summary>
    /// Business logic layer for handling greeting messages.
    /// </summary>
    public class GreetingBL : IGreetingBL
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private string _greetingMessage = "Hello World";

        /// <summary>
        /// Retrieves the current greeting message.
        /// </summary>
        /// <returns>A response model containing the greeting message.</returns>
        public ResponseModel<string> GetGreetingBL()
        {
            logger.Info("Fetching greeting message.");
            return new ResponseModel<string>
            {
                Success = true,
                Data = _greetingMessage,
                Message = "Hello to Greeting App API endpoint"
            };
        }

        /// <summary>
        /// Adds a new greeting message using the provided request model.
        /// </summary>
        /// <param name="requestModel">The request model containing the key and value for the greeting.</param>
        /// <returns>A response model with the newly added greeting message.</returns>
        public ResponseModel<string> AddGreetingBL(RequestModel requestModel)
        {
            logger.Info($"Adding new greeting with Key: {requestModel.Key}, Value: {requestModel.Value}");
            _greetingMessage = $"{requestModel.Key}: {requestModel.Value}";
            return new ResponseModel<string>
            {
                Success = true,
                Data = _greetingMessage,
                Message = $"Key: {requestModel.Key} , Value: {requestModel.Value}"
            };
        }

        /// <summary>
        /// Updates the greeting message with a new value.
        /// </summary>
        /// <param name="requestModel">The request model containing the updated greeting message.</param>
        /// <returns>A response model confirming the update.</returns>
        public ResponseModel<string> UpdateGreetingBL(RequestModel requestModel)
        {
            logger.Info($"Updating greeting message to: {requestModel.Value}");
            _greetingMessage = requestModel.Value;
            return new ResponseModel<string>
            {
                Success = true,
                Data = _greetingMessage,
                Message = "Greeting message updated successfully"
            };
        }

        /// <summary>
        /// Partially updates the greeting message with a new value.
        /// </summary>
        /// <param name="newValue">The new greeting message.</param>
        /// <returns>A response model confirming the partial update.</returns>
        public ResponseModel<string> PartialUpdateGreetingBL(string newValue)
        {
            logger.Info($"Partially updating greeting message to: {newValue}");
            return new ResponseModel<string>
            {
                Success = true,
                Data = newValue,
                Message = "Greeting partially updated successfully"
            };
        }

        /// <summary>
        /// Deletes the greeting message and resets it to an empty string.
        /// </summary>
        /// <returns>A response model confirming the deletion.</returns>
        public ResponseModel<string> DeleteGreetingBL()
        {
            logger.Info("Deleting greeting message.");
            _greetingMessage = "";
            return new ResponseModel<string>
            {
                Success = true,
                Data = "",
                Message = "Greeting deleted successfully"
            };
        }
    }
}
