using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class UserContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Entity.UserEntity> GreetingDB { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
    }
}
