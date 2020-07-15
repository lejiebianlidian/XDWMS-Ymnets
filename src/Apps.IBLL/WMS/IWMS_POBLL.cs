﻿using System.Collections.Generic;
using Apps.Common;
using Apps.Models.WMS;

namespace Apps.IBLL.WMS
{
    public partial interface IWMS_POBLL
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
         /// 对导入进行附加的校验，例如物料编码是否存在等。
         /// </summary>
         /// <param name="model"></param>
         //void AdditionalCheckExcelData(ref WMS_POModel model);

        /// <summary>
        /// 根据where字符串获取列表数据
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        List<WMS_POModel> GetListByWhere(ref GridPager pager, string where);

        /// <summary>
        /// 根据where字符串获取按单据编号分组后的列表数据。
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<WMS_POModel> GetListByWhereAndGroupBy(ref GridPager pager, string where);

        /// <summary>
        /// 删除采购订单（整单删除）
        /// </summary>
        /// <param name="oper"></param>
        /// <param name="PO"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        bool DeletePO(ref ValidationErrors errors, string oper, string PO);
    }
}
