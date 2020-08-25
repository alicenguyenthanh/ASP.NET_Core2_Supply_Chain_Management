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
    [Authorize(Roles = Pages.MainMenu.Shipment.RoleName)]
    public class ShipmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShipmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Print(int id)
        {
            Shipment shipment = _context.Shipment
                .SingleOrDefault(x => x.ShipmentId.Equals(id));


            if (shipment == null)
            {
                return NotFound();
            }


            SalesOrder salesOrder = _context.SalesOrder
                .Include(x => x.SalesOrderLines)
                .SingleOrDefault(x => x.SalesOrderId.Equals(shipment.SalesOrderId));
            Branch branch = _context.Branch
                .SingleOrDefault(x => x.BranchId.Equals(salesOrder.BranchId));
            Customer customer = _context.Customer
                .SingleOrDefault(x => x.CustomerId.Equals(salesOrder.CustomerId));
            Currency currency = _context.Currency
                .SingleOrDefault(x => x.CurrencyId.Equals(branch.CurrencyId));

            PrintShipmentViewModel vm = new PrintShipmentViewModel();
            vm.Shipment = shipment;
            vm.SalesOrder = salesOrder;
            vm.Branch = branch;
            vm.Customer = customer;
            vm.Currency = currency;
            vm.Lines = new List<PrintShipmentLineViewModel>();
            foreach (var item in salesOrder.SalesOrderLines)
            {
                PrintShipmentLineViewModel lineVm = new PrintShipmentLineViewModel();
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