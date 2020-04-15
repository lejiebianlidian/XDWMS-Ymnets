﻿using System.Collections.Generic;
using Apps.Common;
using Apps.Models.WMS;

namespace Apps.IBLL.WMS
{
    public partial interface IWMS_Feed_ListBLL
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
         //void AdditionalCheckExcelData(ref WMS_Feed_ListModel model);
    
         /// <summary>
         /// 根据where字符串获取列表数据。
         /// </summary>
         /// <param name="pager"></param>
         /// <param name="whereStr"></param>
         List<WMS_Feed_ListModel> GetListByWhere(ref GridPager pager, string where);

        List<WMS_Feed_ListModel> GetListForConfirm(ref GridPager pager, string releaseBillNums);

        /// <summary>
        /// 根据where字符串获取按单据编号分组后的列表数据。
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<WMS_Feed_ListModel> GetListByWhereAndGroupBy(ref GridPager pager, string where);

        /// <summary>
        /// 打印投料单
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="opt"></param>
        /// <param name="feedBillNum"></param>
        /// <returns></returns>
        string PrintFeedList(ref ValidationErrors errors, string opt, string feedBillNum, int id);

        /// <summary>
        /// 取消打印备料投料单
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="opt"></param>
        /// <param name="releaseBillNum"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool UnPrintFeedList(ref ValidationErrors errors, string opt, string releaseBillNum, int id);

        /// <summary>
        /// 确认投料单
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="opt"></param>
        /// <param name="releaseBillNum"></param>
        /// <returns></returns>
        bool ConfirmFeedList(ref ValidationErrors errors, string opt, string releaseBillNums);

        decimal GetSumByWhere(string where, string sumField);
    }
}
