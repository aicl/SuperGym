using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("CONCEPTOS")]
	public partial class Concepto{

 		public Concepto(){}

 		[Alias("ID")]
		[Sequence("GEN_CONCEPTOS_ID")]
		[PrimaryKey]
		[AutoIncrement]
		[Required]
		public System.Int16 Id { get; set;} 

		[Alias("DESCRIPCION")]
		[Required]
		public System.String Descripcion { get; set;} 

		[Alias("FACTOR")]
		[Required]
		public System.Int16 Factor { get; set;} 

		[Alias("CLASIFICACION")]
		[Required]
		public System.String Clasificacion { get; set;} 

 	}
 }

