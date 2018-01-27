using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using NHibernate;
using System.Collections;

namespace Aheadrace.TraceSmart.Framework.Data
{
    public class DataRepository
    {
        private static SqlConnection _connection;
        private static string connectionString = string.Empty;

        public DataRepository()
        {
            connectionString = ConfigurationManager.ConnectionStrings["TraceSmartServicesConnection"].ToString();
            //connectionString = "Data Source=SESHU; Initial Catalog=POG; user id = sa; password= sa;";
                _connection = new SqlConnection(connectionString);
        }

        /// <summary>
		/// Execute a scalar SQL statement and return the result.
		/// </summary>
		/// <returns></returns>
		public virtual object ExecuteScalarSql(string sqlStatement)
        {
            return ExecuteScalarSql(sqlStatement, null);
        }

        /// <summary>
        /// Execute a scalar SQL statement and return the result.
        /// </summary>
        /// <param name="sqlStatement"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual object ExecuteScalarSql(string sqlStatement, Dictionary<string, object> parameters)
        {
            IDbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = sqlStatement;
            cmd.CommandType = CommandType.Text;

            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    IDbDataParameter parameter = cmd.CreateParameter();
                    parameter.ParameterName = kvp.Key;
                    parameter.Value = kvp.Value;
                    cmd.Parameters.Add(parameter);
                }
            }

