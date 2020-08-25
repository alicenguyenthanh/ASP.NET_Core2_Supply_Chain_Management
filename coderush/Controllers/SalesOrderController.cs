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
    [Authorize(Roles = Pages.MainMenu.SalesOrder.RoleName)]
    public class SalesOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            SalesOrder salesOrder = _context.SalesOrder.SingleOrDefault(x => x.SalesOrderId.Equals(id));

            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        public IActionResult Print(int id)
        {
            SalesOrder salesOrder = _context.SalesOrder
                .Include(x => x.SalesOrderLines)
                .SingleOrDefault(x => x.SalesOrderId.Equals(id));

            if (salesOrder == null)
            {
                return NotFound();
            }

            Branch branch = _context.Branch
                .SingleOrDefault(x => x.BranchId.Equals(salesOrder.BranchId));
            Customer customer = _context.Customer
                .SingleOrDefault(x => x.CustomerId.Equals(salesOrder.CustomerId));
            Currency currency = _context.Currency
                .SingleOrDefault(x => x.CurrencyId.Equals(branch.CurrencyId));

            PrintSalesOrderViewModel vm = new PrintSalesOrderViewModel();
            vm.SalesOrder = salesOrder;
            vm.Branch = branch;
            vm.Customer = customer;
            vm.Currency = currency;
            vm.Lines = new List<PrintSalesOrderLineViewModel>();
            foreach (var item in salesOrder.SalesOrderLines)
            {
                PrintSalesOrderLineViewModel lineVm = new PrintSalesOrderLineViewModel();
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