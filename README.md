Requisitos de Software:
mono http://www.go-mono.com/mono-downloads/download.html
redis http://redis.io
servicestack http://www.servicestack.net/
firebirdsql http://www.firebirdsql.org/
reportman http://reportman.sourceforge.net/
extjs http://www.sencha.com/

==========================

Todas la librerias necesarias para compilar y ejecutar el software se encuentran en lib/

En etc/ se consiguen los archivos de configuracion necesarios para que 
la aplicacion funcione con apache, mono, redis, etc.
modifique los archivos de etc/ de acuerdo con su configuracion:
ubicacion de la carpetas, archivos etc ( no cambie los nombres de los "alias"  en los conf del apache  y mod_mono)

en scripts/ se consiguen las instrucciones para compilar e instalar  mono y redis

en fbdata/ se encuentra una bd de ejemplo

modifique de acuerdo con sus necesidades (datos ) los archivos
SuperGym.Servicio/SuperGym.Servicio.Autenticacion/web.config
y
SuperGym.Servicio/SuperGym.Servicio.Personas/web.config

en SuperGym.WebApp/ debe instalar:
la carpeta extjs con la ultima version de la libreria ext ( 4.0.7-gpl) 
y 
la carpeta ux (que se encuentra en extjs/examples ) o hacer un enlace

en SuperGym.WebApp/ debe crear las carpetas fotos, facturas y log
dentro de log crear el archivo servicio-personas.log 
( o el nombre que le dio en SuperGym.Servicio/SuperGym.Servicio.Personas/log4net.conf al archivo de logs)

Despues de compilar y configurar la aplicacion puede entrar desde el navegador :
http://localhost/bp
usando usuario y usuario  o admin  y admin ( usuario y clave)


Para ver los servicios expuestos entre por:
http://localhost/servicio/metadata


==========================
Open Source License
------------------------------------------------------------------------------------------
This software is licensed under the terms of the Open Source GPL 3.0 license. 

http://www.gnu.org/licenses/gpl.html

This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
without even the implied warranty of MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, AND NON-INFRINGEMENT OF THIRD-PARTY INTELLECTUAL PROPERTY RIGHTS.
See the GNU General Public License for more details.

