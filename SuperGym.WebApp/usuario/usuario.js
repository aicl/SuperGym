Ext.require([
    'Ext.form.*', 
    'Ext.layout.container.Column',
    'Ext.tab.Panel',
	'Ext.window.MessageBox',
	'Ext.tip.*'
]);

Ext.onReady(function(){
	
    var url= location.protocol + "//" + location.host+'/servicio';
    var modo='Correo'; // Correo o GrupoActividad
    
    Ext.QuickTips.init();
    
    // -------------------------------- modelos --------------------
    Ext.define('Usuario',{
    	extend: 'Ext.data.Model',
    	idProperty: 'Id',
    	fields:[
    		{name: 'Id', type:'int'},
    		{name: 'Nombre', type:'string'},
    		{name: 'Clave', type:'string'},
    		{name: 'Correo' ,  type:'string'},
    		{name: 'Activo' ,  type:'bool'}
    	]
    });
    
    Ext.define('Actividad',{
    	extend: 'Ext.data.Model',
    	idProperty: 'Id',
    	fields:[
    		{name: 'Id', type:'int'},
    		{name: 'Nombre', type:'string'}
    	]
    });
    
    Ext.define('Grupo',{
    	extend: 'Ext.data.Model',
    	idProperty: 'Id',
    	fields:[
    		{name: 'Id', type:'int'},
    		{name: 'Descripcion', type:'string'},
    		{name: 'Nombre', type:'string'},
    		{name: 'Directorio', type:'string'},
    		{name: 'Menu', type:'bool'},
    	]
    });
    
    Ext.define('Correo',{
    	extend: 'Ext.data.Model',
    	idProperty: 'Id',
    	fields:[
    		{name: 'Id', type:'int'},
    		{name: 'IdUsuario', type:'int'},
    		{name: 'IdActividad', type:'int'}
    	]
    });
    
    
    Ext.define('GrupoUsuario',{
    	extend: 'Ext.data.Model',
    	idProperty: 'Id',
    	fields:[
    		{name: 'IdGrupo', type:'int'},
    		{name: 'IdUsuario', type:'int'}
    	]
    });
    
    
    Ext.define('GrupoActividad',{
    	extend: 'Ext.data.Model',
    	fields:[
    		{name: 'IdGrupo', type:'int'},
    		{name: 'IdActividad', type:'int'}
    	]
    });
    
    // --------------------- stores --------------------------------------------------
	var storeUsuarios= Ext.create('Ext.data.Store', {
		model:'Usuario',
		proxy:  createAjaxProxy( {
	        url : url+'/json/syncreply/Usuario',
        	root: 'Usuarios',
        	method:'get'
    	}),
		autoLoad:true
	});
	    	
	var storeActividades= Ext.create('Ext.data.Store', {
		model:'Actividad',
		proxy:  createAjaxProxy( {
	        url : url+'/json/syncreply/Actividad',
        	root: 'Actividades',
        	method:'get'
    	}),
		autoLoad:true
	});
	
	
	var storeGrupos= Ext.create('Ext.data.Store', {
		model:'Grupo',
		proxy:  createAjaxProxy( {
	        url : url+'/json/syncreply/Grupo',
        	root: 'Grupos',
        	method:'get'
    	}),
		autoLoad:true
	});
	
	var storeCorreos= Ext.create('Ext.data.Store', {
		model:'Correo',
    	autoLoad:false
	});
	
	var storeGruposUsuarios= Ext.create('Ext.data.Store', {
		model:'GrupoUsuario',
    	autoLoad:false
	});
	
	var storeGruposActividades= Ext.create('Ext.data.Store', {
		model:'GrupoActividad',
    	autoLoad:false
	});
	
	// ------------------ grids -----------------------------------------------
	
	var gridUsuarios = Ext.create('Ext.grid.Panel', {
		frame: true,
		selType: 'rowmodel',
        store: storeUsuarios,
        height: 500,
        width: 400,
		bodyStyle:'padding:5px 5px 0',
        viewConfig: {
            stripeRows: true
        },
        columns: [{
            text     : 'Nombre',
            flex     : 1,
            sortable : true,
            dataIndex: 'Nombre'
        },{
            text     : 'Correo',
            width    : 120,
            dataIndex: 'Correo'
        },{
            text     : 'Activo',
            width    : 40,
            sortable : true,
            dataIndex: 'Activo',
            renderer: function(value, metadata, record, store){
            	if(value)
            		return 'Si';
            	else 
            		return 'No';
           	}
        }],
        dockedItems: [{
            xtype: 'toolbar',
            items: [{
            	id: 'buttonInsertarUsuario',
                text:'Nuevo',
                tooltip:'Crear Usuario',
                iconCls:'add',
                handler:function(){
                	showWindowUsuario('post');  
                }
            },'-',{
            	id: 'buttonActualizarUsuario',
                text:'Actualizar',
                tooltip:'Actualizar Usuario',
                disabled:true,
                iconCls:'edit',
                handler:function(){
                	showWindowUsuario('put'); 
                }
            },'-',{
            	id: 'buttonCambiarClave',
                text:'Clave',
                tooltip:'Cambiar Clave',
                disabled:true,
                iconCls:'clave',
                handler:function(){
                	showWindowUsuario('put', 'CambiarClave'); //
                }
            },'-',{
            	id: 'buttonBorrarUsuario',
                text:'Borrar',
                tooltip:'Borrar Usuario',
                iconCls:'remove',
                disabled:true,
                handler:function(){
                	var record = {
	    				SessionId:sessionStorage.id,
	    				Id:gridUsuarios.getSelectionModel().getSelection()[0].getId() 
	    			};
	    			executeAjaxRequest({
						url: url+'/json/syncreply/Usuario',
						method:'delete',
						success: function(result) {   
							storeUsuarios.remove(gridUsuarios.getSelectionModel().getSelection()[0]);		
						},
						params:record
					});	 
                }
            }]		
        }]
    });

 	gridUsuarios.getSelectionModel().on('selectionchange', function(sm, selectedRecord) {
        if (selectedRecord.length) {	
        	Ext.getCmp('buttonActualizarUsuario').setDisabled(false);
        	Ext.getCmp('buttonBorrarUsuario').setDisabled(false);
        	Ext.getCmp('buttonCambiarClave').setDisabled(false);
        	Ext.getCmp('buttonInsertarCorreo').setDisabled(false);
        	Ext.getCmp('buttonInsertarGrupoUsuario').setDisabled(false);
        	var record ={
             	IdUsuario: selectedRecord[0].getId(),
             	SessionId:sessionStorage.id
            };
            executeAjaxRequest({
        		url: url+'/json/syncreply/Correo',
        		method:'get',
        		success: function(result) {			
					storeCorreos.setProxy(new Ext.data.proxy.Memory({
						model:'Correo',
						data: result.Correos
					}) );
				storeCorreos.load();
				gridCorreos.determineScrollbars();
		    	},
		    	params:record
            });
                         
            executeAjaxRequest({
        		url: url+'/json/syncreply/GrupoUsuario',
        		method:'get',
        		success: function(result) {			
					storeGruposUsuarios.setProxy(new Ext.data.proxy.Memory({
						model:'GrupoUsuario',
						data: result.GruposUsuarios
					}) );
				storeGruposUsuarios.load();
				gridGruposUsuarios.determineScrollbars();
		    	},
		    	params:record
            });
            
        }
        else{
        	Ext.getCmp('buttonActualizarUsuario').setDisabled(true);
        	Ext.getCmp('buttonBorrarUsuario').setDisabled(true);
        	Ext.getCmp('buttonCambiarClave').setDisabled(true);
        	Ext.getCmp('buttonInsertarCorreo').setDisabled(true);
        	Ext.getCmp('buttonInsertarGrupoUsuario').setDisabled(true);
        }
	});
	
	
	//
	
	var gridCorreos = Ext.create('Ext.grid.Panel', {
		frame: true,
		title:'Enviar correo al ejecutar:',
		selType: 'rowmodel',
        store: storeCorreos,
        height: 250,
        width:  300,
		bodyStyle:'padding:5px 5px 0',
        viewConfig: {
            stripeRows: true
        },
        columns: [{
            text     : 'Actividad',
            flex     : 1,
            sortable : true,
            dataIndex: 'IdActividad',
            renderer : function( value, metaData, record, rowIndex, colIndex, store,  view ){
				var r = storeActividades.getById(parseInt( value) );
				if (r)
					return r.get('Nombre');
				else
					return 'Undefinido';
			},
        }],
        dockedItems: [{
            xtype: 'toolbar',
            items: [{
            	id: 'buttonInsertarCorreo',
                text:'Nuevo',
                tooltip:'Nuevo actividad para enviar correo al usuario',
                iconCls:'add',
                disabled:true,
                handler:function(){
                  	modo='Correo';
                	windowActividades.show();
                }
            },'-',{
            	id: 'buttonBorrarCorreo',
                text:'Borrar',
                tooltip:'Borrar Usuario',
                iconCls:'remove',
                disabled:true,
                handler:function(){
                	var record = {
	    				SessionId:sessionStorage.id,
	    				Id:gridCorreos.getSelectionModel().getSelection()[0].getId() 
	    			};
	    			executeAjaxRequest({
						url: url+'/json/syncreply/Correo',
						method:'delete',
						success: function(result) {   
							storeCorreos.remove(gridCorreos.getSelectionModel().getSelection()[0]);		
						},
						params:record
					});	 
                }
            }]		
        }]
    });

 	gridCorreos.getSelectionModel().on('selectionchange', function(sm, selectedRecord) {
        if (selectedRecord.length) {	
        	Ext.getCmp('buttonBorrarCorreo').setDisabled(false);
        }
        else{
        	Ext.getCmp('buttonBorrarCorreo').setDisabled(true);
        }
	});
	//
	var gridGruposUsuarios = Ext.create('Ext.grid.Panel', {
		frame: true,
		title:'Pertenece a los siguientes grupos:',
		selType: 'rowmodel',
        store: storeGruposUsuarios,
        height: 250,
        width:  300,
		bodyStyle:'padding:5px 5px 0',
        viewConfig: {
            stripeRows: true
        },
        columns: [{
            text     : 'Nombre',
            flex     : 1,
            sortable : true,
            dataIndex: 'IdGrupo',
            renderer : function( value, metaData, record, rowIndex, colIndex, store,  view ){
				var r = storeGrupos.getById(parseInt( value) );
				if (r)
					return r.get('Nombre');
				else
					return 'Undefinido';
			}
        }],
        dockedItems: [{
            xtype: 'toolbar',
            items: [{
            	id: 'buttonInsertarGrupoUsuario',
                text:'Nuevo',
                tooltip:'Agregar el usuario a un grupo',
                iconCls:'add',
                disabled:true,
                handler:function(){
                	windowGrupos.show();  
                }
            },'-',{
            	id: 'buttonBorrarGrupoUsuario',
                text:'Borrar',
                tooltip:'Borrar grupo para el usuario',
                iconCls:'remove',
                disabled:true,
                handler:function(){
                	var record = {
	    				SessionId:sessionStorage.id,
	    				IdUsuario:gridGruposUsuarios.getSelectionModel().getSelection()[0].get('IdUsuario'),
	    				IdGrupo : gridGruposUsuarios.getSelectionModel().getSelection()[0].get('IdGrupo')
	    			};
	    			executeAjaxRequest({
						url: url+'/json/syncreply/GrupoUsuario',
						method:'delete',
						success: function(result) {   
							storeGruposUsuarios.remove(gridGruposUsuarios.getSelectionModel().getSelection()[0]);		
						},
						params:record
					});	 
                }
            }]		
        }]
    
    });

 	gridGruposUsuarios.getSelectionModel().on('selectionchange', function(sm, selectedRecord) {
        if (selectedRecord.length) {	
        	Ext.getCmp('buttonBorrarGrupoUsuario').setDisabled(false);
        }
        else{
        	Ext.getCmp('buttonBorrarGrupoUsuario').setDisabled(true);
        }
	});
	
	//
	var gridGrupos = Ext.create('Ext.grid.Panel', {
		frame: true,
		selType: 'rowmodel',
        store: storeGrupos,
        height: 300,
        width:  600,
		bodyStyle:'padding:5px 5px 0',
        viewConfig: {
            stripeRows: true
        },
        columns: [{
            text     : 'Descripcion',
            flex     : 1,
            sortable : true,
            dataIndex: 'Descripcion'
            
        },{
            text     : 'Nombre',
            width	 :  150,
            sortable :  true,
            dataIndex: 'Nombre'
            
        },{
            text     : 'Directorio',
            width	 :  100,
            sortable : true,
            dataIndex: 'Directorio'
            
        },{
            text     : 'Menu ?',
            width	 :  80,
            sortable : true,
            dataIndex: 'Menu'
            
        }],
        dockedItems: [{
            xtype: 'toolbar',
            itemId:'gridGruposToolbar',
            items: [{
            	id: 'buttonInsertarGrupo',
                text:'Nuevo',
                tooltip:'Crear un nuevo grupo',
                iconCls:'add',
                handler:function(){
                	var form = formGrupo.getForm();
                	form.reset();
                	form.setValues({Id:0});
                	Ext.getCmp('buttonGuardarGrupo').setText('Insertar');
                }
            },'-',{
            	id: 'buttonBorrarGrupo',
                text:'Borrar',
                tooltip:'Borrar el grupo',
                iconCls:'remove',
                disabled:true,
                handler:function(){
                	var record = {
	    				SessionId:sessionStorage.id,
	    				Id:gridGrupos.getSelectionModel().getSelection()[0].getId()
	    			};
	    			executeAjaxRequest({
						url: url+'/json/syncreply/Grupo',
						method:'delete',
						success: function(result) {   
							storeGrupos.remove(gridGrupos.getSelectionModel().getSelection()[0]);		
						},
						params:record
					});	 
                }
            }]		
        }]
    });

 	gridGrupos.getSelectionModel().on('selectionchange', function(sm, selectedRecord) {
        if (selectedRecord.length) {	
        	Ext.getCmp('buttonBorrarGrupo').setDisabled(false);
        	Ext.getCmp('buttonSeleccionarGrupo').setDisabled(false);
        	Ext.getCmp('buttonInsertarGrupoActividad').setDisabled(false);
        	
        	formGrupo.getForm().loadRecord(selectedRecord[0]);
        	Ext.getCmp('buttonGuardarGrupo').setText('Actualizar');
        	
        	var record ={
             	IdGrupo: selectedRecord[0].getId(),
             	SessionId:sessionStorage.id
            };
        	
        	executeAjaxRequest({
        		url: url+'/json/syncreply/GrupoActividad',
        		method:'get',
        		success: function(result) {			
					storeGruposActividades.setProxy(new Ext.data.proxy.Memory({
						model:'GrupoActividad',
						data: result.GruposActividades
					}) );
				storeGruposActividades.load();
				gridGruposActividades.determineScrollbars();
		    	},
		    	params:record
            });
        }
        else{
        	Ext.getCmp('buttonBorrarGrupo').setDisabled(true);
        	Ext.getCmp('buttonSeleccionarGrupo').setDisabled(true);
        	Ext.getCmp('buttonInsertarGrupoActividad').setDisabled(true);
        	
        	var form = formGrupo.getForm();
            form.reset();
            form.setValues({Id:0});
            Ext.getCmp('buttonGuardarGrupo').setText('Insertar');
        	
        }
	});
	
	var gruposToolbar = gridGrupos.child('#gridGruposToolbar');
    gruposToolbar.add('->', {
    	id: 'buttonSeleccionarGrupo',
        text:'Seleccionar',
        tooltip:'Adicionar Grupo al usuario',
        iconCls:'seleccionar',
        disabled:true,
        handler:function(){
        	var record = {
				SessionId:sessionStorage.id,
				IdUsuario:gridUsuarios.getSelectionModel().getSelection()[0].getId(),
				IdGrupo : gridGrupos.getSelectionModel().getSelection()[0].getId()
			};
			executeAjaxRequest({
				url: url+'/json/syncreply/GrupoUsuario',
				method:'post',
				success: function(result) {   
					var nr = Ext.ModelManager.create(result.GruposUsuarios[0], 'GrupoUsuario') ;
					storeGruposUsuarios.add(nr);
					gridGruposUsuarios.getSelectionModel().doSingleSelect(nr,false);
					gridGruposUsuarios.determineScrollbars();
				},
				params:record
			});	 
        }
    });
	
	//
    var gridGruposActividades = Ext.create('Ext.grid.Panel', {
		frame: true,
		title:'Actividades del grupo:',
		selType: 'rowmodel',
        store: storeGruposActividades,
        height: 315,
        width:  600,
		bodyStyle:'padding:5px 5px 0',
        viewConfig: {
            stripeRows: true
        },
        columns: [{
            text     : 'Nombre',
            flex     : 1,
            sortable : true,
            dataIndex: 'IdActividad',
            renderer : function( value, metaData, record, rowIndex, colIndex, store,  view ){
				var r = storeActividades.getById(parseInt( value) );
				if (r)
					return r.get('Nombre');
				else
					return 'Undefinido';
			}
        }],
        dockedItems: [{
            xtype: 'toolbar',
            items: [{
            	id: 'buttonInsertarGrupoActividad',
                text:'Nuevo',
                tooltip:'Agregar actvidad al  grupo',
                iconCls:'add',
                disabled:true,
                handler:function(){
                	modo='GrupoActividad';
                	windowActividades.show();  
                }
            },'-',{
            	id: 'buttonBorrarGrupoActividad',
                text:'Borrar',
                tooltip:'Borrar actividad para el grupo',
                iconCls:'remove',
                disabled:true,
                handler:function(){
                	var record = {
	    				SessionId:sessionStorage.id,
	    				IdActividad:gridGruposActividades.getSelectionModel().getSelection()[0].get('IdActividad'),
	    				IdGrupo : gridGrupos.getSelectionModel().getSelection()[0].getId()
	    			};
	    			executeAjaxRequest({
						url: url+'/json/syncreply/GrupoActividad',
						method:'delete',
						success: function(result) {   
							storeGruposActividades.remove(gridGruposActividades.getSelectionModel().getSelection()[0]);		
						},
						params:record
					});	 
                }
            }]		
        }]
    
    });
    
    gridGruposActividades.getSelectionModel().on('selectionchange', function(sm, selectedRecord) {
        if (selectedRecord.length) {	
        	Ext.getCmp('buttonBorrarGrupoActividad').setDisabled(false);
        }
        else{
        	Ext.getCmp('buttonBorrarGrupoActividad').setDisabled(true);
        }
	});
    
    //
	var gridActividades = Ext.create('Ext.grid.Panel', {
		frame: true,
		selType: 'rowmodel',
        store: storeActividades,
        height: 460,
        width:  400,
		bodyStyle:'padding:5px 5px 0',
        viewConfig: {
            stripeRows: true
        },
        columns: [{
            text     : 'Nombre',
            flex     : 1,
            sortable : true,
            dataIndex: 'Nombre'
            
        }],
        dockedItems: [{
            xtype: 'toolbar',
            itemId:'gridActividadesToolbar',
            items: [{
            	id: 'buttonInsertarActividad',
                text:'Nuevo',
                tooltip:'Crear un nueva Actividad',
                iconCls:'add',
                handler:function(){
                	formActividad.getForm().reset();
        			formActividad.getForm().setValues({Id:0});
                	Ext.getCmp('buttonGuardarActividad').setText('Insertar');
                }
            },'-',{
            	id: 'buttonBorrarActividad',
                text:'Borrar',
                tooltip:'Borrar la Actividad',
                iconCls:'remove',
                disabled:true,
                handler:function(){
                	var record = {
	    				SessionId:sessionStorage.id,
	    				Id:gridActividades.getSelectionModel().getSelection()[0].getId()
	    			};
	    			executeAjaxRequest({
						url: url+'/json/syncreply/Actividad',
						method:'delete',
						success: function(result) {   
							storeActividades.remove(gridActividades.getSelectionModel().getSelection()[0]);		
						},
						params:record
					});	 
                }
            }]		
        }]
    });

 	gridActividades.getSelectionModel().on('selectionchange', function(sm, selectedRecord) {
        if (selectedRecord.length) {	
        	Ext.getCmp('buttonBorrarActividad').setDisabled(false);
          	Ext.getCmp('buttonSeleccionarActividad').setDisabled(false);
        	Ext.getCmp('buttonGuardarActividad').setText('Actualizar');
        	formActividad.getForm().loadRecord(selectedRecord[0]);
        }
        else{
        	Ext.getCmp('buttonBorrarActividad').setDisabled(true);
        	Ext.getCmp('buttonSeleccionarActividad').setDisabled(true);
        	Ext.getCmp('buttonGuardarActividad').setText('Insertar');
        	formActividad.getForm().reset();
        	formActividad.getForm().setValues({Id:0});
        }
	});
	
	var actividadesToolbar = gridActividades.child('#gridActividadesToolbar');
    actividadesToolbar.add('->', {
    	id: 'buttonSeleccionarActividad',
        text:'Seleccionar',
        tooltip:'Adicionar la actividad seleccionada',
        iconCls:'seleccionar',
        disabled:true,
        handler:function(){
        	var record = {
				SessionId:sessionStorage.id,
				IdActividad: gridActividades.getSelectionModel().getSelection()[0].getId()
			}
			
			if(modo=='Correo')
				record.IdUsuario=gridUsuarios.getSelectionModel().getSelection()[0].getId();
			else
				record.IdGrupo = gridGrupos.getSelectionModel().getSelection()[0].getId();
			
			executeAjaxRequest({
				url: url+'/json/syncreply/'+modo,
				method:'post',
				success: function(result) {
					if(modo=='Correo'){
						var nr = Ext.ModelManager.create(result.Correos[0], 'Correo') ;
						storeCorreos.add(nr);
						gridCorreos.getSelectionModel().doSingleSelect(nr,false);
					}
					else{
						var nr = Ext.ModelManager.create(result.GruposActividades[0], 'GrupoActividad') ;
						storeGruposActividades.add(nr);
						gridGruposActividades.getSelectionModel().doSingleSelect(nr,false);
					}
							
				},
				params:record
			});	 
        }
    });
	
	//
	var formUsuario = Ext.create('Ext.form.Panel', {
		method:'',
		accion:'',
        bodyPadding: 15,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 80,
			labelAlign: 'right'
        },
        defaultType: 'textfield',
        defaults: {
            anchor: '100%',
			labelStyle: 'padding-left:4px;'
        },
        items: [{	
        	xtype:'hidden',  
	        name:'SessionId',  
	        value:sessionStorage.id
	    },{
	        xtype:'hidden',  
	        name:'Id'
	    },{
	    	id:'tbNombreUsuario',
	        fieldLabel: 'Nombre',
	        name: 'Nombre',
			type     : 'textfield',
			allowBlank:false      
        },{
        	id:'tbClaveUsuario',
	        fieldLabel: 'Clave',
	        name: 'Clave',
			inputType     : 'password',
			allowBlank:false      
        },{
        	id:'tbCorreoUsuario',
	        fieldLabel: 'Correo',
	        name: 'Correo',
			type     : 'textfield',
			allowBlank:false      
        },{
        	id:'tbEstadoUsuario',
        	xtype: 'checkbox',
        	fieldLabel: 'Activo ?',
        	name : 'Activo'
        }],
		buttons:[{ 
            text:'Guardar',
            formBind: true,	 
            handler:function(){
            	var form = this.up('form').getForm();				            
				var record = form.getFieldValues();
				if (form.accion) record.Accion= form.accion;
				if(form.method=='POST') record.Id=0; 
				executeAjaxRequest({
					url: url+'/json/syncreply/usuario',
					method: form.method,
					success: function(result) {
						if (form.method=='PUT'){
							record = form.getFieldValues(true);
							var ur = storeUsuarios.getById(parseInt( record.Id) );
							ur.beginEdit();
							for( var r in record){
								ur.set(r, record[r])
							}
							if(!record.Activo) ur.set('Activo',false);  // bug ?? cuando pasa de true a false no lo devuelve ?
							ur.endEdit();
							ur.commit(); 	
						}
						else{
                        	var nr = Ext.ModelManager.create(result.Usuarios[0], 'Usuario') ;
							storeUsuarios.add(nr);
							gridUsuarios.getSelectionModel().doSingleSelect(nr,false);
							
						}
						windowUsuario.hide();
				    },
					params:record
				});
			}              
	    }] 
    });

    //
    
    var formGrupo = Ext.create('Ext.form.Panel', {
    	width: 340,
    	bodyPadding: 15,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 80,
			labelAlign: 'right'
        },
        defaultType: 'textfield',
        defaults: {
            anchor: '100%',
			labelStyle: 'padding-left:4px;'
        },
        items: [{	
        	xtype:'hidden',  
	        name:'SessionId',  
	        value:sessionStorage.id
	    },{
	        xtype:'hidden',  
	        name:'Id'
	    },{
	        fieldLabel: 'Descripcion',
	        name: 'Descripcion',
			allowBlank:false      
        },{
	        fieldLabel: 'Nombre',
	        name: 'Nombre',
	        allowBlank:false      
        },{
	        fieldLabel: 'Directorio',
	        name: 'Directorio',
			allowBlank:false      
        },{
        	xtype: 'checkbox',
        	fieldLabel: 'Menu ?',
        	name : 'Menu'
        }],
		buttons:[{ 
			id: 'buttonGuardarGrupo',
            text:'Insertar',
            formBind: true,	 
            handler:function(){
            	var form = this.up('form').getForm();				            
				var record = form.getFieldValues();
				executeAjaxRequest({
					url: url+'/json/syncreply/Grupo',
					method: ( record.Id!='0'? 'put':'post' ),
					success: function(result) {
						if (record.Id!='0'){
							record = form.getFieldValues(true);
							var ur = storeGrupos.getById(parseInt( record.Id) );
							ur.beginEdit();
							for( var r in record){
								ur.set(r, record[r])
							}
							if(!record.Menu) ur.set('Menu',false);  // bug ?? cuando pasa de true a false no lo devuelve ?
							ur.endEdit();
							ur.commit(); 	
						}
						else{
                        	var nr = Ext.ModelManager.create(result.Grupos[0], 'Grupo') ;
							storeGrupos.add(nr);
							gridGrupos.getSelectionModel().doSingleSelect(nr,false);
							
						}
				    },
					params:record
				});
			}              
	    }] 
    });

    
    //
    var formActividad = Ext.create('Ext.form.Panel', {
    	bodyPadding: 15,
    	width: 370,  //780-400
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 80,
			labelAlign: 'right'
        },
        defaultType: 'textfield',
        defaults: {
            anchor: '100%',
			labelStyle: 'padding-left:4px;'
        },
        items: [{	
        	xtype:'hidden',  
	        name:'SessionId',  
	        value:sessionStorage.id
	    },{
	        xtype:'hidden',  
	        name:'Id'
	    },{
	        fieldLabel: 'Nombre',
	        name: 'Nombre',
			type     : 'textfield',
			allowBlank:false      
        }],
		buttons:[{
			id:'buttonGuardarActividad',
            text:'Guardar',
            formBind: true,	 
            handler:function(){
            	var form = this.up('form').getForm();				            
				var record = form.getFieldValues();
				executeAjaxRequest({
					url: url+'/json/syncreply/Actividad',
					method: (record.Id != "0")? 'put':'post',
					success: function(result) {
						if (record.Id != "0"){
							record = form.getFieldValues(true);
							var ur = storeActividades.getById(parseInt( record.Id) );
							ur.beginEdit();
							for( var r in record){
								ur.set(r, record[r])
							}
							ur.endEdit();
							ur.commit(); 	
						}
						else{
                        	var nr = Ext.ModelManager.create(result.Actividades[0], 'Actividad') ;
							storeActividades.add(nr);
							gridActividades.getSelectionModel().doSingleSelect(nr,false);
							
						}
				    },
					params:record
				});
			}              
	    }] 
    });

    
    //
    
	var windowUsuario = Ext.create('Ext.Window',{
    	 closable: true,
         closeAction: 'hide',
         height: 250,
         width: 450,
         layout: 'fit',
         modal: true,
         y:35,
         items:[
         	formUsuario
         ]
    });
	 
    var windowGrupos = Ext.create('Ext.Window',{
    	 closable: true,
         closeAction: 'hide',
         height: 650,
         width: 950,
         layout: {
            type: 'table',
            columns: 2
        },
         modal: true,
         y:65,
         items:[
         	gridGrupos, formGrupo, gridGruposActividades
         ]
    });
    
    var windowActividades = Ext.create('Ext.Window',{
    	 closable: true,
         closeAction: 'hide',
         height: 500,
         width: 780,
         layout: 'hbox',
         modal: true,
         y:165,
         items:[
         	gridActividades, formActividad
         ]
    });
    
  	var showWindowUsuario = function(method, accion){
  		var form = formUsuario.getForm();
  		form.method= Ext.util.Format.uppercase(method);
  		form.accion=accion;
  		form.reset();
  		if( form.method=='POST') {
  			Ext.getCmp('tbNombreUsuario').setDisabled( false);
  			Ext.getCmp('tbClaveUsuario').setDisabled( false);
  			Ext.getCmp('tbCorreoUsuario').setDisabled( false);
  			Ext.getCmp('tbEstadoUsuario').setDisabled( false);
  		}
  		else if ( form.method=='PUT'){
  			var r = gridUsuarios.getSelectionModel().getSelection()[0];
  			form.loadRecord(r);
  			if(form.accion=="CambiarClave"){
  				Ext.getCmp('tbNombreUsuario').setDisabled( true);
  				Ext.getCmp('tbClaveUsuario').setDisabled( false);
  				Ext.getCmp('tbCorreoUsuario').setDisabled( true);
  				Ext.getCmp('tbEstadoUsuario').setDisabled( true);
  			}
  			else{
  				Ext.getCmp('tbNombreUsuario').setDisabled( false);
  				Ext.getCmp('tbClaveUsuario').setDisabled( true);
  				Ext.getCmp('tbCorreoUsuario').setDisabled( false);
  				Ext.getCmp('tbEstadoUsuario').setDisabled( false);
  			}
  			  			
  		}
  		
  		windowUsuario.show();
  	}

	
    var panelModulo = Ext.create('Ext.Panel', {
        id:'panelModulo',
        baseCls:'x-plain',
        renderTo: 'modulo',
        layout: {
            type: 'table',
            columns: 2
        },
        items:[
			gridUsuarios, 
			{
				xtype:'panel',
				height: 500,
				width:310,
				baseCls:'x-plain',
				layout: {
       				type: 'vbox',       // Arrange child items vertically
        			align: 'center'
    			},
				items:[
					gridCorreos, gridGruposUsuarios,
				]	
			}
		]
    });
        	    
});

