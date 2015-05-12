using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel_G08.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/Admin/

        public ActionResult Index()
        {
            if (Session["IsLogin"] == null)
            {
                Response.Redirect("Login");
                //return View("Login");
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["IsLogin"] != null)
                return View("Index");
            return View("Login");
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            //Nếu chưa nhập đầy đủ username và password thì báo lỗi
            if (username == null || password == null || username == "" || password == "")
            {
                ViewBag.Error = "Username or password is missing!";
                return View("Login");
            }
            //Nếu nhập đủ
            else
            {
                var dataContext = new Travel_G08DataContext();
                var checkUser = (from TaiKhoan in dataContext.TaiKhoans
                                 from NhanVien in dataContext.NhanViens
                                 where TaiKhoan.maTaiKhoan == NhanVien.maTaiKhoan
                                 where TaiKhoan.email == username
                                 where TaiKhoan.matKhau == password
                                 where TaiKhoan.trangThai == "Mở"
                                 select new { TaiKhoan, NhanVien});
                if (!checkUser.Any())
                {
                    ViewBag.Error = "Wrong username or password!";
                    return View("Login");
                }
                else
                {

                    foreach (var s in checkUser)
                    {
                        if (s.TaiKhoan.LoaiTaiKhoan.maLoaiTaiKhoan == 1 || s.TaiKhoan.LoaiTaiKhoan.maLoaiTaiKhoan == 2)
                        {
                            Session["IsLogin"] = true;
                            Session["Name"] = s.NhanVien.hoTen;
                            if (s.TaiKhoan.LoaiTaiKhoan.maLoaiTaiKhoan == 1)
                                Session["Role"] = "Manager";
                            else
                                if (s.TaiKhoan.LoaiTaiKhoan.maLoaiTaiKhoan == 2)
                                    Session["Role"] = "Staff";
                                else
                                    Session["Role"] = "Customer";
                            return View("Index");
                        }
                        else
                        {
                            ViewBag.Error = "Wrong username or password!";
                            return View("Login");
                        }
                    }
                }
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();
            Response.Redirect("Login");
            return View();
        }
    }
}
