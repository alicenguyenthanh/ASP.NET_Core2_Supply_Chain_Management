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
    [Route("api/Bill")]
    public class BillController : Controller
    {
        private readonly IFunctional _functionalService;
        private readonly INumberSequence _numberSequence;

        public BillController(IFunctional functionalService,
                        INumberSequence numberSequence)
        {
            _functionalService = functionalService;
            _numberSequence = numberSequence;
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetAmount([FromRoute]int id)
        {
            var Amount = 0.0;
            Bill bill = _functionalService.GetById<Bill>(id);
            if (bill != null)
            {
                GoodsReceivedNote grn = _functionalService.GetById<GoodsReceivedNote>(bill.GoodsReceivedNoteId);
                if (grn != null)
                {
                    PurchaseOrder purchaseOrder = _functionalService.GetById<PurchaseOrder>(grn.PurchaseOrderId);
                    if (purchaseOrder != null)
                    {
                        Amount = purchaseOrder.Total;
                    }
                }
            }
            return Ok(new { Amount });
        }

        [HttpGet("[action]")]
        public IActionResult GetNotPaidYet()
        {
            List<Bill> bills = new List<Bill>();
            try
            {
                List<PaymentVoucher> vouchers = new List<PaymentVoucher>();
                vouchers = _functionalService.GetList<PaymentVoucher>().ToList();

                List<int> ids = new List<int>();

                foreach (var item in vouchers)
                {
                    ids.Add(item.BillId);
                }

                bills = _functionalService.GetList<Bill>()
                    .Where(x => !ids.Contains(x.BillId))
                    .ToList();
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(bills);
        }


        [HttpGet]
        public IActionResult Get()
        {
            List<Bill> Items = _functionalService.GetList<Bill>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Bill> payload)
        {
            Bill value = payload.value;
            value.BillName = _numberSequence.GetNumberSequence("BILL");
            var result = _functionalService.Insert<Bill>(value);
            value = (Bill)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<BillViewModel> payload)
        {
            BillViewModel value = payload.value;
            var result = _functionalService
                .Update<BillViewModel, Bill>(value, Convert.ToInt32(value.BillId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Bill> payload)
        {
            var result = _functionalService.Delete<Bill>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}