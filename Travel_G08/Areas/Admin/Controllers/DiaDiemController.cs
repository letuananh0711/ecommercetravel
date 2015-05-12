using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Travel_G08.Areas.Admin.Controllers
{
    public class DiaDiemController : Controller
    {
        //
        // GET: /Admin/DiaDiem/
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
            int count = dataContext.DiaDiems.Count();
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
            return View(dataContext.DiaDiems.Skip(start).Take(offset));
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
            List<QuocGia> dsQG = dataContext.QuocGias.ToList();
            ViewBag.dsQG = new SelectList(dsQG, "maQuocGia", "tenQuocGia");
            if (Message != "")
                ViewBag.Message = Message;
            if (Error != "")
                ViewBag.Error = Error;
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Them(string country, string destination, string description, HttpPostedFileBase image)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            DiaDiem dd = new DiaDiem();
            dd.maQuocGia = int.Parse(country);
            dd.tenDiaDiem = destination;
            dd.gioiThieu = description;
            try
            {           
                if (image != null)
                {
                    dd.hinhAnh = image.FileName;
                    image.SaveAs(Path.Combine(Server.MapPath("~/Content/img/DiaDiem"), image.FileName));
                }
                dataContext.DiaDiems.InsertOnSubmit(dd);
                dataContext.SubmitChanges();
                return RedirectToAction("Them", new { Message = "Add new destination successfully", Error = "" });
            }
            catch
            {
                ViewBag.Error = "Error adding destination";
                return RedirectToAction("Them", new { Message = "", Error = "Error adding destination" });
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
            var dd = (from DiaDiem in dataContext.DiaDiems where DiaDiem.maDiaDiem == int.Parse(ID) select DiaDiem).Single();
            List<QuocGia> dsQG = dataContext.QuocGias.ToList();
            ViewBag.dsQG = dsQG;
            if (Message != "")
                ViewBag.Message = Message;
            if (Error != "")
                ViewBag.Error = Error;
            return View(dd);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Sua(string ID, string country, string destination, string description, HttpPostedFileBase image)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            var dd = (from DiaDiem in dataContext.DiaDiems where DiaDiem.maDiaDiem == int.Parse(ID) select DiaDiem).Single();
            try
            {
                dd.maQuocGia = int.Parse(country);
                dd.tenDiaDiem = destination;
                dd.gioiThieu = description;
                if (image != null)
                {
                    if (dd.hinhAnh != null)
                    {
                        if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Content/img/DiaDiem"), dd.hinhAnh)))
                        {
                            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Content/img/DiaDiem"), dd.hinhAnh));
                        }
                        image.SaveAs(Path.Combine(Server.MapPath("~/Content/img/DiaDiem"), image.FileName));
                    }
                    dd.hinhAnh = image.FileName;
                }
                dataContext.SubmitChanges();
                return RedirectToAction("Sua", new { ID = ID, Message = "Update destination successfully", Error = "" });
            }
            catch
            {
                return RedirectToAction("Sua", new { ID = ID, Message = "", Error = "Error updating destination" });
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
            var dd = (from DiaDiem in dataContext.DiaDiems where DiaDiem.maDiaDiem == int.Parse(ID) select DiaDiem).Single();
            try
            {
                dataContext.DiaDiems.DeleteOnSubmit(dd);
                dataContext.SubmitChanges();
            }
            catch
            {
                ViewBag.Error = "Error deleting destination";
            }
            return RedirectToAction("DanhSach");
        }
    }
}
