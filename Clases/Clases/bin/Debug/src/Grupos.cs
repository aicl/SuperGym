using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("GRUPOS")]
	public partial class Grupos{

 		public Grupos(){}

 		[Alias("IDGRUPO")]
		[PrimaryKey]
		[Required]
		public System.Int16 Idgrupo { get; set;} 

		[Alias("DESCRIPCION")]
		[Required]
		public System.String Descripcion { get; set;} 

		[Alias("NOMBRE")]
		public System.String Nombre { get; set;} 

 	}
 }
