using AutoMapper;
using coderush.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BillTypeViewModel, BillType>();
            CreateMap<BillType, BillTypeViewModel>();

            CreateMap<Bill, BillViewModel>();
            CreateMap<BillViewModel, Bill>();

            CreateMap<Branch, BranchViewModel>();
            CreateMap<BranchViewModel, Branch>();

            CreateMap<CashBank, CashBankViewModel>();
            CreateMap<CashBankViewModel, CashBank>();

            CreateMap<Currency, CurrencyViewModel>();
            CreateMap<CurrencyViewModel, Currency>();

            CreateMap<Customer, CustomerViewModel>();
            CreateMap<CustomerViewModel, Customer>();

            CreateMap<CustomerType, CustomerTypeViewModel>();
            CreateMap<CustomerTypeViewModel, CustomerType>();

            CreateMap<GoodsReceivedNote, GoodsReceivedNoteViewModel>();
            CreateMap<GoodsReceivedNoteViewModel, GoodsReceivedNote>();

            CreateMap<Invoice, InvoiceViewModel>();
            CreateMap<InvoiceViewModel, Invoice>();

            CreateMap<InvoiceType, InvoiceTypeViewModel>();
            CreateMap<InvoiceTypeViewModel, InvoiceType>();

            CreateMap<PaymentReceive, PaymentReceiveViewModel>();
            CreateMap<PaymentReceiveViewModel, PaymentReceive>();

            CreateMap<PaymentType, PaymentTypeViewModel>();
            CreateMap<PaymentTypeViewModel, PaymentType>();

            CreateMap<PaymentVoucher, PaymentVoucherViewModel>();
            CreateMap<PaymentVoucherViewModel, PaymentVoucher>();

            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductViewModel, Product>();

            CreateMap<ProductType, ProductTypeViewModel>();
            CreateMap<ProductTypeViewModel, ProductType>();

            CreateMap<PurchaseOrder, PurchaseOrderViewModel>();
            CreateMap<PurchaseOrderViewModel, PurchaseOrder>();

            CreateMap<PurchaseOrderLine, PurchaseOrderLineViewModel>();
            CreateMap<PurchaseOrderLineViewModel, PurchaseOrderLine>();

            CreateMap<PurchaseType, PurchaseTypeViewModel>();
            CreateMap<PurchaseTypeViewModel, PurchaseType>();

            CreateMap<SalesOrder, SalesOrderViewModel>();
            CreateMap<SalesOrderViewModel, SalesOrder>();

            CreateMap<SalesOrderLine, SalesOrderLineViewModel>();
            CreateMap<SalesOrderLineViewModel, SalesOrderLine>();

            CreateMap<SalesType, SalesTypeViewModel>();
            CreateMap<SalesTypeViewModel, SalesType>();

            CreateMap<UnitOfMeasure, UnitOfMeasureViewModel>();
            CreateMap<UnitOfMeasureViewModel, UnitOfMeasure>();

            CreateMap<UserProfile, UserProfileViewModel>();
            CreateMap<UserProfileViewModel, UserProfile>();

            CreateMap<Vendor, VendorViewModel>();
            CreateMap<VendorViewModel, Vendor>();

            CreateMap<VendorType, VendorTypeViewModel>();
            CreateMap<VendorTypeViewModel, VendorType>();

            CreateMap<Warehouse, WarehouseViewModel>();
            CreateMap<WarehouseViewModel, Warehouse>();

            CreateMap<Shipment, ShipmentViewModel>();
            CreateMap<ShipmentViewModel, Shipment>();

        }
    }
}
