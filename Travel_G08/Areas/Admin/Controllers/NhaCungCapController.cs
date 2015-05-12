using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel_G08.Areas.Admin.Controllers
{
    public class NhaCungCapController : Controller
    {
        //
        // GET: /Admin/NhaCungCap/
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
            int count = dataContext.NCCDichVus.Count();
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
            return View(dataContext.NCCDichVus.Skip(start).Take(offset));
        }
        public ActionResult Them(string Message, string Error)
        {
            var dataContext = new Travel_G08DataContext();
            if (Message != "")
                ViewBag.Message = Message;
            if (Error != "")
                ViewBag.Error = Error;
            return View(dataContext.LoaiNCCDichVus);
        }
        [HttpPost]
        public ActionResult Them(string type, string name, string webservice, string username, string password)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            NCCDichVu temp = new NCCDichVu();
            temp.maLoaiNCC = int.Parse(type);
            temp.tenNCC = name;
            temp.diaChiWS = webservice;
            temp.username = username;
            temp.password = password;
            try
            {
                dataContext.NCCDichVus.InsertOnSubmit(temp);
                dataContext.SubmitChanges();
                return RedirectToAction("Them", new { Message = "Add new supplier successfully!", Error = ""});
            }
            catch
            {
                return RedirectToAction("Them", new { Message = "", Error = "Error adding new supplier!"});
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
            var NCC = (from NCCDichVu in dataContext.NCCDichVus where NCCDichVu.maNCC == int.Parse(ID) select NCCDichVu).Single();
            ViewBag.NCC = NCC;
            return View(dataContext.LoaiNCCDichVus);
        }
        [HttpPost]
        public ActionResult Sua(string ID, string type, string name, string webservice, string username, string password)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            var NCC = (from NCCDichVu in dataContext.NCCDichVus where NCCDichVu.maNCC == int.Parse(ID) select NCCDichVu).Single();
            NCC.maLoaiNCC = int.Parse(type);
            NCC.tenNCC = name;
            NCC.diaChiWS = webservice;
            NCC.username = username;
            NCC.password = password;
            try
            {
                dataContext.SubmitChanges();
                return RedirectToAction("Sua", new { ID = ID, Message = "Update supplier successully!", Error = ""});
            }
            catch
            {
                return RedirectToAction("Sua", new { ID = ID, Message = "", Error = "Error updating supplier!" });
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
            var NCC = (from NCCDichVu in dataContext.NCCDichVus where NCCDichVu.maNCC == int.Parse(ID) select NCCDichVu).Single();
            dataContext.NCCDichVus.DeleteOnSubmit(NCC);
            try
            {
                dataContext.SubmitChanges();
                
            }
            catch
            {
            }
            return RedirectToAction("DanhSach");
        }
    }
}
