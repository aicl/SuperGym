using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("TIPOS_PAGO")]
	public partial class TiposPago{

 		public TiposPago(){}

 		[Alias("IDTIPO_PAGO")]
		[PrimaryKey]
		[Required]
		public System.Int16 IdtipoPago { get; set;} 

		[Alias("NOMBRE")]
		public System.String Nombre { get; set;} 

		[Alias("VALOR")]
		public System.Decimal? Valor { get; set;} 

		[Alias("DIAS")]
		public System.Int32? Dias { get; set;} 

 	}
 }
