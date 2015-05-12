using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel_G08.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
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
            int count = dataContext.KhachHangs.Count();
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
            return View(dataContext.KhachHangs.Skip(start).Take(offset));
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
            var khachHang = (from KhachHang in dataContext.KhachHangs where KhachHang.maKhachHang == int.Parse(ID) select KhachHang).Single();
            ViewBag.khachHang = khachHang;
            if (Message != "")
                ViewBag.Message = Message;
            if (Error != "")
                ViewBag.Error = Error;
            return View(khachHang);
        }
        [HttpPost]
        public ActionResult Sua(string ID, string name, string address, string phone, string bank_account_id)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            var khachHang = (from KhachHang in dataContext.KhachHangs where KhachHang.maKhachHang == int.Parse(ID) select KhachHang).Single();
            khachHang.hoTen = name;
            khachHang.diaChi = address;
            khachHang.soDienThoai = phone;
            khachHang.soTaiKhoanNganHang = bank_account_id;
            try
            {
                dataContext.SubmitChanges();
                return RedirectToAction("Sua", new { ID = ID, Message = "Update customer successfully", Error = "" });
            }
            catch
            {
                return RedirectToAction("Sua", new { ID = ID, Message = "", Error = "Error updating customer" });
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
            var kh = (from KhachHang in dataContext.KhachHangs where KhachHang.maKhachHang == int.Parse(ID) select KhachHang).Single();
            try
            {
                dataContext.KhachHangs.DeleteOnSubmit(kh);
                try
                {
                    dataContext.SubmitChanges();
                    return RedirectToAction("DanhSach", new { ID = ID, Message = "Delete customer successfully", Error = "" });
                }
                catch
                {
                    return RedirectToAction("DanhSach", new { ID = ID, Message = "", Error = "Error deleting customer" });
                }
            }
            catch
            {
                return RedirectToAction("DanhSach", new { ID = ID, Message = "", Error = "Customer does not exist" });
            }
        }
    }
}
