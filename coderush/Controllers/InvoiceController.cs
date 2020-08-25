using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.Invoice.RoleName)]
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Print(int id)
        {
            Invoice invoice = _context.Invoice
                .SingleOrDefault(x => x.InvoiceId.Equals(id));

            if (invoice == null)
            {
                return NotFound();
            }
            Shipment shipment = _context.Shipment
                .SingleOrDefault(x => x.ShipmentId.Equals(invoice.ShipmentId));
            SalesOrder salesOrder = _context.SalesOrder
                .Include(x => x.SalesOrderLines)
                .SingleOrDefault(x => x.SalesOrderId.Equals(shipment.SalesOrderId));
            Branch branch = _context.Branch
                .SingleOrDefault(x => x.BranchId.Equals(salesOrder.BranchId));
            Customer customer = _context.Customer
                .SingleOrDefault(x => x.CustomerId.Equals(salesOrder.CustomerId));
            Currency currency = _context.Currency
                .SingleOrDefault(x => x.CurrencyId.Equals(branch.CurrencyId));

            PrintInvoiceViewModel vm = new PrintInvoiceViewModel();
            vm.Invoice = invoice;
            vm.Shipment = shipment;
            vm.SalesOrder = salesOrder;
            vm.Branch = branch;
            vm.Customer = customer;
            vm.Currency = currency;
            vm.Lines = new List<PrintInvoiceLineViewModel>();
            foreach (var item in salesOrder.SalesOrderLines)
            {
                PrintInvoiceLineViewModel lineVm = new PrintInvoiceLineViewModel();
                Product product = _context.Product
                    .SingleOrDefault(x => x.ProductId.Equals(item.ProductId));

                lineVm.SalesOrderLine = item;
                lineVm.Product = product;

                vm.Lines.Add(lineVm);

            }
            return View(vm);
        }
    }
}