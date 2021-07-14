using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTP.Api.Configuration.Roles
{
    public class Authorization
    {
        public enum Roles
        {
            Administrator,
            Moderator,
            Manager,
            Employee
        }
        public const string default_username = "user";
        public const string default_email = "user@secureapi.com";
        public const string default_password = "Pa$$w0rd.";
        public const Roles default_role = Roles.Employee;
    }
}
