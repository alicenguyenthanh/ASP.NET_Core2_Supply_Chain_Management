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
    [Route("api/GoodsReceivedNote")]
    public class GoodsReceivedNoteController : Controller
    {
        private readonly IFunctional _functionalService;
        private readonly INumberSequence _numberSequence;

        public GoodsReceivedNoteController(IFunctional functionalService,
                        INumberSequence numberSequence)
        {
            _functionalService = functionalService;
            _numberSequence = numberSequence;
        }



        [HttpGet("[action]")]
        public IActionResult GetNotBilledYet()
        {
            List<GoodsReceivedNote> goodsReceivedNotes = new List<GoodsReceivedNote>();
            try
            {
                List<Bill> bills = new List<Bill>();
                bills = _functionalService.GetList<Bill>().ToList();
                List<int> ids = new List<int>();

                foreach (var item in bills)
                {
                    ids.Add(item.GoodsReceivedNoteId);
                }

                goodsReceivedNotes = _functionalService.GetList<GoodsReceivedNote>()
                    .Where(x => !ids.Contains(x.GoodsReceivedNoteId))
                    .ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return Ok(goodsReceivedNotes);
        }


        [HttpGet]
        public IActionResult Get()
        {
            List<GoodsReceivedNote> Items = _functionalService.GetList<GoodsReceivedNote>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });

        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<GoodsReceivedNote> payload)
        {
            GoodsReceivedNote value = payload.value;
            value.GoodsReceivedNoteName = _numberSequence.GetNumberSequence("GRN");
            var result = _functionalService.Insert<GoodsReceivedNote>(value);
            value = (GoodsReceivedNote)result.Data;
            return Ok(value);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<GoodsReceivedNoteViewModel> payload)
        {
            GoodsReceivedNoteViewModel value = payload.value;
            var result = _functionalService
                .Update<GoodsReceivedNoteViewModel, GoodsReceivedNote>(value, Convert.ToInt32(value.GoodsReceivedNoteId));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<GoodsReceivedNote> payload)
        {
            var result = _functionalService.Delete<GoodsReceivedNote>(Convert.ToInt32(payload.key));
            return Ok();

        }
    }
}