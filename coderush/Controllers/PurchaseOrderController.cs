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
    [Authorize(Roles = Pages.MainMenu.PurchaseOrder.RoleName)]
    public class PurchaseOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int id)
        {
            PurchaseOrder purchaseOrder = _context.PurchaseOrder.SingleOrDefault(x => x.PurchaseOrderId.Equals(id));

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }

        public IActionResult Print(int id)
        {
            PurchaseOrder purchaseOrder = _context.PurchaseOrder
                .Include(x => x.PurchaseOrderLines)
                .SingleOrDefault(x => x.PurchaseOrderId.Equals(id));

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            Branch branch = _context.Branch
                .SingleOrDefault(x => x.BranchId.Equals(purchaseOrder.BranchId));
            Vendor vendor = _context.Vendor
                .SingleOrDefault(x => x.VendorId.Equals(purchaseOrder.VendorId));
            Currency currency = _context.Currency
                .SingleOrDefault(x => x.CurrencyId.Equals(branch.CurrencyId));

            PrintPurchaseOrderViewModel vm = new PrintPurchaseOrderViewModel();
            vm.PurchaseOrder = purchaseOrder;
            vm.Branch = branch;
            vm.Vendor = vendor;
            vm.Currency = currency;
            vm.Lines = new List<PrintPurchaseOrderLineViewModel>();
            foreach (var item in purchaseOrder.PurchaseOrderLines)
            {
                PrintPurchaseOrderLineViewModel lineVm = new PrintPurchaseOrderLineViewModel();
                Product product = _context.Product
                    .SingleOrDefault(x => x.ProductId.Equals(item.ProductId));

                lineVm.PurchaseOrderLine = item;
                lineVm.Product = product;

                vm.Lines.Add(lineVm);

            }
            return View(vm);
        }
    }
}