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
    [Route("api/SalesType")]
    public class SalesTypeController : Controller
    {
        private readonly IFunctional _functionalService;

        public SalesTypeController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<SalesType> Items = _functionalService.GetList<SalesType>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<SalesType> payload)
        {
            SalesType value = payload.value;
            var result = _functionalService.Insert<SalesType>(value);
            value = (SalesType)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<SalesTypeViewModel> payload)
        {
            SalesTypeViewModel value = payload.value;
            var result = _functionalService
                .Update<SalesTypeViewModel, SalesType>(value, Convert.ToInt32(value.SalesTypeId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<SalesType> payload)
        {
            var result = _functionalService.Delete<SalesType>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}