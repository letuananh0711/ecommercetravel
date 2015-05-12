using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel_G08.Areas.Admin.Controllers
{
    public class TaiKhoanController : Controller
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
            int count = dataContext.TaiKhoans.Count();
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
            return View(dataContext.TaiKhoans.Skip(start).Take(offset));
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
            var dsLoaiTK = (from LoaiTaiKhoan in dataContext.LoaiTaiKhoans select LoaiTaiKhoan);
            ViewBag.dsLoaiTK = dsLoaiTK;
            if (Message != "")
                ViewBag.Message = Message;
            if (Error != "")
                ViewBag.Error = Error;
            return View();
        }
        [HttpPost]
        public ActionResult Them(string account_type, string email, string password, string confirm_password, string status)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            TaiKhoan tk = new TaiKhoan();
    
            tk.maLoaiTaiKhoan = int.Parse(account_type);
            tk.email = email;
            tk.matKhau = password;
            tk.trangThai = status;

            try
            {
                dataContext.TaiKhoans.InsertOnSubmit(tk);
                dataContext.SubmitChanges();
                var tkMoi = dataContext.TaiKhoans.OrderByDescending(x => x.maTaiKhoan).Take(1).Single();
                
                if (int.Parse(account_type) == 3)
                {
                    KhachHang kh = new KhachHang();
                    kh.maTaiKhoan = tkMoi.maTaiKhoan;
                    kh.hoTen = "unknown";
                    kh.diaChi = "unknown";
                    kh.soDienThoai = "unknown";
                    kh.soTaiKhoanNganHang = "unknown";
                    dataContext.KhachHangs.InsertOnSubmit(kh); 
                }
                else
                {
                    NhanVien nv = new NhanVien();
                    nv.maTaiKhoan = tkMoi.maTaiKhoan;
                    nv.hoTen = "unknown";
                    nv.diaChi = "unknown";
                    nv.soDienThoai = "unknown";
                    dataContext.NhanViens.InsertOnSubmit(nv);
                }

                dataContext.SubmitChanges();
                return RedirectToAction("Them", new { Message = "Add new account successully", Error = "" });           
            }
            catch
            {
                return RedirectToAction("Them", new { Message = "", Error = "Error adding new account" });
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
            var taiKhoan = (from TaiKhoan in dataContext.TaiKhoans where TaiKhoan.maTaiKhoan == int.Parse(ID) select TaiKhoan).Single();
            var dsLoaiTKKhachHang = (from LoaiTaiKhoan in dataContext.LoaiTaiKhoans where LoaiTaiKhoan.maLoaiTaiKhoan == 3 select LoaiTaiKhoan);
            var dsLoaiTKNhanVien = (from LoaiTaiKhoan in dataContext.LoaiTaiKhoans where LoaiTaiKhoan.maLoaiTaiKhoan != 3 select LoaiTaiKhoan);

            ViewBag.taiKhoan = taiKhoan;
            ViewBag.dsLoaiTKKhachHang = dsLoaiTKKhachHang;
            ViewBag.dsLoaiTKNhanVien = dsLoaiTKNhanVien;

            if (Message != "")
                ViewBag.Message = Message;
            if (Error != "")
                ViewBag.Error = Error;
            return View(taiKhoan);
        }
        [HttpPost]
        public ActionResult Sua(string ID, string account_type, string password, string confirm_password, string email, string status)
        {
            //Start phân quyền
            if (!phanquyen.Permission(ControllerContext.RouteData.Values["controller"].ToString(), ControllerContext.RouteData.Values["action"].ToString(), role))
            {
                return View("../PhanQuyen/Error");
            }
            //End phân quyền
            var dataContext = new Travel_G08DataContext();
            var taiKhoan = (from TaiKhoan in dataContext.TaiKhoans where TaiKhoan.maTaiKhoan == int.Parse(ID) select TaiKhoan).Single();
            var nhanVien = (from NhanVien in dataContext.NhanViens where NhanVien.maTaiKhoan == taiKhoan.maTaiKhoan select NhanVien).Single();
            var khachHang = (from KhachHang in dataContext.KhachHangs where KhachHang.maTaiKhoan == taiKhoan.maTaiKhoan select KhachHang).Single();
            var dsLoaiTK = (from LoaiTaiKhoan in dataContext.LoaiTaiKhoans select LoaiTaiKhoan);
            taiKhoan.maLoaiTaiKhoan = int.Parse(account_type);
            taiKhoan.matKhau = password;
            taiKhoan.email = email;
            taiKhoan.trangThai = status;
      
            try
            {
                dataContext.SubmitChanges();
                //if (int.Parse(account_type) == 3)
                //{
                //    KhachHang kh = new KhachHang();
                //    kh.maTaiKhoan = nhanVien.maTaiKhoan;
                //    kh.hoTen = nhanVien.hoTen;
                //    kh.diaChi = nhanVien.diaChi;
                //    kh.soDienThoai = nhanVien.soDienThoai;
                //    kh.soTaiKhoanNganHang = "unknown";
                //    dataContext.KhachHangs.InsertOnSubmit(kh);
                //    dataContext.SubmitChanges();

                //    dataContext.NhanViens.DeleteOnSubmit(nhanVien);
                //    dataContext.SubmitChanges();
                //}
                //else
                //{
                //    NhanVien nv = new NhanVien();
                //    nv.maTaiKhoan = khachHang.maTaiKhoan;
                //    nv.hoTen = khachHang.hoTen;
                //    nv.diaChi = khachHang.diaChi;
                //    nv.soDienThoai = khachHang.soDienThoai;
                //    dataContext.NhanViens.InsertOnSubmit(nv);
                //    dataContext.SubmitChanges();

                //    dataContext.KhachHangs.DeleteOnSubmit(khachHang);
                //    dataContext.SubmitChanges();
                //}           
                return RedirectToAction("Sua", new { ID = ID, Message = "Update account successfully", Error = "" });
            }
            catch
            {
                return RedirectToAction("Sua", new { ID = ID, Message = "", Error = "Error updating account" });
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
            var tk = (from TaiKhoan in dataContext.TaiKhoans where TaiKhoan.maTaiKhoan == int.Parse(ID) select TaiKhoan).Single();
            try
            {         
                dataContext.TaiKhoans.DeleteOnSubmit(tk);
                try
                {
                    dataContext.SubmitChanges();                  
                    return RedirectToAction("DanhSach", new { ID = ID, Message = "Delete account successfully", Error = "" });
                }
                catch
                {
                    return RedirectToAction("DanhSach", new { ID = ID, Message = "", Error = "Error deleting account" });
                }
            }
            catch
            {
                return RedirectToAction("DanhSach", new { ID = ID, Message = "", Error = "Account does not exist" });
            }
        }
    }
}
