using System;
using BusinessLayer.Interface;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private string _greetingMessage = "Hello World";
        private readonly IGreetingRL _greetingRL;

        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
        }

        /// <summary>
        /// Retrieves the current greeting message with user attributes.
        /// </summary>
        /// <returns>ResponseModel containing the formatted greeting message.</returns>
        public ResponseModel<string> GetGreetingBL(string firstName = "", string lastName = "")
        {
            logger.Info("Fetching greeting message.");


            string finalGreeting = firstName + lastName;

            if (string.IsNullOrEmpty(finalGreeting.Trim()))

            {
                finalGreeting = "Hello World!";
            }

            return new ResponseModel<string>
            {
                Success = true,
                Data = finalGreeting,
                Message = $"Hello, {finalGreeting}"
            };
        }

        /// <summary>
        /// Adds a new greeting message using the provided request model.
        /// </summary>
        /// <param name="requestModel">Request model containing the key and value for the greeting.</param>
        /// <returns>ResponseModel with the newly set greeting message.</returns>
        public ResponseModel<string> AddGreetingBL(RequestModel requestModel)
        {
            return _greetingRL.AddGreetingRL(requestModel);
        }

        /// <summary>
        /// Updates the greeting message with a new value.
        /// </summary>
        /// <param name="requestModel">Request model containing the updated greeting message.</param>
        /// <returns>ResponseModel confirming the updated greeting message.</returns>
        public ResponseModel<string> UpdateGreetingBL(RequestModel requestModel)
        {
            logger.Info($"Updating greeting message to: {requestModel.LastName}");
            _greetingMessage = requestModel.LastName;
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
        /// <param name="newValue">The new greeting message to update partially.</param>
        /// <returns>ResponseModel confirming the partial update.</returns>
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
        /// <returns>ResponseModel confirming the deletion of the greeting message.</returns>
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