// gpl version 3.0
Ext.require([
    'Ext.form.*',
    'Ext.layout.container.Column',
    'Ext.tab.Panel',
	'Ext.window.MessageBox',
    'Ext.tip.*'
]);


Ext.onReady(function(){

    Ext.QuickTips.init();
    var url= location.protocol + "//" + location.host+'/autenticacion/';
 	
    /*
    * ================   form de logeo  =======================
    */

    var simple = Ext.create('Ext.form.Panel', {
        frame:true,
        title: 'Registro....',
        bodyStyle:'padding:5px 5px 0',
        width: 350,
        fieldDefaults: {
            msgTarget: 'side',
            labelWidth: 75
        },
        defaultType: 'textfield',
        defaults: {
            anchor: '100%'
        },

        items: [{
            fieldLabel: 'Usuario',
            name: 'UserName',
            allowBlank:false
        },{
            fieldLabel: 'Clave',
            name: 'Password',
			inputType:'password' ,
            allowBlank:false
        }],

		buttons:[{ 
            text:'Login',
            formBind: true,	 
            handler:function(){
				sessionStorage.removeItem("id");
				sessionStorage.removeItem("grupos");
				sessionStorage.removeItem("actividades");
				sessionStorage.removeItem("usuario");
				var form = simple.getForm();     
				var record =  form.getFieldValues();
				if ( ! form.isValid()) {
					Ext.Msg.alert(' Registre todos los datos por favor -) ');
					return ;
				}	
				
       			executeAjaxRequest({
					url: url+'Json/SyncReply/LoginData',
	                method:'POST', 
				    success: function(result) {
						sessionStorage["id"]=result.Id;
						sessionStorage["grupos"]=Ext.encode(result.Grupos);
						sessionStorage["actividades"]=Ext.encode(result.Actividades);
						sessionStorage["usuario"]= record.UserName;
						window.location = record.UserName=='registro.bp'?'ingreso' :'menu';
				    },
				    params:record
       			});					
			}              
	    }] 
       
    });

    simple.render(document.body);

    });




