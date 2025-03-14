﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        ResponseModel<string> AddGreetingRL(RequestModel requestModel);
        UserEntity GetUserById(int id);
        public List<UserEntity> GetAllUsers();
        UserEntity PartialUpdateGreetingBL(RequestModel requestModel);
        public UserEntity DeleteGreetingRL(RequestModel requestModel);
    }
}
