using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        ResponseModel<string> GetGreetingBL(string FirstName = "", string LastName = "");
        ResponseModel<string> AddGreetingBL(RequestModel requestModel);
        ResponseModel<string> UpdateGreetingBL(RequestModel requestModel);
        ResponseModel<UserEntity> PartialUpdateGreetingBL(RequestModel requestModel);
        ResponseModel<string> DeleteGreetingBL();

        ResponseModel<UserEntity> GetUserById(int id);
        ResponseModel<List<UserEntity>> GetAllUsers();
    }
}
