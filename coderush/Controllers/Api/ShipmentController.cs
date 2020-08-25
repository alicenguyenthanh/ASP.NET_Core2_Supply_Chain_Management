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
    [Route("api/Shipment")]
    public class ShipmentController : Controller
    {
        private readonly IFunctional _functionalService;
        private readonly INumberSequence _numberSequence;

        public ShipmentController(IFunctional functionalService,
                        INumberSequence numberSequence)
        {
            _functionalService = functionalService;
            _numberSequence = numberSequence;
        }



        [HttpGet("[action]")]
        public IActionResult GetNotInvoicedYet()
        {
            List<Shipment> shipments = new List<Shipment>();
            try
            {
                List<Invoice> invoices = new List<Invoice>();
                invoices = _functionalService.GetList<Invoice>().ToList();
                List<int> ids = new List<int>();

                foreach (var item in invoices)
                {
                    ids.Add(item.ShipmentId);
                }

                shipments = _functionalService.GetList<Shipment>()
                    .Where(x => !ids.Contains(x.ShipmentId))
                    .ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(shipments);
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Shipment> Items = _functionalService.GetList<Shipment>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Shipment> payload)
        {
            Shipment value = payload.value;
            value.ShipmentName = _numberSequence.GetNumberSequence("DO");
            var result = _functionalService.Insert<Shipment>(value);
            value = (Shipment)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<ShipmentViewModel> payload)
        {
            ShipmentViewModel value = payload.value;
            var result = _functionalService
                .Update<ShipmentViewModel, Shipment>(value, Convert.ToInt32(value.ShipmentId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Shipment> payload)
        {
            var result = _functionalService.Delete<Shipment>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}