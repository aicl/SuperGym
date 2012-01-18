using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmSimple;
using SuperGym.Records;
using SuperGym.Records.Auxiliares;
namespace SuperGym.Tablas
{
	public static class Factory
	{
		
		public static DateTime FechaTerminacion(this DateTime date, double dias){
			if(dias<30)
				return date.AddDays(dias-1);
			
			int meses =(int) dias/30;
			int residuo = (int) dias%30;
			
			return date.AddMonths(meses).AddDays(-1).AddDays(residuo);
			
			
		}
		
		public static string SiguienteNumero(this string numero, int longitud ){
		
			longitud= longitud==0?9:longitud;
			if(string.IsNullOrEmpty(numero))
				return "0".PadLeft(longitud,'0');
			
			Int32 n;
			
			if(Int32.TryParse(numero, out n) )
				return (n+1).ToString().PadLeft( longitud ,'0');
			else
				return "0".PadLeft(longitud,'0');
			
		}
		
	
		public static Usuario Usuario(this IDbConnectionFactory dbFactory, string nombre, string clave){
			return dbFactory.Exec(dbCmd => 
				    dbCmd.FirstOrDefault<Usuario>( "NOMBRE={0} and CLAVE={1}", nombre,clave));
		}
		
		public static List<Usuario> Usuarios( this IDbConnectionFactory dbFactory,
		                                     string sqlFilter, params object[] filterParams){
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<Usuario>( sqlFilter, filterParams) );
		}
		
