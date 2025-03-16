using System;
using System.Collections.Generic;
using BusinessLayer.Interface;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
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

            string finalGreeting = (firstName + " " + lastName).Trim();
            if (string.IsNullOrEmpty(finalGreeting))
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
        /// Retrieves a greeting by its unique ID.
        /// </summary>
        public ResponseModel<GreetingEntity> GetGreetingById(int id)
        {
            logger.Info($"Fetching greeting with ID: {id}");

            var val = _greetingRL.GetGreetingById(id);
            if (val == null)
            {
                logger.Warn($"Greeting with ID {id} not found.");
                return new ResponseModel<GreetingEntity> { Success = false, Message = "Greeting not found", Data = null };
            }

            logger.Info($"Greeting with ID {id} found.");
            return new ResponseModel<GreetingEntity> { Success = true, Message = "Greeting found", Data = val };
        }

        /// <summary>
        /// Retrieves all greetings from the database.
        /// </summary>
        public ResponseModel<List<GreetingEntity>> GetAllGreetings(int userId)
        {
            logger.Info("Fetching all greetings.");
            var result = _greetingRL.GetAllGreetings(userId);

            if (result.Count < 1)
            {
                logger.Warn("No greetings found in the database.");
                return new ResponseModel<List<GreetingEntity>> { Success = false, Message = "No greetings found" };
            }

            logger.Info("All greetings fetched successfully.");
            return new ResponseModel<List<GreetingEntity>> { Success = true, Message = "All greetings retrieved", Data = result };
        }

        /// <summary>
        /// Adds a new greeting message using the provided request model.
        /// </summary>
        public ResponseModel<string> AddGreetingBL(RequestModel requestModel, int userId)
        {
            return _greetingRL.AddGreetingRL(requestModel, userId);
        }

        /// <summary>
        /// Updates the greeting message with a new value.
        /// </summary>
        public ResponseModel<GreetingEntity> UpdateGreetingBL(RequestModel requestModel, int userId)
        {
            logger.Info($"Updating greeting message for user ID: {userId}");
            var updatedGreeting = _greetingRL.UpdateGreeting(requestModel, userId);

            if (updatedGreeting == null)
            {
                return new ResponseModel<GreetingEntity> { Success = false, Message = "Greeting update failed", Data = null };
            }

            return new ResponseModel<GreetingEntity>
            {
                Success = true,
                Data = updatedGreeting,
                Message = "Greeting updated successfully"
            };
        }

        /// <summary>
        /// Partially updates the greeting message with a new value.
        /// </summary>
        public ResponseModel<GreetingEntity> PartialUpdateGreetingBL(RequestModel requestModel, int userId)
        {
            var updatedGreeting = _greetingRL.UpdateGreetingMessage(requestModel, userId);

            if (updatedGreeting == null)
            {
                return new ResponseModel<GreetingEntity> { Success = false, Message = "No greeting found for this user", Data = null };
            }

            logger.Info($"Partially updating greeting message to: {requestModel.Message}");
            return new ResponseModel<GreetingEntity> { Success = true, Message = "Greeting updated successfully", Data = updatedGreeting };
        }

        /// <summary>
        /// Deletes a greeting message.
        /// </summary>
        public ResponseModel<GreetingEntity> DeleteGreetingBL(RequestModel requestModel, int userId)
        {
            var result = _greetingRL.DeleteGreetingRL(requestModel, userId);
            if (result == null)
            {
                return new ResponseModel<GreetingEntity> { Success = false, Message = "Greeting not found", Data = null };
            }

            logger.Info("Deleting greeting message.");
            return new ResponseModel<GreetingEntity> { Success = true, Message = "Greeting deleted successfully", Data = result };
        }
    }
}
