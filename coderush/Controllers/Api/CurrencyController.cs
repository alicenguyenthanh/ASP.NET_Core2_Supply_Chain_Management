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
    [Route("api/Currency")]
    public class CurrencyController : Controller
    {
        private readonly IFunctional _functionalService;

        public CurrencyController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }
        

        [HttpGet("[action]/{id}")]
        public IActionResult GetByBranchId([FromRoute]int id)
        {
            Branch branch = new Branch();
            Currency currency = new Currency();
            branch = _functionalService.GetById<Branch>(id);
            if (branch != null && branch.CurrencyId != 0)
            {
                currency = _functionalService.GetById<Currency>(branch.CurrencyId);
            }
            return Ok(currency);
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Currency> Items = _functionalService.GetList<Currency>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Currency> payload)
        {
            Currency value = payload.value;
            var result = _functionalService.Insert<Currency>(value);
            value = (Currency)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<CurrencyViewModel> payload)
        {
            CurrencyViewModel value = payload.value;
            var result = _functionalService
                .Update<CurrencyViewModel, Currency>(value, Convert.ToInt32(value.CurrencyId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Currency> payload)
        {
            var result = _functionalService.Delete<Currency>(Convert.ToInt32(payload.key));
            return Ok();

        }

    }
}