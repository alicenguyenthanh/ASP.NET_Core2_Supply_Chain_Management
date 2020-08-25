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
    [Route("api/Vendor")]
    public class VendorController : Controller
    {
        private readonly IFunctional _functionalService;

        public VendorController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Vendor> Items = _functionalService.GetList<Vendor>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Vendor> payload)
        {
            Vendor value = payload.value;
            var result = _functionalService.Insert<Vendor>(value);
            value = (Vendor)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<VendorViewModel> payload)
        {
            VendorViewModel value = payload.value;
            var result = _functionalService
                .Update<VendorViewModel, Vendor>(value, Convert.ToInt32(value.VendorId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Vendor> payload)
        {
            var result = _functionalService.Delete<Vendor>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}