/////////////////////////////////
// File Name: mBanner.js       //
// By: Manish Kumar Namdeo     //
/////////////////////////////////

// BANNER OBJECT
function Banner(objName)
   {
	this.obj = objName;
	this.aNodes = [];
	this.currentBanner =0;
	};

// ADD NEW BANNER
Banner.prototype.add = function(bannerType, bannerPath, bannerDuration, height, width, hyperlink,target,type,bannerid,bannername) {
this.aNodes[this.aNodes.length] = new Node(this.obj + "_" + this.aNodes.length, bannerType, bannerPath, bannerDuration, height, width, hyperlink, target, type, bannerid, bannername);
};

// Node object
function Node(name, bannerType, bannerPath, bannerDuration, height, width, hyperlink, target, type, bannerid, bannername) {
	this.name = name;
	this.bannerType = bannerType;
	this.bannerPath = bannerPath;
	this.bannerDuration = bannerDuration;
	this.height = height
	this.width = width;
	this.hyperlink= hyperlink;
	this.target=target;
	this.type=type;
	this.bannerid = bannerid;
	this.bannername = bannername;
	//alert (name +"|" + bannerType +"|" + bannerPath +"|" + bannerDuration +"|" + height +"|" + width + "|" + hyperlink+"|"+target);
};
// Outputs the banner to the page
Banner.prototype.toString = function() {
	var str = ""
	
	//alert(this.aNodes.length);
	for (var iCtr=0; iCtr < this.aNodes.length; iCtr++) //old: iCtr=0
	    { 
	    //alert(pos);
		str = str + '<span name="'+this.aNodes[iCtr].name+'" '
		str = str + 'id="'+this.aNodes[iCtr].name+'" ';
		str = str + 'class="m_banner_hide" ';
		str = str + 'bgcolor="#FFFCDA" ';	// CHANGE BANNER COLOR HERE
		str = str + 'align="center" ';
		str = str + 'valign="top" >\n';
		if (this.aNodes[iCtr].hyperlink != ""){
			//str = str + '<a href="'+this.aNodes[iCtr].hyperlink+'" target="_blank">';
			//alert('str:'+str);
		    str = str + '<a  style=\"line-height: 0em;\" onclick="countVisits(\'' + this.aNodes[iCtr].bannerid + '&type=' + this.aNodes[iCtr].type + '\')" href="' + this.aNodes[iCtr].hyperlink + '" target="' + this.aNodes[iCtr].target + '">';
			
		}
		if ( this.aNodes[iCtr].bannerType == "TEXT" )
		{
			str=str+ '<iframe width="'+this.aNodes[iCtr].width+'" height="'+this.aNodes[iCtr].height+'" src="'+this.aNodes[iCtr].bannerPath+'" marginwidth="0" marginheight="0" scrolling="no" frameborder="0"></iframe> ';
		}	
		if ( this.aNodes[iCtr].bannerType == "FLASH" ){
		    str = str + '<a  style=\"line-height: 0em;\" onclick="countVisits(\'' + this.aNodes[iCtr].bannerid + '&type=' + this.aNodes[iCtr].type + '\');"><object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" '
			//str=str+ 'codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0" '
			str=str+ 'width="'+this.aNodes[iCtr].width+'" '
			str=str+ 'height="'+this.aNodes[iCtr].height+'" '
			str=str+ 'id="bnr_'+this.aNodes[iCtr].name+'" align="middle" VIEWASTEXT> '
			str=str+ '<param name="allowScriptAccess" value="always" />'
			str=str+ '<param name="movie" value="'+this.aNodes[iCtr].bannerPath+'" />'
			//str=str+ 'param name="FlashVars" value="'+vars+'" />'
			str=str+ '<param name="wmode" value="transparent" />'
			//str=str+ '<param name="wmode" value="" />'
			str=str+ '<param name="menu" value="false" />'
			str=str+ '<param name="quality" value="high" />'
			str=str+ '<embed src="'+this.aNodes[iCtr].bannerPath+'" wmode="transparent" menu="false" quality="high" width="'+this.aNodes[iCtr].width+'" height="'+this.aNodes[iCtr].height+'" name="bnr_'+this.aNodes[iCtr].name+'" allowScriptAccess="always" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />'
			//str=str+ '<embed src="'+this.aNodes[iCtr].bannerPath+'" wmode="" menu="false" quality="high" width="'+this.aNodes[iCtr].width+'" height="'+this.aNodes[iCtr].height+'" name="bnr_'+this.aNodes[iCtr].name+'" allowScriptAccess="always" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />'
			str=str+ '</embed></object></a>'
			
		}else if ( this.aNodes[iCtr].bannerType == "IMAGE" ){
			str = str + '<img src="'+this.aNodes[iCtr].bannerPath+'" ';
			str = str + 'alt="' + this.aNodes[iCtr].bannername + '" title="' + this.aNodes[iCtr].bannername + '" border="0" ';
			str = str + 'height="'+this.aNodes[iCtr].height+'" ';
			str = str + 'width="'+this.aNodes[iCtr].width+'">';
		}

		if (this.aNodes[iCtr].hyperlink != ""){
			str = str + '</a>';
		}

		str += '</span>';
			}
			//alert('str :'+str);
	document.write(str);
str="";
return str;
};

