﻿using System;
using System.Collections.Generic;
using Apps.Common;
using Apps.Models.WMS;

namespace Apps.IBLL.WMS
{
    public partial interface IWMS_Product_EntryBLL
    {
         /// <summary>
         /// 导入Excel文件，当发生导入错误时，回写错误信息，并且全部回滚。
         /// </summary>
         /// <param name="oper">操作员ID</param>
         /// <param name="filePath"></param>
         /// <param name="errors"></param>
         /// <returns></returns>
         bool ImportExcelData(string oper, string filePath, ref ValidationErrors errors);
    
         /// <summary>
         /// 根据where字符串获取列表数据。
         /// </summary>
         /// <param name="pager"></param>
         /// <param name="whereStr"></param>
         List<WMS_Product_EntryModel> GetListByWhere(ref GridPager pager, string where);

        /// <summary>
        /// 不分页的合计数
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sumField"></param>
        /// <returns></returns>
        decimal GetSumByWhere(string where, string sumField);
    }
}
