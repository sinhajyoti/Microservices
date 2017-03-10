using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMicroService.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace CoreMicroService.Repository
{
    public class OrderRepository: IRepository<Order>
    {
        private string connectionString;
        public OrderRepository(IConfiguration config)
        {
            connectionString=config.GetValue<string>("DBInfo:ConnectionString");
        }

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }

        public void Add(Order item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("INSERT INTO order (OrderId,OrderName,OrderAmount,OrderStatus,CorrelationId,SequenceId,LastUpdatedby,LastUpdatedOn) VALUES(@OrderId,@OrderName,@OrderAmount,@OrderStatus,@CorrelationId,@SequencId,@LastUpdatedby,@LastUpdatedOn)", item);
                dbConnection.Close();
            }
        }

        public IEnumerable<Order> FindAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Order>("SELECT OrderId,OrderName,OrderAmount,OrderStatus,CorrelationId,SequenceId,LastUpdatedby,LastUpdatedOn FROM Order");
            }
        }

        public Order FindByID(string id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Order>("SELECT OrderId,OrderName,OrderAmount,OrderStatus,CorrelationId,SequenceId,LastUpdatedby,LastUpdatedOn FROM Order WHERE OrderId = @OrderId", new { OrderId = id }).FirstOrDefault();
            }
        }

        public void Remove(string id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("Delete FROM Order WHERE OrderId = @OrderId", new { OrderId = id });
                dbConnection.Close();
            }
        }

        public void Delete(Order item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("UPDATE order set OrderStatus=@OrderStatus,LastUpdatedby=@LastUpdatedby WHERE OrderId = @OrderId and CorrelationId=@CorrelationId", item);
                dbConnection.Close();
            }
        }

        public void Update(Order item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                dbConnection.Execute("UPDATE order set OrderId=@OrderId,OrderName=@OrderName,OrderAmount=@OrderAmount,OrderStatus=@OrderStatus,CorrelationId=@CorrelationId,SequenceId=@SequenceId,LastUpdatedby=@LastUpdatedby,LastUpdatedOn=@LastUpdatedOn WHERE OrderId = @OrderId", item);
                dbConnection.Close();
            }
        }
        
    }
}
