using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aheadrace.TraceSmart.DataContracts.User
{
    public class User : Aheadrace.TraceSmart.DataContracts.Common.Common
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Role Role { get; set; }
    }

    public class Role : Aheadrace.TraceSmart.DataContracts.Common.Common
    {
        public int RoleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
