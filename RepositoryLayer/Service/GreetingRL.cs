using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
            };

            _context.Users.Add(userEntity);
            _context.SaveChanges();

            logger.Info("Greeting message saved successfully.");
            return new ResponseModel<string>
            {
                Success = true,
                Data = $"Hello, {final}",
                Message = "Greeting message saved successfully"
            };
        }
    }
}
