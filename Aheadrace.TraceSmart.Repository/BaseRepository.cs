using Aheadrace.TraceSmart.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aheadrace.TraceSmart.Repository
{
    public abstract class BaseRepository : IBaseRepository
    {
        /// <summary>
        /// creates/gets an instance of IDataProvider
        /// </summary>
        static BaseRepository()
        {

        }

        /// <summary>
        /// Gets the DataProvider
        /// </summary>
        public IDataProvider Provider
        {
            get
            {
                return null;
            }
        }        
    }
}
