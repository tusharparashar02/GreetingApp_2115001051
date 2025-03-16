using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        private readonly RedisCache _cacheService;
        static string Key = "Greeting";

        public GreetingRL(UserContext context, RedisCache cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public ResponseModel<string> AddGreetingRL(RequestModel requestModel, int userId)
        {
            logger.Info($"Adding new greeting for UserId: {userId}");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                logger.Warn($"User with ID {userId} not found.");
                return new ResponseModel<string> { Success = false, Data = null, Message = "User not found" };
            }

            string final = string.IsNullOrWhiteSpace(requestModel.FirstName) && string.IsNullOrWhiteSpace(requestModel.LastName)
                ? "Hello World"
                : (requestModel.FirstName + " " + requestModel.LastName).Trim();

            var greetingEntity = new GreetingEntity
            {
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName,
                Message = $"Morning, {final}",
                UserId = userId
            };

            _context.Greetings.Add(greetingEntity);
            _context.SaveChanges();

            logger.Info("Greeting message saved successfully.");
            return new ResponseModel<string> { Success = true, Data = $"Hello, {final}", Message = "Greeting message saved successfully" };
        }

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

        public List<GreetingEntity> GetAllGreetings(int userId)
        {
            string cacheKey = $"Greetings_{userId}";

            var response = _cacheService.GetData(cacheKey);
            if (!string.IsNullOrEmpty(response))
            {
                var data = JsonSerializer.Deserialize<List<GreetingEntity>>(response);
                if (data != null && data.Any())
                {
                    logger.Info($"Data retrieved from Redis for UserId: {userId}");
                    return data;
                }
            }

            logger.Info($"Fetching greetings from DB for UserId: {userId}");
            var result = _context.Greetings.Where(g => g.UserId == userId).ToList();

            if (!result.Any())
                logger.Warn($"No greetings found for UserId: {userId}");
            else
            {
                logger.Info($"Retrieved {result.Count} greetings for UserId: {userId}");
                _cacheService.SaveCache(cacheKey, result);
            }

            return result;
        }

        public GreetingEntity UpdateGreetingMessage(RequestModel requestModel, int userId)
        {
            logger.Info($"Updating message for GreetingId: {requestModel.Id}, UserId: {userId}");

            var greeting = _context.Greetings.FirstOrDefault(e => e.Id == requestModel.Id && e.UserId == userId);
            if (greeting == null)
            {
                logger.Warn($"Greeting with ID {requestModel.Id} not found or doesn't belong to UserId {userId}.");
                return null;
            }

            greeting.Message = requestModel.Message;
            _context.SaveChanges();

            logger.Info($"Greeting with ID {requestModel.Id} message updated successfully.");
            return greeting;
        }

        public GreetingEntity UpdateGreeting(RequestModel requestModel, int userId)
        {
            logger.Info($"Updating greeting for GreetingId: {requestModel.Id}, UserId: {userId}");

            var greeting = _context.Greetings.FirstOrDefault(e => e.Id == requestModel.Id && e.UserId == userId);
            if (greeting == null)
            {
                logger.Warn($"Greeting with ID {requestModel.Id} not found or doesn't belong to UserId {userId}.");
                return null;
            }

            greeting.Message = requestModel.Message;
            greeting.FirstName = requestModel.FirstName;
            greeting.LastName = requestModel.LastName;
            _context.SaveChanges();

            logger.Info($"Greeting with ID {requestModel.Id} updated successfully.");
            return greeting;
        }

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

            string cacheKey = $"Greetings_{userId}";
            var cachedData = _cacheService.GetData(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                var greetings = JsonSerializer.Deserialize<List<GreetingEntity>>(cachedData);
                if (greetings != null)
                {
                    greetings.RemoveAll(g => g.Id == requestModel.Id);

                    if (greetings.Any())
                    {
                        _cacheService.SaveCache(cacheKey, greetings);
                        logger.Info($"Redis cache updated after deleting GreetingId: {requestModel.Id}");
                    }
                    else
                    {
                        _cacheService.DeleteCache(cacheKey);
                        logger.Info($"Redis cache key {cacheKey} deleted as no greetings remain.");
                    }
                }
            }

            return greeting;
        }
    }
}
