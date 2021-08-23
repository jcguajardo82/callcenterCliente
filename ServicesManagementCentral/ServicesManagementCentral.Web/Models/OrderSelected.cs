using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public partial class OrderSelected
    {
        public Int32 OrderNo { get; set; }//int
        public Int64 CnscOrder { get; set; }//bigint
        public Int32 StoreNum { get; set; }//int
        public string UeNo { get; set; }//varchar
        public Int16 StatusUe { get; set; }//tinyint
        public DateTime OrderDate { get; set; }//date
        public TimeSpan OrderTime { get; set; }// time
        public DateTime OrderDeliveryDate { get; set; }// date
        public TimeSpan OrderDeliveryTime { get; set; }// time
        public string CreatedBy { get; set; }// varchar
        public string DeliveryType { get; set; }//  varchar
        public string UeType { get; set; }// varchar
        public Boolean IsPickingManual { get; set; }//bit
        public Int32 CustomerNo { get; set; }//int
        public string CustomerName { get; set; }//varchar
        public string Phone { get; set; }//varchar
        public string Address1 { get; set; }//  varchar
        public string Address2 { get; set; }//  varchar
        public string City { get; set; }// varchar
        public string StateCode { get; set; }// varchar
        public string PostalCode { get; set; }// varchar
        public string Reference { get; set; }// varchar
        public string NameReceives { get; set; }// varchar
        public decimal Total { get; set; }// decimal
        public Int32 NumPoints { get; set; }//int
        public Int32 NumCashier { get; set; }// int
        public Int32 NumPos { get; set; }// int
        public string TransactionId { get; set; }//varchar
        public string MethodPayment { get; set; }// varchar
        public string CardNumber { get; set; }//varchar
        public string ShipperName { get; set; }//varchar
        public DateTime ShippingDate { get; set; }// datetime
        public string TransactionNo { get; set; }// varchar
        public string TrackingNo { get; set; }// varchar
        public Int32 NumBags { get; set; }//int
        public Int32 NumCoolers { get; set; }//int
        public Int32 NumContainers { get; set; }//int
        public string Terminal { get; set; }//varchar
        public DateTime DeliveryDate { get; set; }//    date
        public TimeSpan DeliveryTime { get; set; }//  time
        public string IdReceive { get; set; }// varchar
        public string NameReceive { get; set; }//varchar
        public string Comments { get; set; }// varchar



        public List<OrderDetail> Detail { get; set; }
    }
}