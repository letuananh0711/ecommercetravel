using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel_G08.Areas.Admin.Controllers
{
    public class PhieuDatTourController : Controller
    {
        //
        // GET: /Admin/PhieuDatTour/
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
            int count = dataContext.PhieuDatTours.Count();
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
            return View(dataContext.PhieuDatTours.Skip(start).Take(offset));
        }

        public ActionResult DanhSachNguoiThamGiaTour(string ID)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            var danhSach = (from ds in dataContext.DanhSachNguoiThamGiaTours
                        where ds.maPhieuDatTour == int.Parse(ID)
                        select ds).ToList();

            return View(danhSach);
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
            var pdt = (from PhieuDatTour in dataContext.PhieuDatTours where PhieuDatTour.maPhieuDatTour == int.Parse(ID) select PhieuDatTour).Single();
            ViewBag.PhieuDatTour = pdt;

            if (Message != "")
                ViewBag.Message = Message;
            if (Error != "")
                ViewBag.Error = Error;
            return View(pdt);
        }
        [HttpPost]
        public ActionResult Sua(string ID, string status)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            var pdt = (from PhieuDatTour in dataContext.PhieuDatTours where PhieuDatTour.maPhieuDatTour == int.Parse(ID) select PhieuDatTour).Single();

            pdt.trangThai = status;
            try
            {
                dataContext.SubmitChanges();
                return RedirectToAction("Sua", new { ID = ID, Message = "Update booking tour successfully", Error = "" });
            }
            catch
            {
                return RedirectToAction("Sua", new { ID = ID, Message = "", Error = "Error updating booking tour" });
            }
        }

    }
}
