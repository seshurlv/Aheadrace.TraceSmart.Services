using Aheadrace.TraceSmart.DataContracts.User;
using Aheadrace.TraceSmart.Repository.Contracts.Login;
using Aheadrace.TraceSmart.Repository.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aheadrace.TraceSmart.Framework;
using Aheadrace.TraceSmart.Business.Contract.Login;

namespace Aheadrace.TraceSmart.Business.Login
{
    public class LoginBLL : ILoginBLL
    {
        public List<User> VerifyLoginCredentials(string username, string password)
        {
            ILoginRepository loginRepo = new LoginRepository();
            return loginRepo.VerifyLoginCredentials(username, password);
        }
    }
}
