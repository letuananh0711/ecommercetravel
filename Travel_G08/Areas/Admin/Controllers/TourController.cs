using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel_G08.Areas.Admin.Controllers
{
    public class TourController : Controller
    {
        //
        // GET: /Admin/Tour/
        PhanQuyenController phanquyen = new PhanQuyenController();
        string role;
        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            if (ctx.HttpContext.Session["Role"] != null)
            {
                role = ControllerContext.HttpContext.Session["Role"].ToString();
            }
        }
        public struct ListDestinationByCountry
        {
            public string TenQuocGia { get; set; }
            public List<DiaDiem> TenDiaDiems { get; set; }
        }
        public struct ListServiceByCountry
        {
            public string TenQuocGia { get; set; }
            public List<DichVu> TenDichVus { get; set; }
        }
        public struct ListTemplateByType
        {
            public string TenLoaiTour { get; set; }
            public List<TourMau> TenTourMaus { get; set; }
        }
        public struct StructDiaDiem
        {
            public string TenDiaDiem { get; set; }
            public string MaDiaDiem { get; set; }
        }
        public struct StructDichVu
        {
            public string MaDichVu { get; set; }
            public string TenDichVu { get; set; }
        }
        public partial class PartialTemplate
        {
            public string LoaiTour { get; set; }
            public string TenTour { get; set; }
            public string SoNgayDem { get; set; }
            public string DiemBatDau { get; set; }
            public string GioiThieu { get; set; }
            public List<string> dsDiaDiem { get; set; }
            public List<string> dsDichVu { get; set; }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return RedirectToAction("Login", "Admin");
        }
        public ActionResult DanhSach()
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            string base_URL = "DanhSach";
            int count = dataContext.Tours.Count();
            //phan trang ko quan tam
            string URL_segment;
            try
            {
                URL_segment = Request.Url.Segments[4];
            }
            catch (Exception)
            {
                URL_segment = null;
            }
            Libraries.Pagination pagination = new Libraries.Pagination(base_URL, URL_segment, count);
            string pageLinks = pagination.GetPageLinks();
            int start = (pagination.CurPage - 1) * pagination.PerPage;
            int offset = pagination.PerPage;
            if (URL_segment != null)
                pageLinks = pageLinks.Replace(base_URL + "/", "");
            ViewBag.pageLinks = pageLinks;
            //phan trang ko quan tam

            ViewBag.count = count;
            return View(dataContext.Tours.Skip(start).Take(offset));
        }
        public ActionResult Them(string Message, string Error)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            //Start Xử lý cho Manual
            List<ListDestinationByCountry> ListDestinationByCountry = new List<ListDestinationByCountry>();
            ListDestinationByCountry temp1 = new ListDestinationByCountry();
            List<ListServiceByCountry> ListServiceByCountry = new List<ListServiceByCountry>();
            ListServiceByCountry temp2 = new ListServiceByCountry();
            foreach (QuocGia qg in dataContext.QuocGias.ToList())
            {
                temp1.TenQuocGia = qg.tenQuocGia;
                temp2.TenQuocGia = qg.tenQuocGia;
                var dsDiaDiem = (from DiaDiem in dataContext.DiaDiems where DiaDiem.maQuocGia == qg.maQuocGia select DiaDiem).ToList();
                var dsDichVu = (from DichVu in dataContext.DichVus where DichVu.DiaDiem1.maQuocGia == qg.maQuocGia select DichVu).ToList();
                temp1.TenDiaDiems = dsDiaDiem;
                temp2.TenDichVus = dsDichVu;
                ListDestinationByCountry.Add(temp1);
                ListServiceByCountry.Add(temp2);
            }

            ViewBag.ListDestinationByCountry = ListDestinationByCountry;
            ViewBag.ListServiceByCountry = ListServiceByCountry;
            //End Xử lý cho Manual

            //Start Xử lý cho template
            List<string> dsLoaiTour = new List<string> { "City tour - One day tour", "Tour quốc tế", "Tour trong nước" };
            List<ListTemplateByType> ListTemplateByType = new List<ListTemplateByType>();
            ListTemplateByType temp = new ListTemplateByType();
            foreach (var s in dsLoaiTour)
            {
                temp = new ListTemplateByType();
                temp.TenLoaiTour = s;
                var dsTourMau = (from TourMau in dataContext.TourMaus where TourMau.loaiTour == s select TourMau).ToList();
                temp.TenTourMaus = dsTourMau;
                ListTemplateByType.Add(temp);
            }
            ViewBag.ListTemplateByType = ListTemplateByType;
            //End Xử lý cho template
            if (Message != "")
                ViewBag.Message = Message;
            if (Error != "")
                ViewBag.Error = Error;
            return View();
        }
        public ActionResult TemplateInfo(string ID)
        {

            var PartialTemplate = new PartialTemplate();
            var dataContext = new Travel_G08DataContext();
            var temp = (from TourMau in dataContext.TourMaus where TourMau.maTour == int.Parse(ID) select TourMau).Single();
            var dsDiaDiem = (from DanhSachDiaDiemTourMau in dataContext.DanhSachDiaDiemTourMaus where DanhSachDiaDiemTourMau.maTour == int.Parse(ID) select DanhSachDiaDiemTourMau);
            var dsDichVu = (from DanhSachDichVuTourMau in dataContext.DanhSachDichVuTourMaus where DanhSachDichVuTourMau.maTour == int.Parse(ID) select DanhSachDichVuTourMau);
            PartialTemplate.LoaiTour = temp.loaiTour;
            PartialTemplate.TenTour = temp.tenTour;
            PartialTemplate.SoNgayDem = temp.soNgayDem;
            PartialTemplate.DiemBatDau = temp.DiaDiem.tenDiaDiem;
            PartialTemplate.GioiThieu = temp.gioiThieu;
            PartialTemplate.dsDiaDiem = new List<string>();
            PartialTemplate.dsDichVu = new List<string>();
            foreach (var s in dsDiaDiem)
            {
                PartialTemplate.dsDiaDiem.Add(s.DiaDiem.tenDiaDiem);
            }
            foreach (var s in dsDichVu)
            {
                PartialTemplate.dsDichVu.Add(s.DichVu.tenDichVu);
            }
            ViewBag.PartialTemplate = PartialTemplate;
            return PartialView("_partialTemplate", PartialTemplate);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ThemManual(string type, string name, string duration, string start_destination, string description, string inputListDestination, string inputListService, string start_date, string price, string status, string slug)
        {

            string[] destinations = inputListDestination.Split(' ');
            string[] services = inputListService.Split(' ');
            var dataContext = new Travel_G08DataContext();
            Tour tourTemp = new Tour();
            tourTemp.loaiTour = type;
            tourTemp.tenTour = name;
            tourTemp.soNgayDem = duration;
            tourTemp.maDiemBatDau = int.Parse(start_destination);
            tourTemp.gioiThieu = description;
            if(start_date != null && start_date != "")
                tourTemp.ngayThucHien = DateTime.Parse(start_date);
            if(price != null && price != "")
                tourTemp.giaTour = float.Parse(price);
            tourTemp.slug = slug;
            tourTemp.trangThaiTour = status;
            try
            {
                dataContext.Tours.InsertOnSubmit(tourTemp);
                dataContext.SubmitChanges();
                var newTour = dataContext.Tours.OrderByDescending(x => x.maTour).Take(1).Single();
                //Start Tạo danh sách địa điểm mới
                DanhSachDiaDiemTour ddTemp;
                foreach (var s in destinations)
                {
                    ddTemp = new DanhSachDiaDiemTour();
                    if (s == "")
                        break;
                    else
                    {
                        ddTemp.maTour = newTour.maTour;
                        ddTemp.maDiaDiem = int.Parse(s);
                        dataContext.DanhSachDiaDiemTours.InsertOnSubmit(ddTemp);
                    }
                }

                //End Tạo danh sách địa điểm mới

                //Start Tạo danh sách dịch vụ mới
                DanhSachDichVuTour dvTemp;
                foreach (var s in services)
                {
                    dvTemp = new DanhSachDichVuTour();
                    if (s == "")
                        break;
                    else
                    {
                        dvTemp.maTour = newTour.maTour;
                        dvTemp.maDichVu = int.Parse(s);
                        dataContext.DanhSachDichVuTours.InsertOnSubmit(dvTemp);
                    }
                }
                //End Tạo danh sách dịch vụ mới
                dataContext.SubmitChanges();
                return RedirectToAction("Them", new { Message = "Add new tour successfully!", Error = "" });
            }
            catch
            {
                return RedirectToAction("Them", new { Message = "", Error = "Error adding new tour!" });
            }
        }
        public ActionResult ThemTemplate(string list_template)
        {

            var dataContext = new Travel_G08DataContext();
            Tour tourTemp = new Tour();
            var tourMauTemp = (from TourMau in dataContext.TourMaus where TourMau.maTour == int.Parse(list_template) select TourMau).Single();
            tourTemp.loaiTour = tourMauTemp.loaiTour;
            tourTemp.tenTour = tourMauTemp.tenTour;
            tourTemp.soNgayDem = tourMauTemp.soNgayDem;
            tourTemp.maDiemBatDau = tourMauTemp.maDiemBatDau;
            dataContext.Tours.InsertOnSubmit(tourTemp);
            try
            {
                dataContext.SubmitChanges();
                return RedirectToAction("Them", new { Message = "Create new tour successfully!", Error = ""});
            }
            catch
            {
                return RedirectToAction("Them", new { Message = "", Error = "Error creating new tour!" });
            }
        }
        public ActionResult Sua(string ID, string Message, string Error)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            List<ListDestinationByCountry> ListDestinationByCountry = new List<ListDestinationByCountry>();
            ListDestinationByCountry temp1 = new ListDestinationByCountry();
            List<ListServiceByCountry> ListServiceByCountry = new List<ListServiceByCountry>();
            ListServiceByCountry temp2 = new ListServiceByCountry();
            var dsDiaDiemTourTemp = (from DanhSachDiaDiemTour in dataContext.DanhSachDiaDiemTours where DanhSachDiaDiemTour.maTour == int.Parse(ID) select DanhSachDiaDiemTour);
            List<StructDiaDiem> dsDiaDiemTour = new List<StructDiaDiem>();
            StructDiaDiem ddTourTemp = new StructDiaDiem();
            foreach (var s in dsDiaDiemTourTemp)
            {
                ddTourTemp = new StructDiaDiem();
                ddTourTemp.MaDiaDiem = s.maDiaDiem.ToString();
                ddTourTemp.TenDiaDiem = s.DiaDiem.tenDiaDiem;
                dsDiaDiemTour.Add(ddTourTemp);
            }


            var dsDichVuTourTemp = (from DanhSachDichVuTour in dataContext.DanhSachDichVuTours where DanhSachDichVuTour.maTour == int.Parse(ID) select DanhSachDichVuTour);
            List<StructDichVu> dsDichVuTour = new List<StructDichVu>();
            StructDichVu dvTourTemp = new StructDichVu();
            foreach (var s in dsDichVuTourTemp)
            {
                dvTourTemp = new StructDichVu();
                dvTourTemp.MaDichVu = s.maDichVu.ToString();
                dvTourTemp.TenDichVu = s.DichVu.tenDichVu;
                dsDichVuTour.Add(dvTourTemp);
            }

            foreach (QuocGia qg in dataContext.QuocGias.ToList())
            {
                temp1.TenQuocGia = qg.tenQuocGia;
                temp2.TenQuocGia = qg.tenQuocGia;
                var dsDiaDiem = (from DiaDiem in dataContext.DiaDiems where DiaDiem.maQuocGia == qg.maQuocGia select DiaDiem).ToList();
                var dsDichVu = (from DichVu in dataContext.DichVus where DichVu.DiaDiem1.maQuocGia == qg.maQuocGia select DichVu).ToList();
                temp1.TenDiaDiems = dsDiaDiem;
                temp2.TenDichVus = dsDichVu;
                ListDestinationByCountry.Add(temp1);
                ListServiceByCountry.Add(temp2);
            }

            ViewBag.ListDestinationByCountry = ListDestinationByCountry;
            ViewBag.ListServiceByCountry = ListServiceByCountry;
            ViewBag.dsDiaDiemTourMau = dsDiaDiemTour;
            ViewBag.dsDichVuTourMau = dsDichVuTour;
            var tour = (from Tour in dataContext.Tours where Tour.maTour == int.Parse(ID) select Tour).Single();
            if (Message != "")
                ViewBag.Message = Message;
            if (Error != "")
                ViewBag.Error = Error;
            return View(tour);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Sua(string ID, string type, string name, string duration, string start_destination, string description, string inputListDestination, string inputListService, string start_date, string price, string status, string slug)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            string[] destinations = inputListDestination.Split(' ');
            string[] services = inputListService.Split(' ');
            var dataContext = new Travel_G08DataContext();
            var tour = (from Tour in dataContext.Tours where Tour.maTour == int.Parse(ID) select Tour).Single();
            var dsDiaDiem = (from DanhSachDiaDiemTour in dataContext.DanhSachDiaDiemTours where DanhSachDiaDiemTour.maTour == int.Parse(ID) select DanhSachDiaDiemTour);
            var dsDichVu = (from DanhSachDichVuTour in dataContext.DanhSachDichVuTours where DanhSachDichVuTour.maTour == int.Parse(ID) select DanhSachDichVuTour);
            try
            {
                //Start sửa danh sách địa điểm
                //Xóa danh sách cũ
                foreach (var a in dsDiaDiem)
                {
                    dataContext.DanhSachDiaDiemTours.DeleteOnSubmit(a);
                }
                //Tạo danh sách mới
                DanhSachDiaDiemTour ddTemp;
                foreach (var s in destinations)
                {
                    ddTemp = new DanhSachDiaDiemTour();
                    if (s == "")
                        break;
                    else
                    {
                        ddTemp.maTour = int.Parse(ID);
                        ddTemp.maDiaDiem = int.Parse(s);
                        dataContext.DanhSachDiaDiemTours.InsertOnSubmit(ddTemp);
                    }
                }

                //End sửa danh sách địa điểm

                //Start sửa danh sách dịch vụ
                //Xóa danh sách cũ
                foreach (var a in dsDichVu)
                {
                    dataContext.DanhSachDichVuTours.DeleteOnSubmit(a);
                }
                //Tạo danh sách mới
                DanhSachDichVuTour dvTemp;
                foreach (var s in services)
                {
                    dvTemp = new DanhSachDichVuTour();
                    if (s == "")
                        break;
                    else
                    {
                        dvTemp.maTour = int.Parse(ID);
                        dvTemp.maDichVu = int.Parse(s);
                        dataContext.DanhSachDichVuTours.InsertOnSubmit(dvTemp);
                    }
                }

                //End sửa danh sách địa điểm
                tour.loaiTour = type;
                tour.tenTour = name;
                tour.soNgayDem = duration;
                tour.maDiemBatDau = int.Parse(start_destination);
                tour.gioiThieu = description;
                if(start_date != null && start_date != "")
                    tour.ngayThucHien = DateTime.Parse(start_date);
                if(price != null && price != "")
                    tour.giaTour = float.Parse(price);
                tour.slug = slug;
                tour.trangThaiTour = status;
                dataContext.SubmitChanges();
                return RedirectToAction("Sua", new { ID = ID, Message = "Update tour successfully!", Error = "" });
            }
            catch
            {
                return RedirectToAction("Sua", new { ID = ID, Message = "", Error = "Error updating tour!" });
            }
        }
    }
}
