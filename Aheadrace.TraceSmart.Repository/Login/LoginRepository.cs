using Aheadrace.TraceSmart.DataContracts.User;
using Aheadrace.TraceSmart.Framework.Data;
using Aheadrace.TraceSmart.Repository.Contracts;
using Aheadrace.TraceSmart.Repository.Contracts.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Aheadrace.TraceSmart.Repository.Login
{
    public class LoginRepository : DataRepository, ILoginRepository
    {
        DataRepository dbRepo;
        public LoginRepository()
        {
            dbRepo = new DataRepository();
        }        

        public List<User> VerifyLoginCredentials(string username, string password)
        {
            List<User> usrList = new List<User>();
            string[] tableNames = new string[1];
            Dictionary<string, object> cmdParams = new Dictionary<string, object>();
            cmdParams.Add("@UserName", username);
            cmdParams.Add("@UserPassword", password);

            DataSet ds = dbRepo.ExecuteProcedure("ValidateUser", 0, tableNames, cmdParams);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                User usr = new User(){
                    UserID = Convert.ToInt32(ds.Tables[0].Rows[0]["UserID"]),
                    Role = new Role()
                    {
                        Name = Convert.ToString(ds.Tables[0].Rows[0]["RoleName"])
                    }
                };
                usrList.Add(usr);
            }
            return usrList;
        }
    }
}
