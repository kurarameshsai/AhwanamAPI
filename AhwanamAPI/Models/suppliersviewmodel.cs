using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MaaAahwanam.Models;

namespace AhwanamAPI.Models
{
    public class suppliersviewmodel
    {
        public List<ManageVendor> managevendor { get; set; }
        public List<AllSupplierServices> supplierservices { get; set; }
    }
}