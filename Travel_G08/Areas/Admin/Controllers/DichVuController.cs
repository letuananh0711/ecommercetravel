using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel_G08.Areas.Admin.Controllers
{
    public class DichVuController : Controller
    {
        //
        // GET: /Admin/DichVu/
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
            var dataContext = new Travel_G08DataContext();
            string base_URL = "DanhSach";
            int count = dataContext.DichVus.Count();
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
            return View(dataContext.DichVus.Skip(start).Take(offset));
        }
        public ActionResult Them(string Message, string Error)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            var dataContext = new Travel_G08DataContext();
            List<ListDestinationByCountry> listTemp = new List<ListDestinationByCountry>();
            ListDestinationByCountry temp = new ListDestinationByCountry();
            foreach (QuocGia qg in dataContext.QuocGias.ToList())
            {
                temp.TenQuocGia = qg.tenQuocGia;
                var dsDiaDiem = (from DiaDiem in dataContext.DiaDiems where DiaDiem.maQuocGia == qg.maQuocGia select DiaDiem).ToList();
                temp.TenDiaDiems = dsDiaDiem;
                listTemp.Add(temp);
            }
            ViewBag.ListDestinationByCountry = listTemp;
            if (Message != "")
                ViewBag.Message = Message;
            if (Error != "")
                ViewBag.Error = Error;
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Them(string destination, string service, string description)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            var dataContext = new Travel_G08DataContext();
            DichVu dv = new DichVu();
            dv.diaDiem = int.Parse(destination);
            dv.tenDichVu = service;
            dv.gioiThieu = description;
            try
            {
                dataContext.DichVus.InsertOnSubmit(dv);
                dataContext.SubmitChanges();
                return RedirectToAction("Them", new { Message = "Add new service successully", Error = ""});
            }
            catch
            {
                return RedirectToAction("Them", new { Message = "", Error = "Error adding new service" });
            }
        }
        public ActionResult Sua(string ID, string Message, string Error)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            var dataContext = new Travel_G08DataContext();
            var dv = (from DichVu in dataContext.DichVus where DichVu.maDichVu == int.Parse(ID) select DichVu).Single();
            //Lấy danh sách địa điểm
            List<ListDestinationByCountry> listTemp = new List<ListDestinationByCountry>();
            ListDestinationByCountry temp = new ListDestinationByCountry();
            foreach (QuocGia qg in dataContext.QuocGias.ToList())
            {
                temp.TenQuocGia = qg.tenQuocGia;
                var dsDiaDiem = (from DiaDiem in dataContext.DiaDiems where DiaDiem.maQuocGia == qg.maQuocGia select DiaDiem).ToList();
                temp.TenDiaDiems = dsDiaDiem;
                listTemp.Add(temp);
            }
            ViewBag.ListDestinationByCountry = listTemp;
            if (Message != "")
                ViewBag.Message = Message;
            if (Error != "")
                ViewBag.Error = Error;
            return View(dv);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Sua(string ID, string destination, string service, string description)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            var dataContext = new Travel_G08DataContext();
            var dv = (from DichVu in dataContext.DichVus where DichVu.maDichVu == int.Parse(ID) select DichVu).Single();
            dv.diaDiem = int.Parse(destination);
            dv.tenDichVu = service;
            dv.gioiThieu = description;
            try
            {
                dataContext.SubmitChanges();
                return RedirectToAction("Sua", new { ID = ID, Message = "Update service successully", Error = ""});
            }
            catch
            {
                return RedirectToAction("Sua", new { ID = ID, Message = "", Error = "Error updating service" });
            }
        }
        public ActionResult Xoa(string ID)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            var dataContext = new Travel_G08DataContext();
            var dv = (from DichVu in dataContext.DichVus where DichVu.maDichVu == int.Parse(ID) select DichVu).Single();
            dataContext.DichVus.DeleteOnSubmit(dv);
            try
            {
                dataContext.SubmitChanges();
                return RedirectToAction("DanhSach");
            }
            catch
            {
                return RedirectToAction("DanhSach");
            }
            
        }
    }
}
