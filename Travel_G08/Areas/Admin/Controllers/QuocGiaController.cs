using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Travel_G08.Areas.Admin.Controllers
{
    public class QuocGiaController : Controller
    {
        //
        // GET: /Admin/QuocGia/
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
            var dsQuocGia = (from QuocGia in dataContext.QuocGias
                             select QuocGia);
            int count = dataContext.QuocGias.Count();
            string base_URL = "DanhSach";
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
            ViewBag.dsQuocGia = dsQuocGia.Skip(start).Take(offset);
            return View();
        }
        public ActionResult Them()
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Them(string country, string description, HttpPostedFileBase image)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            string path = "";
            QuocGia qg = new QuocGia();
            if (image != null)
            {
                path = Path.Combine(Server.MapPath("~/Content/img/QuocGia"), image.FileName);
                qg.hinhAnh = image.FileName;
            }          
            qg.tenQuocGia = country;
            qg.gioiThieu = description;
            
            try
            {
                dataContext.QuocGias.InsertOnSubmit(qg);
                dataContext.SubmitChanges();
                if (image != null)
                {
                    image.SaveAs(path);
                }
                ViewBag.Message = "Add new country successfully";
            }
            catch
            {
                ViewBag.Error = "Error adding country";
            }
            return View();
        }
        public ActionResult Sua(string ID)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            var qg = (from QuocGia in dataContext.QuocGias where QuocGia.maQuocGia == int.Parse(ID) select QuocGia).Single();
            ViewBag.qg = qg;
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Sua(string ID, string country, string description, HttpPostedFileBase image)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            var qg = (from QuocGia in dataContext.QuocGias where QuocGia.maQuocGia == int.Parse(ID) select QuocGia).Single();
            string oldPath = "";
            if (qg.hinhAnh != null)
            {
                oldPath = Path.Combine(Server.MapPath("~/Content/img/QuocGia"), qg.hinhAnh);
            }
            
            try
            {
                if (image != null)
                {
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                    image.SaveAs(Path.Combine(Server.MapPath("~/Content/img/QuocGia"), image.FileName));
                    qg.hinhAnh = image.FileName;
                }
                else
                {
                    qg.tenQuocGia = country;
                    qg.gioiThieu = description;

                }
                dataContext.SubmitChanges();
                ViewBag.Message = "Update country successully";
                ViewBag.qg = qg;
            }
            catch
            {
                ViewBag.Error = "Error updating country";
            }
            return View();
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
            var qg = (from QuocGia in dataContext.QuocGias where QuocGia.maQuocGia == int.Parse(ID) select QuocGia).Single();
            try
            {
                string oldPath = "";
                if (qg.hinhAnh != null)
                {
                    oldPath = Path.Combine(Server.MapPath("~/Content/img/QuocGia"), qg.hinhAnh);
                }
                dataContext.QuocGias.DeleteOnSubmit(qg);
                dataContext.SubmitChanges();
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
                
            }
            catch
            {
            }
            return RedirectToAction("DanhSach");
        }
    }
}
