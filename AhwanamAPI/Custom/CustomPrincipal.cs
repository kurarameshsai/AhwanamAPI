using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace AhwanamAPI.Custom
{
    public class CustomPrincipal:IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public CustomPrincipal(string userId)
        {
            this.Identity = new GenericIdentity(userId);
        }
        public bool IsInRole(string role)
        {
            if (Roles.Any(role.Contains))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string[] Roles { get; set; }
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public string Username { get; set; }
    }
}