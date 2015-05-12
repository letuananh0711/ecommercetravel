//function ChangeClass(obj) {
//    if (obj.className == 'heading collapsed') {
//        obj.className = 'heading expanded';
//    }
//    else
//        if (obj.className == 'heading expanded') {
//            obj.className = 'heading collapsed';
//        }

//}

////$(".Section").toggle(function(){
////    $(this).children(".heading collapsed").attr("class", "heading expanded");
////},function(){
////    $(this).children(".heading collapsed").attr("class", "heading collapsed");
////});​

//$("#heading collapsed").click(function () {
//    $(".heading collapsed").find("ul").css('display', 'block')
//});
////$(".Section").find("a").attr("class", "heading expanded");
$(document).ready(function () {
    init();
    $('#list_template').change(function () {
        var selectedTemplate = $(this).val();
        //$('#resultDiv').load('@Url.Action("Tour", "TemplateInfo")', { ID: selectedTemplate });
        $.get('/Admin/Tour/TemplateInfo/' + selectedTemplate, function (data) {
            /* data is the pure html returned from action method, load it to your page */
            //window.alert(data);
            $('#resultDiv').html(data);
            /* little fade in effect */
            $('#resultDiv').fadeIn('fast');
        });
    });
});

function keepSystemHeadingState() {
    document.getElementById("system_heading").className = "heading expanded";
    document.getElementById('system_items').style.display = "inline";
    document.getElementById('product_items').style.display = "none";
}

function keepProductHeadingState() {
    document.getElementById("product_heading").className = "heading expanded";
    document.getElementById('product_items').style.display = "inline";
    document.getElementById('system_items').style.display = "none";
}

function init()
{
    $("table .delete-row").click
    (
        function () {
            if (!confirm("Are you sure you want to Delete?")) return false;
            var ele = $(this);
            $.get(ele.attr('href'), "", function () {
                ele.parents("tr").fadeOut('slow');
            });
            return false; // khong chuyen trang
        }
    );
}

//Remove div
//$(document).ready(function () {
//    $("#tutn").live('click', function (e) {
//        $("#first_time").remove();
//        e.preventDefault();
//    });
//});
function deleteDiv(buttonName) {
    //document.getElementById(divID).remove();
    buttonName.parentNode.parentNode.removeChild(buttonName.parentNode);
    refreshInputListService();
    refreshInputListDestination();
}
//Create div
function createDivDestination() {
    //Start Lấy thông tin mã địa điểm và tên địa điểm đang được chọn
    var select = document.getElementById("list_destination");
    var maDiaDiem = select.options[select.selectedIndex].value;
    var tenDiaDiem = select.options[select.selectedIndex].text;
    //End of Lấy thông tin mã địa điểm và tên địa điểm đang được chọn

    //Start Tạo nút xóa
    var button = document.createElement('input');
    button.setAttribute('type', 'button');
    button.setAttribute('value', 'X');
    button.setAttribute('onclick', 'deleteDiv(this)');
    //End Tạo nút xóa

    //Start Thêm div chứa địa điểm được chọn
    var mainDiv = document.getElementById('listDestination');
    var divChild = document.createElement('div');
    divChild.setAttribute('id',maDiaDiem);
    var content = document.createTextNode("  " + tenDiaDiem);
    divChild.appendChild(button);
    divChild.appendChild(content);   
    mainDiv.appendChild(divChild);
    //End Thêm div chứa địa điểm được chọn
    refreshInputListDestination();
}
function refreshInputListDestination() {
    //Lấy danh sách các địa điểm đã dc chọn
    var textBoxContent = '';
    var parentDiv = document.getElementById('listDestination').children;
    var listDestinationChosen = new Array();
    for (var i = 0; i < parentDiv.length; i++) {
        //window.alert(parentDiv[i].getAttribute('id'));
        listDestinationChosen.push(parentDiv[i].getAttribute('id'));
        textBoxContent = textBoxContent + parentDiv[i].getAttribute('id') + ' ';
    }
    var textBox = document.getElementById('inputListDestination').value = textBoxContent;
}


function createDivService() {
    //Start Lấy thông tin mã dịch vụ và tên dịch vụ đang được chọn
    var select = document.getElementById("list_service");
    var maDichVu = select.options[select.selectedIndex].value;
    var tenDichVu = select.options[select.selectedIndex].text;
    //End of Lấy thông tin mã dịch vụ và tên dịch vụ đang được chọn

    //Start Tạo nút xóa
    var button = document.createElement('input');
    button.setAttribute('type', 'button');
    button.setAttribute('value', 'X');
    button.setAttribute('onclick', 'deleteDiv(this)');
    //End Tạo nút xóa

    //Start Thêm div chứa dịch vụ được chọn
    var mainDiv = document.getElementById('listService');
    var divChild = document.createElement('div');
    divChild.setAttribute('id', maDichVu);
    var content = document.createTextNode("  " + tenDichVu);
    divChild.appendChild(button);
    divChild.appendChild(content);
    mainDiv.appendChild(divChild);
    //End Thêm div chứa dịch vụ được chọn

    refreshInputListService(); 
}
function refreshInputListService() {
    //Lấy danh sách các dịch vụ đã dc chọn
    var textBoxContent = '';
    var parentDiv = document.getElementById('listService').children;
    var listDestinationChosen = new Array();
    for (var i = 0; i < parentDiv.length; i++) {
        //window.alert(parentDiv[i].getAttribute('id'));
        listDestinationChosen.push(parentDiv[i].getAttribute('id'));
        textBoxContent = textBoxContent + parentDiv[i].getAttribute('id') + ' ';
    }
    var textBox = document.getElementById('inputListService').value = textBoxContent;
}

function showHideDiv() {
    var select = document.getElementById("option_newtour");
    var option = select.options[select.selectedIndex].value;
    if (option == '0') {
        document.getElementById('by_template').style.display = "inline";
        document.getElementById('manual').style.display = "none";
    }
    else {
        document.getElementById('by_template').style.display = "none";
        document.getElementById('manual').style.display = "inline";
    }

}