		public static List<Usuario> Usuarios( this IDbConnectionFactory dbFactory){
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<Usuario>( ) );
		}
		
		
		public static void ActualizarUsuario(this IDbConnectionFactory dbFactory, Usuario usuario){
			
			string command= string.Format("Update usuarios set nombre='{0}', correo ='{1}', activo='{2}' " +
				" where idusuario='{3}'",
			                            usuario.Nombre, usuario.Correo, usuario.Activo?1:0, usuario.Id);
			dbFactory.Exec(
				dbCmd => {
				dbCmd.CommandText=command;
				dbCmd.ExecuteNonQuery();
				} ) ;		
		}
		
		public static void CambiarClaveUsuario(this IDbConnectionFactory dbFactory, Usuario usuario){
			
			string command= string.Format("Update usuarios set clave='{0}'  " +
				" where idusuario='{1}'",
			                            usuario.Clave, usuario.Id);
			dbFactory.Exec(
				dbCmd => {
				dbCmd.CommandText=command;
				dbCmd.ExecuteNonQuery();
				} ) ;		
		}
		
		public static Permisos Permisos( this IDbConnectionFactory dbFactory, Int16 idUsuario){
			
			Permisos permisos= new Permisos();
			
			StringBuilder  var1 = new StringBuilder();
			var1.Append("SELECT grupos.idgrupo          AS \"IdGrupo\", \n");
			var1.Append("       grupos.nombre           AS \"NombreGrupo\", \n");
			var1.Append("       grupos.directorio           AS \"Directorio\", \n");
			var1.Append("       grupos.menu           AS \"Menu\", \n");
			var1.Append("       actividades.idactividad AS \"IdActividad\", \n");
			var1.Append("       actividades.nombre      AS \"NombreActividad\" \n");
			var1.Append("FROM   grupos_usuarios \n");
			var1.Append("       JOIN grupos_actividades \n");
			var1.Append("         ON grupos_actividades.idgrupo = grupos_usuarios.idgrupo \n");
			var1.Append("       JOIN grupos \n");
			var1.Append("         ON grupos.idgrupo = grupos_usuarios.idgrupo \n");
			var1.Append("       JOIN actividades \n");
			var1.Append("         ON actividades.idactividad = grupos_actividades.idactividad \n");
			var1.AppendFormat("WHERE  grupos_usuarios.idusuario = '{0}' ", idUsuario);
					
			List<UsuarioGruposActividades> uga=
				dbFactory.Exec(
				    dbCmd => 
						dbCmd.Select<UsuarioGruposActividades>(var1.ToString()) 
				);
			
			
			var actividades=
					(from act in uga
					orderby act.NombreActividad
					select act.IdActividad).Distinct().ToList();
					
			foreach(var a in actividades){
				permisos.Actividades.Add( new Actividad(){
					Id= a,
					Nombre= uga.FirstOrDefault( r => r.IdActividad== a).NombreActividad
				});
			}
			
			
			var grupos=
					(from gr in uga
					orderby gr.NombreGrupo
					select gr.IdGrupo).Distinct().ToList();
			
			foreach (var g in grupos){
				var gr = uga.FirstOrDefault( r=>r.IdGrupo== g);
				permisos.Grupos.Add( new UsuarioGrupo(){
					Id= g,
					Nombre= gr.NombreGrupo,
					Directorio=gr.Directorio,
					Menu = gr.Menu
				});
			}
			
			return permisos;
		}
			
		
		public static Persona Persona(this IDbConnectionFactory dbFactory, string documento){
			return dbFactory.Exec(dbCmd => 
				    dbCmd.FirstOrDefault<Persona>( "DOCUMENTO={0}", documento));
		}
		
		public static Persona Persona(this IDbConnectionFactory dbFactory, Int32 id){
			return dbFactory.Exec(dbCmd => 
				    dbCmd.FirstOrDefault<Persona>( "IDPERSONA={0}", id));
		}
		
		public static List<Persona> Personas(this IDbConnectionFactory dbFactory, 
		                                     string sqlFilter, params object[] filterParams){
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<Persona>( sqlFilter, filterParams )
			                      .OrderBy(r=> r.PrimerApellido).ToList()  );
		}
		
		public static List<Profesion> Profesiones( this IDbConnectionFactory dbFactory){
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<Profesion>( ).OrderBy(r=> r.Nombre).ToList()  );
		}

		
		public static List<TipoDocumento> TiposDocumento( this IDbConnectionFactory dbFactory){
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<TipoDocumento>( ).OrderBy(r=> r.Nombre).ToList()  );
		}
		
		public static List<Clasificacion> Clasificaciones( this IDbConnectionFactory dbFactory){
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<Clasificacion>( ).OrderBy(r=> r.Nombre).ToList()  );
		}
		
		public static List<Municipio> Municipios( this IDbConnectionFactory dbFactory){
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<Municipio>( ).OrderBy(r=> r.Nombre).ToList()  );
		}
		
		public static UltimaFactura UltimaFactura( this IDbConnectionFactory dbFactory, int longitud){
	
			StringBuilder  var1 = new StringBuilder();
			var1.Append("SELECT numero_factura \n");
			var1.Append("FROM   pagos a \n");
			var1.AppendFormat("    where CHAR_LENGTH( a.NUMERO_FACTURA)='{0}'",longitud);
			var1.Append("ORDER  BY a.numero_factura DESC \n");
			var1.Append("rows 1 ");
			
			UltimaFactura uf= dbFactory.Exec(
			    dbCmd => 
					dbCmd.Select<UltimaFactura>(var1.ToString()).FirstOrDefault() 
			);
					
			return !(uf == default(UltimaFactura)) ?
				uf:
				new UltimaFactura(){Numero="".SiguienteNumero(longitud)};	
			
		}
				
		public static List<TipoPago> TiposPago( this IDbConnectionFactory dbFactory){
			
			return dbFactory.Exec(
			    dbCmd => 
					dbCmd.Select<TipoPago>() 
			);
		}
		
		public static TipoPago TipoPago( this IDbConnectionFactory dbFactory, Int16 id){
			
			return dbFactory.Exec(
			    dbCmd => 
					dbCmd.Select<TipoPago>("IDTIPO_PAGO={0}", id).FirstOrDefault() 
			);
		}
		
		public static Pago FacturaVigente(this IDbConnectionFactory dbFactory, Int32 idPersona){
			
			return dbFactory.Exec(
			    dbCmd => 
					dbCmd.Select<Pago>("IDPERSONA={0} and FECHAINICIO<={1} and FECHATERMINACION>={1} and ACTIVA='1' ",
			                                         idPersona, DateTime.Today).
			                      OrderByDescending(r=>r.Numero).FirstOrDefault() 
			);
		}
		
		
		public static Pago Factura( this IDbConnectionFactory dbFactory, Int32 id){
			return dbFactory.Exec(
			    dbCmd => 
					dbCmd.GetById<Pago>(id) 
			);
		}
		
		public static Pago Factura(this IDbConnectionFactory dbFactory, string numero){
			return dbFactory.Exec(
			    dbCmd => 
					dbCmd.Select<Pago>("NUMERO_FACTURA={0}",
			                           numero).FirstOrDefault() 
			);
		}
		
		
		public static List<Pago> PersonaFacturas( this IDbConnectionFactory dbFactory, Int32 idPersona){
			return dbFactory.Exec(
			    dbCmd => 
					dbCmd.Select<Pago>("IDPERSONA={0} ", idPersona).OrderByDescending( r=> r.Numero).ToList()
			);
			
		}
				
		
		public static Ingreso UltimoIngreso(this IDbConnectionFactory dbFactory, Int32 idPersona){
			return dbFactory.Exec(
			    dbCmd => 
					dbCmd.Select<Ingreso>("IDPERSONA={0} ORDER BY FECHA DESCENDING ROWS 1",
			                                         idPersona).FirstOrDefault() 
			);
		}
		
		
		public static List<FacturacionDia> ListaFacturacionDia(this IDbConnectionFactory dbFactory, 
			DateTime fechaInicial, DateTime fechaFinal){
			
			StringBuilder  var1 = new StringBuilder();
			var1.Append("SELECT a.fechapago                        AS \"FechaPago\", \n");
			var1.Append("       tp.nombre                          AS \"Nombre\", \n");
			var1.Append("       CAST (COUNT(a.idpago) AS SMALLINT) AS \"Cantidad\", \n");
			var1.Append("       SUM(a.valor_total)                 AS \"Valor\" \n");
			var1.Append("FROM   pagos a \n");
			var1.Append("       JOIN tipos_pago tp \n");
			var1.Append("         ON tp.idtipo_pago = a.idtipo_pago \n");
			var1.AppendFormat("WHERE  a.fechapago BETWEEN '{0}' AND '{1}'  and a.activa='1' \n", 
				fechaInicial.ToString("dd.MM.yyyy"), 
				fechaFinal.ToString("dd.MM.yyyy ")  );
			var1.Append("GROUP  BY a.fechapago, \n");
			var1.Append("          tp.nombre ");
			
			return dbFactory.Exec(
				dbCmd =>
				dbCmd.Select<FacturacionDia>(var1.ToString())
			);
		}
		
		public static List<PersonaActiva> PersonasActivas(this IDbConnectionFactory dbFactory,
		                                                  DateTime? fechaCorte){
			
			StringBuilder  var1 = new StringBuilder();
			var1.Append("SELECT personas.idpersona                                   AS \"Id\", \n");
			var1.Append("       TRIM(personas.nombres \n");
			var1.Append("             || ' ' \n");
			var1.Append("             || personas.primer_apellido \n");
			var1.Append("             || ' ' \n");
			var1.Append("             || personas.segundo_apellido)                  AS \"NombreCompleto\", \n");
			var1.Append("       personas.documento                                   AS \"Documento\", \n");
			var1.Append("       personas.fecha_nacimiento                            AS \"FechaNacimiento\" \n");
			var1.Append("       , \n");
			var1.Append("       personas.sexo                                        AS \"Sexo\", \n");
			var1.Append("       personas.nombre_barrio                               AS \"Barrio\", \n");
			var1.Append("       personas.direccion_residencia                        AS \"Direccion\", \n");
			var1.Append("       personas.telefono                                    AS \"Telefono\", \n");
			var1.Append("       personas.celular                                     AS \"Celular\", \n");
			var1.Append("       personas.email                                       AS \"Email\", \n");
			var1.Append("       personas.empresa                                     AS \"Empresa\", \n");
			var1.Append("       pagos.numero_factura                                 AS \"UltimaFactura\", \n");
			var1.Append("       pagos.fechainicio                                    AS \"Inicio\", \n");
			var1.Append("       pagos.fechaterminacion                               AS \"Terminacion\", \n");
			var1.Append("       pagos.valor_total                                    AS \"Valor\", \n");
			var1.Append("       pagos.observacion                                    AS \"Observacion\", \n");
			var1.Append("       Coalesce(ingresos.ultimo_ingreso, pagos.fechainicio) AS \"UltimoIngreso\", \n");
			var1.Append("       cast(conteo.ingresos as integer)                     AS \"Entradas\", \n");
			var1.Append("       cast( ( current_date - Coalesce(CAST(ingresos.ultimo_ingreso AS DATE), \n");
			var1.Append("                        CAST(pagos.fechainicio AS DATE)) ) as integer)  AS \"DiasAusencia\", \n");
			var1.Append("       tp.nombre                                            AS \"TipoPago\" \n");
			var1.Append("FROM   (SELECT MAX(pagos.numero_factura) AS numero_factura, \n");
			var1.Append("               personas.idpersona        AS idpersona \n");
			var1.Append("        FROM   personas \n");
			var1.Append("               JOIN pagos \n");
			var1.Append("                 ON pagos.idpersona = personas.idpersona \n");
			var1.AppendFormat("        WHERE  pagos.fechaterminacion >= {0} and pagos.activa='1' ", 
			                  fechaCorte.HasValue
			                  ? string.Format("'{0}'",fechaCorte.Value.ToString("dd.MM.yyyy"))
			                  :"( current_date - 1 )") ;
			var1.Append("        GROUP  BY personas.idpersona) activos \n");
			var1.Append("       JOIN personas \n");
			var1.Append("         ON personas.idpersona = activos.idpersona \n");
			var1.Append("       JOIN pagos \n");
			var1.Append("         ON pagos.numero_factura = activos.numero_factura \n");
			var1.Append("       JOIN tipos_pago tp \n");
			var1.Append("         ON tp.idtipo_pago = pagos.idtipo_pago \n");
			var1.Append("       LEFT JOIN (SELECT MAX(fi.fecha) AS ultimo_ingreso, \n");
			var1.Append("                         fi.idpersona  AS idpersona \n");
			var1.Append("                  FROM   fechasingreso fi \n");
			var1.Append("                  GROUP  BY fi.idpersona) ingresos \n");
			var1.Append("         ON ingresos.idpersona = activos.idpersona \n");
			var1.Append("       LEFT JOIN (SELECT coalesce(COUNT(fi.idfechaingreso),0) AS ingresos, \n");
			var1.Append("                         fi.idpersona             AS idpersona \n");
			var1.Append("                  FROM   fechasingreso fi \n");
			var1.Append("                  GROUP  BY fi.idpersona) conteo \n");
			var1.Append("         ON conteo.idpersona = activos.idpersona ");
			
			return dbFactory.Exec(
				dbCmd =>
				dbCmd.Select<PersonaActiva>(var1.ToString())
			);
		}
		
		public static List<PersonaInactiva> PersonasInactivas(this IDbConnectionFactory dbFactory,
		                                                  Int32 diasTopeInferior,
		                                                  Int32 diasTopeSuperior){
			
			StringBuilder  var1 = new StringBuilder();
			var1.Append("SELECT personas.idpersona                                   AS \"Id\", \n");
			var1.Append("       TRIM(personas.nombres \n");
			var1.Append("             || ' ' \n");
			var1.Append("             || personas.primer_apellido \n");
			var1.Append("             || ' ' \n");
			var1.Append("             || personas.segundo_apellido)                  AS \"NombreCompleto\", \n");
			var1.Append("       personas.documento                                   AS \"Documento\", \n");
			var1.Append("       personas.fecha_nacimiento                            AS \"FechaNacimiento\" \n");
			var1.Append("       , \n");
			var1.Append("       personas.sexo                                        AS \"Sexo\", \n");
			var1.Append("       personas.nombre_barrio                               AS \"Barrio\", \n");
			var1.Append("       personas.direccion_residencia                        AS \"Direccion\", \n");
			var1.Append("       personas.telefono                                    AS \"Telefono\", \n");
			var1.Append("       personas.celular                                     AS \"Celular\", \n");
			var1.Append("       personas.email                                       AS \"Email\", \n");
			var1.Append("       personas.empresa                                     AS \"Empresa\", \n");
			var1.Append("       pagos.numero_factura                                 AS \"UltimaFactura\", \n");
			var1.Append("       pagos.fechainicio                                    AS \"Inicio\", \n");
			var1.Append("       pagos.fechaterminacion                               AS \"Terminacion\", \n");
			var1.Append("       pagos.valor_total                                    AS \"Valor\", \n");
			var1.Append("       pagos.observacion                                    AS \"Observacion\", \n");
			var1.Append("       Coalesce(ingresos.ultimo_ingreso, pagos.fechainicio) AS \"UltimoIngreso\", \n");
			var1.Append("       conteo.ingresos                                      AS \"Entradas\", \n");
			var1.Append("       ( current_date - Coalesce(CAST(ingresos.ultimo_ingreso AS DATE), \n");
			var1.Append("                        CAST(pagos.fechainicio AS DATE)) )  AS \"DiasAusencia\", \n");
			var1.Append("       tp.nombre                                            AS \"TipoPago\" \n");
			var1.Append("FROM   (SELECT MAX(pagos.numero_factura) AS numero_factura, \n");
			var1.Append("               personas.idpersona        AS idpersona \n");
			var1.Append("        FROM   personas \n");
			var1.Append("               JOIN pagos \n");
			var1.Append("                 ON pagos.idpersona = personas.idpersona \n");
			var1.Append("        where pagos.ACTIVA='1' \n");
			var1.Append("        GROUP  BY personas.idpersona) activos \n");
			var1.Append("       JOIN personas \n");
			var1.Append("         ON personas.idpersona = activos.idpersona \n");
			var1.Append("       JOIN pagos \n");
			var1.Append("         ON pagos.numero_factura = activos.numero_factura \n");
			var1.Append("       JOIN tipos_pago tp \n");
			var1.Append("         ON tp.idtipo_pago = pagos.idtipo_pago \n");
			var1.Append("       LEFT JOIN (SELECT MAX(fi.fecha) AS ultimo_ingreso, \n");
			var1.Append("                         fi.idpersona  AS idpersona \n");
			var1.Append("                  FROM   fechasingreso fi \n");
			var1.Append("                  GROUP  BY fi.idpersona) ingresos \n");
			var1.Append("         ON ingresos.idpersona = activos.idpersona \n");
			var1.Append("       LEFT JOIN (SELECT coalesce( COUNT(fi.idfechaingreso),0 ) AS ingresos, \n");
			var1.Append("                         fi.idpersona             AS idpersona \n");
			var1.Append("                  FROM   fechasingreso fi \n");
			var1.Append("                  GROUP  BY fi.idpersona) conteo \n");
			var1.Append("         ON conteo.idpersona = activos.idpersona \n");
			var1.Append("WHERE  ( current_date - Coalesce(CAST(ingresos.ultimo_ingreso AS DATE),"); 
            var1.AppendFormat("            CAST(pagos.fechainicio AS DATE)) ) BETWEEN {0} AND {1}",  
                        diasTopeInferior, diasTopeSuperior);
			
			return dbFactory.Exec(
				dbCmd =>
				dbCmd.Select<PersonaInactiva>(var1.ToString())
			);
		 

		}
		
		
		public static List<Caja> Cajas(this IDbConnectionFactory dbFactory, string anio, string mes){
			
			string periodo= string.Format("{0}{1}", anio, mes.PadLeft(2,'0'));
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<Caja>( "PERIODO={0}", periodo));
		}
		
		public static Caja Caja(this IDbConnectionFactory dbFactory, Int32 id){
				
			return dbFactory.Exec(dbCmd => 
				    dbCmd.GetById<Caja>( id) );
		}
		
		
		public static Caja Caja(this IDbConnectionFactory dbFactory, DateTime fecha ) {
				
			return dbFactory.Exec(dbCmd => 
				    dbCmd.FirstOrDefault<Caja>( "FECHA= {0} and  ASENTADO_POR IS NOT NULL" , fecha));
		}
		
		
		public static List<Decaja> DeCajas(this IDbConnectionFactory dbFactory, Int32 idCaja){

			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<Decaja>( "IDCAJA={0}", idCaja));
		}
		
		
		public static List<DeCajaClasificacion> DeCajasClasificacion(this IDbConnectionFactory dbFactory,
		                                                             Int32 idCaja){
			StringBuilder  var1 = new StringBuilder();
			var1.Append("SELECT coalesce(conceptos.clasificacion,'Indefinido') as \"Clasificacion\",  \n");
			var1.Append("       decaja.* \n");
			var1.Append("FROM   decaja \n");
			var1.Append("       left JOIN conceptos \n");
			var1.Append("         ON conceptos.descripcion = decaja.concepto \n");
			var1.AppendFormat("WHERE  decaja.idcaja =  '{0}' \n", idCaja );
			
			return dbFactory.Exec(
				dbCmd =>
				dbCmd.Select<DeCajaClasificacion>(var1.ToString())
			);
			
		}
		
		
		public static List<DeCajaClasificacion> DeCajasClasificacion(this IDbConnectionFactory dbFactory, DateTime desde,
		                                   DateTime hasta, bool soloAsentadas){
				
			StringBuilder  var1 = new StringBuilder();
			var1.Append("SELECT coalesce(conceptos.clasificacion,'Indefinido') as \"Clasificacion\",  \n");
			var1.Append("       decaja.* \n");
			var1.Append("FROM   caja \n");
			var1.Append("       JOIN decaja \n");
			var1.Append("         ON decaja.idcaja = caja.id \n");
			var1.Append("       left JOIN conceptos \n");
			var1.Append("         ON conceptos.descripcion = decaja.concepto \n");
			var1.AppendFormat("WHERE  caja.fecha BETWEEN '{0}' AND '{1}' \n",
			                  desde.ToString("dd.MM.yyyy"),
			                  hasta.ToString("dd.MM.yyyy"));
			if (soloAsentadas)
				var1.Append("       AND caja.asentado_por IS NOT NULL ");
			
			
			return dbFactory.Exec(
				dbCmd =>
				dbCmd.Select<DeCajaClasificacion>(var1.ToString())
			);

					
		}
		
		public static List<Decaja> DeCajas(this IDbConnectionFactory dbFactory, DateTime desde,
		                                   DateTime hasta, bool soloAsentadas){
				
			StringBuilder  var1 = new StringBuilder();
			var1.Append("SELECT decaja.* \n");
			var1.Append("FROM   caja \n");
			var1.Append("       JOIN decaja \n");
			var1.Append("         ON decaja.idcaja = caja.id \n");
			var1.AppendFormat("WHERE  caja.fecha BETWEEN '{0}' AND '{1}' \n",
			                  desde.ToString("dd.MM.yyyy"),
			                  hasta.ToString("dd.MM.yyyy"));
			if (soloAsentadas)
				var1.Append("       AND caja.asentado_por IS NOT NULL ");
			
			
			return dbFactory.Exec(
				dbCmd =>
				dbCmd.Select<Decaja>(var1.ToString())
			);

					
		}
		
		
		public static List<UsuarioCorreo> UsuariosCorreos (this IDbConnectionFactory dbFactory, string actividad){
			return UsuariosCorreos(dbFactory, actividad, true);
		}
		
		public static List<UsuarioCorreo> UsuariosCorreos (this IDbConnectionFactory dbFactory, string actividad, 
		                                                   bool  soloActivos){
			
			StringBuilder  var1 = new StringBuilder();
			var1.Append("SELECT usuarios.nombre AS \"Nombre\", \n");
			var1.Append("       usuarios.correo AS \"Correo\" \n");
			var1.Append("FROM   actividades \n");
			var1.Append("       JOIN correos \n");
			var1.Append("         ON correos.idactividad = actividades.idactividad \n");
			var1.Append("       JOIN usuarios \n");
			var1.Append("         ON usuarios.idusuario = correos.idusuario \n");
			var1.AppendFormat("WHERE  actividades.nombre = '{0}' \n", actividad);
			if( soloActivos) 
				var1.Append("          and usuarios.activo = '1' ");
			
			return dbFactory.Exec(
				dbCmd =>
				dbCmd.Select<UsuarioCorreo>(var1.ToString())
			);
		
		}
		
		
		public static List<Suspension> Suspensiones(this IDbConnectionFactory dbFactory, Int32 idPago){
			return dbFactory.Exec(
			    dbCmd => 
					dbCmd.Select<Suspension>("IDPAGO={0}",
			                                         idPago)
			);
		}
		
		public static Suspension Suspension(this IDbConnectionFactory dbFactory, Int32 id){
			return dbFactory.Exec(
			    dbCmd => 
					dbCmd.GetById<Suspension>(id) 
			);
		}
		
		
		public static List<ConceptoValor> CajaConsolidado(this IDbConnectionFactory dbFactory, 
		                                                  DateTime desde, DateTime hasta){
			
			StringBuilder  var1 = new StringBuilder();
			var1.Append("SELECT 0                  AS \"Factor\", \n");
			var1.Append("       'Saldo Anterior'   AS \"Concepto\", \n");
			var1.Append("       caja.saldoanterior AS \"Valor\",  \n");
			var1.Append("       'Saldo Anterior' AS \"Clasificacion\"  \n");
			var1.Append("FROM   (SELECT MIN(caja.fecha) inicio \n");
			var1.Append("        FROM   caja \n");
			var1.AppendFormat("        WHERE  caja.fecha BETWEEN '{0}' AND '{1}' \n", desde.ToString("dd.MM.yyyy"),
			                  hasta.ToString("dd.MM.yyyy"));
			var1.Append("               AND caja.asentado_por IS NOT NULL) fecha \n");
			var1.Append("       JOIN caja \n");
			var1.Append("         ON caja.fecha = fecha.inicio \n");
			var1.Append("UNION ALL \n");
			var1.Append("SELECT movi.* \n");
			var1.Append("FROM   (SELECT decaja.factor                   AS \"Factor\", \n");
			var1.Append("               decaja.concepto                   AS \"Concepto\", \n");
			var1.Append("               SUM(decaja.valor * decaja.factor)                         AS \n");
			var1.Append("               \"Valor\", \n");
			var1.Append("               coalesce(CONCEPTOS.CLASIFICACION,'Indefinido') as \"Clasificacion\" \n");
			var1.Append("        FROM   caja \n");
			var1.Append("               JOIN decaja \n");
			var1.Append("                 ON decaja.idcaja = caja.id \n");
			var1.Append("       left join conceptos  on CONCEPTOS.DESCRIPCION= DECAJA.CONCEPTO \n");
			var1.AppendFormat("        WHERE  caja.fecha BETWEEN '{0}' AND '{1}' \n", desde.ToString("dd.MM.yyyy"),
			                  hasta.ToString("dd.MM.yyyy"));
			var1.Append("               AND caja.asentado_por IS NOT NULL \n");
			var1.Append("        GROUP  BY decaja.factor, \n");
			var1.Append("                  \"Clasificacion\", \n" );
			var1.Append("                  decaja.concepto \n");
			var1.Append("        ORDER  BY \"Factor\" DESC) movi ");
			
			return dbFactory.Exec(
				dbCmd =>
				dbCmd.Select<ConceptoValor>(var1.ToString())
			);
			
		}
	
		public static List<FacturaDiaDetalle> FacturasDiaDetalle(this IDbConnectionFactory dbFactory, DateTime fecha){
			
			StringBuilder  var1 = new StringBuilder();
			var1.Append("SELECT p.documento                                    AS \"Documento\", \n");
			var1.Append("       p.nombres \n");
			var1.Append("        || ' ' \n");
			var1.Append("        || p.primer_apellido \n");
			var1.Append("        || (' ' \n");
			var1.Append("                 || Coalesce(p.segundo_apellido, '')) AS \"Nombres\", \n");
			var1.Append("       p.telefono                                     AS \"Telefono\", \n");
			var1.Append("       p.celular                                      AS \"Celular\", \n");
			var1.Append("       a.numero_factura                               AS \"Numero\", \n");
			var1.Append("       a.fechainicio                                  AS \"FechaInicio\", \n");
			var1.Append("       a.fechaterminacion                             AS \"FechaTerminacion\", \n");
			var1.Append("       a.valor_unitario                               AS \"ValorUnitario\", \n");
			var1.Append("       a.cantidad                                     AS \"Cantidad\", \n");
			var1.Append("       a.valor_total                                  AS \"ValorTotal\", \n");
			var1.Append("       a.observacion                                  AS \"Observacion\", \n");
			var1.Append("       a.activa                                       AS \"Activa\" \n");
			var1.Append("FROM   pagos a \n");
			var1.Append("       JOIN personas p \n");
			var1.Append("         ON p.idpersona = a.idpersona \n");
			var1.AppendFormat("WHERE  a.fechapago = '{0}' ", fecha.ToString("dd.MM.yyyy"));	
			
			return dbFactory.Exec(
				dbCmd =>
				dbCmd.Select<FacturaDiaDetalle>(var1.ToString())
			);
			
		}
		
		
		public static List<Concepto> Conceptos( this IDbConnectionFactory dbFactory){
			return dbFactory.Exec(
				dbCmd =>
				dbCmd.Select<Concepto>()
			);
		}
		
		
		public static List<Cartera>Cartera(this IDbConnectionFactory dbFactory, bool conSaldo){
			
			
			StringBuilder  var1 = new StringBuilder();
			var1.Append("SELECT p.documento                                  AS \"Documento\", \n");
			var1.Append("       p.nombres \n");
			var1.Append("        || ' ' \n");
			var1.Append("        || p.primer_apellido \n");
			var1.Append("        || ( ' ' \n");
			var1.Append("              || Coalesce(p.segundo_apellido, '') ) AS \"Nombre\", \n");
			var1.Append("       s.saldo                                      AS \"Saldo\", \n");
			var1.Append("       s.fecha                                      AS \"Fecha\", \n");
			var1.Append("       p.telefono                                   AS \"Telefono\", \n");
			var1.Append("       p.celular                                    AS \"Celular\" \n");
			var1.Append("FROM   saldoporcobrar s \n");
			var1.Append("       JOIN personas p \n");
			var1.Append("         ON p.idpersona = s.idpersona \n");
			if (conSaldo) var1.Append("WHERE  s.saldo > 0 ");
			
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<Cartera>( var1.ToString() ) );
			
		}
				
	}
}
