using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IUserManager
    {
        User AuthenticateUser(string email, string password);
        bool UpdatePassword(User user, string oldPassword, string newPassword);
        bool FindUserByEmail(string email);
    }
}
