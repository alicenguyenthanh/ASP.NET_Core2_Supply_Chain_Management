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

namespace coderush.Controllers.Api
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/InvoiceType")]
    public class InvoiceTypeController : Controller
    {
        private readonly IFunctional _functionalService;

        public InvoiceTypeController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<InvoiceType> Items = _functionalService.GetList<InvoiceType>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<InvoiceType> payload)
        {
            InvoiceType value = payload.value;
            var result = _functionalService.Insert<InvoiceType>(value);
            value = (InvoiceType)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<InvoiceTypeViewModel> payload)
        {
            InvoiceTypeViewModel value = payload.value;
            var result = _functionalService
                .Update<InvoiceTypeViewModel, InvoiceType>(value, Convert.ToInt32(value.InvoiceTypeId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<InvoiceType> payload)
        {
            var result = _functionalService.Delete<InvoiceType>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}