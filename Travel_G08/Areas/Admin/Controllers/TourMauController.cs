using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel_G08.Areas.Admin.Controllers
{
    public class TourMauController : Controller
    {
        //
        // GET: /Admin/TourMau/
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
            int count = dataContext.TourMaus.Count();
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
            return View(dataContext.TourMaus.Skip(start).Take(offset));
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
            if (Message != "")
                ViewBag.Message = Message;
            if (Error != "")
                ViewBag.Error = Error;
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Them(string type, string name, string duration, string start_destination, string description, string inputListDestination, string inputListService)
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
            TourMau tourMau = new TourMau();
            tourMau.loaiTour = type;
            tourMau.tenTour = name;
            tourMau.soNgayDem = duration;
            tourMau.maDiemBatDau = int.Parse(start_destination);
            tourMau.gioiThieu = description;
            try
            {
                dataContext.TourMaus.InsertOnSubmit(tourMau);
                dataContext.SubmitChanges();
                var newTourMau = dataContext.TourMaus.OrderByDescending(x => x.maTour).Take(1).Single();
                //Start Tạo danh sách địa điểm mới
                DanhSachDiaDiemTourMau ddTemp;
                foreach (var s in destinations)
                {
                    ddTemp = new DanhSachDiaDiemTourMau();
                    if (s == "")
                        break;
                    else
                    {
                        ddTemp.maTour = newTourMau.maTour;
                        ddTemp.maDiaDiem = int.Parse(s);
                        dataContext.DanhSachDiaDiemTourMaus.InsertOnSubmit(ddTemp);
                    }
                }

                //End Tạo danh sách địa điểm mới

                //Start Tạo danh sách dịch vụ mới
                DanhSachDichVuTourMau dvTemp;
                foreach (var s in services)
                {
                    dvTemp = new DanhSachDichVuTourMau();
                    if (s == "")
                        break;
                    else
                    {
                        dvTemp.maTour = newTourMau.maTour;
                        dvTemp.maDichVu = int.Parse(s);
                        dataContext.DanhSachDichVuTourMaus.InsertOnSubmit(dvTemp);
                    }
                }
                //End Tạo danh sách dịch vụ mới
                dataContext.SubmitChanges();
                return RedirectToAction("Them", new { Message = "Add new tempplate successfully!", Error = ""});
            }
            catch
            {
                return RedirectToAction("Them", new { Message = "", Error = "Error adding new template!"});
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
            var dsDiaDiemTourMauTemp = (from DanhSachDiaDiemTourMau in dataContext.DanhSachDiaDiemTourMaus where DanhSachDiaDiemTourMau.maTour == int.Parse(ID) select DanhSachDiaDiemTourMau);
            List<StructDiaDiem> dsDiaDiemTourMau = new List<StructDiaDiem>();
            StructDiaDiem ddTourMauTemp = new StructDiaDiem();
            foreach (var s in dsDiaDiemTourMauTemp)
            {
                ddTourMauTemp = new StructDiaDiem();
                ddTourMauTemp.MaDiaDiem = s.maDiaDiem.ToString();
                ddTourMauTemp.TenDiaDiem = s.DiaDiem.tenDiaDiem;
                dsDiaDiemTourMau.Add(ddTourMauTemp);
            }


            var dsDichVuTourMauTemp = (from DanhSachDichVuTourMau in dataContext.DanhSachDichVuTourMaus where DanhSachDichVuTourMau.maTour == int.Parse(ID) select DanhSachDichVuTourMau);
            List<StructDichVu> dsDichVuTourMau = new List<StructDichVu>();
            StructDichVu dvTourMauTemp = new StructDichVu();
            foreach (var s in dsDichVuTourMauTemp)
            {
                dvTourMauTemp = new StructDichVu();
                dvTourMauTemp.MaDichVu = s.maDichVu.ToString();
                dvTourMauTemp.TenDichVu = s.DichVu.tenDichVu;
                dsDichVuTourMau.Add(dvTourMauTemp);
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
            ViewBag.dsDiaDiemTourMau = dsDiaDiemTourMau;
            ViewBag.dsDichVuTourMau = dsDichVuTourMau;
            var tourMau = (from TourMau in dataContext.TourMaus where TourMau.maTour == int.Parse(ID) select TourMau).Single();
            if (Message != "")
                ViewBag.Message = Message;
            if (Error != "")
                ViewBag.Error = Error;
            return View(tourMau);
        }
        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Sua(string ID, string type, string name, string duration, string start_destination, string description, string inputListDestination, string inputListService)
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
            var tourMau = (from TourMau in dataContext.TourMaus where TourMau.maTour == int.Parse(ID) select TourMau).Single();
            var dsDiaDiem = (from DanhSachDiaDiemTourMau in dataContext.DanhSachDiaDiemTourMaus where DanhSachDiaDiemTourMau.maTour == int.Parse(ID) select DanhSachDiaDiemTourMau);
            var dsDichVu = (from DanhSachDichVuTourMau in dataContext.DanhSachDichVuTourMaus where DanhSachDichVuTourMau.maTour == int.Parse(ID) select DanhSachDichVuTourMau);
            try
            {
                //Start sửa danh sách địa điểm
                //Xóa danh sách cũ
                foreach (var a in dsDiaDiem)
                {
                    dataContext.DanhSachDiaDiemTourMaus.DeleteOnSubmit(a);
                }
                //Tạo danh sách mới
                DanhSachDiaDiemTourMau ddTemp;
                foreach (var s in destinations)
                {
                    ddTemp = new DanhSachDiaDiemTourMau();
                    if (s == "")
                        break;
                    else
                    {
                        ddTemp.maTour = int.Parse(ID);
                        ddTemp.maDiaDiem = int.Parse(s);
                        dataContext.DanhSachDiaDiemTourMaus.InsertOnSubmit(ddTemp);
                    }
                }
                             
                //End sửa danh sách địa điểm

                //Start sửa danh sách dịch vụ
                //Xóa danh sách cũ
                foreach (var a in dsDichVu)
                {
                    dataContext.DanhSachDichVuTourMaus.DeleteOnSubmit(a);
                }
                //Tạo danh sách mới
                DanhSachDichVuTourMau dvTemp;
                foreach (var s in services)
                {
                    dvTemp = new DanhSachDichVuTourMau();
                    if (s == "")
                        break;
                    else
                    {
                        dvTemp.maTour = int.Parse(ID);
                        dvTemp.maDichVu = int.Parse(s);
                        dataContext.DanhSachDichVuTourMaus.InsertOnSubmit(dvTemp);
                    }
                }

                //End sửa danh sách địa điểm

                //Start sửa thông tin tour mẫu
                tourMau.loaiTour = type;
                tourMau.tenTour = name;
                tourMau.soNgayDem = duration;
                tourMau.maDiemBatDau = int.Parse(start_destination);
                tourMau.gioiThieu = description;
                //End sửa thông tin tour mẫu
                dataContext.SubmitChanges();
                return RedirectToAction("Sua", new { ID = ID, Message = "Update template successfully!", Error = ""});
            }
            catch
            {
                return RedirectToAction("Sua", new { ID = ID, Message = "", Error = "Error updating template!" });
            }
        }
        public ActionResult Xoa(string ID)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            var dsDiaDiemTourMau = (from DanhSachDiaDiemTourMau in dataContext.DanhSachDiaDiemTourMaus where DanhSachDiaDiemTourMau.maTour == int.Parse(ID) select DanhSachDiaDiemTourMau);
            var dsDichVuTourMau = (from DanhSachDichVuTourMau in dataContext.DanhSachDichVuTourMaus where DanhSachDichVuTourMau.maTour == int.Parse(ID) select DanhSachDichVuTourMau);
            var tourMau = (from TourMau in dataContext.TourMaus where TourMau.maTour == int.Parse(ID) select TourMau).Single();
            try
            {
                dataContext.DanhSachDiaDiemTourMaus.DeleteAllOnSubmit(dsDiaDiemTourMau);
                dataContext.DanhSachDichVuTourMaus.DeleteAllOnSubmit(dsDichVuTourMau);
                dataContext.TourMaus.DeleteOnSubmit(tourMau);
                dataContext.SubmitChanges();
            }
            catch
            {
            }
            return RedirectToAction("DanhSach");
        }
    }
}
