﻿using Orientation_Api.DataAccess;
using Orientation_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Orientation_Api.Controllers
{
    [RoutePrefix("api/Order")]
    public class OrderController : ApiController
    {
        //GET, Lookup order by orderId , Route:  /api/Order/1
        [HttpGet, Route("{orderid}")]
        public HttpResponseMessage ViewOrder(int orderid)
        {
            try
            {
                var orderDataAccess = new OrderDatabase();
                var order = orderDataAccess.viewOrder(orderid);
                if (order == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"Order Id {orderid} not found");
                }
                return Request.CreateResponse(HttpStatusCode.OK, order);
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Query Error");
            }

        }

        //GET , List all orders , Route: /api/Order/list
        [HttpGet, Route("list")]
        public HttpResponseMessage listOrder()
        {
            try
            {
                var orderDataAccess = new OrderDatabase();
                var orders =  orderDataAccess.listOrder();
                return Request.CreateResponse(HttpStatusCode.OK, orders);
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Query Error");
            }

        }

        //GET , List outstanding orders , Route: /api/Order/outstanding
        [HttpGet, Route("outstanding")]
        public HttpResponseMessage OutstandingOrder()
        {
            try
            {
                var orderDataAccess = new OrderDatabase();
                var orders = orderDataAccess.outstandingOrders();
                if (orders == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No outstanding order found");
                }

                return Request.CreateResponse(HttpStatusCode.OK, orders);
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Query Error");
            }

        }


        // POST(Add new order), route: api/Order/neworder
        [HttpPost, Route("neworder")]
        public HttpResponseMessage AddOrder(Order order )
        {
            try
            {
                var orderDataAccess = new OrderDatabase();
                var rowAddCnt = orderDataAccess.newOrder(order);
                return Request.CreateResponse(HttpStatusCode.Created, "New order Added");
            }
            catch
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Query Error");
            }
        }

        // PUT: api/Order/5
        //[HttpPut, Route("{}")]
        public void UpdateOrder()
        {
        }

        // DELETE: api/Order/5
        public void Delete(int id)
        {
        }
    }
}
