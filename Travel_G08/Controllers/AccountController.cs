using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Travel_G08.Models;

namespace Travel_G08.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();  
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var dataContext = new Travel_G08DataContext();

            if (email == null || password == null || email == "" || password == "")
            {
                ViewBag.Error = "Email or password is missing!";
            }

            else
            {     
                var checkUser = (from TaiKhoan in dataContext.TaiKhoans
                                 from NhanVien in dataContext.NhanViens
                                 from KhachHang in dataContext.KhachHangs
                                 where TaiKhoan.maTaiKhoan == NhanVien.maTaiKhoan || TaiKhoan.maTaiKhoan == KhachHang.maTaiKhoan
                                 where TaiKhoan.email == email
                                 where TaiKhoan.matKhau == password
                                 where TaiKhoan.trangThai == "Mở"
                                 select new {TaiKhoan, NhanVien, KhachHang});
                if (!checkUser.Any())
                {
                    ViewBag.Error = "Wrong email or password! Please try again!";
                }
                else
                {
                    foreach (var s in checkUser)
                    {
                        //Nếu là khách hàng
                        if (s.TaiKhoan.maLoaiTaiKhoan == 3)
                        {
                            Session["IsLogin"] = true;
                            Session["ID"] = s.KhachHang.maKhachHang;
                            Session["Name"] = s.KhachHang.hoTen;
                            Session["Email"] = s.TaiKhoan.email;
                            Session["Address"] = s.KhachHang.diaChi; 
                            Session["Phone"] = s.KhachHang.soDienThoai;
                            Session["BankAccountID"] = s.KhachHang.soTaiKhoanNganHang;
                            
                            Session["Role"] = "Customer";

                            if (Session["CurrentUrl"] == null)
                                return RedirectToAction("Index", "Home");
                            else
                                return Redirect(Session["CurrentUrl"].ToString());
                        }

                        //Nếu là quản lý hoặc nhân viên
                        if (s.TaiKhoan.maLoaiTaiKhoan == 1 || s.TaiKhoan.maTaiKhoan == 2)
                        {
                            Session["IsLogin"] = true;
                            Session["Name"] = s.NhanVien.hoTen;
                            if (s.TaiKhoan.maLoaiTaiKhoan == 1)
                                Session["Role"] = "Manager";
                            if (s.TaiKhoan.maLoaiTaiKhoan == 2)
                                Session["Role"] = "Staff";
                                                      
                            if (Session["CurrentUrl"] == null)
                                return RedirectToAction("Index", "Home");
                            else
                                return Redirect(Session["CurrentUrl"].ToString());
                        }

                        else
                        {
                            ViewBag.Error = "Wrong email or password! Please try again!";
                        }
                    }
                }   
            }

            var checkUserConfirm = (from TaiKhoan in dataContext.TaiKhoans
                                    where TaiKhoan.email == email
                                    where TaiKhoan.matKhau == password
                                    where TaiKhoan.trangThai == "Chưa xác nhận"
                                    select TaiKhoan);
            if (checkUserConfirm.Any())
            {
                ViewBag.Error = "Account hasn't been confirmed yet ! Please wait the administrator to confirm !";
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string email, string password, string confirm_password, string name, string address, string phone, string bank_account_ID)
        {
            var dataContext = new Travel_G08DataContext();
            TaiKhoan tk = new TaiKhoan();

            //Là khách hàng
            tk.maLoaiTaiKhoan = 3;
            tk.email = email;
            tk.matKhau = password;

            //Khách hàng chờ admin xác nhận
            tk.trangThai = "Chưa xác nhận";

            try
            {
                dataContext.TaiKhoans.InsertOnSubmit(tk);
                dataContext.SubmitChanges();
                var tkMoi = dataContext.TaiKhoans.OrderByDescending(x => x.maTaiKhoan).Take(1).Single();

                KhachHang kh = new KhachHang();
                kh.maTaiKhoan = tkMoi.maTaiKhoan;
                kh.hoTen = name;
                kh.diaChi = address;
                kh.soDienThoai = phone;
                kh.soTaiKhoanNganHang = bank_account_ID;

                if (address == null || phone == null || bank_account_ID == null || address == "" || phone == "" || bank_account_ID == "")
                {
                    kh.diaChi = "unknown";
                    kh.soDienThoai = "unknown";
                    kh.soTaiKhoanNganHang = "unknown";
                }

                dataContext.KhachHangs.InsertOnSubmit(kh);
                dataContext.SubmitChanges();

                ViewBag.Message = "Registering new account is complete! Please wait the administrator to confirm !";
            }
            catch
            {
                ViewBag.Error = "Error registering new account ! Please try again !";
            }   
            return View();
        }

        //public ActionResult MyFirstAction()
        //{
        //    return RedirectToAction("MyNextAction",
        //        new { r = Request.Url.ToString() });
        //}

        //public ActionResult MyNextAction()
        //{
        //    return Redirect(Request.QueryString["r"]);
        //}

        //public ActionResult Create(string returnUrl)
        //{
        //    // If no return url supplied, use referrer url.
        //    // Protect against endless loop by checking for empty referrer.
        //    if (String.IsNullOrEmpty(returnUrl)
        //        && Request.UrlReferrer != null
        //        && Request.UrlReferrer.ToString().Length > 0)
        //    {
        //        return RedirectToAction("Create",
        //            new { returnUrl = Request.UrlReferrer.ToString() });
        //    }

        //    // Do stuff...
        //    //MyEntity entity = GetNewEntity();

        //    //return View(entity);
        //    return View();
        //}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Create(MyEntity entity, string returnUrl)
        //{
        //    try
        //    {
        //        // TODO: add create logic here

        //        // If redirect supplied, then do it, otherwise use a default
        //        if (!String.IsNullOrEmpty(returnUrl))
        //            return Redirect(returnUrl);
        //        else
        //            return RedirectToAction("Index", "Home");
        //    }
        //    catch
        //    {
        //        return View();  // Reshow this view, with errors
        //    }
        //}

        public ActionResult AccountDetailInfo(string ID)
        {
            if (Session["Role"] == "Manager" || Session["Role"] == "Staff")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var dataContext = new Travel_G08DataContext();
                var khachHang = (from kh1 in dataContext.KhachHangs 
                                 from kh2 in dataContext.TaiKhoans
                                     where kh1.maTaiKhoan == kh2.maTaiKhoan
                                     && kh1.maKhachHang == int.Parse(ID)
                                     select kh1).Single();

                var danhSachPhieuDatTourKH = (from ds in dataContext.PhieuDatTours
                                              where ds.maKhachHang == int.Parse(ID)
                                              select ds).ToList();

                ViewBag.danhSachPhieuDatTourKH = danhSachPhieuDatTourKH;
                return View(khachHang);
            }
        }

        public ActionResult BookingDetailInfo(string ID)
        {
            return RedirectToAction("BookTourInfo", "Tours", new {ID = ID});
        }
    }
}
