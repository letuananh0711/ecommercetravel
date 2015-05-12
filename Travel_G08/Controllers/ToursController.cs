using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace Travel_G08.Controllers
{
    public class ToursController : Controller
    {
        //
        // GET: /Tours/
		public struct _KhachHang
        {
            public string hoTen { get;set; }
            public DateTime ngaySinh { get; set; }
            public string diaChi { get; set; }
            public string gioiTinh { get; set; }
            public string doTuoi { get; set; }
        }
        public ActionResult Index()
        {
            var dataContext = new Travel_G08DataContext();
            var tourhot = (from Tour in dataContext.Tours select Tour).Take(8).ToList();
            var othertour = (from Tour in dataContext.Tours select Tour);
            
            List<List<Tour>> ds2Tour = new List<List<Tour>>();
            int i = 0;
            List<Tour> temp = new List<Tour>();
            for (int j = 0; j < tourhot.Count(); j++)
            {
                temp.Add(tourhot[j]);
                
                i++;
                if (i == 2)
                {
                    i = 0;
                    ds2Tour.Add(temp);
                    temp = new List<Tour>();
                }
                else
                {
                    if (j == tourhot.Count() - 1)
                    {
                        ds2Tour.Add(temp);
                        temp = new List<Tour>();
                    }
                }
            }

            ViewBag.OtherTour = othertour;
            ViewBag.DS2Tours = ds2Tour;

            return View();
        }

        public ActionResult CityTours()
        {
            var dataContext = new Travel_G08DataContext();
            var tourhot = (from Tour in dataContext.Tours where Tour.loaiTour == "City tour - One day tour" select Tour).Take(8).ToList();
            var othertour = (from Tour in dataContext.Tours where Tour.loaiTour == "City tour - One day tour" select Tour);
            
            List<List<Tour>> ds2Tour = new List<List<Tour>>();
            int i = 0;
            List<Tour> temp = new List<Tour>();
            for (int j = 0; j < tourhot.Count(); j++)
            {
                temp.Add(tourhot[j]);

                i++;
                if (i == 2)
                {
                    i = 0;
                    ds2Tour.Add(temp);
                    temp = new List<Tour>();
                }
                else
                {
                    if (j == tourhot.Count() - 1)
                    {
                        ds2Tour.Add(temp);
                        temp = new List<Tour>();
                    }
                }
            }

            ViewBag.OtherTour = othertour;
            ViewBag.DS2Tours = ds2Tour;

            return View();
        }

        public ActionResult DomesticTours()
        {
            var dataContext = new Travel_G08DataContext();
            var tourhot = (from Tour in dataContext.Tours where Tour.loaiTour == "Tour trong nước" select Tour).Take(8).ToList();
            var othertour = (from Tour in dataContext.Tours where Tour.loaiTour == "Tour trong nước" select Tour);

            List<List<Tour>> ds2Tour = new List<List<Tour>>();
            int i = 0;
            List<Tour> temp = new List<Tour>();
            for (int j = 0; j < tourhot.Count(); j++)
            {
                temp.Add(tourhot[j]);

                i++;
                if (i == 2)
                {
                    i = 0;
                    ds2Tour.Add(temp);
                    temp = new List<Tour>();
                }
                else
                {
                    if (j == tourhot.Count() - 1)
                    {
                        ds2Tour.Add(temp);
                        temp = new List<Tour>();
                    }
                }
            }

            ViewBag.OtherTour = othertour;
            ViewBag.DS2Tours = ds2Tour;

            return View();
        }

        public ActionResult InternationalTours()
        {
            var dataContext = new Travel_G08DataContext();
            var tourhot = (from Tour in dataContext.Tours where Tour.loaiTour == "Tour quốc tế" select Tour).Take(8).ToList();
            var othertour = (from Tour in dataContext.Tours where Tour.loaiTour == "Tour quốc tế" select Tour);
            //var _tourhot = (from Tour in dataContext.Tours where Tour.loaiTour == "Tour quốc tế" select Tour).Take(8);

            List<List<Tour>> ds2Tour = new List<List<Tour>>();
            int i = 0;
            List<Tour> temp = new List<Tour>();
            for (int j = 0; j < tourhot.Count(); j++)
            {
                temp.Add(tourhot[j]);

                i++;
                if (i == 2)
                {
                    i = 0;
                    ds2Tour.Add(temp);
                    temp = new List<Tour>();
                }
                else
                {
                    if (j == tourhot.Count() - 1)
                    {
                        ds2Tour.Add(temp);
                        temp = new List<Tour>();
                    }
                }
            }

            ViewBag.OtherTour = othertour;
            ViewBag.DS2Tours = ds2Tour;
            //ViewBag.TourHot = _tourhot;

            return View();
        }

        public ActionResult BookTour(string slug)
        {
            var dataContext = new Travel_G08DataContext();
            Tour tour = (from Tour in dataContext.Tours
                         where Tour.slug == slug
                         select Tour).Single();
            ViewBag.BookTour = tour;

            //Nếu là manager hoặc staff thì không thể vào trang book tour
            if (Session["Role"] == "Manager" || Session["Role"] == "Staff")
            {
                return RedirectToAction("Index", "Home");
            }
            
            //Nếu là customer: login hoặc chưa login đều vào được trang book tour
            else
            {
                return View(tour);
            }
        }
        [HttpPost]
        public ActionResult BookTour(string maTour, string maKhachHang,
                                        string txtHoTen, string txtDiaChi, string txtDTDiDong, string txtEmail, string txtTIN,
                                        string txt_soNguoiLon, string txt_soTreEm, string txt_soTreNho, string txt_tongSoKhach, string tongTien, string txtGhiChu, string dsTongKet,
                                        string phieuDatTourMoi)
        {
            string[] dsKhachHang = dsTongKet.Split('|');
            //Biến _KhachHangs chứa danh sách các người tham gia tour
            List<_KhachHang> _KhachHangs = new List<_KhachHang>();
            _KhachHang temp;
            foreach (var infoKH in dsKhachHang)
            {
                if (infoKH == "")
                    continue;
                temp = new _KhachHang();
                string[] info = infoKH.Split('@');
                temp.hoTen = info[0];
                temp.ngaySinh = DateTime.Parse(info[1]);
                temp.diaChi = info[2];
                temp.gioiTinh = info[3];
                temp.doTuoi = info[4];
                _KhachHangs.Add(temp);
            }

            var dataContext = new Travel_G08DataContext();
            Tour tour = (from Tour in dataContext.Tours
                         where Tour.maTour == int.Parse(maTour)
                         select Tour).Single();

            PhieuDatTour pdt = new PhieuDatTour();

            pdt.maTour = int.Parse(maTour);
            pdt.ngayDat = DateTime.Now;
            pdt.soNguoiLon = int.Parse(txt_soNguoiLon);
            pdt.soTreEm = int.Parse(txt_soTreEm);
            pdt.soTreNho = int.Parse(txt_soTreNho);

            float _giaTour1Nguoi = float.Parse(tour.giaTour.ToString());
            float _tongTien = int.Parse(txt_tongSoKhach) * _giaTour1Nguoi;
            pdt.tongTien = _tongTien;

            pdt.trangThai = "Chưa xác nhận";

            pdt.ghiChu = txtGhiChu;

            //Kiểm tra đã login hay chưa
            //Đã login thì ghi mã khách hàng xuống phiếu đặt tour
            if (Session["IsLogin"] != null)
            {
                //Điền thông tin mã khách hàng vào phiếu đặt tour
                pdt.maKhachHang = int.Parse(maKhachHang);

                //Update thông tin khách hàng nếu có sự thay đổi
                var khCu = (from KhachHang in dataContext.KhachHangs where KhachHang.maKhachHang == int.Parse(maKhachHang) select KhachHang).Single();
                khCu.hoTen = txtHoTen;
                khCu.diaChi = txtDiaChi;
                khCu.soDienThoai = txtDTDiDong;
                khCu.soTaiKhoanNganHang = txtTIN;

                if (khCu.diaChi == null || khCu.soTaiKhoanNganHang == null || khCu.diaChi == "" || khCu.soTaiKhoanNganHang == "")
                {
                    khCu.diaChi = "unknown";
                    khCu.soTaiKhoanNganHang = "unknown";
                }
                
                try
                {
                    dataContext.SubmitChanges();
                    //ViewBag.Message = "Update acccount successfully";
                }
                catch
                {
                    //ViewBag.Error = "Error updating account";
                }
            }

            //Nếu chưa login thì đăng ký mã khách hàng mới
            else
            {
                //Tạo tài khoản mới
                TaiKhoan tk = new TaiKhoan();
                tk.maLoaiTaiKhoan = 3;
                tk.email = txtEmail;
                tk.matKhau = "unknown";
                tk.trangThai = "Chưa xác nhận";

                try
                {
                    dataContext.TaiKhoans.InsertOnSubmit(tk);
                    dataContext.SubmitChanges();
                    var tkMoi = dataContext.TaiKhoans.OrderByDescending(x => x.maTaiKhoan).Take(1).Single();

                    //Tạo khách hàng mới
                    KhachHang khMoi = new KhachHang();
                    khMoi.maTaiKhoan = tkMoi.maTaiKhoan;
                    khMoi.hoTen = txtHoTen;
                    khMoi.diaChi = txtDiaChi;
                    khMoi.soDienThoai = txtDTDiDong;
                    khMoi.soTaiKhoanNganHang = txtTIN;

                    if (khMoi.diaChi == null || khMoi.soTaiKhoanNganHang == null || khMoi.diaChi == "" || khMoi.soTaiKhoanNganHang == "")
                    {
                        khMoi.diaChi = "unknown";
                        khMoi.soTaiKhoanNganHang = "unknown";
                    }   

                    dataContext.KhachHangs.InsertOnSubmit(khMoi);
                    dataContext.SubmitChanges();

                    //Điền thông tin mã khách hàng vừa được tạo mới vào phiếu đặt tour
                    var _khMoi = dataContext.KhachHangs.OrderByDescending(x => x.maKhachHang).Take(1).Single();
                    pdt.maKhachHang = _khMoi.maKhachHang;
                }
                catch
                {
                }
            }

            try
            {
                dataContext.PhieuDatTours.InsertOnSubmit(pdt);
                dataContext.SubmitChanges();
                var pdtMoi = dataContext.PhieuDatTours.OrderByDescending(x => x.maPhieuDatTour).Take(1).Single();
                int maPhieu = pdtMoi.maPhieuDatTour;
                DanhSachNguoiThamGiaTour dsTemp;
                foreach (var s in _KhachHangs)
                {
                    dsTemp = new DanhSachNguoiThamGiaTour();
                    dsTemp.maPhieuDatTour = maPhieu;
                    dsTemp.hoTenKhachHang = s.hoTen;
                    dsTemp.ngaySinh = s.ngaySinh;
                    dsTemp.diaChi = s.diaChi;
                    dsTemp.gioiTinh = s.gioiTinh;
                    dsTemp.doTuoi = s.doTuoi;
                    dataContext.DanhSachNguoiThamGiaTours.InsertOnSubmit(dsTemp);
                }
                dataContext.SubmitChanges();
                //Thông báo thành công
                //................

                phieuDatTourMoi = pdtMoi.maPhieuDatTour.ToString();
            }
            catch
            {
            }        
            ViewBag.BookTour = tour;
            return RedirectToAction("BookTourInfo", new { ID = phieuDatTourMoi });

        }

        public ActionResult BookTourInfo(string ID)
        {
            var dataContext = new Travel_G08DataContext();
            PhieuDatTour pdt = (from PhieuDatTour in dataContext.PhieuDatTours
                                where PhieuDatTour.maPhieuDatTour == int.Parse(ID)
                                select PhieuDatTour).Single();

            //DanhSachNguoiThamGiaTour ds = (from DanhSachNguoiThamGiaTour in dataContext.DanhSachNguoiThamGiaTours
            //                               where DanhSachNguoiThamGiaTour.maPhieuDatTour == pdt.maPhieuDatTour
            //                               select DanhSachNguoiThamGiaTour).ToList();
            IQueryable<DanhSachNguoiThamGiaTour> ds =(from DanhSachNguoiThamGiaTour in dataContext.DanhSachNguoiThamGiaTours
                                          where DanhSachNguoiThamGiaTour.maPhieuDatTour == pdt.maPhieuDatTour
                                         select DanhSachNguoiThamGiaTour);
            List<DanhSachNguoiThamGiaTour> listKH = ds.ToList();

            int count = listKH.Count();

            ViewBag.ListPassengers = listKH;
            ViewBag.count = count;
            ViewBag.BookTourInfo = pdt;


            return View(pdt);
        }

        //Xem chi tiết thông tin tour
        public ActionResult TourDetailInfo(string slug)
        {
            var dataContext = new Travel_G08DataContext();
            Tour tour = (from Tour in dataContext.Tours
                         where Tour.slug == slug
                         select Tour).Single();

            ViewBag.TourDetailInfo = tour;
            return View(tour);
        }

        public ActionResult TourDetailService(string slug)
        {
            var dataContext = new Travel_G08DataContext();
            Tour tour = (from Tour in dataContext.Tours
                         where Tour.slug == slug
                         select Tour).Single();

            //float giaTourTreEm, giaTourTreNho;
            //giaTourTreEm
            double giaTourTreEm = Convert.ToDouble(tour.giaTour) * 70 / 100;
            double giaTourTreNho = Convert.ToDouble(tour.giaTour) * 50 / 100;

            ViewBag.TourDetailService = tour;
            ViewBag.PriceTourChildren = giaTourTreEm;
            ViewBag.PriceTourKid = giaTourTreNho;

            return View(tour);
        }

        public ActionResult PostToPaypal(string tour_name, string amount)
        {
            Travel_G08.Models.Paypal paypal = new Models.Paypal();

            paypal.cmd = "_xclick";
            paypal.business = ConfigurationManager.AppSettings["BusinessAccountKey"];

            bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSandbox"]);
            if (useSandbox)
            {
                ViewBag.actionURL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            }
            else
            {
                ViewBag.actionURL = "https://www.paypal.com/cgi-bin/webscr";
            }

            paypal.cancel_return = System.Configuration.ConfigurationManager.AppSettings["CancelURL"];
            paypal.@return = ConfigurationManager.AppSettings["ReturnURL"];
            paypal.notify_url = ConfigurationManager.AppSettings["NotifyURL"];

            paypal.currency_code = ConfigurationManager.AppSettings["CurrencyCode"];

            paypal.item_name = tour_name;
            paypal.amount = amount;

            return View(paypal);
        }

        public ActionResult ReturnFromPaypal()
        {
            
            return View();
        }

        public ActionResult CancelFromPaypal()
        {

            return View();
        }
    }
}
