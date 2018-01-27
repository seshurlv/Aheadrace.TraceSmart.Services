using Aheadrace.TraceSmart.DataContracts.User;
using Aheadrace.TraceSmart.Facade.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Aheadrace.TraceSmart.Services.Controllers
{
    public class LoginController : ApiController
    {

        public LoginController()
        {
            
        }

        [HttpGet]
        [AllowAnonymous]
        public List<User> GetServiceToken(string username, string password)
        {
            return PrepareToken(username, password);
        }
        
        private List<User> PrepareToken(string username, string password)
        {
            //var userName = Request.Headers.Contains("username") ? Request.Headers.GetValues("uname").FirstOrDefault() : string.Empty;
            //var password = Request.Headers.Contains("pword") ? Request.Headers.GetValues("pword").FirstOrDefault() : string.Empty;
            Login log = new Login();
            return log.VerifyLoginCredentials(username, password);
        }
    }
}
