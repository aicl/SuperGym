Ext.require([
    'Ext.form.*',
    'Ext.layout.container.Column',
    'Ext.tab.Panel',
	'Ext.window.MessageBox',
    'Ext.tip.*'
]);

function verVentanaDescarga(id, mailMessage){
	Ext.create('Ext.window.Window', {
		closable:true,
	    height: (mailMessage?mailMessage:"").length<150? 120:300,
	    width: 450,
	    layout: 'fit',
	    html:Ext.String.format('<center>{0}<br/><br/><center><a href="../facturas/{1}.pdf">Descargar Factura</a></center>',
							    					mailMessage ,id)
	}).show();
}

Ext.onReady(function(){

    Ext.QuickTips.init();
    //var bd = Ext.getBody();
    var disableAnular= ! checkActividad('Pago.Anular');
    
    var url= location.protocol + "//" + location.host+'/servicio';
    var criterio='Documento';
    var modo = 'Insert';

    /*
	* ==========  Modelos ==========================
	*/ 

	Ext.define('Profesion', {
        extend: 'Ext.data.Model',
		idProperty :'Id',
        fields: [
            {type: 'number', name: 'Id'},
            {type: 'string', name: 'Nombre'}
        ]
    });

	Ext.define('TipoDocumento', {
        extend: 'Ext.data.Model',
		idProperty :'Id',
        fields: [
            {type: 'number', name: 'Id'},
            {type: 'string', name: 'Nombre'}
        ]
    });

	Ext.define('Clasificacion', {
        extend: 'Ext.data.Model',
		idProperty :'Id',
        fields: [
            {type: 'number', name: 'Id'},
            {type: 'string', name: 'Nombre'}
        ]
    });

	Ext.define('Municipio', {
        extend: 'Ext.data.Model',
		idProperty :'Id',
        fields: [
            {type: 'number', name: 'Id'},
            {type: 'string', name: 'Nombre'}
        ]
    });

    Ext.define('TipoPago',{
    	extend: 'Ext.data.Model',
    	idProperty: 'Id',
    	fields:[
    		{name: 'Id', type:'number', defaultValue:1},
    		{name: 'Nombre', type:'string'},
    		{name: 'Valor', type:'number'},
    		{name: 'Dias' ,  type:'number'},
    		{name: 'ValidoDesde', type:'date', convert:function(v){ return  convertToDate(v) } },
    		{name: 'ValidoHasta', type:'date', convert: function(v){ return  convertToDate(v) }}
    	]
    });
    
	Ext.define('Persona', {
        extend: 'Ext.data.Model',
		idProperty :'Id',
        fields: [
            {name: 'Celular',  type: 'string'},
            {name: 'DireccionResidencia', type: 'string'},
            {name: 'Documento', type: 'string'},
            {name: 'Email',  type: 'string'},
            {name: 'Empresa',   type: 'string'},
            {name: 'FechaNacimiento',   type: 'date', 
					convert: function(v) { return  convertToDate(v)	} },
            {name: 'FechaRegistro',   type: 'date',
					convert: function(v) {  return  convertToDate(v) } },
            {name: 'Id', type: 'number', defaultValue:0},
            {name: 'IdClasificacion',   type: 'number'},
            {name: 'IdMunicipio', type: 'number'}, 
            {name: 'IdProfesion',   type: 'number'},
            {name: 'IdTipoDocumento',   type: 'number'},
            {name: 'IdUsuarioRegistra',   type: 'number'},
			{name: 'NombreBarrio',     type: 'string'},
			{name: 'Nombres',     type: 'string'},
			{name: 'PrimerApellido',     type: 'string'},
			{name: 'RutaTemplate',     type: 'string'},
			{name: 'SegundoApellido',     type: 'string'},
			{name: 'Sexo',     type: 'string'},
			{name: 'Telefono',     type: 'string'}
        ]
    });
    
    Ext.define('Factura',{
    	extend: 'Ext.data.Model',
		idProperty :'Id',
        fields: [
        	{name: 'Id', type: 'int', defaultValue:0},
        	{name: 'IdTipoPago', type: 'int'},
        	{name: 'IdFormaPago' , type: 'int', defaultValue:1 },
        	{name: 'IdPersona', type: 'int'},
        	{name: 'Numero', type :'string'},
        	{name: 'FechaPago', type :'date', convert: function(v){ return  convertToDate(v) } },
        	{name: 'FechaInicio', type :'date' , convert: function(v){ return  convertToDate(v) } },
        	{name: 'FechaTerminacion', type :'date' , convert: function(v){ return  convertToDate(v) } },
        	{name: 'ValorUnitario', type: 'number'},
        	{name: 'Cantidad', type: 'int'},
        	{name: 'ValorTotal', type: 'number'},
        	{name: 'Observacion', type: 'string'},
        	{name: "IdUsuarioRegistra", type: 'int' },
        	{name: "Activa", type: 'boolean' }
        ]
    });
    

     Ext.define('Suspension',{
    	extend: 'Ext.data.Model',
		idProperty :'Id',
        fields: [
        	{name: 'Id', type: 'int', defaultValue:0},
        	{name: 'IdPago', type: 'int'},
        	{name: 'Fecha', type :'date', convert: function(v){ return  convertToDate(v) } },
        	{name: 'Desde', type :'date' , convert: function(v){ return  convertToDate(v) } },
        	{name: 'Hasta', type :'date' , convert: function(v){ return  convertToDate(v) } },
        	{name: 'FechaTerminacion', type :'date' , convert: function(v){ return  convertToDate(v) } },
        	{name: 'NuevaFecha', type :'date' , convert: function(v){ return  convertToDate(v) } }
        ]
    });
    
    
	/*
	* ==========  Stores ==========================
	*/ 

	var storePersonas= Ext.create('Ext.data.Store', {
		model:'Persona',
		autoLoad:false
	});

	var storeFacturas= Ext.create('Ext.data.Store', {
		model:'Factura',
		autoLoad:false
	});
	
	var storeSuspensiones= Ext.create('Ext.data.Store', {
		model:'Suspension',
		autoLoad:false
	});
	
	
	var storeProfesiones = Ext.create('Ext.data.Store', {
	    model: 'Profesion',
		proxy: createAjaxProxy( {
			url : url+'/json/asynconeway/ProfesionesGet',
           	root: 'Profesiones'
    	}),
	    autoLoad: true
	});

	var storeClasificaciones = Ext.create('Ext.data.Store', {
	    model: 'Clasificacion',
		proxy: createAjaxProxy( {
	        url : url+'/json/asynconeway/ClasificacionesGet',
           	root: 'Clasificaciones'
    	}),
	    autoLoad: true
	});

	var storeTiposDocumento = Ext.create('Ext.data.Store', {
	    model: 'TipoDocumento',
		proxy: createAjaxProxy({
	        url : url+'/json/asynconeway/TiposDocumentoGet',
           	root: 'TiposDocumento',
    	}),
	    autoLoad: true
	});

	var storeMunicipios = Ext.create('Ext.data.Store', {
	    model: 'Municipio',
		proxy: createAjaxProxy( {
	        url : url+'/json/asynconeway/MunicipiosGet',
        	root: 'Municipios',
    	}),
	    autoLoad: true
	});


	var storeTiposPago = Ext.create('Ext.data.Store', {
	    model: 'TipoPago',
		proxy: createAjaxProxy( {
			url : url+'/json/asynconeway/FacturaTipoPagoGet',
           	root: 'TiposPago',
    	}),
	    autoLoad: true
	});
	
	/*
	* ==========  Vistas  ==========================
	*/ 

	var comboProfesiones = Ext.create('Ext.form.field.ComboBox', {
        fieldLabel: 'Profesion',
        displayField: 'Nombre',
		valueField: 'Id',
		name:'IdProfesion',
        store: storeProfesiones,
        queryMode: 'local',
        typeAhead: true,
        forceSelection:true,
    });

	var comboTiposDocumento = Ext.create('Ext.form.field.ComboBox', {
        fieldLabel: 'Tipo Documento',
        displayField: 'Nombre',
		valueField: 'Id',
		name:'IdTipoDocumento',
        store: storeTiposDocumento,
        queryMode: 'local',
        typeAhead: true,
        forceSelection:true
    });
	
	var comboClasificaciones = Ext.create('Ext.form.field.ComboBox', {
        fieldLabel: 'Clasificacion',
        displayField: 'Nombre',
		valueField: 'Id',
		name:'IdClasificacion',
        store: storeClasificaciones,
        queryMode: 'local',
        typeAhead: true,
        forceSelection:true
    });

	var comboMunicipios = Ext.create('Ext.form.field.ComboBox', {
        fieldLabel: 'Municipio',
        displayField: 'Nombre',
		valueField: 'Id',
		name:'IdMunicipio',
        store: storeMunicipios,
        queryMode: 'local',
        typeAhead: true,
        forceSelection:true
    });

    var comboTiposPago = Ext.create('Ext.form.field.ComboBox', {
        fieldLabel: 'Tipo de Pago',
        displayField: 'Nombre',
		valueField: 'Id',
		name:'IdTipoPago',
        store: storeTiposPago,
        queryMode: 'local',
        typeAhead: true,
        forceSelection:true,
        listeners   : {  
        	beforerender: function(combo){
        		if (gridFacturas.getSelectionModel().getSelection().length )
					combo.setValue( gridFacturas.getSelectionModel().getSelection()[0].get('IdTipoPago') );   		
        		else
            		combo.setValue(1);// El ID de la opción por defecto  
        	},
            select: function(combo, value, options){
            	var ff = formFactura.getForm();
            	var record = value[0].data;
            	var cantidad = ff.findField('Cantidad').getValue();
            	var fi = ff.findField('FechaInicio').getValue();
            	ff.findField('ValorUnitario').setValue(record.Valor);
            	ff.findField('ValorTotal').setValue( cantidad * record.Valor);
            	ff.findField('FechaTerminacion').setValue( fechaTerminacion(fi,  cantidad*record.Dias ) );
            } 
        }
    });
        
    
	var gridPersonas = Ext.create('Ext.grid.Panel', {
		frame: true,
		selType: 'rowmodel',
        store: storePersonas,
        columns: [{
            text     : 'Documento',
            flex     : 1,
            sortable : true,
            dataIndex: 'Documento'
        },{
            text     : 'Nombres',
            width    : 150,
            sortable : true,
            dataIndex: 'Nombres'
        },{
            text     : 'Apellidos',
            width    : 150,
           	sortable : true,
            renderer : function( value, metaData, record, rowIndex, colIndex, store,  view ){
				return Ext.String.format('{0} {1}', value, record.get("SegundoApellido") );
			},
            dataIndex: 'PrimerApellido'
        }],
        height: 115,
        width: 400,
		bodyStyle:'padding:5px 5px 0',
        viewConfig: {
            stripeRows: true
        }
    });

 	gridPersonas.getSelectionModel().on('selectionchange', function(sm, selectedRecord) {
        if (selectedRecord.length) {
            var fp =formPersona.getForm();
            var ff =formFoto.getForm();
            fp.reset();
			ff.reset();
			ff.findField('Id').setValue(selectedRecord[0].data.Id );
            fp.loadRecord(Ext.ModelManager.create(selectedRecord[0].data,'Persona'));
            imgPersona.setSrc('../fotos/'+ selectedRecord[0].data.Id+'.jpg');
            storeFacturas.removeAll(false);
            
        }
	});

	var imgPersona = Ext.create('Ext.Img', {
    	src: '../resources/icons/fam/user.png',
    	width:150, height: 200
    });
    
    
    var gridFacturas = Ext.create('Ext.grid.Panel', {
		frame: true,
		selType: 'rowmodel',
        store: storeFacturas,
        columns: [{
            text     : 'Numero',
            flex     : 1,
            sortable : true,
            dataIndex: 'Numero',
            renderer: function(value, metadata, record, store){
           		var activa= record.get('Activa');
           		if (activa)  {
            		return '<div style="color:black;">' + value + '</div>';
        		} else  {
            		return '<div style="color:red;">' + value + '</div>';
        		}
           	}
        },{
            text     : 'Inicio',
            width    : 120,
            sortable : true,
            dataIndex: 'FechaInicio',
            renderer : Ext.util.Format.dateRenderer('d.m.Y')
        },{
            text     : 'Terminacion',
            width    : 120,
           	sortable : true,
           	dataIndex: 'FechaTerminacion',
            renderer : Ext.util.Format.dateRenderer('d.m.Y')
        }],
        height: 200,
        width: 390,
		bodyStyle:'padding:5px 5px 0',
        viewConfig: {
            stripeRows: true
        },
        dockedItems: [{
            xtype: 'toolbar',
            items: [{
            	id: 'buttonFacturar',
                text:'Nueva Factura',
                tooltip:'Crear factura para la persona seleccionada',
                iconCls:'add',
                disabled: true,  
                handler:function(){
	            	var form = formPersona.getForm();
	            	var ff = formFactura.getForm();
	            	var record = form.getFieldValues();
	            	ff.setValues({IdPersona:record.Id,
	            		Nombres: Ext.String.format('{0} {1} {2}', record.Nombres, record.PrimerApellido, record.SegundoApellido),
	            		Documento:record.Documento,
	            		Cantidad: 1,
	            		FechaInicio: new Date(),
	            		FechaPago: new Date(),
	            		Telefono: record.Telefono});
	            	
	            	var tp =comboTiposPago.getValue() || 1;
	            	var rs = storeTiposPago.getById(tp);
	            	
	            	ff.findField('ValorUnitario').setValue(rs.data.Valor);
	            	ff.findField('ValorTotal').setValue( ff.findField('Cantidad').getValue() * rs.data.Valor);
	            	
	            	executeAjaxRequest({
	            		url: url+'/json/asynconeway/FacturaSiguienteNumeroGet',
	            		success: function(result) {
							ff.findField('Numero').setValue(result.Numero );
							Ext.getCmp('buttonGuardarFactura').setDisabled( false);
							windowFacturacion.show();
					    },
					    params:{SessionId:sessionStorage.id}
	            	})			            		
	            }
            }, '-',{
                id: 'buttonVerFactura',
                tooltip:'Ver detalles de la factura seleccionada',
                iconCls:'preview',
                disabled: true,
                handler:function(){
                	var record = formPersona.getForm().getFieldValues();
	        		formFactura.getForm().setValues({IdPersona:record.Id,
	        		Nombres: Ext.String.format('{0} {1} {2}', record.Nombres, record.PrimerApellido, record.SegundoApellido),
	        		Documento:record.Documento,
	        		Telefono: record.Telefono});
                	
                	Ext.getCmp('buttonGuardarFactura').setDisabled( true );
					windowFacturacion.show();	
                }
        	},'-',{
                id: 'buttonImprimirFactura',
                tooltip:'imprimir la factura seleccionada',
                iconCls:'print',
                disabled: true,
                handler: function(){
                	verVentanaDescarga( gridFacturas.getSelectionModel().getSelection()[0].getId(),'');
                }
        	},'-',{
                id: 'buttonEnviarFactura',
                tooltip:'Enviar la factura seleccionada por mail',
                iconCls:'mail',
                disabled: true,
                handler: function(){
                	var mail = formPersona.getForm().findField('Email').getValue();
                	if(!checkEmail(mail)){
                		Ext.shared.msg('<div style="color:#FF0000;">Error</div>', 
                			Ext.String.format('<div style="color:#FF0000;">Mail no valido:<br />"{0}"</div>',mail) );
                		return;
                	}
                	
                	executeAjaxRequest({
                		url: url+'/json/asynconeway/FacturaPagoSendMail',
                		success: function(result) {			
							Ext.shared.msg('Listo', Ext.String.format('Factura enviada a:"{0}"',mail) );
                		},
                		params:{
                			Id: gridFacturas.getSelectionModel().getSelection()[0].getId() ,
                			SessionId:sessionStorage.id
                		}
                		
                	});
                }
        	},'-',{
                id: 'buttonCargarFacturas',
                tooltip:'Traer Todas las Facturas de la persona seleccionada',
                iconCls:'load',
                disabled: true,
                handler: function(){
                	var record ={
                		Id: formPersona.getForm().getRecord().get('Id') ,
                		SessionId:sessionStorage.id
                	};
                	executeAjaxRequest({
                		url: url+'/json/asynconeway/PersonaFacturasGet',
                		success: function(result) {			
							storeFacturas.setProxy(new Ext.data.proxy.Memory({
								model:'Factura',
								data: result.Facturas
							}) );
						storeFacturas.load();						
				    	},
				    	params:record
                	});
                }
        	},'-',{
                id: 'buttonAnularFactura',
                tooltip:'anula la factura seleccionada',
                iconCls:'desasentar',
                disabled: true,
                handler: function(){
                	var record ={
                		Id: gridFacturas.getSelectionModel().getSelection()[0].getId() ,
                		SessionId:sessionStorage.id
                	};
                	executeAjaxRequest({
                		url: url+'/json/asynconeway/FacturaPagoAnular',
                		success: function(result) {
                			var ur = storeFacturas.getById(parseInt( record.Id) );
							ur.beginEdit();
							var fr = result.Factura;
							for( var r in ur.data){
								ur.set(r, fr[r])
							}
							ur.endEdit();
							ur.commit();
							Ext.getCmp('buttonAnularFactura').setDisabled( ! ur.Activa  ) ;
				    	},
				    	params:record
                	});
                }
        	},'-',{
                id: 'buttonVerSuspensiones',
                tooltip:'ver/insertar/borrar suspensiones',
                iconCls:'stop',
                disabled: true,
                handler: function(){
                	var record = {
	    				SessionId:sessionStorage.id,
	    				IdPago: gridFacturas.getSelectionModel().getSelection()[0].getId()
	    			}
	    	           			
	    			executeAjaxRequest({
						url: url+'/json/asynconeway/SuspensionesGet',
						success: function(result) {   
							storeSuspensiones.setProxy(new Ext.data.proxy.Memory({
								model:'Suspension',
								data: result.Suspensiones
							}) );
							storeSuspensiones.load();	
							windowSuspensiones.show();
						},
						params:record
					});
              	   	
                }
        	}]
        }]
    });

 	gridFacturas.getSelectionModel().on('selectionchange', function(sm, selectedRecord) {
        if (selectedRecord.length) {
        	Ext.getCmp('buttonImprimirFactura').setDisabled( false);
        	Ext.getCmp('buttonEnviarFactura').setDisabled( false);
        	Ext.getCmp('buttonVerFactura').setDisabled( false);
        	Ext.getCmp('buttonAnularFactura').setDisabled( false || disableAnular || ! selectedRecord[0].get('Activa')  );
        	Ext.getCmp('buttonVerSuspensiones').setDisabled( false || ! selectedRecord[0].get('Activa') );
        	//formFactura.getForm().loadRecord(Ext.ModelManager.create(selectedRecord[0].data,'Factura'));
        	formFactura.getForm().loadRecord(selectedRecord[0]);
        }
        else{
        	Ext.getCmp('buttonImprimirFactura').setDisabled( true);
        	Ext.getCmp('buttonEnviarFactura').setDisabled( true);
        	Ext.getCmp('buttonVerFactura').setDisabled( true);
        	Ext.getCmp('buttonAnularFactura').setDisabled( true);
        	Ext.getCmp('buttonVerSuspensiones').setDisabled( true);
        }
	});

    

	var gridSuspensiones = Ext.create('Ext.grid.Panel', {
		frame: true,
		selType: 'rowmodel',
        store: storeSuspensiones,
        columns: [{
            text     : 'Fecha Registro',
            flex     : 1,
            sortable : true,
            dataIndex: 'Fecha',
            renderer: Ext.util.Format.dateRenderer('d.m.Y')
        },{
            text     : 'Desde',
            width    : 80,
            sortable : true,
            dataIndex: 'Desde',
            renderer : Ext.util.Format.dateRenderer('d.m.Y')
        },{
            text     : 'Hasta',
            width    : 80,
           	sortable : true,
           	dataIndex: 'Hasta',
            renderer : Ext.util.Format.dateRenderer('d.m.Y')
        },{
            text     : 'Antigua Fecha',
            width    : 80,
           	sortable : true,
           	dataIndex: 'FechaTerminacion',
            renderer : Ext.util.Format.dateRenderer('d.m.Y')
        },{
            text     : 'Nueva Fecha',
            width    : 80,
           	sortable : true,
           	dataIndex: 'NuevaFecha',
            renderer : Ext.util.Format.dateRenderer('d.m.Y')
        }],
        height: 200,
        width: 420,
		bodyStyle:'padding:5px 5px 0',
        viewConfig: {
            stripeRows: true
        },
        dockedItems: [{
            xtype: 'toolbar',
            items: [{
            	id: 'buttonSuspensionInsert',
                tooltip:'registrar suspension del servicio',
                iconCls:'add',
                disabled: false,  
                handler:function(){
	            	windowSuspensionInsert.show();			            		
	            }
            }, '-',{
                id: 'buttonSuspensionDelete',
                tooltip:'borrar suspension seleccionada',
                iconCls:'remove',
                disabled: true,
                handler:function(){
                	var record = {
                		Id: gridSuspensiones.getSelectionModel().getSelection()[0].getId(),
                		SessionId: sessionStorage.id
                	}
					
					executeAjaxRequest({
						url: url+'/json/asynconeway/SuspensionDelete',
						success: function(result) {
	                        
							gridSuspensiones.getStore().remove(gridSuspensiones.getSelectionModel().getSelection()[0]);
							var ur =  gridFacturas.getSelectionModel().getSelection()[0]; //.getId(); //storeFacturas.getById(parseInt( record.IdPago) );
							ur.beginEdit();
							var fr = result.Factura;
							for( var r in ur.data){
								ur.set(r, fr[r])
							}
							ur.endEdit();
							ur.commit();
							
						},
						params:record
					});
                }
            }]
        }]
    });

 	gridSuspensiones.getSelectionModel().on('selectionchange', function(sm, selectedRecord) {
        if (selectedRecord.length) {
        	Ext.getCmp('buttonSuspensionDelete').setDisabled( false);
        }
        else{
        	Ext.getCmp('buttonSuspensionDelete').setDisabled( true);
        	
        }
	});
	
	
    /*
     * ===========  Fomr para la subir la foto==============
    */
    
    var formFoto = Ext.create('Ext.form.Panel', {
    	width: 300,
    	height: 80,
    	bodyPadding: 10,
    	frame: true,    
    	items: [{xtype:'hidden',  
            name:'SessionId',  
            value:sessionStorage.id
    	},{
            xtype:'hidden',
            name: 'Id',
            value:0
        },{
        xtype: 'filefield',
        name: 'fileupload',
        msgTarget: 'side',
        allowBlank: false,
        anchor: '100%',
        buttonText: '',
    	buttonConfig: {
    		iconCls: 'upload-icon'
    	},
    	emptyText: 'Seleccione el archivo'
    	}],

    	buttons: [{
			text: 'Cargar',
			formBind: true,
        	handler: function() {
				var form = this.up('form').getForm();
            	if(!form.isValid()){
            		Ext.Msg.alert('Debe Seleccionar un archivo -) ');
					return ;
            	}
            	if( form.findField('Id').getValue()==0  ){
            		Ext.Msg.alert('No hay persona seleccionada  -) ');
					return ;
            	}
            	form.submit({
                	url: url+'/json/asynconeway/FotoUpload',
                	waitMsg: 'Cargando Archivo...',
                	success: function(form, action) {
                		imgPersona.setSrc('../fotos/'+ form.findField('Id').getValue()+'.jpg');
                		Ext.shared.msg('Listo', 'El archivo  ha sido cargado.');
                		form.findField('fileupload').inputEl.dom.value = '';
                	},
                	failure: function(form, action) {
                		Ext.Msg.alert('Error', action.result.msg);
                		form.findField('fileupload').inputEl.dom.value = ''
                	}
            	});   
			}      
    	}]
	});
    
	
    /*
	  ==========  Formulario para Pedir Documento ==================
	*/ 

	var formDocumento = Ext.create('Ext.form.Panel', {
        frame:true,
        bodyStyle:'padding:5px 5px 0',
        width: 460,
		height: 115,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 120,
			labelAlign: 'right',
        },
        defaultType: 'textfield',
        defaults: {
            anchor: '100%'
        },
        items: [{
            xtype:'hidden',  
            name:'SessionId',  
            value:sessionStorage.id
        },{
            fieldLabel: 'Valor a Buscar',
            name: 'Documento',
            allowBlank:false
        },{
            xtype: 'radiogroup',
            fieldLabel: 'Buscar Por',
            cls: 'x-check-group-alt',
            items: [
                {boxLabel: 'Documento', name: 'Criterio', inputValue: 'Documento', checked: true},
                {boxLabel: 'Nombres',  name: 'Criterio', inputValue: 'Nombres'},
				{boxLabel: 'Primer Apellido',  name: 'Criterio', inputValue: 'PrimerApellido'},
            ]
        }],
		buttons:[{ 
            text:'Buscar',
            formBind: true,	 
            handler:function(){
            	modo='Insert';
				var fp =formPersona.getForm();
				var ff =formFoto.getForm();
				fp.reset();
				ff.reset();
				storePersonas.removeAll(false); // silent=true
				storeFacturas.removeAll(false);
				imgPersona.setSrc('../resources/icons/fam/user.png');
				var form = formDocumento.getForm();            
				var record = form.getValues()  ;
				var fullUrl;
				criterio=record.Criterio;
				if(criterio=='Documento') {
					fullUrl=url+'/json/asynconeway/PersonaGet';
					delete record.Criterio;
				}
				else{
					record.Valor= record.Documento;
					fullUrl=url+'/json/asynconeway/PersonasGet';
					delete record.Documento;
				};
				if ( ! form.isValid()) {
					Ext.Msg.alert(' Debe Indicar el Numero de Documento por Favor -) ');
					return ;
				};
				executeAjaxRequest({
					url: fullUrl,
					success: function(result) {
                        var data;
						if(criterio=='Documento'){
							if(! result.Persona ){
                            	fp.setValues({Documento:form.getValues().Documento});
								Ext.shared.msg('Atencion','No existe persona con documento:'+ form.getValues().Documento + ' -( '); 
								return ;
							}
							data=[result.Persona];
						}
						else{
							if( result.Count==0 ){
								Ext.shared.msg('Atencion','No existe personas con '+ criterio + '=' +form.getValues().Documento + ' -( '); 
								return ;
							}
							data=result.Personas
						};
						modo='Update';
						fp.loadRecord(Ext.ModelManager.create(data[0], 'Persona') );
						ff.findField('Id').setValue(data[0].Id );
						storePersonas.setProxy(new Ext.data.proxy.Memory({
							model:'Persona',
							data: data
						}) );
						storePersonas.load();
						imgPersona.setSrc('../fotos/'+ data[0].Id+'.jpg');
				    },
					callback: function(result, success) {
						Ext.getCmp('buttonGuardar').setText( (modo=='Insert')?'Insertar':'Actualizar');
						Ext.getCmp('buttonFacturar').setDisabled( (modo=='Insert')?true:false);
						Ext.getCmp('buttonCargarFacturas').setDisabled( (modo=='Insert')?true:false);        					
	    			},
					params:record
				});
			}              
	    }] 
    });

    //formDocumento.render(document.body);
    //bd.createChild({tag: 'h2', html: 'Datos'});

    /*
      ================  Formulario Modificar-Insertar-Borrar Persona ===========
    */

    var formPersona = Ext.create('Ext.form.Panel', {
        frame:true,
        bodyStyle:'padding:5px 5px 0',
        width: 460,
		height: 490,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 120,
			labelAlign: 'right',
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
            name: 'Id'
        },{
            fieldLabel: 'Documento',
            name: 'Documento',
            allowBlank:false
        },
			comboTiposDocumento,
		{
            fieldLabel: 'Nombres',
            name: 'Nombres',
            allowBlank:false
        },{
            fieldLabel: 'Primer Apellido',
            name: 'PrimerApellido',
            allowBlank:false
        },{
            fieldLabel: 'Segundo Apellido',
            name: 'SegundoApellido'
        },{
            xtype: 'radiogroup',
            fieldLabel: 'Sexo',
            cls: 'x-check-group-alt',
            items: [
                {boxLabel: 'Masculino', name: 'Sexo', inputValue: 'M'},
                {boxLabel: 'Femenino',  name: 'Sexo', inputValue: 'F'}
            ]
        },{
            fieldLabel: 'Fecha Nacimiento',
            name: 'FechaNacimiento',
			format:	'd.m.Y',
			xtype     : 'datefield'
        }, 
			comboClasificaciones,
			comboProfesiones,
            comboMunicipios
		,{
            fieldLabel: 'Barrio',
            name: 'NombreBarrio'
        }
		,{
            fieldLabel: 'Direccion',
            name: 'DireccionResidencia'
        }
		,{
            fieldLabel: 'Telefono',
            name: 'Telefono'
        }
		,{
            fieldLabel: 'Celular',
            name: 'Celular'
        }
		,{
            fieldLabel: 'Mail',
            name: 'Email'
        }
		,{
            fieldLabel: 'Empresa',
            name: 'Empresa'
        }],
		buttons:[{ 
			id:'buttonGuardar',
            text:'Insertar',
            formBind: true,	 
            handler:function(){
				var form = formPersona.getForm();
				var ff = formFoto.getForm();  
				if ( ! form.isValid()) {
					Ext.Msg.alert(' Registre todos los datos por favor -) ');
					return ;
				}	
                var record = form.getFieldValues();
                if(modo=='Insert'){
                	delete record.Id;
                	record.IdTipoDocumento=record.IdTipoDocumento||1;
                	record.IdMunicipio=record.IdMunicipio||0; 
                	record.IdProfesion=record.IdProfesion||0;
                	record.IdClasificacion=record.IdClasificacion||1;
                	record.Sexo=record.Sexo||'M';
                	record.FechaNacimiento = record.FechaNacimiento || new Date(1900,0,0);
                }
				record.FechaNacimiento= convertToUTC(record.FechaNacimiento);
				
				executeAjaxRequest({
					url: url+'/json/asynconeway/Persona'+modo,
					success: function(result) {
						if(modo=='Insert'){
							form.loadRecord(Ext.ModelManager.create(result.Persona, 'Persona') );
							ff.findField('Id').setValue(result.Persona.Id );
							storePersonas.setProxy(new Ext.data.proxy.Memory({
								model:'Persona',
								data: [result.Persona]
							}) );
							storePersonas.load();
							imgPersona.setSrc('../fotos/'+ result.Persona.Id+'.jpg');
						}
						modo='Update';
				    },
					callback: function(result, success) {
						Ext.getCmp('buttonGuardar').setText( (modo=='Insert')?'Insertar':'Actualizar');
						Ext.getCmp('buttonFacturar').setDisabled( (modo=='Insert')?true:false);	
	    			},
					params:record
				})				
			}              
	    }] 
    });

    /*
      ================  Formulario Insertar-Imprimir Factura ===========
    */
    var formFactura = Ext.create('Ext.form.Panel', {
        
        bodyPadding: 10,
        fieldDefaults: {
            labelAlign: 'right',
            labelWidth: 110,
            msgTarget: 'qtip'
        },
        items: [{
            xtype:'hidden',  
            name:'SessionId',  
            value:sessionStorage.id
        },{
            xtype:'hidden',
            name: 'Id'
        },{
            xtype:'hidden',
            name: 'IdPersona'
        },// Contact info
        {
            xtype: 'fieldset',
            title: 'Informacion del Cliente',
            defaultType: 'textfield',
            layout: 'anchor',
            defaults: {
                anchor: '100%'
            },
            items: [{
                xtype: 'fieldcontainer',
                fieldLabel: 'Nombres',
                layout: 'hbox',
                combineErrors: true,
                defaultType: 'textfield',
                defaults: {
                    hideLabel: 'true'
                },
                items: [{
                    name: 'Nombres',
                    submitValue:false,
                    flex: 2,
                    disabled: true
                }]
            }, {
                xtype: 'container',
                layout: 'hbox',
                defaultType: 'textfield',
                items: [{
                    fieldLabel: 'Documento',
                    name: 'Documento',
                    submitValue:false,
                    flex: 1,
                    disabled: true
                }, {
                    fieldLabel: 'Telefono',
                    name: 'Telefono',
                    submitValue:false,
                    flex:1,
                    disabled: true
                }]
            }]
        },{
        	xtype: 'fieldset',
            title: 'Datos de la Factura',
            defaultType: 'textfield',
            layout: 'anchor',
            defaults: {
                anchor: '100%'
            },
            items: [{
            	fieldLabel: 'Numero',
                name: 'Numero',
                readOnly :true,
                height:35,
                readOnlyCls :'x-item-readonly-numero-factura'
            },
            {
            	xtype:'datefield',
            	fieldLabel: 'Fecha Expedicion',
                name: 'FechaPago',
                format:	'd.m.Y',
                value: new Date(),
                submitValue:false,
                disabled: true
            },
            comboTiposPago,
            {
            	xtype: 'numberfield',
            	fieldLabel: 'Cantidad',
                name: 'Cantidad',
                value: 1,
                minValue: 1,
                listeners: {
           			change: function(field, value) {
           				var ff = formFactura.getForm();
           				var vu = unFormatString( ff.findField('ValorUnitario').getValue() );
            			ff.findField('ValorTotal').setValue( vu*value);
                	}
        		}
            },{
            	xtype:'datefield',
            	fieldLabel: 'Fecha Inicio',
                name: 'FechaInicio',
                format:	'd.m.Y',
                value: new Date(),
                listeners   : {  
		            select: function( control, newValue, oldValue, options){
		            	var ff = formFactura.getForm();
		            	var tp =comboTiposPago.getValue() || 1;
            			var rs = storeTiposPago.getById(tp);
		            	var record = rs.data;
		            	var cantidad = ff.findField('Cantidad').getValue();
		            	var fi = newValue
		            	ff.findField('ValorUnitario').setValue(record.Valor);
		            	ff.findField('ValorTotal').setValue( cantidad * record.Valor);
		            	ff.findField('FechaTerminacion').setValue( fechaTerminacion(fi,  cantidad*record.Dias ) );
		            } 
		        }
            },{
            	fieldLabel: 'Observacion',
                name: 'Observacion'
            }]
        },{
        	xtype: 'fieldset',
        	cls:'factura-info',
            title: 'Valores',
            defaultType: 'textfield',
            layout: 'anchor',
            defaults: {
                anchor: '100%'
            },labelWidth: 110,
            items: [{
            	xtype:'datefield',
            	fieldLabel: 'Fecha Terminacion',
                name: 'FechaTerminacion',
                format:	'd.m.Y',
                value: new Date(),
                submitValue:false,
                disabled: true,
                disabledCls:'x-item-disabled-fecha-terminacion',
                height:40,
                labelWidth: 220
            },{
                fieldLabel: 'Valor Unitario',
                name: 'ValorUnitario',
                submitValue:false,
                disabled: true,
                disabledCls:'x-item-disabled-valor-unitario',
                height:30,
                labelWidth: 220,
                listeners: {
					change: function(object, newValue, oldValue){
						newValue = formatString(newValue) ;
						object.setValue(newValue);
					}
                }
            },{
                fieldLabel: 'Total',
                name: 'ValorTotal',
                submitValue:false,
                disabled: true,
                disabledCls:'x-item-disabled-valor-total',
                height:35,
                labelWidth: 220,
                listeners: {
					change: function(object, newValue, oldValue){
						newValue = formatString(newValue) ;
						object.setValue(newValue);
					}
                }
            }]
        }],
		buttons:[{
	    	id:'buttonGuardarFactura',
            text:'Guardar',
            formBind: true,	 
            handler:function(){
            	var form = this.up('form').getForm();
            	if ( ! form.isValid()) {
					Ext.Msg.alert(' Registre todos los datos por favor -) ');
					return ;
				}
            	var record = form.getFieldValues();
            	delete record.Id;     				               
				record.FechaInicio= convertToUTC(record.FechaInicio);
				executeAjaxRequest({
					url: url+'/json/asynconeway/FacturaPagoInsert', 
				    success: function(result) {
						var nr = Ext.ModelManager.create(result.Factura, 'Factura') ;
						form.loadRecord(nr);
						storeFacturas.add(nr);
						gridFacturas.getSelectionModel().doSingleSelect(nr,false);
						Ext.getCmp('buttonGuardarFactura').setDisabled( true );
						if(result.PrintSuccess){
							verVentanaDescarga(result.Factura.Id,
							result.MailSuccess?
							Ext.String.format('Correo enviado exitosamente a:<br />"{0}"', formPersona.getForm().findField("Email").getValue() ):
							Ext.String.format('<div style="color:#FF0000;">Error al Enviar Correo:<br />{0}</div>',result.MailMessage ) );
						}
						else{
							Ext.shared.msg('Error al Generar la Factura', result.PrintMessage + ' -( ');
						}
				    },
					params:record
				});            	
			}              
	    }] 
       
    });
    
    var panelModulo = Ext.create('Ext.Panel', {
        id:'panelModulo',
        baseCls:'x-plain',
        renderTo: 'modulo',
        layout: {
            type: 'table',
            columns: 2
        },
        items:[
			formDocumento,
			gridPersonas,
			formPersona,
			{
				xtype:'panel',
				width:400,
				height: 490, 
				layout: {
       				type: 'vbox',       // Arrange child items vertically
        			align: 'center',    
        			padding: 5
    			},
				items:[
					imgPersona,
					formFoto,
					gridFacturas
				]	
			}
		]
    });

    var windowFacturacion = Ext.create('Ext.Window',{
    	 closable: true,
         closeAction: 'hide',
         width: 800,
         height: 600,
         items:[
         	formFactura
         ]
    });  
    
    
    var formSuspensionInsert = Ext.create('Ext.form.Panel', {
    	frame:true,
        bodyStyle:'padding:5px 5px 0',
        width: 350,
        height: 250,
		fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 120,
			labelAlign: 'right',
        },
        defaults: {
            anchor: '100%',
			labelStyle: 'padding-left:4px;'
        },
        items: [{
            xtype:'hidden',  
            name:'SessionId',  
            value:sessionStorage.id
        },{
            fieldLabel: 'Suspender Desde',
            name: 'Desde',
			format:	'd.m.Y',
			xtype     : 'datefield',
			allowBlank: false
        },{
            fieldLabel: 'Suspender Hasta',
            name: 'Hasta',
			format:	'd.m.Y',
			xtype     : 'datefield',
			allowBlank: false
        }],
		buttons:[{ 
			id:'buttonGuardarSuspension',
            text:'Insertar',
            formBind: true,	 
            handler:function(){
				var form = formSuspensionInsert.getForm();
				
				if ( ! form.isValid()) {
					Ext.Msg.alert(' Registre todos los datos por favor -) ');
					return ;
				}	
                
                var record = form.getFieldValues();        
				record.Desde= convertToUTC(record.Desde);
				record.Hasta= convertToUTC(record.Hasta);
				record.IdPago = gridFacturas.getSelectionModel().getSelection()[0].getId();
				executeAjaxRequest({
					url: url+'/json/asynconeway/SuspensionInsert',
					success: function(result) {
                        var nr = Ext.ModelManager.create(result.Suspension, 'Suspension') ;
						storeSuspensiones.add(nr);
						gridSuspensiones.getSelectionModel().doSingleSelect(nr,false);
						
						var ur = storeFacturas.getById(parseInt( record.IdPago) );
						ur.beginEdit();
						var fr = result.Factura;
						for( var r in ur.data){
							ur.set(r, fr[r])
						}
						ur.endEdit();
						ur.commit();
						
					},
					callback: function(result, success){
						windowSuspensionInsert.hide();
					},
					params:record
				});			
			}              
	    }] 
       
    });
       
    var windowSuspensiones = Ext.create('Ext.Window',{
    	modal:true,
    	closable: true,
        closeAction: 'hide',
        width: 440,
        height: 250,
        layout:'fit',
        items:[
         	gridSuspensiones
         ]
    });   
    
    var windowSuspensionInsert= Ext.create('Ext.Window',{
    	modal:true,
    	closable: true,
        closeAction: 'hide',
        width: 380,
        height: 270,
        layout:'fit',
        items:[
         	formSuspensionInsert
         ]
    });    
    
});
