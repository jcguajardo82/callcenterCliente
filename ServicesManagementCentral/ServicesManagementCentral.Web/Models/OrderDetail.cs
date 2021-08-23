using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class OrderDetail
    {
        public Int32 OrderNo { get; set; }//int
        public Int64 CnscOrder { get; set; }//bigint
        public int ItemCnsc { get; set; }//smallint
        public Int32 ProductId { get; set; }//int
        public Int64 Barcode { get; set; }//bigint
        public string ProductName { get; set; }//varchar
        public decimal Quantity { get; set; }//decimal
        public decimal Price { get; set; }//decimal
        public string Observations { get; set; }//varchar
        public string UnitMeasure { get; set; }//varchar
        public Boolean ItemSupplied { get; set; }//bit
    }
}