using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel_G08.Controllers
{
    public class HomeController : Controller
    {
        Travel_G08DataContext _dataContext = new Travel_G08DataContext();

        public struct ListDestinationByCountry
        {
            public string TenQuocGia { get; set; }
            public List<DiaDiem> TenDiaDiems { get; set; }
        }

        public ActionResult Index(string _loaiTour)
        {
            List<ListDestinationByCountry> ListDestinationByCountry = new List<ListDestinationByCountry>();
            ListDestinationByCountry temp1 = new ListDestinationByCountry();
            foreach (QuocGia qg in _dataContext.QuocGias.ToList())
            {
                if (qg.maQuocGia == 2)
                    continue;
                temp1.TenQuocGia = qg.tenQuocGia;
                var dsDiaDiem = (from DiaDiem in _dataContext.DiaDiems where DiaDiem.maQuocGia == qg.maQuocGia select DiaDiem).ToList();
                var dsDichVu = (from DichVu in _dataContext.DichVus where DichVu.DiaDiem1.maQuocGia == qg.maQuocGia select DichVu).ToList();
                temp1.TenDiaDiems = dsDiaDiem;
                ListDestinationByCountry.Add(temp1);
            }

            // Lấy 3 tour mới nhất
            var _lastestTours = (from tour
                                in _dataContext.Tours
                                 orderby tour.maTour descending
                                 select tour).Take(3);

            ViewBag.lastestTours = _lastestTours;

            ViewBag.ListDestinationByCountry = ListDestinationByCountry;
            ViewBag.listDesTinationDomestic = (from DiaDiem in _dataContext.DiaDiems where DiaDiem.maQuocGia == 2 select DiaDiem);
            return View();
        }

        public ActionResult About()
        {
            //Session["CurrentUrl"] = Request.Url.ToString();
            return View();
        }

        public ActionResult Search(string _loaiTour, string _diemXuatPhat,
                                           string _diemDenTN, string _diemDenNG,
                                           string _giaTourTN, string _giaTourNG, string _ngayDi)
        {
            string[] priceRange = _giaTourTN.Split(' ');

            List<string> listSearchInfo = new List<string>(); // danh sách nội dung search

            var diaDiem = (from dd in _dataContext.DiaDiems where dd.maDiaDiem == int.Parse(_diemXuatPhat) select dd).Single(); // lấy tên địa điểm tương ứng

            if (_loaiTour == "TN"){
                listSearchInfo.Insert(0, "Domestic tours");
                listSearchInfo.Insert(1, diaDiem.tenDiaDiem);
                if (_diemDenTN != "0"){
                    listSearchInfo.Add(_diemDenTN);
                }
                else {
                    listSearchInfo.Add("Anywhere");
                }
                if (_giaTourTN != "-1"){
                    listSearchInfo.Add(priceRange[0]);
                    listSearchInfo.Add(priceRange[1]);
                }
                else{
                    listSearchInfo.Add("Any price");
                    listSearchInfo.Add("Any price");
                }
            }

            if (_loaiTour == "NG"){
                listSearchInfo.Insert(0, "International tours");
                listSearchInfo.Insert(1, diaDiem.tenDiaDiem);
                if (_diemDenNG != "0"){
                    listSearchInfo.Add(_diemDenNG);
                }
                else{
                    listSearchInfo.Add("Anywhere");
                }
                if (_giaTourNG != "-1"){
                    listSearchInfo.Add(priceRange[0]);
                    listSearchInfo.Add(priceRange[1]);
                }
                else{
                    listSearchInfo.Add("Any price");
                    listSearchInfo.Add("Any price");
                }
            }

            if (_ngayDi != "" && _ngayDi != null){
                listSearchInfo.Add(_ngayDi);
            }
            else {
                listSearchInfo.Add("Any day");
            }

            List<Tour> listTourSearchTN = new List<Tour>(); // danh sách tour search trong nước
            List<Tour> listTourSearchNG = new List<Tour>(); // danh sách tour search nước ngoài
            List<Tour> listTourSearchContainDestinationTN = new List<Tour>(); // danh sách tour chứ điểm đến trong nước
            List<Tour> listTourSearchContainDestinationNG = new List<Tour>(); // danh sách tour chứ điểm đến trong nước

            listTourSearchContainDestinationTN = (from tour1 in _dataContext.Tours
                                                  from tour2 in _dataContext.DanhSachDiaDiemTours
                                                  where tour1.maTour == tour2.maTour && tour2.maDiaDiem == int.Parse(_diemDenTN)
                                                  select tour1).ToList();

            listTourSearchContainDestinationNG = (from tour1 in _dataContext.Tours
                                                  from tour2 in _dataContext.DanhSachDiaDiemTours
                                                  where tour1.maTour == tour2.maTour && tour2.maDiaDiem == int.Parse(_diemDenNG)
                                                  select tour1).ToList();

            //////////////////////////////////////////////////////////////////////////////////////////////

            //Tìm kiếm tour trong nước
            if (_loaiTour == "TN"){ // tour trong nước
                if (_diemDenTN != "0"){ // tour theo dia diem den
                    if (_giaTourTN != "-1"){ // tour theo mức giá
                        if (_ngayDi != "" && _ngayDi != null){ // tour theo ngày
                            listTourSearchTN = (from tour in listTourSearchContainDestinationTN
                                                where tour.loaiTour == "Tour trong nước"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                && tour.ngayThucHien == System.DateTime.Parse(_ngayDi)
                                                && tour.giaTour >= float.Parse(priceRange[0])
                                                && tour.giaTour <= float.Parse(priceRange[1])
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchTN;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                        else{ // chọn tất cả các ngày
                            listTourSearchTN = (from tour in listTourSearchContainDestinationTN
                                                where tour.loaiTour == "Tour trong nước"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                && tour.giaTour >= float.Parse(priceRange[0])
                                                && tour.giaTour <= float.Parse(priceRange[1])
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchTN;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                    }
                    else{ // chọn tất cả các mức giá tour
                        if (_ngayDi != "" && _ngayDi != null){ // tour theo ngày
                            listTourSearchTN = (from tour in listTourSearchContainDestinationTN
                                                where tour.loaiTour == "Tour trong nước"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                && tour.ngayThucHien == System.DateTime.Parse(_ngayDi)
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchTN;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                        else{ // chọn tất cả các ngày
                            listTourSearchTN = (from tour in listTourSearchContainDestinationTN
                                                where tour.loaiTour == "Tour trong nước"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchTN;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                    }
                }
                else{ //tour không theo dia diem den
                    if (_giaTourTN != "-1"){ // tour theo mức giá
                        if (_ngayDi != "" && _ngayDi != null){ // tour theo ngày
                            listTourSearchTN = (from tour in _dataContext.Tours
                                                where tour.loaiTour == "Tour trong nước"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                && tour.ngayThucHien == System.DateTime.Parse(_ngayDi)
                                                && tour.giaTour >= float.Parse(priceRange[0])
                                                && tour.giaTour <= float.Parse(priceRange[1])
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchTN;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                        else{ // chọn tất cả các ngày
                            listTourSearchTN = (from tour in _dataContext.Tours
                                                where tour.loaiTour == "Tour trong nước"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                && tour.giaTour >= float.Parse(priceRange[0])
                                                && tour.giaTour <= float.Parse(priceRange[1])
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchTN;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                    }
                    else{ // chọn tất cả các mức giá tour
                        if (_ngayDi != "" && _ngayDi != null){ // tour theo ngày
                            listTourSearchTN = (from tour in _dataContext.Tours
                                                where tour.loaiTour == "Tour trong nước"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                && tour.ngayThucHien == System.DateTime.Parse(_ngayDi)
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchTN;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                        else{ // chọn tất cả các ngày
                            listTourSearchTN = (from tour in _dataContext.Tours
                                                where tour.loaiTour == "Tour trong nước"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchTN;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                    }
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////Tìm kiếm tour nước ngoài
            if (_loaiTour == "NG"){ // tour quốc tế
                if (_diemDenNG != "0"){ // tour theo địa điểm đến
                    if (_giaTourNG != "-1"){ // tour theo mức giá
                        if (_ngayDi != "" && _ngayDi != null){ // tour theo ngày
                            listTourSearchNG = (from tour in listTourSearchContainDestinationNG
                                                where tour.loaiTour == "Tour quốc tế"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                && tour.ngayThucHien == System.DateTime.Parse(_ngayDi)
                                                && tour.giaTour >= float.Parse(priceRange[0])
                                                && tour.giaTour <= float.Parse(priceRange[1])
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchNG;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                        else{ // chọn tất cả các ngày
                            listTourSearchNG = (from tour in listTourSearchContainDestinationNG
                                                where tour.loaiTour == "Tour quốc tế"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                && tour.giaTour >= float.Parse(priceRange[0])
                                                && tour.giaTour <= float.Parse(priceRange[1])
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchNG;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                    }
                    else{ // chọn tất cả các mức giá tour
                        if (_ngayDi != "" && _ngayDi != null){ // tour theo ngày
                            listTourSearchNG = (from tour in listTourSearchContainDestinationNG
                                                where tour.loaiTour == "Tour quốc tế"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                && tour.ngayThucHien == System.DateTime.Parse(_ngayDi)
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchNG;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                        else{ // chọn tất cả các ngày
                            listTourSearchNG = (from tour in listTourSearchContainDestinationNG
                                                where tour.loaiTour == "Tour quốc tế"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchNG;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                    }
                }
                else{ //tour không theo dia diem den
                    if (_giaTourNG != "-1"){ // tour theo mức giá
                        if (_ngayDi != "" && _ngayDi != null){ // tour theo ngày
                            listTourSearchNG = (from tour in _dataContext.Tours
                                                where tour.loaiTour == "Tour quốc tế"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                && tour.ngayThucHien == System.DateTime.Parse(_ngayDi)
                                                && tour.giaTour >= float.Parse(priceRange[0])
                                                && tour.giaTour <= float.Parse(priceRange[1])
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchNG;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                        else{ // chọn tất cả các ngày
                            listTourSearchNG = (from tour in _dataContext.Tours
                                                where tour.loaiTour == "Tour quốc tế"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                && tour.giaTour >= float.Parse(priceRange[0])
                                                && tour.giaTour <= float.Parse(priceRange[1])
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchNG;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                    }
                    else{ // chọn tất cả các mức giá tour
                        if (_ngayDi != "" && _ngayDi != null){ // tour theo ngày
                            listTourSearchNG = (from tour in _dataContext.Tours
                                                where tour.loaiTour == "Tour quốc tế"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                && tour.ngayThucHien == System.DateTime.Parse(_ngayDi)
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchNG;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                        else{ // chọn tất cả các ngày
                            listTourSearchNG = (from tour in _dataContext.Tours
                                                where tour.loaiTour == "Tour quốc tế"
                                                && tour.maDiemBatDau == int.Parse(_diemXuatPhat)
                                                select tour).ToList();
                            ViewBag.searchTours = listTourSearchNG;
                            ViewBag.searchInfos = listSearchInfo;
                            return View();
                        }
                    }
                }
            }
            return View();
        }

        public ActionResult QuickSearch(string _keyQuickSearch)
        {
            List<Tour> listTourQuickSearch = new List<Tour>();

            listTourQuickSearch = (from tour in _dataContext.Tours
                                   where tour.loaiTour.Contains(@_keyQuickSearch)
                                   || tour.tenTour.Contains(@_keyQuickSearch)
                                   || tour.slug.Contains(@_keyQuickSearch)
                                   || tour.gioiThieu.Contains(@_keyQuickSearch)
                                   select tour).ToList();

            ViewBag.quickSearchTours = listTourQuickSearch;
            ViewBag.keySearch = _keyQuickSearch;
            return View();
        }
    }
}
