using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        public string GetGreeting(string firstName = "", string lastName = "")
        {
            // Both first and last name provided
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return $"Hello {firstName} {lastName}";
            }
            // Only first name provided
            else if (!string.IsNullOrEmpty(firstName))
            {
                return $"Hello {firstName}";
            }
            // Only last name provided
            else if (!string.IsNullOrEmpty(lastName))
            {
                return $"Hello {lastName}";
            }
            // No names provided
            else
            {
                return "Hello World";
            }
        }
    }
}
