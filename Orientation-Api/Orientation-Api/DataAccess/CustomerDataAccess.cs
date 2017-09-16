using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using Dapper;
using Orientation_Api.Models;

namespace Orientation_Api.DataAccess
{
    public class CustomerDataAccess
    {
        //get all the customer data
        public List<Customer> GetAllCustomers()
        {
            using (var Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Bangazon"].ConnectionString))
            {
                Connection.Open();
                var result = Connection.Query<Customer>("select * from Customer where Active = 1");
                return result.ToList();
            }
        }
    //------------------------------------------------------------------------
        //update the customer state to be InActive by passing the customerId
        public int CustomerInactive(int CustomerId)
        {
             using ( var Connection = new SqlConnection (ConfigurationManager.ConnectionStrings["Bangazon"].ConnectionString))
             {
                Connection.Open();
                var Result = Connection.Execute("update Customer set Active = 'false' where CustomerId = @Id",
                                                  new { Id = CustomerId });
                return Result;
             }
        }

    //--------------------------------------------------------------------------
        //update the customer state to be Active by passing the customerId
        public int CustomerActive(int CustomerId)
        {
            using (var Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Bangazon"].ConnectionString))
            {
                Connection.Open();
                return Connection.Execute("update Customer set Active = 'true' where CustomerId = @Id",
                                                  new { Id = CustomerId });
            }

        }
    //--------------------------------------------------------------------------
        //update Customer lastName
        public int CustomerUpdateLastName(int CustomerId, string LastName)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Bangazon"].ConnectionString))
            {
                connection.Open();

               return connection.Execute("update Customer " +
                                                "set LastName = @LastName " +
                                                "where CustomerId = @CustomerId",
                                       new { LastName = LastName, CustomerId = CustomerId });
                
            }
        }

    //--------------------------------------------------------------------------
        //adding new customer
        public int NewCustomer(Customer customer)
        {
            using (var Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Bangazon"].ConnectionString))
            {

                var sql = @"Insert into Customer (FirstName, LastName, Address, Active, Telephone, Email)
                            Values (@FirstName, @LastName, @Address, @Active, @Telephone, @Email)";

                Connection.Open();
                return Connection.Execute(sql,  new { FirstName = customer.FirstName,
                                                        LastName = customer.LastName,
                                                        Address = customer.Address,
                                                        Active = customer.Active,
                                                        Telephone = customer.Telephone,
                                                        Email = customer.Email});

            }
        }
    }
}