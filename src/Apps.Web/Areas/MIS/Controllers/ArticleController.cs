﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//	   生成时间 2013-04-25 15:26:22 by YmNets
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Apps.IBLL;
using Apps.Common;
using Apps.Models;
using Apps.Models.MIS;
using Apps.Web.Core;
using Apps.IBLL.MIS;
using Apps.Core.PageControl;
using Unity.Attributes;

namespace Apps.Web.Areas.MIS.Controllers
{
    public class ArticleController : BaseController
    {
        /// <summary>
        /// 业务层注入
        /// </summary>
        [Dependency]
        public IMIS_ArticleBLL m_BLL { get; set; }
        [Dependency]
        public IMIS_Article_CategoryBLL categoryBLL { get; set; }
        ValidationErrors errors = new ValidationErrors();

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns>视图</returns>
        [SupportFilter]
        public ActionResult Index(string querystr, string cid, int id = 1)
        {
            //条数
            int rows = 3;
            //数据
            GridPager pager = new GridPager()
            {
                page = id,
                rows = rows,
                sort = "CreateTime",
                order = "desc",
            };
            List<MIS_ArticleModel> orders = new List<MIS_ArticleModel>();
            orders = m_BLL.GetList(ref pager, "", "", true, "", 2);
            var list = new BaseList<MIS_ArticleModel>(id, rows, pager.totalRows, pager.totalPages, orders);//pager.totalPages,
            return View(list);

        }


        #region 详细

        public ActionResult Details(string id)
        {

            MIS_ArticleModel entity = m_BLL.GetById(id);
            return View(entity);
        }

        #endregion



    }
}
