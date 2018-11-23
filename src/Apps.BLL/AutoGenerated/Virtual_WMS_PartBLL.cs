//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Models;
using Apps.Common;
using Unity.Attributes;
using System.Transactions;
using Apps.BLL.Core;
using Apps.Locale;
using LinqToExcel;
using System.IO;
using System.Text;
using Apps.IDAL.WMS;
using Apps.Models.WMS;
using Apps.IBLL.WMS;
namespace Apps.BLL.WMS
{
	public partial class WMS_PartBLL: Virtual_WMS_PartBLL,IWMS_PartBLL
	{
        

	}
	public class Virtual_WMS_PartBLL
	{
        [Dependency]
        public IWMS_PartRepository m_Rep { get; set; }

		public virtual List<WMS_PartModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<WMS_Part> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								
								a=>a.PartCode.Contains(queryStr)
								|| a.PartName.Contains(queryStr)
								|| a.PartType.Contains(queryStr)
								|| a.CustomerCode.Contains(queryStr)
								|| a.LogisticsCode.Contains(queryStr)
								|| a.OtherCode.Contains(queryStr)
								
								|| a.StoreMan.Contains(queryStr)
								|| a.Status.Contains(queryStr)
								|| a.CreatePerson.Contains(queryStr)
								
								|| a.ModifyPerson.Contains(queryStr)
								
								);
            }
            else
            {
                queryData = m_Rep.GetList();
            }
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }

		public virtual List<WMS_PartModel> GetListByUserId(ref GridPager pager, string userId,string queryStr)
		{
			return new List<WMS_PartModel>();
		}
		
		public virtual List<WMS_PartModel> GetListByParentId(ref GridPager pager, string queryStr,object parentId)
        {
			return new List<WMS_PartModel>();
		}

        public virtual List<WMS_PartModel> CreateModelList(ref IQueryable<WMS_Part> queryData)
        {

            List<WMS_PartModel> modelList = (from r in queryData
                                              select new WMS_PartModel
                                              {
													Id = r.Id,
													PartCode = r.PartCode,
													PartName = r.PartName,
													PartType = r.PartType,
													CustomerCode = r.CustomerCode,
													LogisticsCode = r.LogisticsCode,
													OtherCode = r.OtherCode,
													PCS = r.PCS,
													StoreMan = r.StoreMan,
													Status = r.Status,
													CreatePerson = r.CreatePerson,
													CreateTime = r.CreateTime,
													ModifyPerson = r.ModifyPerson,
													ModifyTime = r.ModifyTime,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, WMS_PartModel model)
        {
            try
            {
                WMS_Part entity = m_Rep.GetById(model.Id);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new WMS_Part();
               				entity.Id = model.Id;
				entity.PartCode = model.PartCode;
				entity.PartName = model.PartName;
				entity.PartType = model.PartType;
				entity.CustomerCode = model.CustomerCode;
				entity.LogisticsCode = model.LogisticsCode;
				entity.OtherCode = model.OtherCode;
				entity.PCS = model.PCS;
				entity.StoreMan = model.StoreMan;
				entity.Status = model.Status;
				entity.CreatePerson = model.CreatePerson;
				entity.CreateTime = model.CreateTime;
				entity.ModifyPerson = model.ModifyPerson;
				entity.ModifyTime = model.ModifyTime;
  

                if (m_Rep.Create(entity))
                {
                    return true;
                }
                else
                {
                    errors.Add(Resource.InsertFail);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }



         public virtual bool Delete(ref ValidationErrors errors, object id)
        {
            try
            {
                if (m_Rep.Delete(id) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

        public virtual bool Delete(ref ValidationErrors errors, object[] deleteCollection)
        {
            try
            {
                if (deleteCollection != null)
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        if (m_Rep.Delete(deleteCollection) == deleteCollection.Length)
                        {
                            transactionScope.Complete();
                            return true;
                        }
                        else
                        {
                            Transaction.Current.Rollback();
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

		
       

        public virtual bool Edit(ref ValidationErrors errors, WMS_PartModel model)
        {
            try
            {
                WMS_Part entity = m_Rep.GetById(model.Id);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.Id = model.Id;
				entity.PartCode = model.PartCode;
				entity.PartName = model.PartName;
				entity.PartType = model.PartType;
				entity.CustomerCode = model.CustomerCode;
				entity.LogisticsCode = model.LogisticsCode;
				entity.OtherCode = model.OtherCode;
				entity.PCS = model.PCS;
				entity.StoreMan = model.StoreMan;
				entity.Status = model.Status;
				entity.CreatePerson = model.CreatePerson;
				entity.CreateTime = model.CreateTime;
				entity.ModifyPerson = model.ModifyPerson;
				entity.ModifyTime = model.ModifyTime;
 


                if (m_Rep.Edit(entity))
                {
                    return true;
                }
                else
                {
                    errors.Add(Resource.NoDataChange);
                    return false;
                }

            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

      

        public virtual WMS_PartModel GetById(object id)
        {
            if (IsExists(id))
            {
                WMS_Part entity = m_Rep.GetById(id);
                WMS_PartModel model = new WMS_PartModel();
                              				model.Id = entity.Id;
				model.PartCode = entity.PartCode;
				model.PartName = entity.PartName;
				model.PartType = entity.PartType;
				model.CustomerCode = entity.CustomerCode;
				model.LogisticsCode = entity.LogisticsCode;
				model.OtherCode = entity.OtherCode;
				model.PCS = entity.PCS;
				model.StoreMan = entity.StoreMan;
				model.Status = entity.Status;
				model.CreatePerson = entity.CreatePerson;
				model.CreateTime = entity.CreateTime;
				model.ModifyPerson = entity.ModifyPerson;
				model.ModifyTime = entity.ModifyTime;
 
                return model;
            }
            else
            {
                return null;
            }
        }


		 /// <summary>
        /// 校验Excel数据,这个方法一般用于重写校验逻辑
        /// </summary>
        public virtual bool CheckImportData(string fileName, List<WMS_PartModel> list,ref ValidationErrors errors )
        {
          
            var targetFile = new FileInfo(fileName);

            if (!targetFile.Exists)
            {

                errors.Add("导入的数据文件不存在");
                return false;
            }

            var excelFile = new ExcelQueryFactory(fileName);

            //对应列头
			 				 excelFile.AddMapping<WMS_PartModel>(x => x.PartCode, "PartCode");
				 excelFile.AddMapping<WMS_PartModel>(x => x.PartName, "PartName");
				 excelFile.AddMapping<WMS_PartModel>(x => x.PartType, "PartType");
				 excelFile.AddMapping<WMS_PartModel>(x => x.CustomerCode, "CustomerCode");
				 excelFile.AddMapping<WMS_PartModel>(x => x.LogisticsCode, "LogisticsCode");
				 excelFile.AddMapping<WMS_PartModel>(x => x.OtherCode, "OtherCode");
				 excelFile.AddMapping<WMS_PartModel>(x => x.PCS, "PCS");
				 excelFile.AddMapping<WMS_PartModel>(x => x.StoreMan, "StoreMan");
				 excelFile.AddMapping<WMS_PartModel>(x => x.Status, "Status");
				 excelFile.AddMapping<WMS_PartModel>(x => x.CreatePerson, "CreatePerson");
				 excelFile.AddMapping<WMS_PartModel>(x => x.CreateTime, "CreateTime");
				 excelFile.AddMapping<WMS_PartModel>(x => x.ModifyPerson, "ModifyPerson");
				 excelFile.AddMapping<WMS_PartModel>(x => x.ModifyTime, "ModifyTime");
 
            //SheetName
            var excelContent = excelFile.Worksheet<WMS_PartModel>(0);
            int rowIndex = 1;
            //检查数据正确性
            foreach (var row in excelContent)
            {
                var errorMessage = new StringBuilder();
                var entity = new WMS_PartModel();
						 				  entity.Id = row.Id;
				  entity.PartCode = row.PartCode;
				  entity.PartName = row.PartName;
				  entity.PartType = row.PartType;
				  entity.CustomerCode = row.CustomerCode;
				  entity.LogisticsCode = row.LogisticsCode;
				  entity.OtherCode = row.OtherCode;
				  entity.PCS = row.PCS;
				  entity.StoreMan = row.StoreMan;
				  entity.Status = row.Status;
				  entity.CreatePerson = row.CreatePerson;
				  entity.CreateTime = row.CreateTime;
				  entity.ModifyPerson = row.ModifyPerson;
				  entity.ModifyTime = row.ModifyTime;
 
                //=============================================================================
                if (errorMessage.Length > 0)
                {
                    errors.Add(string.Format(
                        "第 {0} 列发现错误：{1}{2}",
                        rowIndex,
                        errorMessage,
                        "<br/>"));
                }
                list.Add(entity);
                rowIndex += 1;
            }
            if (errors.Count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public virtual void SaveImportData(IEnumerable<WMS_PartModel> list)
        {
            try
            {
                using (DBContainer db = new DBContainer())
                {
                    foreach (var model in list)
                    {
                        WMS_Part entity = new WMS_Part();
                       						entity.Id = 0;
						entity.PartCode = model.PartCode;
						entity.PartName = model.PartName;
						entity.PartType = model.PartType;
						entity.CustomerCode = model.CustomerCode;
						entity.LogisticsCode = model.LogisticsCode;
						entity.OtherCode = model.OtherCode;
						entity.PCS = model.PCS;
						entity.StoreMan = model.StoreMan;
						entity.Status = model.Status;
						entity.CreatePerson = model.CreatePerson;
						entity.CreateTime = ResultHelper.NowTime;
						entity.ModifyPerson = model.ModifyPerson;
						entity.ModifyTime = model.ModifyTime;
 
                        db.WMS_Part.Add(entity);
                    }
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }
		public virtual bool Check(ref ValidationErrors errors, object id,int flag)
        {
			return true;
		}

        public virtual bool IsExists(object id)
        {
            return m_Rep.IsExist(id);
        }
		
		public void Dispose()
        { 
            
        }

	}
}
