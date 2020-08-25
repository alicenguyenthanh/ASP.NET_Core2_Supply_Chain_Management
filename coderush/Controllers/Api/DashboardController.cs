using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Models;
using coderush.Models.SyncfusionViewModels;
using coderush.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Dashboard")]
    public class DashboardController : Controller
    {
        private readonly IFunctional _functionalService;

        public DashboardController(IFunctional functionalService)
        {
            _functionalService = functionalService;
        }

        [HttpGet("[action]")]
        public IActionResult ProductByProductType()
        {
            List<ChartViewModel> result = new List<ChartViewModel>();
            List<ProductType> productTypes = _functionalService.GetList<ProductType>()
                .ToList();
            foreach (var item in productTypes)
            {
                int count = _functionalService.GetList<Product>()
                    .Where(x => x.ProductTypeId.Equals(item.ProductTypeId))
                    .Count();

                result.Add(new ChartViewModel
                {
                    x = item.ProductTypeName,
                    text = item.ProductTypeName,
                    y = count
                });
            }
            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult SalesCycle()
        {
            List<ChartViewModel> result = new List<ChartViewModel>();
            
            double salesOrder = Convert.ToDouble(_functionalService.GetList<SalesOrder>().Count());
            double shipment = Convert.ToDouble(_functionalService.GetList<Shipment>().Count());
            double invoice = Convert.ToDouble(_functionalService.GetList<Invoice>().Count());
            double paymentReceive = Convert.ToDouble(_functionalService.GetList<PaymentReceive>().Count());
            double total = salesOrder + shipment + invoice + paymentReceive;
            double percentageSalesOrder = (salesOrder / total) * 100.0;
            double percentageShipment = (shipment / total) * 100.0;
            double percentageInvoice = (invoice / total) * 100.0;
            double percentagePaymentReceive = (paymentReceive / total) * 100.0;

            result.Add(new ChartViewModel
            {
                x = "Payment Receive",
                text = percentagePaymentReceive.ToString("n2") + "%",
                y = percentagePaymentReceive
            });

            result.Add(new ChartViewModel
            {
                x = "Invoice",
                text = percentageInvoice.ToString("n2") + "%",
                y = percentageInvoice
            });

            result.Add(new ChartViewModel
            {
                x = "Shipment",
                text = percentageShipment.ToString("n2") + "%",
                y = percentageShipment
            });

            result.Add(new ChartViewModel
            {
                x = "Sales Order",
                text = percentageSalesOrder.ToString("n2") + "%",
                y = percentageSalesOrder
            });

            return Ok(result);
        }

        [HttpGet("[action]")]
        public IActionResult SalesCycleMonthly()
        {
            List<ChartViewModel> salesOrder = new List<ChartViewModel>();
            List<ChartViewModel> shipment = new List<ChartViewModel>();
            List<ChartViewModel> invoice = new List<ChartViewModel>();
            List<ChartViewModel> paymentReceive = new List<ChartViewModel>();
            DateTime start = new DateTime(DateTime.Now.Year, 1, 1);
            DateTime end = start.Date.AddMonths(11);
            for (DateTime i = start.Date; i <= end.Date; i = i.Date.AddMonths(1))
            {
                double countSalesOrder = _functionalService.GetList<SalesOrder>()
                    .Where(x => x.OrderDate.Month.Equals(i.Month) && x.OrderDate.Year.Equals(i.Year)).Count();
                salesOrder.Add(new ChartViewModel { x = i.Date.ToString("MMM"), y = countSalesOrder });

                double countShipment = _functionalService.GetList<Shipment>()
                    .Where(x => x.ShipmentDate.Month.Equals(i.Month) && x.ShipmentDate.Year.Equals(i.Year)).Count();
                shipment.Add(new ChartViewModel { x = i.Date.ToString("MMM"), y = countShipment });

                double countInvoice = _functionalService.GetList<Invoice>()
                    .Where(x => x.InvoiceDate.Month.Equals(i.Month) && x.InvoiceDate.Year.Equals(i.Year)).Count();
                invoice.Add(new ChartViewModel { x = i.Date.ToString("MMM"), y = countInvoice });

                double countPaymentReceive = _functionalService.GetList<PaymentReceive>()
                    .Where(x => x.PaymentDate.Month.Equals(i.Month) && x.PaymentDate.Year.Equals(i.Year)).Count();
                paymentReceive.Add(new ChartViewModel { x = i.Date.ToString("MMM"), y = countPaymentReceive });
            }
            return Ok(new {
                dataSalesOrder = salesOrder,
                textSalesOrder = "Sales Order",
                dataShipment = shipment,
                textShipment = "Shipment",
                dataInvoice = invoice,
                textInvoice = "Invoice",
                dataPaymentReceive = paymentReceive,
                textPaymentReceive = "Payment Receive"
            });
        }
    }
}