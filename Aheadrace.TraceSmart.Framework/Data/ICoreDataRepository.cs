using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aheadrace.TraceSmart.Framework.Data
{
    public interface ICoreDataRepository<T>
    {
        T PopulateRecord(SqlDataReader reader);

        IEnumerable<T> GetRecords(SqlCommand command);        

        T GetRecord(SqlCommand command);        

        IEnumerable<T> ExecuteStoredProc(SqlCommand command);
        
    }
}
