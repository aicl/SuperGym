
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
	public static class Extensiones
	{
		public static List<T> Get<T>(this T record, IDbConnectionFactory dbFactory)where T : new()
		{
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<T>( ) );
		}
		
		public static void Post<T> (this T record, IDbConnectionFactory dbFactory )  where T : new()
		{
			dbFactory.Exec(dbCmd => 
			    dbCmd.Insert<T>( record) );
		}
		
		public static void Put<T> (this T record, IDbConnectionFactory dbFactory )  where T : new()
		{
			dbFactory.Exec(dbCmd => 
			    dbCmd.Update<T>( record) );
		}
		
		
		public static void Delete<T> (this T record, IDbConnectionFactory dbFactory )  
			where T :  new() 
		{
			dbFactory.Exec(dbCmd => 
			    dbCmd.Delete<T>( record) );
		}
		
		public static List<GrupoUsuario> GetByIdUsuario(this GrupoUsuario record, IDbConnectionFactory dbFactory)
		{
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<GrupoUsuario>( "IDUSUARIO={0}", record.IdUsuario) );
		}
		
		public static List<GrupoUsuario> GetByIdGrupo(this GrupoUsuario record, IDbConnectionFactory dbFactory)
		{
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<GrupoUsuario>( "IDGRUPO={0}", record.IdGrupo) );
		}
		
		
		//
		
		
		public static List<Correo> GetByIdUsuario(this Correo record, IDbConnectionFactory dbFactory)
		{
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<Correo>( "IDUSUARIO={0}", record.IdUsuario) );
		}
		
		public static List<Correo> GetByIdActividad(this Correo record, IDbConnectionFactory dbFactory)
		{
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<Correo>( "IDACTIVIDAD={0}", record.IdActividad) );
		}
		
		public static List<Actividad> GetByName(this Actividad record, IDbConnectionFactory dbFactory)
		{
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<Actividad>( "NOMBRE LIKE {0}", record.Nombre+"%") );
		}
		
		//
		
		
		public static List<GrupoActividad> GetByIdActividad(this GrupoActividad record, IDbConnectionFactory dbFactory)
		{
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<GrupoActividad>( "IDACTIVIDAD={0}", record.IdActividad) );
		}
		
		public static List<GrupoActividad> GetByIdGrupo(this GrupoActividad record, IDbConnectionFactory dbFactory)
		{
			return dbFactory.Exec(dbCmd => 
				    dbCmd.Select<GrupoActividad>( "IDGRUPO={0}", record.IdGrupo) );
		}
		
		
	}
}

