using System;
using System.Collections.Generic;
using System.Linq;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using NLog;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UserContext _context;

        public GreetingRL(UserContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new greeting message to the database, linked to a specific user.
        /// </summary>
        /// <param name="requestModel">Request model containing user details.</param>
        /// <param name="userId">The ID of the logged-in user.</param>
        /// <returns>ResponseModel with the saved greeting message.</returns>
        public ResponseModel<string> AddGreetingRL(RequestModel requestModel, int userId)
        {
            logger.Info($"Adding new greeting for UserId: {userId}");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                logger.Warn($"User with ID {userId} not found.");
                return new ResponseModel<string>
                {
                    Success = false,
                    Data = null,
                    Message = "User not found"
                };
            }

            string final = string.IsNullOrWhiteSpace(requestModel.FirstName) &&
                           string.IsNullOrWhiteSpace(requestModel.LastName)
                           ? "Hello World"
                           : (requestModel.FirstName + " " + requestModel.LastName).Trim();

            var greetingEntity = new GreetingEntity
            {
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName,
                Message = $"Morning, {final}",
                UserId = userId // Foreign key linking to the user
            };

            _context.Greetings.Add(greetingEntity);
            _context.SaveChanges();

            logger.Info("Greeting message saved successfully.");
            return new ResponseModel<string>
            {
                Success = true,
                Data = $"Hello, {final}",
                Message = "Greeting message saved successfully"
            };
        }

        /// <summary>
        /// Retrieves a greeting by its unique ID.
        /// </summary>
        /// <param name="id">The unique identifier of the greeting.</param>
        /// <returns>Greeting entity if found; otherwise, null.</returns>
        public GreetingEntity GetGreetingById(int id)
        {
            logger.Info($"Fetching greeting with ID: {id}");
            var greeting = _context.Greetings.FirstOrDefault(x => x.Id == id);

            if (greeting == null)
            {
                logger.Warn($"Greeting with ID {id} not found.");
                return null;
            }

            logger.Info($"Greeting with ID {id} found.");
            return greeting;
        }

        /// <summary>
        /// Retrieves all greetings for the currently logged-in user.
        /// </summary>
        /// <param name="userId">The ID of the logged-in user.</param>
        /// <returns>List of greeting entities belonging to the user.</returns>
        public List<GreetingEntity> GetAllGreetings(int userId)
        {
            logger.Info($"Fetching greetings for UserId: {userId}");

            var result = _context.Greetings.Where(g => g.UserId == userId).ToList();

            if (!result.Any())
            {
                logger.Warn($"No greetings found for UserId: {userId}");
            }
            else
            {
                logger.Info($"Retrieved {result.Count} greetings for UserId: {userId}");
            }

            return result;
        }

        /// <summary>
        /// Updates the message of an existing greeting if the provided ID is correct.
        /// </summary>
        /// <param name="requestModel">Request model containing the greeting ID and new message.</param>
        /// <param name="userId">The ID of the logged-in user.</param>
        /// <returns>Updated greeting entity if successful; otherwise, null.</returns>
        public GreetingEntity UpdateGreetingMessage(RequestModel requestModel, int userId)
        {
            logger.Info($"Updating message for GreetingId: {requestModel.Id}, UserId: {userId}");

            var greeting = _context.Greetings.FirstOrDefault(e => e.Id == requestModel.Id && e.UserId == userId);
            if (greeting == null)
            {
                //Console.WriteLine("Into Update");
                logger.Warn($"Greeting with ID {requestModel.Id} not found or doesn't belong to UserId {userId}.");
                return null;
            }

            greeting.Message = requestModel.Message;
            _context.SaveChanges();

            logger.Info($"Greeting with ID {requestModel.Id} message updated successfully.");
            return greeting;
        }




        /// <summary>
        /// Updates the message of an existing greeting if the provided ID is correct.
        /// </summary>
        /// <param name="requestModel">Request model containing the greeting ID and new message.</param>
        /// <param name="userId">The ID of the logged-in user.</param>
        /// <returns>Updated greeting entity if successful; otherwise, null.</returns>
        public GreetingEntity UpdateGreeting(RequestModel requestModel, int userId)
        {
            logger.Info($"Updating message for GreetingId: {requestModel.Id}, UserId: {userId}");

            var greeting = _context.Greetings.FirstOrDefault(e => e.Id == requestModel.Id && e.UserId == userId);
            if (greeting == null)
            {
                //Console.WriteLine("Into Update");
                logger.Warn($"Greeting with ID {requestModel.Id} not found or doesn't belong to UserId {userId}.");
                return null;
            }

            greeting.Message = requestModel.Message;
            greeting.FirstName = requestModel.FirstName;
            greeting.LastName = requestModel.LastName;

            _context.SaveChanges();

            logger.Info($"Greeting with ID {requestModel.Id} message updated successfully.");
            return greeting;
        }






        /// <summary>
        /// Deletes a greeting from the database if the provided ID matches.
        /// </summary>
        /// <param name="requestModel">Request model containing the greeting ID.</param>
        /// <param name="userId">The ID of the logged-in user.</param>
        /// <returns>Deleted greeting entity if found; otherwise, null.</returns>
        public GreetingEntity DeleteGreetingRL(RequestModel requestModel, int userId)
        {
            logger.Info($"Attempting to delete GreetingId: {requestModel.Id}, UserId: {userId}");

            var greeting = _context.Greetings.FirstOrDefault(e => e.Id == requestModel.Id && e.UserId == userId);
            if (greeting == null)
            {
                logger.Warn($"Greeting with ID {requestModel.Id} not found or doesn't belong to UserId {userId}.");
                return null;
            }

            _context.Greetings.Remove(greeting);
            _context.SaveChanges();

            logger.Info($"Greeting with ID {requestModel.Id} deleted successfully.");
            return greeting;
        }
    }
}
