//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using Apps.Models;
using System;
using System.ComponentModel.DataAnnotations;
namespace Apps.Models.Sys
{

	public partial class SysUserConfigModel:Virtual_SysUserConfigModel
	{
		
	}
	public class Virtual_SysUserConfigModel
	{
		[Display(Name = "非主键ID")]
		public virtual string Id { get; set; }
		[Display(Name = "名称")]
		public virtual string Name { get; set; }
		[Display(Name = "值")]
		public virtual string Value { get; set; }
		[Display(Name = "类型")]
		public virtual string Type { get; set; }
		[Display(Name = "状态")]
		public virtual Nullable<bool> State { get; set; }
		[Display(Name = "所属用户")]
		public virtual string UserId { get; set; }
		}
}