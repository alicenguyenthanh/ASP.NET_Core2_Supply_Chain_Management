using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class SalesOrder : BaseModel
    {
        public int SalesOrderId { get; set; }
        [Display(Name = "Order Number")]
        public string SalesOrderName { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }

        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }

        [Display(Name = "Customer Ref. Number")]
        public string CustomerRefNumber { get; set; }
        [Display(Name = "Sales Type")]
        public int SalesTypeId { get; set; }
        public string Remarks { get; set; }
        public double Amount { get; set; }
        public double SubTotal { get; set; }
        public double Discount { get; set; }
        public double Tax { get; set; }
        public double Freight { get; set; }
        public double Total { get; set; }
        public List<SalesOrderLine> SalesOrderLines { get; set; } = new List<SalesOrderLine>();
    }

    public class SalesOrderViewModel
    {
        public int SalesOrderId { get; set; }
        [Display(Name = "Order Number")]
        public string SalesOrderName { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }

        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }

        [Display(Name = "Customer Ref. Number")]
        public string CustomerRefNumber { get; set; }
        [Display(Name = "Sales Type")]
        public int SalesTypeId { get; set; }
        public string Remarks { get; set; }
        public double Amount { get; set; }
        public double SubTotal { get; set; }
        public double Discount { get; set; }
        public double Tax { get; set; }
        public double Freight { get; set; }
        public double Total { get; set; }
        public List<SalesOrderLine> SalesOrderLines { get; set; } = new List<SalesOrderLine>();
    }

    public class PrintSalesOrderViewModel
    {
        public SalesOrder SalesOrder { get; set; }
        public Branch Branch { get; set; }
        public Customer Customer { get; set; }
        public Currency Currency { get; set; }
        public List<PrintSalesOrderLineViewModel> Lines { get; set; }
    }

    public class PrintSalesOrderLineViewModel
    {
        public Product Product { get; set; }
        public SalesOrderLine SalesOrderLine { get; set; }
    }
}
