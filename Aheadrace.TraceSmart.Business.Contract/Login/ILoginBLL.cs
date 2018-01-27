using Aheadrace.TraceSmart.DataContracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aheadrace.TraceSmart.Business.Contract.Login
{
    public interface ILoginBLL
    {
        List<User> VerifyLoginCredentials(string username, string password);
    }
}
