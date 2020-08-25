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
    [Route("api/PurchaseOrderLine")]
    public class PurchaseOrderLineController : Controller
    {
        private readonly IFunctional _functionalService;
        private readonly IMapper _mapper;

        public PurchaseOrderLineController(IFunctional functionalService,
            IMapper mapper)
        {
            _mapper = mapper;
            _functionalService = functionalService;
        }



        private PurchaseOrderLineViewModel Recalculate(PurchaseOrderLineViewModel purchaseOrderLine)
        {
            try
            {
                purchaseOrderLine.Amount = purchaseOrderLine.Quantity * purchaseOrderLine.Price;
                purchaseOrderLine.DiscountAmount = (purchaseOrderLine.DiscountPercentage * purchaseOrderLine.Amount) / 100.0;
                purchaseOrderLine.SubTotal = purchaseOrderLine.Amount - purchaseOrderLine.DiscountAmount;
                purchaseOrderLine.TaxAmount = (purchaseOrderLine.TaxPercentage * purchaseOrderLine.SubTotal) / 100.0;
                purchaseOrderLine.Total = purchaseOrderLine.SubTotal + purchaseOrderLine.TaxAmount;

            }
            catch (Exception)
            {

                throw;
            }

            return purchaseOrderLine;
        }

        private void UpdatePurchaseOrder(int purchaseOrderId)
        {
            try
            {
                PurchaseOrder purchaseOrder = new PurchaseOrder();
                purchaseOrder = _functionalService.GetById<PurchaseOrder>(purchaseOrderId);

                if (purchaseOrder != null)
                {
                    List<PurchaseOrderLine> lines = new List<PurchaseOrderLine>();
                    lines = _functionalService.GetList<PurchaseOrderLine>()
                        .Where(x => x.PurchaseOrderId.Equals(purchaseOrderId))
                        .ToList();

                    //update master data by its lines
                    purchaseOrder.Amount = lines.Sum(x => x.Amount);
                    purchaseOrder.SubTotal = lines.Sum(x => x.SubTotal);
                    
                    purchaseOrder.Discount = lines.Sum(x => x.DiscountAmount);
                    purchaseOrder.Tax = lines.Sum(x => x.TaxAmount);

                    purchaseOrder.Total = purchaseOrder.Freight + lines.Sum(x => x.Total);

                    _functionalService.Update<PurchaseOrder>(purchaseOrder);
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
            var headers = Request.Headers["PurchaseOrderId"];
            int purchaseOrderId = Convert.ToInt32(headers);

            List<PurchaseOrderLine> Items = _functionalService.GetList<PurchaseOrderLine>()
                .Where(x => x.PurchaseOrderId.Equals(purchaseOrderId))
                .ToList();

            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<PurchaseOrderLineViewModel> payload)
        {
            PurchaseOrderLineViewModel vm = payload.value;
            vm = this.Recalculate(vm);
            PurchaseOrderLine value = new PurchaseOrderLine();
            _mapper.Map<PurchaseOrderLineViewModel, PurchaseOrderLine>(vm, value);
            var result = _functionalService.Insert<PurchaseOrderLine>(value);
            value = (PurchaseOrderLine)result.Data;
            this.UpdatePurchaseOrder(value.PurchaseOrderId);
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<PurchaseOrderLineViewModel> payload)
        {
            PurchaseOrderLineViewModel value = payload.value;
            value = this.Recalculate(value);
            var result = _functionalService
                .Update<PurchaseOrderLineViewModel, PurchaseOrderLine>(value, Convert.ToInt32(value.PurchaseOrderLineId));
            this.UpdatePurchaseOrder(value.PurchaseOrderId);
            return Ok(result.Data);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<PurchaseOrderLine> payload)
        {
            PurchaseOrderLine poline = _functionalService.GetById<PurchaseOrderLine>(Convert.ToInt32(payload.key));
            var result = _functionalService.Delete<PurchaseOrderLine>(poline);
            this.UpdatePurchaseOrder(poline.PurchaseOrderId);
            return Ok();

        }
    }
}