            return cmd.ExecuteScalar();
        }

        /// <summary>
		/// Executes a Stored Procedure and returns the results.
		/// </summary>
		/// <param name="procedureName"></param>
		/// <param name="CommandTimeout">Number of Seconds that this query is allowed to run.</param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public virtual DataTable ExecuteProcedure(string procedureName, int commandTimeout, Dictionary<string, object> parameters)
        {
            string[] tables = { "DataTable" };
            return ExecuteProcedure(procedureName, commandTimeout, tables, parameters).Tables["DataTable"];
        }

        /// <summary>
        /// Executes a Stored Procedure and returns the results as a DataSet.  Must provide the names of the tables that will be populated.
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="CommandTimeout">Number of Seconds that this query is allowed to run.</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual DataSet ExecuteProcedure(string procedureName, int commandTimeout, string[] tableNames, Dictionary<string, object> parameters)
        {
            Dictionary<string, object> outParameters = null;
            return ExecuteProcedure(procedureName, commandTimeout, tableNames, parameters, ref outParameters);
        }

        /// <summary>
        /// Executes a Stored Procedure and return the results as a Dataset and outparams. Must provide the names of the tables that will be populated.
        /// </summary>
        /// <param name="procedureName">Nmae of stored Procedure</param>
        /// <param name="commandTimeout">Time before long running storeproc is terminated </param>
        /// <param name="tableNames">Table name where data should be populated</param>
        /// <param name="parameters">Input Parameters to Stored proc</param>
        /// <param name="outParameters">Output Parameters from stored proc</param>
        /// <returns>Dataset and Output param dictionary</returns>
        public virtual DataSet ExecuteProcedure(string procedureName, int commandTimeout, string[] tableNames, Dictionary<string, object> parameters, ref Dictionary<string, object> outParameters)
        {
            DataSet data = new DataSet();
            data = GetDataForSP(procedureName, _connection, commandTimeout, tableNames, parameters, ref outParameters);
            return data;
        }

        /// <summary>
		/// Not generally recommended to call since will not be cross-DB platform safe.  These functions are here for specific methods and operations - and the
		/// standard ORM load and save methods are what should normally be used.
		/// </summary>
		/// <param name="sqlStatement"></param>
		/// <returns></returns>
		public DataSet ExecuteSelectSql(string sqlStatement)
        {
            return this.ExecuteSelectSql(sqlStatement, 30, null);
        }

        /// <summary>
        /// Not generally recommended to call since will not be cross-DB platform safe.  These functions are here for specific methods and operations - and the
        /// standard ORM load and save methods are what should normally be used.
        /// </summary>
        /// <param name="sqlStatement"></param>
        /// <param name="CommandTimeout"></param>
        /// <returns></returns>
        public DataSet ExecuteSelectSql(string sqlStatement, int CommandTimeout, Dictionary<string, object> parameters)
        {
            return ExecuteSelectSql(sqlStatement, CommandTimeout, parameters, null);
        }

        /// <summary>
        /// Not generally recommended to call since will not be cross-DB platform safe.  These functions are here for specific methods and operations - and the
        /// standard ORM load and save methods are what should normally be used.
        /// </summary>
        /// <param name="sqlStatement"></param>
        /// <param name="CommandTimeout"></param>
        /// <param name="infoMessage">Event handler to trap info and errors.</param>
        /// <returns></returns>
        public DataSet ExecuteSelectSql(string sqlStatement, int CommandTimeout, Dictionary<string, object> parameters, SqlInfoMessageEventHandler infoMessage)
        {
            if (infoMessage != null)
            {
                _connection.FireInfoMessageEventOnUserErrors = true;
                _connection.InfoMessage += infoMessage;
            }

            IDbCommand command = _connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = sqlStatement;
            command.CommandTimeout = CommandTimeout;

            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> param in parameters)
                {
                    if (param.Value is IList && param.Value.GetType().IsGenericType)
                    {
                        command.AddArrayParameters(param.Key, param.Value);
                    }
                    else
                    {
                        IDbDataParameter sqlParam = command.CreateParameter();
                        sqlParam.ParameterName = param.Key;
                        sqlParam.Value = (param.Value == null) ? DBNull.Value : param.Value;
                        command.Parameters.Add(sqlParam);
                    }
                }
            }

            //Bind the SQLDataAdaptor object so we can quickly convert this result set into Memory.
            SqlDataAdapter adaptor = new SqlDataAdapter((SqlCommand)command);

            //Create a new DataSet object that will be used to store the results of the query.
            DataSet set = new DataSet();

            //Fill the new set with the results of the query
            adaptor.Fill(set);

            return set;
        }

        /// <summary>
        /// Executes a SPROC and returns the first result, i.e the first column of the first row.
        /// </summary>
        /// <param name="storedProcName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual object ExecuteScalarSproc(string storedProcName, Dictionary<string, object> parameters)
        {
            IDbCommand cmd = _connection.CreateCommand();
            cmd.CommandText = storedProcName;
            cmd.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> param in parameters)
                {
                    IDbDataParameter sprocParam = cmd.CreateParameter();
                    sprocParam.ParameterName = param.Key;
                    sprocParam.Value = (param.Value == null) ? DBNull.Value : param.Value;
                    cmd.Parameters.Add(sprocParam);
                }
            }

            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// Execute a Stored Proc with the passed in parameters.
        /// </summary>
        /// <param name="sprocName"></param>
        /// <param name="parameters"></param>
        public virtual void ExecuteNonQuery(string sprocName, Dictionary<string, object> parameters)
        {
            ExecuteNonQuery(sprocName, CommandType.StoredProcedure, parameters);
        }

        /// <summary>
        /// Execute update/Insert statement with passed in parameters
        /// </summary>
        /// <param name="cmdTxt"></param>
        /// <param name="parameters"></param>
        public virtual void ExecuteNonQueryStmt(string cmdTxt, Dictionary<string, object> parameters)
        {
            ExecuteNonQuery(cmdTxt, CommandType.Text, parameters);
        }

        /// <summary>
        /// Execute a cmdText with the passed in parameters.
        /// </summary>
        /// <param name="sprocName"></param>
        /// <param name="parameters"></param>
        private void ExecuteNonQuery(string cmdText, CommandType cmdType, Dictionary<string, object> parameters)
        {
            IDbCommand command = _connection.CreateCommand();
            command.CommandText = cmdText;
            command.CommandType = cmdType;
            if (parameters != null && parameters.Count > 0)
            {
                foreach (KeyValuePair<string, object> paramKvp in parameters)
                {
                    IDbDataParameter sprocParam = command.CreateParameter();
                    sprocParam.ParameterName = paramKvp.Key;
                    sprocParam.Value = (paramKvp.Value == null) ? DBNull.Value : paramKvp.Value;
                    command.Parameters.Add(sprocParam);
                }
            }

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute a Stored Proc with the passed in parameters.
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="sprocName"></param>
        /// <param name="parameters"></param>
        public virtual void ExecuteNonQuery(string connString, string sprocName, Dictionary<string, object> parameters)
        {
            IDbCommand command = _connection.CreateCommand();
            command.CommandText = sprocName;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            if (parameters != null && parameters.Count > 0)
            {
                foreach (KeyValuePair<string, object> paramKvp in parameters)
                {
                    System.Data.IDbDataParameter sprocParam = command.CreateParameter();
                    sprocParam.ParameterName = paramKvp.Key;
                    sprocParam.Value = (paramKvp.Value == null) ? DBNull.Value : paramKvp.Value;
                    command.Parameters.Add(sprocParam);
                }
            }
            command.ExecuteNonQuery();            
        }

        //public virtual T PopulateRecord(SqlDataReader reader)
        //{
        //    return null;
        //}

        //protected IEnumerable<T> GetRecords(SqlCommand command)
        //{
        //    var list = new List<T>();
        //    command.Connection = _connection;
        //    _connection.Open();
        //    try
        //    {
        //        var reader = command.ExecuteReader();
        //        try
        //        {
        //            while (reader.Read())
        //                list.Add(PopulateRecord(reader));
        //        }
        //        finally
        //        {
        //            // Always call Close when done reading.
        //            reader.Close();
        //        }
        //    }
        //    finally
        //    {
        //        _connection.Close();
        //    }
        //    return list;
        //}

        //protected T GetRecord(SqlCommand command)
        //{
        //    T record = null;
        //    command.Connection = _connection;
        //    _connection.Open();
        //    try
        //    {
        //        var reader = command.ExecuteReader();
        //        try
        //        {
        //            while (reader.Read())
        //            {
        //                record = PopulateRecord(reader);
        //                break;
        //            }
        //        }
        //        finally
        //        {
        //            // Always call Close when done reading.
        //            reader.Close();
        //        }
        //    }
        //    finally
        //    {
        //        _connection.Close();
        //    }
        //    return record;
        //}

        //protected IEnumerable<T> ExecuteStoredProc(SqlCommand command)
        //{
        //    var list = new List<T>();
        //    command.Connection = _connection;
        //    command.CommandType = CommandType.StoredProcedure;
        //    _connection.Open();
        //    try
        //    {
        //        var reader = command.ExecuteReader();
        //        try
        //        {
        //            while (reader.Read())
        //            {
        //                var record = PopulateRecord(reader);
        //                if (record != null) list.Add(record);
        //            }
        //        }
        //        finally
        //        {
        //            // Always call Close when done reading.
        //            reader.Close();
        //        }
        //    }
        //    finally
        //    {
        //        _connection.Close();
        //    }
        //    return list;
        //}

        /// <summary>
        /// Executes the stored proc, on the supplied connection, and returns the IDataReader instance (use this in multi-threaded cases, managing the cycle of your own connection)
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="CommandTimeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private DataSet GetDataForSP(string procedureName, IDbConnection connection, int commandTimeout, string[] tableNames, Dictionary<string, object> parameters, ref Dictionary<string, object> outParameters)
        {
            IDbCommand command = GetCommandForSP(procedureName, connection, commandTimeout, parameters, outParameters);

            //open the connection if necessary
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            //Load data
            DataSet data = new DataSet();
            using (IDataReader reader = command.ExecuteReader())
            {
                data.Load(reader, LoadOption.OverwriteChanges, tableNames);
            }

            //Load output params
            Dictionary<string, object> outParamData = new Dictionary<string, object>();
            if (outParameters != null && outParameters.Count > 0)
            {
                foreach (string key in outParameters.Keys)
                    outParamData.Add(key, (command.Parameters[key] as IDbDataParameter).Value.ToString());

                //assign to out parameters
                outParameters = outParamData;
            }

            return data;
        }

        private IDbCommand GetCommandForSP(string procedureName, IDbConnection connection, int commandTimeout, Dictionary<string, object> parameters, Dictionary<string, object> outParameters = null)
        {
            IDbCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = procedureName;
            command.CommandTimeout = commandTimeout;

            if (parameters != null && parameters.Count > 0)
            {
                foreach (string key in parameters.Keys)
                {
                    IDbDataParameter param = command.CreateParameter();
                    param.ParameterName = key;
                    object value = parameters[key];
                    if (value == null)
                        param.Value = DBNull.Value;
                    else
                        param.Value = value;

                    command.Parameters.Add(param);
                }
            }

            // Output parameter other than varchar/nvarchar may not work. It may require code refactor
            if (outParameters != null && outParameters.Count > 0)
            {
                foreach (string key in outParameters.Keys)
                {
                    IDbDataParameter param = command.CreateParameter();
                    param.ParameterName = key;
                    param.Direction = ParameterDirection.Output;
                    param.Size = 4000;
                    param.Value = DBNull.Value;
                    command.Parameters.Add(param);
                }
            }

            return command;
        }

    }

    public static class DbCommandExt
    {
        /// <summary>
        /// This will add an array of parameters to a SqlCommand. This is used for an IN statement.
        /// Use the returned value for the IN part of your SQL call. (i.e. SELECT * FROM table WHERE field IN (@@paramNameRoot))
        /// </summary>
        /// <param name="cmd">The dbCommand object to add parameters to.</param>
        /// <param name="paramNameRoot">What the parameter should be named followed by a unique value for each value. This value starts with @@ in the CommandText will be replaced.</param>
        /// <param name="values">The array that need to be added as parameters.</param>
        public static void AddArrayParameters(this IDbCommand cmd, string paramNameRoot, object values)
        {
            /* An array cannot be simply added as a parameter to a SqlCommand so we need to loop through things and add it manually.
			 * Each item in the array will end up being it's own SqlParameter so the return value for this must be used as part of the
			 * IN statement in the CommandText.
			 */
            var parameterNames = new List<string>();
            var paramNbr = 1;
            foreach (var value in values as IList)
            {
                var paramName = string.Format("@{0}{1}", paramNameRoot, paramNbr++);
                parameterNames.Add(paramName);

                IDbDataParameter sqlParam = cmd.CreateParameter();
                sqlParam.ParameterName = paramName;
                sqlParam.Value = value;
                cmd.Parameters.Add(sqlParam);
            }

            cmd.CommandText = cmd.CommandText.Replace("@@" + paramNameRoot, string.Join(", ", parameterNames));

        }
    }
}
