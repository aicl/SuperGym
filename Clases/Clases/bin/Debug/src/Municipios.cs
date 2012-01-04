using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("MUNICIPIOS")]
	public partial class Municipios{

 		public Municipios(){}

 		[Alias("IDMUNICIPIO")]
		[PrimaryKey]
		[Required]
		public System.Int16 Idmunicipio { get; set;} 

		[Alias("CENSO_CIUDAD")]
		[Required]
		public System.String CensoCiudad { get; set;} 

		[Alias("IDDEPARTAMENTO")]
		[Required]
		public System.Int16 Iddepartamento { get; set;} 

		[Alias("CODIGO_MUNICIPIO")]
		[Required]
		public System.String CodigoMunicipio { get; set;} 

		[Alias("NOMBRE_MUNICIPIO")]
		[Required]
		public System.String NombreMunicipio { get; set;} 

		[Alias("IDRESPONSABLE")]
		public System.Int32? Idresponsable { get; set;} 

		[Alias("NUMEROVOTOS")]
		[Required]
		public System.Int32 Numerovotos { get; set;} 

 	}
 }
