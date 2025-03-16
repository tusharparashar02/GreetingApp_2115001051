using System.Collections.Generic;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        ResponseModel<string> AddGreetingRL(RequestModel requestModel, int userId);

        GreetingEntity GetGreetingById(int id);

        List<GreetingEntity> GetAllGreetings(int userId);

        GreetingEntity UpdateGreetingMessage(RequestModel requestModel, int userId);

        GreetingEntity DeleteGreetingRL(RequestModel requestModel, int userId);

        GreetingEntity UpdateGreeting(RequestModel requestModel, int userId);
    }
}
