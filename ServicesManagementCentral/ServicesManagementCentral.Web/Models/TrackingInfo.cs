using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class TrackingInfo
    {
        public decimal PackageHeight { get; set; }
        public decimal PackageLength { get; set; }
        public decimal PackageWeight { get; set; }
        public decimal PackageWidth { get; set; }
        public string PackageType { get; set; }
        public int OrderNo { get; set; }
        public long Barcode { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public string TrackingServiceName { get; set; }
    }
}