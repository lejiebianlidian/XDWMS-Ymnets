//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using Apps.Models;
using Apps.Models.Spl;
using Apps.IDAL.Spl;
using System;
using System.Linq;
using System.Data.SqlClient;

namespace Apps.DAL.Spl
{
	public partial class Spl_WarehouseAllocationRepository
	{
        public void UpdateWareStockPileAllocation(string WarehouseAllocationId)
        {

            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@WarehouseAllocationId",WarehouseAllocationId),
            };
            Context.Database.ExecuteSqlCommand(@"
                declare 
                @WareDetailsId varchar(50), 
                @FromWarehouseId varchar(50),
                @ToWarehouseId varchar(50),
                @Quantity int,
                @Price money
                declare auth_cur cursor for
                select a.WareDetailsId,b.FromWarehouseId,b.ToWarehouseId,a.Quantity,a.Price from dbo.Spl_WarehouseAllocationDetails a join Spl_WarehouseAllocation b
                on a.WarehouseAllocationId=b.id where  WarehouseAllocationId=@WarehouseAllocationId
                open auth_cur
                fetch next from auth_cur into @WareDetailsId,@FromWarehouseId,@ToWarehouseId,@Quantity,@Price
                while (@@fetch_status=0)
                  begin
	                if(select COUNT(*) from Spl_WareStockPile where WareDetailsId=@WareDetailsId and WarehouseId=@ToWarehouseId)=0
	                begin
		                insert into Spl_WareStockPile(id,WareDetailsId,WarehouseId,firstenterdate,lastleavedate,Quantity,price,createtime) 
		                select NEWID(),@WareDetailsId,@ToWarehouseId,GETDATE(),null,@Quantity,@Price,GETDATE()
	                end
	                else
	                begin
		                update Spl_WareStockPile set Quantity=Quantity+@Quantity,Price=@Price where WareDetailsId=@WareDetailsId and WarehouseId=@ToWarehouseId
		                update Spl_WareStockPile set Quantity=Quantity-@Quantity,Price=@Price where WareDetailsId=@WareDetailsId and WarehouseId=@FromWarehouseId
	                end
                    --可能数据错误的处理方式
                    fetch next from auth_cur into @WareDetailsId,@FromWarehouseId,@ToWarehouseId,@Quantity,@Price
                  end
                close auth_cur
                deallocate auth_cur", para);
        }

        public IQueryable<Spl_WareAllocationReportModel> GetWareAllocationList(string warehouseId, DateTime begin, DateTime end)
        {
            string strSql = @"
select yy.Name CategoryName,ss.Name,ss.Code,ss.Size,ss.Brand,ss.Unit,ss.Vender,ss.SalePrice,ss.WareCategoryId,
(select quantity from dbo.Spl_WareStockPile where WarehouseId=yy.WarehouseId and WareDetailsId=yy.WareDetailsId) nowQuantity,
ss.LowerLimit,
Quantity,ss.Material,
Quantity*ss.SalePrice as QuantityTotal
  from Spl_WareDetails ss 
join (
select a.WareDetailsId,a.WarehouseId,SUM(Quantity) Quantity,d.Name
from dbo.Spl_WarehouseAllocationDetails a 
join dbo.Spl_WarehouseAllocation b on a.WarehouseAllocationId = b.Id
join dbo.Spl_WareDetails c on a.WareDetailsId = c.id
join dbo.Spl_WareCategory d on c.WareCategoryId = d.Id
where b.State = 1 and a.WarehouseId = @warehouseId and b.InTime >=@begin and b.InTime<=@end
group by a.WareDetailsId,a.WarehouseId,d.Name) yy on ss.id=yy.WareDetailsId
 order by ss.WareCategoryId";

            SqlParameter[] para = new SqlParameter[]
            {
                new SqlParameter("@warehouseId",warehouseId),
                new SqlParameter("@begin",begin),
                new SqlParameter("@end",end)
            };

            return Context.Database.SqlQuery<Spl_WareAllocationReportModel>(strSql, para).AsQueryable();
        }
    }
}