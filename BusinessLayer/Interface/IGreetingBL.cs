using System.Collections.Generic;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        ResponseModel<string> GetGreetingBL(string firstName = "", string lastName = "");

        ResponseModel<string> AddGreetingBL(RequestModel requestModel, int userId);

        ResponseModel<GreetingEntity> UpdateGreetingBL(RequestModel requestModel, int userId);

        ResponseModel<GreetingEntity> PartialUpdateGreetingBL(RequestModel requestModel, int userId);

        ResponseModel<GreetingEntity> DeleteGreetingBL(RequestModel requestModel, int userId);

        ResponseModel<GreetingEntity> GetGreetingById(int id);

        ResponseModel<List<GreetingEntity>> GetAllGreetings(int userId);
    }
}
