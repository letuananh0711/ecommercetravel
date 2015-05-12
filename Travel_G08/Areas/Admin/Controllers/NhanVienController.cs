using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel_G08.Areas.Admin.Controllers
{
    public class NhanVienController : Controller
    {
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
            int count = dataContext.NhanViens.Count();
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
            return View(dataContext.NhanViens.Skip(start).Take(offset));          
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
            var nhanVien = (from NhanVien in dataContext.NhanViens where NhanVien.maNhanVien == int.Parse(ID) select NhanVien).Single();
            ViewBag.nhanVien = nhanVien;
            if (Message != "")
                ViewBag.Message = Message;
            if (Error != "")
                ViewBag.Error = Error;
            return View(nhanVien);
        }
        [HttpPost]
        public ActionResult Sua(string ID, string name, string address, string phone)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            var nhanVien = (from NhanVien in dataContext.NhanViens where NhanVien.maNhanVien == int.Parse(ID) select NhanVien).Single();
            nhanVien.hoTen = name;
            nhanVien.diaChi = address;
            nhanVien.soDienThoai = phone;
            try
            {
                dataContext.SubmitChanges();
                return RedirectToAction("Sua", new { ID = ID, Message = "Update employee successfully", Error = "" });
            }
            catch
            {
                return RedirectToAction("Sua", new { ID = ID, Message = "", Error = "Error updating employee" });
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
            var nv = (from NhanVien in dataContext.NhanViens where NhanVien.maNhanVien == int.Parse(ID) select NhanVien).Single();
            try
            {
                dataContext.NhanViens.DeleteOnSubmit(nv);
                try
                {
                    dataContext.SubmitChanges();
                    return RedirectToAction("DanhSach", new { ID = ID, Message = "Delete employee successfully", Error = "" });
                }
                catch
                {
                    return RedirectToAction("DanhSach", new { ID = ID, Message = "", Error = "Error deleting employee" });
                }
            }
            catch
            {
                return RedirectToAction("DanhSach", new { ID = ID, Message = "", Error = "Employee does not exist" });
            }
        }
    }
}
