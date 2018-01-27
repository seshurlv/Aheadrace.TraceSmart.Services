using Aheadrace.TraceSmart.DataContracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aheadrace.TraceSmart.Facade.Contracts.Login
{
    public interface ILogin
    {
        List<User> VerifyLoginCredentials(string username, string password);
    }
}
