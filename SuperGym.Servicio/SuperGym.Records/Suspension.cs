using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("SUSPENSION")]
	public partial class Suspension{

 		public Suspension(){}

 		[Alias("ID")]
		[Sequence("GEN_SUSPENSION_ID")]
		[PrimaryKey]
		[AutoIncrement]
		[Required]
		public System.Int32 Id { get; set;} 

		[Alias("IDPAGO")]
		[Required]
		public System.Int32 IdPago { get; set;} 

		[Alias("FECHA")]
		[Required]
		public System.DateTime Fecha { get; set;} 

		[Alias("FECHA_TERMINACION")]
		[Required]
		public System.DateTime FechaTerminacion { get; set;} 

		[Alias("NUEVA_FECHA")]
		[Required]
		public System.DateTime NuevaFecha { get; set;} 

		[Alias("DESDE")]
		[Required]
		public System.DateTime Desde { get; set;} 

		[Alias("HASTA")]
		[Required]
		public System.DateTime Hasta { get; set;} 

 	}
 }