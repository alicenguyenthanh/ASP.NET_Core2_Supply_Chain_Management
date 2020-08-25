using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using coderush.Data;
using coderush.Models;
using coderush.Services;
using coderush.Models.SyncfusionViewModels;
using Microsoft.AspNetCore.Authorization;

namespace coderush.Controllers.Api
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/SalesOrder")]
    public class SalesOrderController : Controller
    {
        private readonly IFunctional _functionalService;
        private readonly INumberSequence _numberSequence;

        public SalesOrderController(IFunctional functionalService,
                        INumberSequence numberSequence)
        {
            _functionalService = functionalService;
            _numberSequence = numberSequence;
        }



        [HttpGet("[action]")]
        public IActionResult GetNotShippedYet()
        {
            List<SalesOrder> salesOrders = new List<SalesOrder>();
            try
            {
                List<Shipment> shipments = new List<Shipment>();
                shipments = _functionalService.GetList<Shipment>().ToList();
                List<int> ids = new List<int>();

                foreach (var item in shipments)
                {
                    ids.Add(item.SalesOrderId);
                }

                salesOrders = _functionalService.GetList<SalesOrder>()
                    .Where(x => !ids.Contains(x.SalesOrderId))
                    .ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(salesOrders);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById(int id)
        {
            SalesOrder result = _functionalService.GetById<SalesOrder>(id);

            return Ok(result);
        }

        private void UpdateSalesOrder(int salesOrderId)
        {
            try
            {
                SalesOrder salesOrder = new SalesOrder();
                salesOrder = _functionalService.GetById<SalesOrder>(salesOrderId);

                if (salesOrder != null)
                {
                    List<SalesOrderLine> lines = new List<SalesOrderLine>();
                    lines = _functionalService.GetList<SalesOrderLine>()
                        .Where(x => x.SalesOrderId.Equals(salesOrder.SalesOrderId))
                        .ToList();

                    //update master data by its lines
                    salesOrder.Amount = lines.Sum(x => x.Amount);
                    salesOrder.SubTotal = lines.Sum(x => x.SubTotal);

                    salesOrder.Discount = lines.Sum(x => x.DiscountAmount);
                    salesOrder.Tax = lines.Sum(x => x.TaxAmount);

                    salesOrder.Total = salesOrder.Freight + lines.Sum(x => x.Total);

                    _functionalService.Update<SalesOrder>(salesOrder);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<SalesOrder> Items = _functionalService.GetList<SalesOrder>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<SalesOrder> payload)
        {
            SalesOrder value = payload.value;
            value.SalesOrderName = _numberSequence.GetNumberSequence("SO");
            var result = _functionalService.Insert<SalesOrder>(value);
            value = (SalesOrder)result.Data;
            this.UpdateSalesOrder(value.SalesOrderId);
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<SalesOrderViewModel> payload)
        {
            SalesOrderViewModel value = payload.value;
            var result = _functionalService
                .Update<SalesOrderViewModel, SalesOrder>(value, Convert.ToInt32(value.SalesOrderId));
            this.UpdateSalesOrder(value.SalesOrderId);
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<SalesOrder> payload)
        {
            var result = _functionalService.Delete<SalesOrder>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}