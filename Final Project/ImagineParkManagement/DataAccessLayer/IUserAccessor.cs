using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IUserAccessor
    {
        int VerifyUsernameAndPassword(string email, string passwordHash);
        User SelectUserByEmail(string email);
        int UpdatePasswordHash(string email, string oldPasswordHash, string newPasswordHash);
    }
}
