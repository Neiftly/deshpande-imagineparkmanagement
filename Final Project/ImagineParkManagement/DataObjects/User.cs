using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class User
    {
        public int? UserID { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public string StreetAddress { get; private set; }
        public string ZIPCode { get; private set; }
        public List<string> Roles { get; private set; }

        public User(int? userID, string firstName, string lastName, 
            string phone, string email, string streetAddress, 
            string zIPCode, List<string> roles)
        {
            this.UserID = userID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phone;
            this.Email = email;
            this.StreetAddress = streetAddress;
            this.ZIPCode = zIPCode;
            this.Roles = roles;
        }
    }
}
