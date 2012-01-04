using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("CAJA")]
	public partial class Caja{

 		public Caja(){}

 		[Alias("ID")]
		[Sequence("GEN_CAJA_ID")]
		[PrimaryKey]
		[AutoIncrement]
		[Required]
		public System.Int32 Id { get; set;} 

		[Alias("FECHA")]
		[Required]
		public System.DateTime Fecha { get; set;} 

		[Alias("FECHA_ASENTADO")]
		public System.DateTime? FechaAsentado { get; set;} 

		[Alias("INGRESOS")]
		[Required]
		public System.Decimal Ingresos { get; set;} 

		[Alias("SALIDAS")]
		[Required]
		public System.Decimal Salidas { get; set;} 

		[Alias("ASENTADO_POR")]
		public System.Int16? AsentadoPor { get; set;}
		
		[Alias("SALDOANTERIOR")]
		[Required]
		public System.Decimal SaldoAnterior { get; set;} 
		
		[Alias("TRASLADAR_A")]
		[Required]
		public System.DateTime? TrasladarA { get; set;} 
		

	}
	
	[Alias("CAJA")]
	public partial class AsentadorCaja{

 		public AsentadorCaja(){}

 		[Alias("ID")]
		[PrimaryKey]
		public System.Int32 Id { get; set;} 
		
		[Alias("ASENTADO_POR")]
		public System.Int16? AsentadoPor { get; set;} 
		
		[Alias("TRASLADAR_A")]
		public System.DateTime TrasladarA { get; set;} 

	}
	
	
	
	
 }