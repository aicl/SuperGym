using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("ACTIVIDADES")]
	public partial class Actividades{

 		public Actividades(){}

 		[Alias("IDACTIVIDAD")]
		[PrimaryKey]
		[Required]
		public System.Int16 Idactividad { get; set;} 

		[Alias("NOMBRE")]
		public System.String Nombre { get; set;} 

 	}
 }
