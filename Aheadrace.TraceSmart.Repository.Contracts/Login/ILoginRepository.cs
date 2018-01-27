using Aheadrace.TraceSmart.DataContracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aheadrace.TraceSmart.Repository.Contracts.Login
{
    public interface ILoginRepository
    {
        List<User> VerifyLoginCredentials(string username, string password);
    }
}
