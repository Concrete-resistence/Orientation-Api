using Orientation_Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;

namespace Orientation_Api.DataAccess
{
    public class OrderDatabase
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Bangazon"].ConnectionString;

//----------------------------------------------------------------------------------
        //creating new order and return the order id 
        //and use the order id for creating the order for multiple products
        public int newOrder(Order order)
        {
            var sql = @"Insert into [dbo].[Order]
                               (CustomerId
                               ,OrderDate
                               ,TotalPrice
                               ,Paid
                               )
                               values
                               (@customerId
                               ,CURRENT_TIMESTAMP
                               ,0
                               ,@paid)
                     select Cast(Scope_Identity() as int)";

            var sqlLine = @"INSERT INTO [dbo].[LineItem]
                              ([ProductId]
                              ,[OrderId]
                              ,[Quantity])
                        VALUES
                              (@productId
                              ,@orderID
                              ,@quantity)";

            using (var connection = new SqlConnection(connectionString))
            {
                //execute sql command to get Order Id
                var orderId = connection.ExecuteScalar<int>(sql, 
                    new {customerId = order.CustomerId, paid = order.Paid});
                //loop through each OrderLine and pass the order id 
                foreach (var item in order.OrderLine)
                {
                    connection.Execute(sqlLine, new { item.ProductId, orderID = orderId, item.Quantity });
                }
                return orderId;
            }
        }

//-----------------------------------------------------------------------
        public Order viewOrder(int orderId)
        {
            var sqlOrder = @"SELECT OrderId
                      , CustomerId
                      , OrderDate
                      , TotalPrice
                      , Paid
                   FROM [dbo].[Order]
                  WHERE [Order].OrderId = @orderId";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Order>(sqlOrder, new { orderId = orderId }).FirstOrDefault();
            }
        }
//--------------------------------------------------------------------------------------------
    //list of orders
        public List<Order> listOrder()
        {
            var sqlOrder = @"SELECT OrderId
                      , CustomerId
                      , OrderDate
                      , TotalPrice
                      , Paid
                   FROM [dbo].[Order]";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Order>(sqlOrder).ToList();
            }
        }

//----------------------------------------------------------------------------------------------------
    //list of unpaied orders
        public List<Order> outstandingOrders()
        {
            var sqlOrder = @"SELECT OrderId
                      , CustomerId
                      , OrderDate
                      , TotalPrice
                      , Paid
                   FROM [dbo].[Order]
                  WHERE Paid = 'false'";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Order>(sqlOrder).ToList();
            }
        }

        public int updatePaidOrder(int id)

        {
            var sqlOrder = @"UPDATE [dbo].[Order]
                     SET Paid = 'true'
                  WHERE OrderId = @id";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Execute(sqlOrder, new { id = id });
            }
        }
        
    }
}


        
