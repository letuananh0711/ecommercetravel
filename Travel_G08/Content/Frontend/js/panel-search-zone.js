//Change panel
function getObjby(name)
{   if (document.getElementById)
	{	return document.getElementById(name);
	}else if (document.all)
	{	return document.all[name];
	}else if (document.layers)
	{	return document.layers[name];
	}
}
function setPanel(ucid,_value)
{	var Obj=ucid;
	var Field=getObjby(Obj);
	if(Field!=null)
	{
	Field.removeAttribute('style');
	Field.style.display=_value;
	Field.setAttribute('style','display:'+_value);
	
	}
	else{
	alert('Can not find:'+Obj);
	}
}
function selectPanel(_value)
{
	if(_value=='searchtour')
		{
	
	//getObjby('_pnl_tour').style.backgroundColor='#FFD50D';
	//getObjby('_pnl_search').style.backgroundColor='#444444';
		}
   else{
	
	//getObjby('_pnl_search').style.backgroundColor='#FFD50D';
	//getObjby('_pnl_tour').style.backgroundColor='#444444';
    }
    switch (_value) {
        case 'searchtour':
            setPanel('panelSearchTour', 'block');
            setPanel('panelSearchBooking', 'none');
            setPanel('panelSearchScore', 'none');
			setPanel('panelSearchTransport', 'none');
            getObjby('_pnl_tour').className = 'borderTR';
            getObjby('_pnl_search').className = 'borderB';
            getObjby('_pnl_score').className = 'borderB';
			getObjby('_pnl_transport').className = 'borderB';
            break;
        case 'searchbooking':
            setPanel('panelSearchTour', 'none');
            setPanel('panelSearchBooking', 'block');
            setPanel('panelSearchScore', 'none');
			setPanel('panelSearchTransport', 'none');
            getObjby('_pnl_tour').className = 'borderB';
            getObjby('_pnl_search').className = 'borderTR';
            getObjby('_pnl_score').className = 'borderB';
			getObjby('_pnl_transport').className = 'borderB';
            break;
        case 'searchscore':
            setPanel('panelSearchTour', 'none');
            setPanel('panelSearchBooking', 'none');
            setPanel('panelSearchScore', 'block');
			setPanel('panelSearchTransport', 'none');
            getObjby('_pnl_tour').className = 'borderB';
            getObjby('_pnl_search').className = 'borderB';
            getObjby('_pnl_score').className = 'borderTR';
			getObjby('_pnl_transport').className = 'borderB';
            break;
		case 'searchtransport':
            setPanel('panelSearchTour', 'none');
            setPanel('panelSearchBooking', 'none');
            setPanel('panelSearchScore', 'none');
			setPanel('panelSearchTransport', 'block');
            getObjby('_pnl_tour').className = 'borderB';
            getObjby('_pnl_search').className = 'borderB';
            getObjby('_pnl_score').className = 'borderB';
			getObjby('_pnl_transport').className = 'borderTR';
            break;
    }	
}
/*
function defaultInit()
{
    var cidd = '0';
//try{cidd=parseURL('cid');}catch(err){cidd=0;}	
	if(cidd==33)
		{
		setPanel('panelSearchTour','none');
		setPanel('panelSearchBooking','block');		
		}
		else{
		setPanel('panelSearchTour','block');
		setPanel('panelSearchBooking','none');		
		}
}
if (window.attachEvent){ window.attachEvent("onload",defaultInit);}
else{
defaultInit();
}

*/

//Search box
function getObjby(name)
	{   
		if (document.getElementById)
		{	return document.getElementById(name);
		}else if (document.all)
		{	return document.all[name];
		}else if (document.layers)
		{	return document.layers[name];
		}
	}
	function SetDiemDen(ucid,_value)
	{	var Obj=ucid;
		var Field=getObjby(Obj);
		if(Field!=null)
		{
			Field.removeAttribute('style');
			Field.style.display=_value;
			Field.setAttribute('style','display:'+_value);
		}
		else{
			alert('Can not find:'+Obj);
		}
	}
	function selectDiemDen(_value)
	{
		if(_value=='TN')
			{
				SetDiemDen('diemDenTN','block');
				SetDiemDen('giaDiemDenTN','block');
				SetDiemDen('diemDenNG','none');
				SetDiemDen('giaDiemDenNG','none');
			}
	   else{
			SetDiemDen('diemDenTN','none');
			SetDiemDen('giaDiemDenTN','none');
			SetDiemDen('diemDenNG','block');
			SetDiemDen('giaDiemDenNG','block');
		}
	}

	
