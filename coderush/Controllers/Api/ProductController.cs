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
    [Route("api/Product")]
    public class ProductController : Controller
    {
        private readonly IFunctional _functionalService;

        public ProductController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById([FromRoute]int id)
        {
            Product product = new Product();
            product = _functionalService.GetById<Product>(id);
            return Ok(product);

        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Product> Items = _functionalService.GetList<Product>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Product> payload)
        {
            Product value = payload.value;
            value.Barcode = value.Barcode.ToUpper();
            var result = _functionalService.Insert<Product>(value);
            value = (Product)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<ProductViewModel> payload)
        {
            ProductViewModel value = payload.value;
            value.Barcode = value.Barcode.ToUpper();
            var result = _functionalService
                .Update<ProductViewModel, Product>(value, Convert.ToInt32(value.ProductId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Product> payload)
        {
            var result = _functionalService.Delete<Product>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}