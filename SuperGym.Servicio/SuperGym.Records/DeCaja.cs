using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("DECAJA")]
	public partial class Decaja{

 		public Decaja(){}

 		[Alias("ID")]
		[Sequence("GEN_DECAJA_ID")]
		[PrimaryKey]
		[AutoIncrement]
		[Required]
		public System.Int32 Id { get; set;} 

		[Alias("IDCAJA")]
		[Required]
		public System.Int32 IdCaja { get; set;} 

		[Alias("CONCEPTO")]
		[Required]
		public System.String Concepto { get; set;} 

		[Alias("FACTOR")]
		[Required]
		public System.Int16 Factor { get; set;} 

		[Alias("DESCRIPCION")]
		[Required]
		public System.String Descripcion { get; set;} 

		[Alias("VALOR")]
		[Required]
		public System.Decimal Valor { get; set;} 

		[Alias("DOCUMENTO")]
		public System.String Documento { get; set;} 

		[Alias("NOMBRE")]
		public System.String Nombre { get; set;} 

 	}
 }