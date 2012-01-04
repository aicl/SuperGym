using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("DEPARTAMENTOS")]
	public partial class Departamentos{

 		public Departamentos(){}

 		[Alias("IDDEPARTAMENTO")]
		[PrimaryKey]
		[Required]
		public System.Int16 Iddepartamento { get; set;} 

		[Alias("CODIGO_DEPARTAMENTO")]
		[Required]
		public System.String CodigoDepartamento { get; set;} 

		[Alias("NOMBRE_DEPARTAMENTO")]
		[Required]
		public System.String NombreDepartamento { get; set;} 

		[Alias("IDRESPONSABLE")]
		public System.Int32? Idresponsable { get; set;} 

		[Alias("NUMEROVOTOS")]
		[Required]
		public System.Int32 Numerovotos { get; set;} 

 	}
 }
