﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class GreetingEntity
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string Message {  get; set; } = string.Empty;

        // Foreign Key
        public int UserId { get; set; }

        // Navigation Property
        [ForeignKey("UserId")]
        public UserEntity User { get; set; }
    }
}
