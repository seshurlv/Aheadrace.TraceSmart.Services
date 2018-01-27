using Aheadrace.TraceSmart.DataContracts.User;
using Aheadrace.TraceSmart.Facade.Contracts.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aheadrace.TraceSmart.Business;
using Aheadrace.TraceSmart.Business.Login;
using Aheadrace.TraceSmart.Business.Contract.Login;

namespace Aheadrace.TraceSmart.Facade.Login
{
    public class Login : ILogin
    {
        public List<User> VerifyLoginCredentials(string username, string password)
        {
            ILoginBLL logBll = new LoginBLL();
            return logBll.VerifyLoginCredentials(username, password);
        }
    }
}
