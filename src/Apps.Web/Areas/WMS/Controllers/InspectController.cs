﻿using System.Collections.Generic;
using System.Linq;
using Apps.Web.Core;
using Apps.IBLL.WMS;
using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;
using Apps.IBLL;
using Apps.Models.WMS;
using Unity.Attributes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Data;

namespace Apps.Web.Areas.WMS.Controllers
{
    public class InspectController : BaseController
    {
        [Dependency]
        public IWMS_AIBLL m_BLL { get; set; }
        ValidationErrors errors = new ValidationErrors();

        [SupportFilter]
        public ActionResult Index()
        {
            //定义送检状态下拉框的值
            List<ReportType> InspTypes = new List<ReportType>();
            InspTypes.Add(new ReportType() { Type = 0, Name = "" });
            InspTypes.Add(new ReportType() { Type = 1, Name = "未入库" });
            InspTypes.Add(new ReportType() { Type = 2, Name = "已入库" });
            ViewBag.InStoreStatus = new SelectList(InspTypes, "Name", "Name");

            return View();
        }
        [HttpPost]
        [SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string inspectBillNum, string po, string supplierShortName, string partCode, DateTime beginDate, DateTime endDate, string inStoreStatus)
        {
            if (!String.IsNullOrEmpty(po))
            {
                po = po.Trim();
            }
            if (!String.IsNullOrEmpty(inspectBillNum))
            {
                inspectBillNum = inspectBillNum.Trim();
            }
            if (!String.IsNullOrEmpty(supplierShortName))
            {
                supplierShortName = supplierShortName.Trim();
            }
            if (!String.IsNullOrEmpty(partCode))
            {
                partCode = partCode.Trim();
            }
            //TODO:查询出检验状态=已送检 and 入库状态=未入库 的记录
            //List<WMS_AIModel> list = m_BLL.GetListByWhere(ref pager, "InspectStatus == \"已检验\" && InStoreStatus == \"未入库\" ")
            //    .OrderBy(p => p.InspectBillNum).ToList();
            List<WMS_AIModel> list = m_BLL.GetListByWhere(ref pager, "WMS_PO.PO.Contains(\"" + po + "\")&&InspectBillNum.Contains(\"" + inspectBillNum + "\") && WMS_PO.WMS_Supplier.SupplierShortName.Contains(\""
               + supplierShortName + "\")&& WMS_PO.WMS_Part.PartCode.Contains(\"" + partCode + "\")&& InStoreStatus.Contains(\"" + inStoreStatus + "\")&& InspectDate>=(\""
               + beginDate + "\")&& InspectDate<=(\"" + endDate + "\")");
            GridRows<WMS_AIModel> grs = new GridRows<WMS_AIModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }

