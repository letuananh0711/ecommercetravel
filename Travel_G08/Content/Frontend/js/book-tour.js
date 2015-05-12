var ent; // Global var is declared here so we can access in both functions
// This shows the pop up window next to the calling element
function show_text(t, dname) 
{
	var xp, yp, op
	xp = dname.offsetLeft; // Element's offset x in pixels
	yp = dname.offsetTop; // Element's offset y in pixels
	// Now loop through all parent containers, adding offsets as we do so
	while (dname.offsetParent) 
	{
		op = dname.offsetParent; // Get container parent
		xp = xp + op.offsetLeft; // Add this element's offset x in pixels
		yp = yp + op.offsetTop; 	// Add this element's offset y in pixels
		dname = dname.offsetParent; // Update current container
	}
	var newdiv = document.createElement('div');
	newdiv.setAttribute('id', "ent");
	document.body.appendChild(newdiv);
	ent = document.getElementById("ent")	// Get the main element
	if (ent) 
	{
		// Change these to customise your popup window
		ent.style.color = "#000000";
		ent.style.padding = "2px 3px 2px 3px";
		ent.style.background = "#eee";
		ent.style.border = "1px solid #0066cb";
		// Don't, however, change these
		ent.style.position = 'absolute';
		ent.style.left = (xp + 10) + "px";
		ent.style.top = (yp + 25) + "px";
		ent.innerHTML = t;
		ent.style.display = "block";
	}
}

// This clears the pop up element
function clear_text(dname) 
{
	ent = document.getElementById("ent");
	if (ent) 
	{
		document.body.removeChild(ent);
	}
}

function setValue() {
	var TongSoKhach;
	var TongSoKhachNguoiLon;
	var TongSoKhachTreEm;
	var TongSoKhachTreNho;
	
	//TongSoKhach =document.tour_booking.txt_tongSoKhach.value;
	TongSoKhachNguoiLon=document.tour_booking.txt_soNguoiLon.value;
	TongSoKhachTreEm=document.tour_booking.txt_soTreEm.value;
	TongSoKhachTreNho=document.tour_booking.txt_soTreNho.value;
	var TongSoKhach = parseInt(TongSoKhachNguoiLon) + parseInt(TongSoKhachTreEm) + parseInt(TongSoKhachTreNho);
	
	if(TongSoKhach <= 0) {
		alert("We need at least 1 person.");
		document.tour_booking.txt_soNguoiLon.value = 1;
		document.tour_booking.txt_tongSoKhach.value = 1;
	}
	else {
		document.tour_booking.txt_tongSoKhach.value = TongSoKhach;
		var table = document.getElementById("tableCustomer");
		var rowCount = table.rows.length - 5;
		if(rowCount < TongSoKhach) {
			//alert("add");
			var n = TongSoKhach - rowCount;
			addRow("tableCustomer", n);
		}
		else {
			if(rowCount == TongSoKhach) {
				//alert("không đổi");
			}
			else {
				var n = rowCount - TongSoKhach;
				deleteRow("tableCustomer", n);
				//alert("delete");
			}
		}
	}
}
	
function addRow(tableID, n) {
	var table = document.getElementById(tableID);
	var rowCount = table.rows.length;
	
	var colCount = table.rows[2].cells.length;
	
	for(var j=0; j< n; j++) {
		var row = table.insertRow(rowCount - 2);
		for(var i=0; i< colCount; i++) {
			var newcell = row.insertCell(i);
			newcell.innerHTML = table.rows[2].cells[i].innerHTML;
			switch(newcell.childNodes[0].type) {
				case "text":
					newcell.childNodes[0].value = "";
					break;
				case "checkbox":
					newcell.childNodes[0].checked = false;
					break;
				case "select-one":
					newcell.childNodes[0].selectedIndex = 0;
					break;
			}
		}
	}
}

function deleteRow(tableID, n) {
	try {
		var table = document.getElementById(tableID);
		for(var j=0; j< n; j++) {
			var rowCount = table.rows.length;
			table.deleteRow(rowCount - 3);
		}
	}
	catch(e) {
		alert(e);
	}
}

var error;
var flag;

function CheckNull(str, row, col) {
	if(str.length == 0) {
		flag[row][col] = true;
		error[row][col] = "We need a name.";
		return true;	
	}
}

function CheckName(str, row, col) {
	var iChars = "1234567890~`!@#$%^&*()+=[]\\\';,./{}|\":<>?";

	if(CheckNull(str, row, col)) {
		return true;	
	}

	for (var i = 0; i < str.length; i++)	{
		if (iChars.indexOf(str.charAt(i)) != -1) {
			flag[row][col] = true;
			error[row][col] = "Cannot contain any special character.";
			return;
		}
	}
	
	flag[row][col] = false;
	error[row][col] = "";
}

function CheckAddressClient(str, row, col) {
	var iChars = "~`!@#$%^&*()+=[]\\\';,.{}|\":<>?";
	
	for (var i = 0; i < str.length; i++) {
		if (iChars.indexOf(str.charAt(i)) != -1) {
			flag[row][col] = true;
			error[row][col] = "Cannot contain any special character.";
			return;
		}
	}
	
	flag[row][col] = false;
	error[row][col] = "";
}

function CheckBirthdayClient(str, row, col) {
	var iChars = "~`!@#$%^&*()+=[]\\\';,.{}|\":<>?";
	
	for (var i = 0; i < str.length; i++) {
		if (iChars.indexOf(str.charAt(i)) != -1) {
			flag[row][col] = true;
			error[row][col] = "Cannot contain any special character.";
			return;
		}
	}
	
	var arr = str.split("/");
	
	if (arr[2] < 0 || arr[0] > 12 || arr[0] < 0 || arr[1] < 0 || arr[1] > 31) {
		flag[row][col] = true;
		error[row][col] = "Date format: mm/dd/yyyy";
		return;
	}
	
	flag[row][col] = false;
	error[row][col] = "";
}


