using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("GRUPOS_ACTIVIDADES")]
	public partial class GruposActividades{

 		public GruposActividades(){}

 		[Alias("IDGRUPO")]
		[PrimaryKey]
		[Required]
		public System.Int16 Idgrupo { get; set;} 

		[Alias("IDACTIVIDAD")]
		[PrimaryKey]
		[Required]
		public System.Int16 Idactividad { get; set;} 

 	}
 }
