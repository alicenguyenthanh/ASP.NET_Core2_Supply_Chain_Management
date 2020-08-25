using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Models
{
    public class Shipment : BaseModel
    {
        public int ShipmentId { get; set; }
        [Display(Name = "Shipment Number")]
        public string ShipmentName { get; set; }
        [Display(Name = "Sales Order")]
        public int SalesOrderId { get; set; }
        public DateTimeOffset ShipmentDate { get; set; }
        [Display(Name = "Shipment Type")]
        public int ShipmentTypeId { get; set; }
        [Display(Name = "Warehouse")]
        public int WarehouseId { get; set; }
        [Display(Name = "Full Shipment")]
        public bool IsFullShipment { get; set; } = true;
        public string Remarks { get; set; }
    }

    public class ShipmentViewModel
    {
        public int ShipmentId { get; set; }
        [Display(Name = "Shipment Number")]
        public string ShipmentName { get; set; }
        [Display(Name = "Sales Order")]
        public int SalesOrderId { get; set; }
        public DateTimeOffset ShipmentDate { get; set; }
        [Display(Name = "Shipment Type")]
        public int ShipmentTypeId { get; set; }
        [Display(Name = "Warehouse")]
        public int WarehouseId { get; set; }
        [Display(Name = "Full Shipment")]
        public bool IsFullShipment { get; set; } = true;
        public string Remarks { get; set; }
    }

    public class PrintShipmentViewModel
    {
        public Shipment Shipment { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public Branch Branch { get; set; }
        public Customer Customer { get; set; }
        public Currency Currency { get; set; }
        public List<PrintShipmentLineViewModel> Lines { get; set; }
    }

    public class PrintShipmentLineViewModel
    {
        public Product Product { get; set; }
        public SalesOrderLine SalesOrderLine { get; set; }
    }
}
