﻿using System;
using System.Linq;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using NLog;

namespace RepositoryLayer.Service
{
    //Users
    public class GreetingRL : IGreetingRL
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UserContext _context;

        /// <summary>
        /// Initializes a new instance of the GreetingRL class.
        /// </summary>
        /// <param name="context">Database context for user entity.</param>
        public GreetingRL(UserContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new greeting message to the database.
        /// </summary>
        /// <param name="requestModel">Request model containing user details.</param>
        /// <returns>ResponseModel with the saved greeting message.</returns>
        public ResponseModel<string> AddGreetingRL(RequestModel requestModel)
        {
            logger.Info("Adding new greeting message to database.");

            string final;
            if (string.IsNullOrWhiteSpace(requestModel.FirstName) && string.IsNullOrWhiteSpace(requestModel.LastName))
            {
                final = "Hello World";
            }
            else
            {
                final = (requestModel.FirstName + " " + requestModel.LastName).Trim();
            }

            var userEntity = new UserEntity
            {
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName,
                Message = $"Morning, {final}"
            };

            _context.GreetingDB.Add(userEntity);
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
        /// Retrieves a user by their unique ID.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>User entity if found; otherwise, null.</returns>
        public UserEntity GetUserById(int id)
        {
            logger.Info($"Fetching user with ID: {id}");
            var val = _context.GreetingDB.FirstOrDefault(x => x.Id == id);

            if (val == null)
            {
                logger.Warn($"User with ID {id} not found.");
                return null;
            }

            logger.Info($"User with ID {id} found.");
            return val;
        }

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>List of user entities.</returns>
        public List<UserEntity> GetAllUsers()
        {
            logger.Info("Fetching all users from the database.");
            var result = _context.GreetingDB.ToList();
            logger.Info("All users fetched successfully.");
            return result;
        }

        /// <summary>
        /// Updates the message of an existing user if the provided ID is correct.
        /// </summary>
        /// <param name="requestModel">Request model containing the user ID and new message.</param>
        /// <returns>Updated user entity if successful; otherwise, null.</returns>
        public UserEntity PartialUpdateGreetingBL(RequestModel requestModel)
        {
            logger.Info($"Updating message for user with ID: {requestModel.Id}");

            var user = _context.GreetingDB.FirstOrDefault(e => e.Id == requestModel.Id);
            if (user == null)
            {
                logger.Warn($"User with ID {requestModel.Id} not found.");
                return null;
            }

            user.Message = requestModel.Message;
            _context.SaveChanges();

            logger.Info($"User with ID {requestModel.Id} message updated successfully.");
            return user;
        }

        /// <summary>
        /// Deletes a user from the database if the provided ID matches.
        /// </summary>
        /// <param name="requestModel">Request model containing the user ID.</param>
        /// <returns>Deleted user entity if found; otherwise, null.</returns>
        public UserEntity DeleteGreetingRL(RequestModel requestModel)
        {
            logger.Info($"Attempting to delete user with ID: {requestModel.Id}");

            var result = _context.GreetingDB.FirstOrDefault(e => e.Id == requestModel.Id);
            if (result == null)
            {
                logger.Warn($"User with ID {requestModel.Id} not found.");
                return null;
            }

            _context.GreetingDB.Remove(result);
            _context.SaveChanges();

            logger.Info($"User with ID {requestModel.Id} deleted successfully.");
            return result;
        }
    }
}
