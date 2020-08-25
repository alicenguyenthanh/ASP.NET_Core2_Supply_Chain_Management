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
    [Route("api/PurchaseOrder")]
    public class PurchaseOrderController : Controller
    {
        private readonly IFunctional _functionalService;
        private readonly INumberSequence _numberSequence;

        public PurchaseOrderController(IFunctional functionalService,
                        INumberSequence numberSequence)
        {
            _functionalService = functionalService;
            _numberSequence = numberSequence;
        }



        [HttpGet("[action]")]
        public IActionResult GetNotReceivedYet()
        {
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();
            try
            {
                List<GoodsReceivedNote> grns = new List<GoodsReceivedNote>();
                grns = _functionalService.GetList<GoodsReceivedNote>().ToList();
                List<int> ids = new List<int>();

                foreach (var item in grns)
                {
                    ids.Add(item.PurchaseOrderId);
                }

                purchaseOrders = _functionalService.GetList<PurchaseOrder>()
                    .Where(x => !ids.Contains(x.PurchaseOrderId))
                    .ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(purchaseOrders);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetById(int id)
        {
            PurchaseOrder result = _functionalService.GetById<PurchaseOrder>(id);

            return Ok(result);
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
            List<PurchaseOrder> Items = _functionalService.GetList<PurchaseOrder>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<PurchaseOrder> payload)
        {
            PurchaseOrder value = payload.value;
            value.PurchaseOrderName = _numberSequence.GetNumberSequence("PO");
            var result = _functionalService.Insert<PurchaseOrder>(value);
            value = (PurchaseOrder)result.Data;
            this.UpdatePurchaseOrder(value.PurchaseOrderId);
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<PurchaseOrderViewModel> payload)
        {
            PurchaseOrderViewModel value = payload.value;
            var result = _functionalService
                .Update<PurchaseOrderViewModel, PurchaseOrder>(value, Convert.ToInt32(value.PurchaseOrderId));
            this.UpdatePurchaseOrder(value.PurchaseOrderId);
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<PurchaseOrder> payload)
        {
            var result = _functionalService.Delete<PurchaseOrder>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}