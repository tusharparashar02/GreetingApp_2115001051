using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        Entity.User RegisterRL(UserDTO userDTO);
        User LoginRL(LoginDTO loginDTO);
        bool ForgetRL(ForgetDTO forgetDTO);
    }
}
