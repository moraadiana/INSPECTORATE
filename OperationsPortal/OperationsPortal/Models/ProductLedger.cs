using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsPortal.Models
{
	public class ProductLedger
	{
        public string contractNo { get; set; }
        public string Product { get; set; }
        public string facilityName { get; set; }
        public string vesselName { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string unitCost { get; set; }
        public string UOM { get; set; }
        public string totalValue { get; set; }
    }
}