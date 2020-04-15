﻿using System;
using System.ComponentModel.DataAnnotations;
using Apps.Models;
namespace Apps.Models.WMS
{
    public partial class WMS_ReturnOrderModel
    {
        [Display(Name = "物料编码")]
        [Required]
        public override Nullable<int> PartID { get; set; }
        [Display(Name = "代理商编码")]
        [Required]
        public override Nullable<int> SupplierId { get; set; }

        [Display(Name = "到货单号")]
        public string ArrivalBillNum { get; set; }

        [Display(Name = "检验单号")]
        public string InspectBillNum { get; set; }

        [Display(Name = "物料编码")]
        public string PartCode { get; set; }

        [Display(Name = "物料名称")]
        public string PartName { get; set; }

        [Display(Name = "供应商简称")]
        public string SupplierShortName { get; set; }

        [Display(Name = "供应商编码")]
        public string SupplierCode { get; set; }

        [Display(Name = "供应商全称")]
        public string SupplierName { get; set; }

        [Display(Name = "库房编码")]
        public string InvCode { get; set; }

        [Display(Name = "库房")]
        public string InvName { get; set; }

        [Display(Name = "到货数量")]
        public decimal? ArrivalQty { get; set; }

        [Display(Name = "合格数量")]
        public decimal? QualifyNum { get; set; }

        [Display(Name = "不合格数量")]
        public decimal? NoQualifyNum { get; set; }

        [Display(Name = "本次退货数量")]
        public decimal? CurReturnQty { get; set; }

        [Display(Name = "本次报废数量")]
        public decimal? CurLossQty { get; set; }
    }
}