// START THE BANNER ROTATION
Banner.prototype.start = function(){	
	this.changeBanner();
	var thisBannerObj = this.obj;
	// CURRENT BANNER IS ALREADY INCREMENTED IN changeBanner() FUNCTION
	setTimeout(thisBannerObj+".start()", this.aNodes[this.currentBanner].bannerDuration * 1000);	

}

// CHANGE BANNER
Banner.prototype.changeBanner = function(){
	var thisBanner;
	var prevBanner = -1;
	if (this.currentBanner>this.aNodes.length-1)
	{
		this.currentBanner=0;
	}
	if (this.currentBanner < this.aNodes.length ){
		thisBanner = this.currentBanner;
		if (this.aNodes.length > 1){
			if ( thisBanner > 0 ){
				prevBanner = thisBanner - 1;
			}else{
				prevBanner = this.aNodes.length-1;
			}
		}
		if (this.currentBanner < this.aNodes.length - 1){
			this.currentBanner = this.currentBanner + 1;
		}else{
			this.currentBanner = 0;
		}
	}
	

	if (prevBanner >= 0){
		document.getElementById(this.aNodes[prevBanner].name).className = "m_banner_hide";
	}
	document.getElementById(this.aNodes[thisBanner].name).className = "m_banner_show";
}
function test()
{
	alert('test');
}

var ms_XMLHttpRequest_ActiveX = "";
var req;
var pid=0;

function getObj(name) 
{   if (document.getElementById) { return document.getElementById(name); }
	else if (document.all)       { return document.all[name]; }
	else if (document.layers)    { return document.layers[name]; }
}
function retrieveURL(url) 
{	if (window.XMLHttpRequest) // branch for native(FireFox) XMLHttpRequest object
	{	req = new XMLHttpRequest();
	} else if (window.ActiveXObject) // branch for IE/Windows ActiveX version
	{   if (ms_XMLHttpRequest_ActiveX != "")
		{	req = new ActiveXObject(ms_XMLHttpRequest_ActiveX);
        } else 
		{	var versions = ["Msxml2.XMLHTTP.7.0", "Msxml2.XMLHTTP.6.0", "Msxml2.XMLHTTP.5.0", "Msxml2.XMLHTTP.4.0", "MSXML2.XMLHTTP.3.0", "MSXML2.XMLHTTP", "Microsoft.XMLHTTP"];
            for (var i=0; i<versions.length ; i++)
			{	try
				{	req = new ActiveXObject(versions[i]);
					if (req!=null)
					{	ms_XMLHttpRequest_ActiveX = versions[i];
                        break;
                    }
                }
                catch(objException) {;}
            }
        }
	}
	if (req!=null)
	{	req.onreadystatechange = processReqChange;
		req.open("GET", url, true);
		
		req.setRequestHeader('User-Agent', 'XMLHttpRequest');
		req.send(null);
	}else
	{	alert("Browse khong support");
	}
}

function processReqChange() 
{	if (req.readyState == 4) // only if req shows "complete"
	{	if (req.status == 200) // only if "OK"
		{	
			/*var htmlValue = req.responseText;
			getObj('lbgetvideo').innerHTML = htmlValue;				
			if(htmlValue.indexOf('.flv')>=0)
			{
				var pos1 = htmlValue.lastIndexOf('pos');				
				var pos2 = htmlValue.lastIndexOf('flv');				
				var file=htmlValue.substring(pos1+3,pos2) + "flv";				
				createplayer(file, false);
			}*/
		} 
		else {	
	//	getObj('getContentAjax').innerHTML='Loading ... <IMG src="./Images/public/spinner.gif" border="0">'
			//alert("There was a problem retrieving data");
			
		}
	}
}
function countVisits(_value)
	{//
  	//retrieveURL(url+what); //("index.php?cid=1&l=2&f=3&ptid="+what); 	
  var requestURL = "/Controls_Public/Ajax/CountAdv.aspx?bid="
		var url = requestURL +_value ; 
		//alert('URL:'+url);
	retrieveURL(url);

    }
    function parseURL(strParamName)
	{ var strReturn = "";
	var strHref = window.location.search;//window.location.href;
		if ( strHref.indexOf("?") > -1 )
				{ 
					var strQueryString = strHref.substr(1);
					var aQueryString = strQueryString.split("&");
						for (var i=0; i<aQueryString.length; i++)
							{ var aParam = aQueryString[i].split("=");
								if (aParam[0]==strParamName)
									{ strReturn = aParam[1];
									break;
										}
							}
				}
		return unescape(strReturn);
	}	