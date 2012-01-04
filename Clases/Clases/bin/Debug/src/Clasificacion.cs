using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("CLASIFICACION")]
	public partial class Clasificacion{

 		public Clasificacion(){}

 		[Alias("IDCLASIFICACION")]
		[PrimaryKey]
		[Required]
		public System.Int16 Idclasificacion { get; set;} 

		[Alias("NOMBRE")]
		public System.String Nombre { get; set;} 

 	}
 }
