using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("TIPOS_PAGO")]
	public partial class TipoPago{

 		public TipoPago(){}

 		[Alias("IDTIPO_PAGO")]
		[PrimaryKey]
		[Required]
		[Sequence("GEN_TIPOS_PAGO_ID")]
		public System.Int16 Id { get; set;} 

		[Alias("NOMBRE")]
		public System.String Nombre { get; set;} 

		[Alias("VALOR")]
		public System.Decimal Valor { get; set;} 

		[Alias("DIAS")]
		public System.Int32 Dias { get; set;} 
		
		[Alias("DESDE")]
		public System.DateTime ValidoDesde { get; set;} 
		
		
		[Alias("HASTA")]
		public System.DateTime ValidoHasta { get; set;} 

 	}
 }
