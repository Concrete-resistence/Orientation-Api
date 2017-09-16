
using Orientation_Api.DataAccess;
using Orientation_Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using Orientation_Api.DataAccess;
using Orientation_Api.Models;

namespace Orientation_Api.Controllers
{
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {

        // GET: api/Product
        [HttpGet, Route("")]
        public HttpResponseMessage Get()
        {
            try
            {
                var ProductData = new ProductDatabase();
                var AllProductList = ProductData.GetAllProducts();
                return Request.CreateResponse(HttpStatusCode.OK, AllProductList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // GET: api/Product/5
        public string Get(int id)
        {
            return "value";
        }
        // POST: Add new product  Route: api/Product/newProduct 
        [HttpPost, Route("newProduct")]
        public HttpResponseMessage AddNewProduct(Product product)
        {
            try
            {
                var productDatabase = new ProductDatabase();
                if (productDatabase.newProduct(product) > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, "New product added");
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Error add new product"); 
            }
            catch (Exception ex)
            {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Error add new product");
            }

        }

        // PUT: api/Product
        [HttpPut, Route("Stock/{Id}")]
        public HttpResponseMessage Put(int Id)
        {
            try
            {
                var productDatabase = new ProductDatabase();
                var ZeroStock = productDatabase.ProductStock(Id);
                if (ZeroStock == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"Hey Sany, We are changing the message to let you know that the Product {Id} is not in stock.");
                }
                else 
                {
                    return Request.CreateResponse(HttpStatusCode.OK, $"Hey Sany, We are changing the message to let you know that the Product {Id} is now in Stock");

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}