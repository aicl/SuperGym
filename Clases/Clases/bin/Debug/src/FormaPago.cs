using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("FORMA_PAGO")]
	public partial class FormaPago{

 		public FormaPago(){}

 		[Alias("IDFORMAPAGO")]
		[PrimaryKey]
		[Required]
		public System.Int16 Idformapago { get; set;} 

		[Alias("NOMBRE_FPAGO")]
		public System.String NombreFpago { get; set;} 

 	}
 }
