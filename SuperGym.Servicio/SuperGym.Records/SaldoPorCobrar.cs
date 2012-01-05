using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("SALDOPORCOBRAR")]
	public partial class SaldoPorCobrar{

 		public SaldoPorCobrar(){}

 		[Alias("IDPERSONA")]
		[PrimaryKey]
		[Required]
		public System.Int32 Idpersona { get; set;} 

		[Alias("SALDO")]
		[Required]
		public System.Decimal Saldo { get; set;} 

		[Alias("FECHA")]
		[Required]
		public System.DateTime Fecha { get; set;} 

 	}
 }
