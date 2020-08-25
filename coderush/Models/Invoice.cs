using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class Invoice : BaseModel
    {
        public int InvoiceId { get; set; }
        [Display(Name = "Invoice Number")]
        public string InvoiceName { get; set; }
        [Display(Name = "Shipment")]
        public int ShipmentId { get; set; }
        [Display(Name = "Invoice Date")]
        public DateTimeOffset InvoiceDate { get; set; }
        [Display(Name = "Invoice Due Date")]
        public DateTimeOffset InvoiceDueDate { get; set; }
        [Display(Name = "Invoice Type")]
        public int InvoiceTypeId { get; set; }
        public string Remarks { get; set; }
    }

    public class InvoiceViewModel
    {
        public int InvoiceId { get; set; }
        [Display(Name = "Invoice Number")]
        public string InvoiceName { get; set; }
        [Display(Name = "Shipment")]
        public int ShipmentId { get; set; }
        [Display(Name = "Invoice Date")]
        public DateTimeOffset InvoiceDate { get; set; }
        [Display(Name = "Invoice Due Date")]
        public DateTimeOffset InvoiceDueDate { get; set; }
        [Display(Name = "Invoice Type")]
        public int InvoiceTypeId { get; set; }
        public string Remarks { get; set; }
    }

    public class PrintInvoiceViewModel
    {
        public Invoice Invoice { get; set; }
        public Shipment Shipment { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public Branch Branch { get; set; }
        public Customer Customer { get; set; }
        public Currency Currency { get; set; }
        public List<PrintInvoiceLineViewModel> Lines { get; set; }
    }

    public class PrintInvoiceLineViewModel
    {
        public Product Product { get; set; }
        public SalesOrderLine SalesOrderLine { get; set; }
    }
}
