Ext.require([
    'Ext.form.*',
    'Ext.layout.container.Column',
    'Ext.tab.Panel',
	'Ext.window.MessageBox',
    'Ext.tip.*'
]);
 
Ext.onReady(function(){

    Ext.QuickTips.init();
    var url= location.protocol + "//" + location.host+'/servicio';
    
	/*
	* ==========  Modelos ==========================
	*/ 
	    
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
        	{name: "IdUsuarioRegistra", type: 'int' }
        ]
    });

	
	var imgPersona = Ext.create('Ext.Img', {
    	src: '../resources/icons/fam/user.png',
    	width:150, height: 200
    });
    

    /*
	  ==========  Formulario para Pedir Documento ==================
	*/ 

	var formDocumento = Ext.create('Ext.form.Panel', {
        frame:true,
        bodyStyle:'padding:5px 5px 0',
        colspan:2,
        width: 450,
		//height: 115,
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
            fieldLabel: 'Digite su Documento',
            name: 'Documento',
            allowBlank:false
        }],
		buttons:[{ 
            text:'Ingresar',
            formBind: true,	 
            handler:function(){
            	
            	var form = formDocumento.getForm();            
				if ( ! form.isValid()) {
					Ext.Msg.alert(' Debe Indicar el Numero de Documento por Favor -) ');
					return ;
				}
            	
				var fp =formPersona.getForm();
				var ff =formFactura.getForm()
				var fu = formUltimoIngreso.getForm();
				fp.reset();
				ff.reset();
				fu.reset();
				imgPersona.setSrc('../resources/icons/fam/user.png');
				var record = form.getValues()  ;
				
				executeAjaxRequest({
					url: url+ '/json/syncreply/PersonaValidar',
					success: function(result) {
                        
						fp.loadRecord(Ext.ModelManager.create(result.Persona, 'Persona') );
						ff.loadRecord(Ext.ModelManager.create(result.Factura, 'Factura') );
						var ui=fu.findField('UltimoIngreso');
						ui.setValue(result.UltimoIngreso)
						ui.setFieldStyle( esHoy(result.UltimoIngreso)
							?'{color:red; font-size:20px;  text-align:center; }'
							:'{color:blue; font-size:20px; text-align:right; }' );												
						imgPersona.setSrc('../fotos/'+ result.Persona.Id+'.jpg');	
				    },
				    callback:function(result, success){
				    	if(result.ResponseStatus.ErrorCode && success && result.Persona){
							fp.loadRecord(Ext.ModelManager.create(result.Persona, 'Persona') );
							imgPersona.setSrc('../fotos/'+ result.Persona.Id+'.jpg');
							return ;
						}
				    },
					params:record	
				})
			}              
	    }] 
       
    });

    /*
      ================  Formulario Datos Basico Persona ===========
    */

    var formPersona = Ext.create('Ext.form.Panel', {
        frame:true,
        bodyStyle:'padding:5px 5px 0',
        width: 300,
		height: 200,
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
            fieldLabel: 'Nombres',
            name: 'Nombres',
            readOnly:true
        },{
            fieldLabel: 'Primer Apellido',
            name: 'PrimerApellido',
            readOnly:true
        },{
            fieldLabel: 'Segundo Apellido',
            name: 'SegundoApellido',
            readOnly:true
        }],
		buttons:[{ 
			id:'buttonLimpiar',
            text:'Limpiar Informacion',
            formBind: true,	 
            handler:function(){
				formPersona.getForm().reset();
				formFactura.getForm().reset();
				formUltimoIngreso.getForm().reset();
			}              
	    }] 
       
    });

    /*
      ================  Formulario datos basicos Factura ===========
    */
    var formFactura = Ext.create('Ext.form.Panel', {
    	colspan:2,
      	width: 450,
        bodyPadding: 10,
        fieldDefaults: {
            labelAlign: 'right',
            labelWidth: 110,
            msgTarget: 'qtip'
        },
        items: [{
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
            },{
            	xtype:'datefield',
            	fieldLabel: 'Fecha Expedicion',
                name: 'FechaPago',
                format:	'd.m.Y',
                readOnly :true
            },{
            	xtype:'datefield',
            	fieldLabel: 'Fecha Inicio',
                name: 'FechaInicio',
                format:	'd.m.Y',
                height:35,
                readOnly :true
            },{
            	xtype:'datefield',
            	fieldLabel: 'Fecha Terminacion',
                name: 'FechaTerminacion',
                format:	'd.m.Y',
                height:40,
                labelWidth: 220,
                readOnly :true
            },{
            	fieldLabel: 'Observacion',
                name: 'Observacion',
                readOnly :true
            }]
        }]
		
    });

    var formUltimoIngreso = Ext.create('Ext.form.Panel', {
    	colspan:2,
      	width: 450,
      	bodyStyle:'padding:5px 5px 0',
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
            fieldLabel: 'Ultimo Ingreso',
            name: 'UltimoIngreso',
            readOnly:true
        }]   	
      	
    });
     
    
    
    var panelModulo = Ext.create('Ext.Panel', {
        id:'panelModulo',
        //frame:true,
        baseCls:'x-plain',
        renderTo: 'modulo',
        layout: {
            type: 'table',
            columns: 2
        },
        items:[
			formDocumento,
			imgPersona,
			formPersona,
			formFactura,
			formUltimoIngreso
		]
    });

            
    
});


