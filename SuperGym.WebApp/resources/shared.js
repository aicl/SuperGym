Ext.shared = function(){
    var msgCt;

    function createBox(t, s){
       return '<div class="msg"><h3>' + t + '</h3><p>' + s + '</p></div>';
    }
    return {
        msg : function(title, format){
            if(!msgCt){
                msgCt = Ext.core.DomHelper.insertFirst(document.body, {id:'msg-div'}, true);
            }
            var s = Ext.String.format.apply(String, Array.prototype.slice.call(arguments, 1));
            var m = Ext.core.DomHelper.append(msgCt, createBox(title, s), true);
            m.hide();
            m.slideIn('t').ghost("t", { delay: 1000, remove: true});
        },

        init : function(){
        }
    };
}();


// my custom functions

function convertToDate(v){
	if (!v) return null;
	
	return (typeof v == 'string')
	?new Date(parseFloat(/Date\(([^)]+)\)/.exec(v)[1]))
	:v ;   
}

function convertToUTC(date){
	return Ext.Date.format(date,'MS');
}

function formatString(value){
	if( typeof(value) =='number') value= value.toString();
	return  Ext.util.Format.currency(value.replace(/[^0-9\.]/g, ''));
}

function formatCurrency(value){
	return  Ext.util.Format.currency(value);
}

function unFormatString(value){
	return  value.replace(/[^0-9\.]/g, '');
}

function fechaTerminacion( date,  dias){
	dias= parseInt(dias);
	if(dias<30)
		return Ext.Date.add(date, Ext.Date.DAY,dias-1);
	var meses = parseInt( dias/30 );
	var residuo = parseInt( dias%30 );
	return Ext.Date.add( Ext.Date.add( Ext.Date.add(date, Ext.Date.MONTH, meses), Ext.Date.DAY, -1),
		Ext.Date.DAY, residuo); 		
}

function esHoy(fecha){
	var d ;
	if( typeof fecha=='object') d= Ext.Date.format(fecha,'d.m.Y');
	else d= Ext.util.Format.substr(fecha,0,10);
	
	var hoy = Ext.Date.format(new Date() ,'d.m.Y');
	return hoy==d;
}


function executeAjaxRequest(config){
	
	Ext.MessageBox.show({
		msg: config.msg || 'Por favor espere...',
		progressText: config.progessText || 'Ejecutado...',
        width: config.width || 300,
		wait:true,
        waitConfig: {interval:200}
   		//icon:'ext-mb-download' //custom class in msg-box.html
	});
	
	Ext.Ajax.request({
		url: config.url + (config.format==undefined 
			? ( Ext.util.Format.uppercase(config.method)=='DELETE'? paramsToUrl(config.params) :'' ) 
			:config.format),
        method: config.method, 
	    success: function(response, options) {
            var result = Ext.decode(response.responseText);
			if(result.ResponseStatus.ErrorCode){
				Ext.shared.msg('Error', result.ResponseStatus.Message + ' -( '); 
				return ;
			}
			if(config.showReady) Ext.shared.msg('Listo', result.ResponseStatus.Message);
			if( config.success ) config.success(result);
			
	    },
	    failure: function(response, options) {
            var result = Ext.decode(response.responseText);
			Ext.shared.msg('Error en el Servidor', response.status + ' -( ');
			Ext.shared.msg('Error' , ( (result.ResponseStatus)? result.ResponseStatus.Message:'Indefinido') + ' -( ');
			if( config.failure ) config.failure(result);
		},
		callback: function(options, success, response) {
			Ext.MessageBox.hide();	
			var result = Ext.decode(response.responseText);
			if( config.callback ) config.callback(result, success);
		},
		params:config.params
	});	
	
}

function paramsToUrl(params){
	var s='';
	for( p in params){
		s= Ext.String.urlAppend(s, Ext.String.format('{0}={1}', p, params[p]));
	};
	return s;
	
}


function executeRestRequest(config){
	config.format= Ext.String.format('?format={0}', config.format|| 'json');
	executeAjaxRequest(config)
}

function createAjaxProxy(config){
	return{
		type: 'ajax',
		url : config.url,
    	reader: {
			type: config.type|| 'json',
        	root: config.root ,
			totalProperty  	: config.totalProperty ||   'Total',
    		successProperty	: config.successProperty || 'Success',
			messageProperty : config.messageProperty || 'ResponseStatus.Message'
		},
		extraParams: config.extraParams || {SessionId:sessionStorage.id},
		pageParam: undefined,
		limitParam: undefined,
		startParam: undefined
    };
}


function logout(redirect){
	
	
	if(sessionStorage.id){
    	executeAjaxRequest({
			url: location.protocol + "//" + location.host+'/autenticacion/'+'Json/SyncReply/LogoutData',
	        method:'POST', 
	        callback: function(result, success){
	    		sessionStorage.clear();
	    		window.location=redirect || '../'
	        },
		    params: {SessionId:sessionStorage.id}
		});			
    }
	else{
		window.location=redirect || '../'	
	}
	
}

function checkActividad(actividad){
	var a= Ext.decode(sessionStorage.actividades);
	return a.indexOf(actividad)>0?true:false;
}

function checkEmail(email) {
	var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
	return filter.test(email)

}