        #region 创建
        [SupportFilter]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [SupportFilter]
        [ValidateInput(false)]
        public JsonResult Create(string arrivalBillNum)
        {
            try
            {
                var inspectBillNum = m_BLL.CreateInspectBill(GetUserTrueName(), arrivalBillNum);
                LogHandler.WriteServiceLog(GetUserTrueName(), "保存送检单成功", "成功", "保存", "WMS_AI");

                //Response.Redirect("~/Report/ReportManager/Show?id=1&searchValues=" + inspectBillNum);
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed, inspectBillNum));
            }
            catch (Exception ex)
            {
                LogHandler.WriteServiceLog(GetUserTrueName(), ex.Message, "失败", "保存", "WMS_AI");
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ex.Message));
            }
        }

        #endregion

        #region 修改
        [SupportFilter]
        public ActionResult Edit(long id)
        {
            WMS_AIModel entity = m_BLL.GetById(id);
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(WMS_AIModel model)
        {
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.Edit(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserTrueName(), "Id" + model.Id + ",ArrivalBillNum" + model.ArrivalBillNum, "成功", "修改", "WMS_AI");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserTrueName(), "Id" + model.Id + ",ArrivalBillNum" + model.ArrivalBillNum + "," + ErrorCol, "失败", "修改", "WMS_AI");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
        }
        #endregion

        #region 详细
        [SupportFilter]
        public ActionResult Details(long id)
        {
            WMS_AIModel entity = m_BLL.GetById(id);
            return View(entity);
        }

        #endregion

        #region 删除
        [HttpPost]
        [SupportFilter]
        public ActionResult Delete(int id)
        {
            
            if (id != 0)
            {
                //if (m_BLL.Delete(ref errors, id))
                if (m_BLL.CancelInspectBill(ref errors, GetUserTrueName(),id))
                {
                    LogHandler.WriteServiceLog(GetUserTrueName(), "Id:" + id, "成功", "删除", "WMS_AI");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserTrueName(), "Id" + id + "," + ErrorCol, "失败", "删除", "WMS_AI");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail));
            }
        }
        #endregion

        #region 导出导入
        [HttpPost]
        [SupportFilter]
        public ActionResult Import(string filePath)
        {
            if (m_BLL.ImportExcelData(GetUserTrueName(), Utils.GetMapPath(filePath), ref errors))
            {
                LogHandler.WriteImportExcelLog(GetUserTrueName(), "WMS_AI", filePath.Substring(filePath.LastIndexOf('/') + 1), filePath, "导入成功");
                return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed, filePath));
            }
            else
            {
                LogHandler.WriteImportExcelLog(GetUserTrueName(), "WMS_AI", filePath.Substring(filePath.LastIndexOf('/') + 1), filePath, "导入失败");
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail, filePath));
            }
        }
        [HttpPost]
        [SupportFilter(ActionName = "Export")]
        public JsonResult CheckExportData(string queryStr)
        {
            List<WMS_AIModel> list = m_BLL.GetList(ref setNoPagerAscById, queryStr);
            if (list.Count().Equals(0))
            {
                return Json(JsonHandler.CreateMessage(0, "没有可以导出的数据"));
            }
            else
            {
                return Json(JsonHandler.CreateMessage(1, "可以导出"));
            }
        }
        [SupportFilter]
        public ActionResult Export(string inspectBillNum, string po, string supplierShortName, string partCode, DateTime beginDate, DateTime endDate, string inStoreStatus)
        {
            //List<WMS_AIModel> list = m_BLL.GetList(ref setNoPagerAscById, queryStr);
            List<WMS_AIModel> list = m_BLL.GetListByWhere(ref setNoPagerAscById, "WMS_PO.PO.Contains(\"" + po + "\")&&InspectBillNum.Contains(\"" + inspectBillNum + "\") && WMS_PO.WMS_Supplier.SupplierShortName.Contains(\""
               + supplierShortName + "\")&& WMS_PO.WMS_Part.PartCode.Contains(\"" + partCode + "\")&& InStoreStatus.Contains(\"" + inStoreStatus + "\")&& InspectDate>=(\""
               + beginDate + "\")&& InspectDate<=(\"" + endDate + "\")");
            JArray jObjects = new JArray();
            foreach (var item in list)
            {
                var jo = new JObject();
                //jo.Add("Id", item.Id);
                jo.Add("送检单号", item.InspectBillNum);
                jo.Add("到货单据号", item.ArrivalBillNum);
                jo.Add("采购订单号", item.PO);
                jo.Add("物料编码", item.PartCode);
                jo.Add("物料名称", item.PartName);
                jo.Add("供应商简称", item.SupplierShortName);
                jo.Add("到货数量", item.ArrivalQty);
                jo.Add("到货箱数", item.BoxQty);
                jo.Add("送检人", item.InspectMan);
                jo.Add("送检日期", item.InspectDate);
                jo.Add("送检状态", item.InspectStatus);
                jo.Add("入库状态", item.InStoreStatus);

                //jo.Add("到货日期", item.ArrivalDate);
                //jo.Add("接收人", item.ReceiveMan);
                //jo.Add("到货状态", item.ReceiveStatus);   
                //jo.Add("检验日期", item.CheckOutDate);
                //jo.Add("检验结果", item.CheckOutResult);
                //jo.Add("合格数量", item.QualifyQty);
                //jo.Add("不合格数量", item.NoQualifyQty);
                //jo.Add("检验说明", item.CheckOutRemark);
                //jo.Add("重新送检单", item.ReInspectBillNum);
                //jo.Add("入库单号", item.InStoreBillNum);
                //jo.Add("InStoreMan", item.InStoreMan);
                //jo.Add("入库仓库", item.InvId);
                //jo.Add("子库", item.SubInvId);                
                //jo.Add("Attr1", item.Attr1);
                //jo.Add("Attr2", item.Attr2);
                //jo.Add("Attr3", item.Attr3);
                //jo.Add("Attr4", item.Attr4);
                //jo.Add("Attr5", item.Attr5);
                //jo.Add("创建人", item.CreatePerson);
                //jo.Add("创建时间", item.CreateTime);
                //jo.Add("修改人", item.ModifyPerson);
                //jo.Add("修改时间", item.ModifyTime);
                jObjects.Add(jo);
            }
            var dt = JsonConvert.DeserializeObject<DataTable>(jObjects.ToString());
            var exportFileName = string.Concat(
                RouteData.Values["controller"].ToString() + "_",
                DateTime.Now.ToString("yyyyMMddHHmmss"),
                ".xlsx");
            return new ExportExcelResult
            {
                SheetName = "Sheet1",
                FileName = exportFileName,
                ExportData = dt
            };
        }
        [SupportFilter(ActionName = "Export")]
        public ActionResult ExportTemplate()
        {
            JArray jObjects = new JArray();
            var jo = new JObject();
            jo.Add("Id", "");
            jo.Add("到货单据号", "");
            jo.Add("采购订单ID", "");
            jo.Add("到货箱数", "");
            jo.Add("到货数量", "");
            jo.Add("到货日期", "");
            jo.Add("接收人", "");
            jo.Add("到货状态", "");
            jo.Add("送检单号", "");
            jo.Add("送检人", "");
            jo.Add("送检日期", "");
            jo.Add("送检状体", "");
            jo.Add("检验日期", "");
            jo.Add("检验结果", "");
            jo.Add("合格数量", "");
            jo.Add("不合格数量", "");
            jo.Add("检验说明", "");
            jo.Add("重新送检单", "");
            jo.Add("入库单号", "");
            jo.Add("InStoreMan", "");
            jo.Add("入库仓库", "");
            jo.Add("入库状态", "");
            jo.Add("Attr1", "");
            jo.Add("Attr2", "");
            jo.Add("Attr3", "");
            jo.Add("Attr4", "");
            jo.Add("Attr5", "");
            jo.Add("创建人", "");
            jo.Add("创建时间", "");
            jo.Add("修改人", "");
            jo.Add("修改时间", "");
            jo.Add("导入的错误信息", "");
            jObjects.Add(jo);
            var dt = JsonConvert.DeserializeObject<DataTable>(jObjects.ToString());
            var exportFileName = string.Concat(
                    RouteData.Values["controller"].ToString() + "_Template",
                    ".xlsx");
            return new ExportExcelResult
            {
                SheetName = "Sheet1",
                FileName = exportFileName,
                ExportData = dt
            };
        }
        #endregion

        #region 加载指定到货单的信息
        [HttpPost]
        [SupportFilter(ActionName = "Index")]
        public JsonResult GetArrivalBillList(GridPager pager, string arrivalBillNum)
        {
            List<WMS_AIModel> list = m_BLL.GetListByWhere(ref pager, "ArrivalBillNum == \"" + arrivalBillNum + "\" && InspectStatus == \"" + "未送检" + "\"").ToList();
            GridRows<WMS_AIModel> grs = new GridRows<WMS_AIModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion

        #region 选择送检单
        /// <summary>
        /// 弹出选择送检单
        /// </summary>
        /// <param name="mulSelect">是否多选</param>
        /// <returns></returns>
        [SupportFilter(ActionName = "Create")]
        public ActionResult InspectBillLookUp(bool mulSelect = false)
        {
            return View();
        }

        [HttpPost]
        [SupportFilter(ActionName = "Create")]
        public JsonResult InspectBillGetList(GridPager pager, string queryStr)
        {
            //TODO:显示有效且已打印的送检单。
            //List<WMS_AIModel> list = m_BLL.GetListByWhere(ref pager, "InspectStatus == \"已送检\" and InStoreStatus == \"未入库\"")
            //    .OrderBy(p => p.InspectBillNum).ToList();
            List<WMS_AIModel> list = m_BLL.GetListByWhereAndGroupByInspectBillNum(ref pager, "InspectStatus == \"已送检\" and InStoreStatus == \"未入库\"");
            GridRows<WMS_AIModel> grs = new GridRows<WMS_AIModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #endregion
    }
}

