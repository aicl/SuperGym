using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("PERSONAS")]
	public partial class Personas{

 		public Personas(){}

 		[Alias("IDPERSONA")]
		[PrimaryKey]
		[Required]
		public System.Int32 Idpersona { get; set;} 

		[Alias("NOMBRES")]
		[Required]
		public System.String Nombres { get; set;} 

		[Alias("PRIMER_APELLIDO")]
		[Required]
		public System.String PrimerApellido { get; set;} 

		[Alias("SEGUNDO_APELLIDO")]
		public System.String SegundoApellido { get; set;} 

		[Alias("IDTIPO_DOCUMENTO")]
		[Required]
		public System.Int16 IdtipoDocumento { get; set;} 

		[Alias("DOCUMENTO")]
		[Required]
		public System.String Documento { get; set;} 

		[Alias("FECHA_NACIMIENTO")]
		[Required]
		public System.DateTime FechaNacimiento { get; set;} 

		[Alias("SEXO")]
		[Required]
		public System.String Sexo { get; set;} 

		[Alias("NOMBRE_BARRIO")]
		[Required]
		public System.String NombreBarrio { get; set;} 

		[Alias("DIRECCION_RESIDENCIA")]
		[Required]
		public System.String DireccionResidencia { get; set;} 

		[Alias("TELEFONO")]
		[Required]
		public System.String Telefono { get; set;} 

		[Alias("CELULAR")]
		public System.String Celular { get; set;} 

		[Alias("EMAIL")]
		public System.String Email { get; set;} 

		[Alias("IDPROFESION")]
		[Required]
		public System.Int16 Idprofesion { get; set;} 

		[Alias("EMPRESA")]
		public System.String Empresa { get; set;} 

		[Alias("FECHA_REGISTRO")]
		[Required]
		public System.DateTime FechaRegistro { get; set;} 

		[Alias("IDUSUARIO_REGISTRA")]
		[Required]
		public System.Int16 IdusuarioRegistra { get; set;} 

		[Alias("IDCLASIFICACION")]
		[Required]
		public System.Int16 Idclasificacion { get; set;} 

		[Alias("RUTATEMPLATE")]
		public System.String Rutatemplate { get; set;} 

		[Alias("IDMUNICIPIO")]
		[Required]
		public System.Int16 Idmunicipio { get; set;} 

 	}
 }
