using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aheadrace.TraceSmart.Repository.Contracts
{
    public interface IBaseRepository
    {
        /// <summary>
        /// The data provider for the Repository.
        /// </summary>
        IDataProvider Provider { get; }        
    }
}
