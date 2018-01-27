using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aheadrace.TraceSmart.Repository.Contracts
{
    public interface IDataProvider
    {
        global::NHibernate.ISession ORMSession { get; }

        void Clear();
        void Close(bool flushSession);
        void Close();
        global::NHibernate.ICriteria CreateCriteria<T>() where T : class;
        global::NHibernate.ICriteria CreateCriteria<T>(string alias) where T : class;
        void DeleteObject(object item, bool transactionSupport);
        void DeleteObject(object item);
        void DisconnectSession();
        void Evict(object obj);
        int ExecuteCountDetachedCriteria(global::NHibernate.Criterion.DetachedCriteria detachedCriteria);
        IList<T> ExecuteDetachedCriteria<T>(global::NHibernate.Criterion.DetachedCriteria detachedCriteria);
        void ExecuteNonQuery(string sprocName, Dictionary<string, object> parameters);
        void ExecuteNonQuery(string connString, string sprocName, Dictionary<string, object> parameters);
        void ExecuteNonQueryStmt(string cmdTxt, Dictionary<string, object> parameters);
        DataTable ExecuteProcedure(string procedureName, int CommandTimeout, Dictionary<string, object> parameters);
        DataSet ExecuteProcedure(string procedureName, int CommandTimeout, string[] tableNames, Dictionary<string, object> parameters);
        DataSet ExecuteProcedure(string procedureName, int commandTimeout, string[] tableNames, Dictionary<string, object> parameters, ref Dictionary<string, object> outParameters);
        IDataReader ExecuteReader(string sqlStatement, int? commandTimeout);
        object ExecuteScalarSproc(string storedProcName, Dictionary<string, object> parameters);
        object ExecuteScalarSql(string sqlStatement, Dictionary<string, object> parameters);
        object ExecuteScalarSql(string sqlStatement);
        //DataSet ExecuteSelectSql(string sqlStatement, int CommandTimeout, Dictionary<string, object> parameters, SqlInfoMessageEventHandler infoMessage);
        DataSet ExecuteSelectSql(string sqlStatement, int CommandTimeout, Dictionary<string, object> parameters);
        DataSet ExecuteSelectSql(string sqlStatement);
        //IList<T> ExecuteSql<T>(string commandText, CommandType commandType, int commandTimeout, Dictionary<string, object> parameters, IList<PropertyInfo> resultColumns) where T : class, new();
        IList<T> ExecuteSql<T>(string commandText, CommandType commandType, int commandTimeout, Dictionary<string, object> parameters) where T : class, new();
        //IList<K> FilterObjects<K>(List<string> filterProperties, List<string> filterTypes, List<object> filterValues, Dictionary<string, DataSortOrder> dataSorts, int? startIndex, int? fetchSize);
        //IList<K> FilterObjects<K>(string filterProperty, object filterObject, int? startIndex, int? fetchSize, DataSortProperty dataSort, out long count) where K : class;
        IList<K> FilterObjects<K>(string filterProperty, object filterObject, int? startIndex, int? fetchSize);
        IList<T> FilterObjects<T>(string filterProperty, object filterObject);
        int FilterObjectsCount<K>(List<string> filterProperties, List<string> filterTypes, List<object> filterValues);
        void Flush();
        int GetCountOfObjects<T>();
        IDataReader GetDataReaderForSP(string procedureName, int CommandTimeout, Dictionary<string, object> parameters);
        global::NHibernate.ICriteria GetICriteriaFromDetachedCriteria(global::NHibernate.Criterion.DetachedCriteria detachedCriteria);
        IDbCommand GetIDBCommand();
        bool IsOpen();
        IList<T> LoadAllObjects<T>();
        object LoadObject(Type type, object id);
        T LoadObject<T>(object id) where T : class;
        IList<T> LoadObjects<T>(int startIndex, int fetchSize);
        void Merge(object item, bool transactionSupport);
        void Open();
        //void Open(BaseNHibernateInterceptor interceptor);
        int RunHQLNonQuery(string hql, Dictionary<string, object> namedParams);
        int RunHQLNonQuery(string hql);
        IList<T> RunHQLQuery<T>(string hql);
        IList RunNamedQuery(string queryName, Dictionary<string, object> namedParams);
        IList RunNamedQuery(string queryName);
        IList<T> RunNamedQuery<T>(string queryName, Dictionary<string, object> namedParams, int? startIndex, int? fetchSize) where T : class;
        IList<T> RunNamedQuery<T>(string queryName, Dictionary<string, object> namedParams) where T : class;
        IList<T> RunNamedQuery<T>(string queryName) where T : class;
        IList RunNamedQuery(string queryName, Dictionary<string, object> namedParams, int? startIndex, int? fetchSize);
        void SaveObject(object item);
        void SaveObject(object item, bool transactionSupport);
        void SaveObjects<T>(IEnumerable<T> items, bool transactionSupport = false);
    }
}