function checkValue() {
	var arrHoTenKhachHang = document.getElementsByName("txt_hoTenKhachHang[]");
	var arrNgaySinh = document.getElementsByName("txt_ngaySinh[]");
	var arrDiaChi = document.getElementsByName("txt_diaChi[]");
	Start();
	
	var arrAtt = "errorNameClient";
	for(var i=0; i< arrHoTenKhachHang.length; i++) {
		CheckNull(arrHoTenKhachHang[i].value, 0, i);
		CheckName(arrHoTenKhachHang[i].value, 0, i);
	}
	var result1 = ShowError(arrAtt, 0);
	
	
	var arrAtt = "errorBirthday";
	for(var i=0; i< arrNgaySinh.length; i++) {
		CheckBirthdayClient(arrNgaySinh[i].value, 1, i);
	}
	var result2 = ShowError(arrAtt, 1);
	
	
	var arrAtt = "errorAddress";
	for(var i=0; i< arrDiaChi.length; i++) {
		CheckAddressClient(arrDiaChi[i].value, 2, i);
	}
	var result3 = ShowError(arrAtt, 2);
	
	//check textbox ho ten
	if(isNull(document.getElementById("id_txtHoTen").value) == true)
	{
		var result4 = 1;
		document.getElementById("id_lbHoTen").style.color = "red";
	}
	else
	{
		var result4 = 0;
		document.getElementById("id_lbHoTen").style.color = "";
	}
	// check textbox so dien thoai di dong
	if(isNull(document.getElementById("id_txtDTDiDong").value) == true)
	{
		var result5 = 1;
		document.getElementById("id_lbDTDiDong").style.color = "red";
	}
	else
	{
		var result5 = 0;
		document.getElementById("id_lbDTDiDong").style.color = "";
	}
	// check textbox email
	if(checkEmail_input(document.getElementById("id_txtEmail")) == false)
	{
		var result6 = 1;
		document.getElementById("id_lbEmail").style.color = "red";
	}
	else
	{
		var result6 = 0;
		document.getElementById("id_lbEmail").style.color = "";
	}
	
	if(result1 == 0 && result2 == 0 && result3 == 0  && result4 == 0 && result5 == 0 && result6 == 0) {
		document.getElementsByName("btnDangKyTour").item(0).type = "submit";
		return true;
	}
	else {
		document.getElementsByName("btnDangKyTour").item(0).type = "hidden";
		return false;	
	}
}

function Start() {
	var count = document.getElementsByName("txt_hoTenKhachHang[]").length;
	error = new Array(3);
	flag = new Array(3);
	for(var i = 0; i < 3; i++) {
		error[i] = new Array(count);
		flag[i] = new Array(count);
	}
}

function ShowError(arrAtt, index) {
	var count = 0;
	var temp = document.getElementsByName(arrAtt + "[]");
	for (i = 0; i < temp.length; i++) {
		if(flag[index][i]) {
			temp[i].innerHTML = error[index][i];
			count++;
		}
		else {
			temp[i].innerHTML = null;
		}
	}
	
	return count;
}

// Check email
function checkEmail_input(field)
{	if(!isEmail(field.value))
	{	
		field.focus();
		field.select();
		return false;
	}
	return true;
}
function isEmail(s)
{	if (s=="")return false;
	if((s.indexOf("")>0)||(s.indexOf("@")==-1)||(s.indexOf(".")==-1)||(s.indexOf("..")!=-1)
		||(s.indexOf("@")!=s.lastIndexOf("@"))||(s.lastIndexOf(".")==s.length-1))
			return false;		
		var str="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-@._0123456789"
		for(var j=0;j<s.length-1;j++)
			if(str.indexOf(s.charAt(j))==-1)
				return false;
		return true;							
}

function isNull(str)
{	if(str==null) return true;

	var NumberOfChar = str.length;
	for (var i=0; i<NumberOfChar; i++)
	{	if (str.charAt(i)!=' ') return false;
	}
	return true;
}

//Edit the counter/limiter value as your wish
var count = "300";
function limiter(){
var tex = document.tour_booking.txtGhiChu.value;
var len = tex.length;
if(len > count){
        tex = tex.substring(0,count);
        document.tour_booking.txtGhiChu.value =tex;
        return false;
}
document.tour_booking.limit.value = count-len;
}

//Xử lý javasciprt và MVC 3
function updateDsKhachHang()
{
    var arrayHoTenKhachHang = document.getElementsByName("txt_hoTenKhachHang[]");
    var arrayNgaySinh = document.getElementsByName("txt_ngaySinh[]");
    var arrayDiaChi = document.getElementsByName("txt_diaChi[]");
    var arrayGioiTinh = document.getElementsByName("cb_gioiTinh[]");
    var arrayTuoi = document.getElementsByName("cb_doTuoi[]");
    var dsTongKet = document.getElementById("dsTongKet");
    var content = "";
    var temp = "";
	for (var i=0; i < arrayHoTenKhachHang.length; i++)
	{
	    temp = arrayHoTenKhachHang[i].value + "@" + arrayNgaySinh[i].value + "@" + arrayDiaChi[i].value + "@" + arrayGioiTinh[i].value + "@" + arrayTuoi[i].value;
	    content += temp;
	    content += "|";
	}
	dsTongKet.value = content;
}

//Xử lý danh sách hành khách
