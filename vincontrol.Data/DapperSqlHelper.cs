using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using DapperExtensions;

namespace vincontrol.Data
{
    public abstract class DapperSqlHelper
    {
        public abstract string ConnectionString { get; }

        public virtual bool Insert<T>(T parameter, string connectionString = null) where T : class
        {
            using (var connection = new SqlConnection(connectionString ?? ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {

                    try
                    {
                        connection.Insert(parameter);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                connection.Close();
                return true;
            }
        }

        public virtual int InsertWithReturnId<T>(T parameter, string connectionString = null) where T : class
        {
            using (var connection = new SqlConnection(connectionString ?? ConnectionString))
            {
                connection.Open();
                var recordId = connection.Insert(parameter);
                connection.Close();
                return recordId;
            }
        }

        public virtual bool Update<T>(T parameter, string connectionString = null) where T : class
        {
            using (var connection = new SqlConnection(connectionString ?? ConnectionString))
            {
                connection.Open();
                connection.Update(parameter);
                connection.Close();
                return true;
            }
        }

        public virtual bool Delete<T>(PredicateGroup predicate, string connectionString = null) where T : class
        {
            using (var connection = new SqlConnection(connectionString ?? ConnectionString))
            {
                connection.Open();                
                connection.Delete<T>(predicate);
                connection.Close();
                return true;
            }
        }

        public virtual IList<T> GetAll<T>(IFieldPredicate predicate = null, IList<ISort> sort = null, string connectionString = null) where T : class
        {
            using (var connection = new SqlConnection(connectionString ?? ConnectionString))
            {
                connection.Open();
                var result = connection.GetList<T>(predicate, sort);
                connection.Close();
                return result.ToList();
            }
        }

        public virtual int GetId(string storedProcedure, dynamic param = null, string connectionString = null, SqlTransaction transaction = null)
        {
            using (var connection = new SqlConnection(connectionString ?? ConnectionString))
            {
                connection.Open();
                if (transaction == null) transaction = connection.BeginTransaction();
                var output = connection.Query<int>(storedProcedure, (object)param, commandType: CommandType.Text, transaction: transaction).FirstOrDefault();
                transaction.Commit();
                connection.Close();
                return output;
            }

        }

        public virtual IEnumerable<int> GetIds(string storedProcedure, dynamic param = null, string connectionString = null, SqlTransaction transaction = null)
        {
            using (var connection = new SqlConnection(connectionString ?? ConnectionString))
            {
                connection.Open();
                if (transaction == null) transaction = connection.BeginTransaction();
                var output = connection.Query<int>(storedProcedure, (object)param, commandType: CommandType.Text, transaction: transaction).AsEnumerable();
                transaction.Commit();
                connection.Close();
                return output;
            }

        }

        public virtual T Find<T>(IFieldPredicate predicate, IList<ISort> sort = null, string connectionString = null) where T : class
        {
            using (var connection = new SqlConnection(connectionString ?? ConnectionString))
            {
                connection.Open();
                var result = connection.GetList<T>(predicate, sort).FirstOrDefault();
                connection.Close();
                return result;
            }
        }

        public virtual int QueryStore(string storedProcedure, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, int? commandTimeout = null, string connectionString = null)
        {
            using (var connection = new SqlConnection(connectionString ?? ConnectionString))
            {
                connection.Open();
                if (transaction == null) transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                var output = connection.Execute(storedProcedure, param: (object)param, transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure);
                transaction.Commit();
                connection.Close();
                return output;
            }

        }
        
        public virtual IEnumerable<T> QueryStore<T>(string storedProcedure, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, bool buffered = true, int? commandTimeout = null, string connectionString = null) where T : class
        {
            using (var connection = new SqlConnection(connectionString ?? ConnectionString))
            {
                connection.Open();
                if (transaction == null) transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                var output = connection.Query<T>(storedProcedure, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure);
                transaction.Commit();
                connection.Close();
                return output;
            }

        }

        public virtual void QueryText(List<string> queries, int start, int size)
        {
            var count = 0; var numberOfQueries = queries.Count;
            
            do
            {
                var failed = true;
                do
                {
                    try
                    {
                        QueryText(string.Join(" ", queries.Skip(start * size).Take(size).ToList()), commandTimeout: 180);                        
                        failed = false;
                    }
                    catch (SqlException ex) { /*Console.WriteLine("ERROR QueryText {0}", ex.Message);*/ }
                } while (failed);
                start++;                                
                count += size;

            } while (count < numberOfQueries);
        }

        public virtual int QueryText(string storedProcedure, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, int? commandTimeout = null, string connectionString = null)
        {
            using (var connection = new SqlConnection(connectionString ?? ConnectionString))
            {
                connection.Open();
                if (transaction == null) transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead);
                var output = connection.Execute(storedProcedure, param: (object)param, transaction: transaction, commandTimeout: commandTimeout, commandType: CommandType.Text);
                transaction.Commit();
                connection.Close();
                return output;                
            }
        }
        
        public virtual IEnumerable<T> QueryText<T>(string storedProcedure, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, bool buffered = true, int? commandTimeout = null, string connectionString = null) where T : class
        {
            using (var connection = new SqlConnection(connectionString ?? ConnectionString))
            {
                connection.Open();
                if (transaction == null) transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                var output = connection.Query<T>(storedProcedure, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: commandTimeout, commandType: CommandType.Text);
                transaction.Commit();
                connection.Close();
                return output;
            }
        }
    }
}
