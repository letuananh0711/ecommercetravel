using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel_G08.Areas.Admin.Controllers
{
    public class PhanQuyenController : Controller
    {
        //
        // GET: /Admin/PhanQuyen/
        public ActionResult Index()
        {
            return View();
        }
        public bool Permission(string controller, string action, string role)
        {
            if (controller == "" || action == "" || controller == null || action == null)
            {
                Response.Redirect("Error");
            }
            switch (role)
            {
                case "Manager":
                    return Permission_Manager(controller, action);
                case "Staff":
                    return Permission_Staff(controller, action);
                case "Customer":
                    return false;
                default:
                    return false;
            }
        }
        public bool Permission_Manager(string controller, string action)
        {
            List<string> dsAction = new List<string>();
            dsAction.Add("TaiKhoan/DanhSach");
            dsAction.Add("TaiKhoan/Sua");
            dsAction.Add("TaiKhoan/Them");
            dsAction.Add("KhachHang/DanhSach");
            dsAction.Add("KhachHang/Sua");
            dsAction.Add("NhanVien/DanhSach");
            dsAction.Add("NhanVien/Sua");

            foreach (var s in dsAction)
            {
                string[] check = s.Split('/');
                if (check[0] == controller && check[1] == action)
                    return true;
            }
            return false;
        }
        public bool Permission_Staff(string controller, string action)
        {
            List<string> dsAction = new List<string>();
            dsAction.Add("DiaDiem/DanhSach");
            dsAction.Add("DiaDiem/Sua");
            dsAction.Add("DiaDiem/Them");
            dsAction.Add("DichVu/DanhSach");
            dsAction.Add("DichVu/Sua");
            dsAction.Add("DichVu/Them");
            dsAction.Add("NhaCungCap/DanhSach");
            dsAction.Add("NhaCungCap/Sua");
            dsAction.Add("NhaCungCap/Them");
            dsAction.Add("QuocGia/DanhSach");
            dsAction.Add("QuocGia/Sua");
            dsAction.Add("QuocGia/Them");
            dsAction.Add("Tour/DanhSach");
            dsAction.Add("Tour/Sua");
            dsAction.Add("Tour/Them");
            dsAction.Add("TourMau/DanhSach");
            dsAction.Add("TourMau/Sua");
            dsAction.Add("TourMau/Them");
            dsAction.Add("TourMau/ThemTemplate");
            dsAction.Add("TourMau/ThemManual");
            dsAction.Add("PhieuDatTour/DanhSach");
            dsAction.Add("PhieuDatTour/DanhSachNguoiThamGiaTour");
            dsAction.Add("PhieuDatTour/Sua");
            foreach (var s in dsAction)
            {
                string[] check = s.Split('/');
                if (check[0] == controller && check[1] == action)
                    return true;
            }
            return false;
        }
    }
}
