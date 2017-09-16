using Orientation_Api.DataAccess;
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
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Query Error" , ex);
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
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Query Error" , ex);
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
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Query Error" , ex);
            }

        }

//-------------------------------------------------------------------------------------------------
        // POST(Add new order), route: api/Order/neworder
        [HttpPost, Route("Createorder")]
        public HttpResponseMessage AddOrder(Order order )
        {
            try
            {
                var orderDataAccess = new OrderDatabase();
               var orderLine = new OrderLineDataAccess();
                var orderId = orderDataAccess.newOrder(order);
                return Request.CreateResponse(HttpStatusCode.Created, $"New order was created , orderID: {orderId}");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Query Error ", ex);
            }
        }

//--------------------------------------------------------------------------
        //post an orderLine with multiple products
        [HttpPost, Route("OrderLine")]
        public HttpResponseMessage CreateOrderLine(List <LineItem> lineitem)
        {
            try
            {
                var order = new OrderLineDataAccess();
                var addedrows = order.CreateOrderLine(lineitem);
                return Request.CreateResponse(HttpStatusCode.Created, $" {addedrows}  order Line was just created ");
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "sorry !!cannot creat the order for some reason ");
            }
        }
//------------------------------------------------------------------
        // PUT: api/Order/5
        [HttpPut, Route("Paid/{id}")]
        public HttpResponseMessage OrderPaid(int id)
        {
            try
            {
                var order = new OrderDatabase();
                var paid = order.updatePaidOrder(id);
                return Request.CreateResponse(HttpStatusCode.OK, $" {id}  has been updated");
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Paid has not been successfully updated.");
            }
        }

        // DELETE: api/Order/5
        public void Delete(int id)
        {
        }
    }
}
