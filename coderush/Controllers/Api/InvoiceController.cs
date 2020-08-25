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
    [Route("api/Invoice")]
    public class InvoiceController : Controller
    {
        private readonly IFunctional _functionalService;
        private readonly INumberSequence _numberSequence;

        public InvoiceController(IFunctional functionalService,
                        INumberSequence numberSequence)
        {
            _functionalService = functionalService;
            _numberSequence = numberSequence;
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetAmount([FromRoute]int id)
        {
            var Amount = 0.0;
            Invoice invoice = _functionalService.GetById<Invoice>(id);
            if (invoice != null)
            {
                Shipment shipment = _functionalService.GetById<Shipment>(invoice.ShipmentId);
                if (shipment != null)
                {
                    SalesOrder salesOrder = _functionalService.GetById<SalesOrder>(shipment.SalesOrderId);
                    if (salesOrder != null)
                    {
                        Amount = salesOrder.Total;
                    }
                }
            }
            return Ok(new { Amount });
        }

        [HttpGet("[action]")]
        public IActionResult GetNotPaidYet()
        {
            List<Invoice> invoices = new List<Invoice>();
            try
            {
                List<PaymentReceive> receives = new List<PaymentReceive>();
                receives = _functionalService.GetList<PaymentReceive>().ToList();
                List<int> ids = new List<int>();

                foreach (var item in receives)
                {
                    ids.Add(item.InvoiceId);
                }

                invoices = _functionalService.GetList<Invoice>()
                    .Where(x => !ids.Contains(x.InvoiceId))
                    .ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(invoices);
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Invoice> Items = _functionalService.GetList<Invoice>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Invoice> payload)
        {
            Invoice value = payload.value;
            value.InvoiceName = _numberSequence.GetNumberSequence("INV");
            var result = _functionalService.Insert<Invoice>(value);
            value = (Invoice)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<InvoiceViewModel> payload)
        {
            InvoiceViewModel value = payload.value;
            var result = _functionalService
                .Update<InvoiceViewModel, Invoice>(value, Convert.ToInt32(value.InvoiceId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Invoice> payload)
        {
            var result = _functionalService.Delete<Invoice>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}