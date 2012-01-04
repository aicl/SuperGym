using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("MUNICIPIOS")]
	public partial class Municipio{

 		public Municipio(){}

 		[Alias("IDMUNICIPIO")]
		[PrimaryKey]
		[Required]
		public System.Int16 Id { get; set;} 

		[Alias("CENSO_CIUDAD")]
		[Required]
		public System.String CensoCiudad { get; set;} 

		[Alias("IDDEPARTAMENTO")]
		[Required]
		public System.Int16 IdDepartamento { get; set;} 

		[Alias("CODIGO_MUNICIPIO")]
		[Required]
		public System.String Codigo { get; set;} 

		[Alias("NOMBRE_MUNICIPIO")]
		[Required]
		public System.String Nombre { get; set;} 

		/*
		[Alias("IDRESPONSABLE")]
		public System.Int32? Idresponsable { get; set;} 

		[Alias("NUMEROVOTOS")]
		[Required]
		public System.Int32 Numerovotos { get; set;} 
		 */

 	}
 }
