using PartTracking.Context.Models.Validator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PartTracking.Context.Models.DTO
{
    public class CustomerWorkOrderView
    {
        #region workorder
        [DisplayName("Key #")]
        public int WOId { get; set; }
        [DisplayName("Work-Order #")]
        public int WorkOrderId { get; set; }
        // this quantity needs to pull from warehouse
        [DisplayName("Required Quantity")]
        public int PartQuantityRequired { get; set; }
        #endregion

        #region partmaster
        [DisplayName("Part #")]
        public int PartMasterId { get; set; }
        [DisplayName("Part Name")]
        public string PartName { get; set; }
        [DisplayName("Part Code")]
        public string PartCode { get; set; }
        // this quantity equals available Quantity @ PartMaster table
        [DisplayName("Available Quantity")]
        public int? PartQuantityAtWarehouse { get; set; }
        #endregion

        #region customerorder
        [DisplayName("Customer Order #")]
        public int CustomerOrderId { get; set; }
        [DisplayName("Customer #")]
        public int CustomerId { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName ("Order Quantity")]
        public int OrderQuantity { get; set; }
        #endregion

        #region
        [DisplayName("Pull Quantity")]
        [Required(ErrorMessage = "Quantity is Required!")]
        [PullQtyAvailableQtyRequireQty("PartQuantityAtWarehouse", "BalanceAfterPull", ErrorMessage = "Requirement : Pull Quantity <= Available Quantity (AND) Pull Quantity <= Balance Quantity !")]
        [PullQtyGreaterThanZero(ErrorMessage = "Pull Quantity Must Be > 0 !")]
        public int PullQuantity { get; set; }

        [DisplayName("Balance Quantity[ZERO]")]
        public int BalanceAfterPull { get; set; }
        #endregion

    }
}
