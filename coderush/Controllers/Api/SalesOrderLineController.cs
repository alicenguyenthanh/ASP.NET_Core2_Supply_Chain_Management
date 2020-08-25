using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using coderush.Data;
using coderush.Models;
using coderush.Models.SyncfusionViewModels;
using Microsoft.AspNetCore.Authorization;
using coderush.Services;
using AutoMapper;

namespace coderush.Controllers.Api
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/SalesOrderLine")]
    public class SalesOrderLineController : Controller
    {
        private readonly IFunctional _functionalService;
        private readonly IMapper _mapper;

        public SalesOrderLineController(IFunctional functionalService,
            IMapper mapper)
        {
            _functionalService = functionalService;
            _mapper = mapper;
        }



        [HttpGet("[action]")]
        public IActionResult GetSalesOrderLineByShipmentId()
        {
            var headers = Request.Headers["ShipmentId"];
            int shipmentId = Convert.ToInt32(headers);
            Shipment shipment = _functionalService.GetById<Shipment>(shipmentId);
            List<SalesOrderLine> Items = new List<SalesOrderLine>();
            if (shipment != null)
            {
                int salesOrderId = shipment.SalesOrderId;
                Items = _functionalService.GetList<SalesOrderLine>()
                    .Where(x => x.SalesOrderId.Equals(salesOrderId))
                    .ToList();
            }
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpGet("[action]")]
        public IActionResult GetSalesOrderLineByInvoiceId()
        {
            var headers = Request.Headers["InvoiceId"];
            int invoiceId = Convert.ToInt32(headers);
            Invoice invoice = _functionalService.GetById<Invoice>(invoiceId);
            List<SalesOrderLine> Items = new List<SalesOrderLine>();
            if (invoice != null)
            {
                int shipmentId = invoice.ShipmentId;
                Shipment shipment = _functionalService.GetById<Shipment>(shipmentId);
                if (shipment != null)
                {
                    int salesOrderId = shipment.SalesOrderId;
                    Items = _functionalService.GetList<SalesOrderLine>()
                        .Where(x => x.SalesOrderId.Equals(salesOrderId))
                        .ToList();
                }
            }
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        private SalesOrderLineViewModel Recalculate(SalesOrderLineViewModel salesOrderLine)
        {
            try
            {
                salesOrderLine.Amount = salesOrderLine.Quantity * salesOrderLine.Price;
                salesOrderLine.DiscountAmount = (salesOrderLine.DiscountPercentage * salesOrderLine.Amount) / 100.0;
                salesOrderLine.SubTotal = salesOrderLine.Amount - salesOrderLine.DiscountAmount;
                salesOrderLine.TaxAmount = (salesOrderLine.TaxPercentage * salesOrderLine.SubTotal) / 100.0;
                salesOrderLine.Total = salesOrderLine.SubTotal + salesOrderLine.TaxAmount;

            }
            catch (Exception)
            {

                throw;
            }

            return salesOrderLine;
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
            var headers = Request.Headers["SalesOrderId"];
            int salesOrderId = Convert.ToInt32(headers);

            List<SalesOrderLine> Items = _functionalService.GetList<SalesOrderLine>()
                .Where(x => x.SalesOrderId.Equals(salesOrderId))
                .ToList();

            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<SalesOrderLineViewModel> payload)
        {
            SalesOrderLineViewModel vm = payload.value;
            vm = this.Recalculate(vm);
            SalesOrderLine value = new SalesOrderLine();
            _mapper.Map<SalesOrderLineViewModel, SalesOrderLine>(vm, value);
            var result = _functionalService.Insert<SalesOrderLine>(value);
            value = (SalesOrderLine)result.Data;
            this.UpdateSalesOrder(value.SalesOrderId);
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<SalesOrderLineViewModel> payload)
        {
            SalesOrderLineViewModel value = payload.value;
            value = this.Recalculate(value);
            var result = _functionalService
                .Update<SalesOrderLineViewModel, SalesOrderLine>(value, Convert.ToInt32(value.SalesOrderLineId));
            this.UpdateSalesOrder(value.SalesOrderId);
            return Ok(result.Data);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<SalesOrderLine> payload)
        {
            SalesOrderLine soline = _functionalService.GetById<SalesOrderLine>(Convert.ToInt32(payload.key));
            var result = _functionalService.Delete<SalesOrderLine>(soline);
            this.UpdateSalesOrder(soline.SalesOrderId);
            return Ok();

        }
    }
}