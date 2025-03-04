using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        ResponseModel<string> GetGreetingBL();
        ResponseModel<string> AddGreetingBL(RequestModel requestModel);
        ResponseModel<string> UpdateGreetingBL(RequestModel requestModel);
        ResponseModel<string> PartialUpdateGreetingBL(string newValue);
        ResponseModel<string> DeleteGreetingBL();
    }
}
