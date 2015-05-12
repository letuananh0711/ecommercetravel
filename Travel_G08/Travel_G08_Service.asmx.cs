using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Travel_G08
{
    /// <summary>
    /// Summary description for Travel_G08_Service
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Travel_G08_Service : System.Web.Services.WebService
    {
        [WebMethod(Description = "Lấy danh sách tour theo loại tour, địa điểm xuất phát và địa điểm đến")]
        public List<Classes.ListTour> GetListTour(string loaiTour, string diemXuatPhat, string diemDen)
        {
            var db_context = new Travel_G08DataContext();
            //Tạo list tour
            List<Classes.ListTour> ListTour = new List<Classes.ListTour>();
            //Tạo tour temp
            Classes.ListTour TourTemp = new Classes.ListTour();
            //Lấy tất cả Tour hiện có
            var danhSachTour = from Tour in db_context.Tours
                               select Tour;
            //Nếu có tìm kiếm theo loại tour
            if (loaiTour != "")
            {
                danhSachTour = from Tour in danhSachTour
                               where Tour.loaiTour == loaiTour
                               select Tour;
            }
            if (diemXuatPhat != "")
            {
                danhSachTour = from Tour in danhSachTour
                               from DiaDiem in db_context.DiaDiems
                               where Tour.maDiemBatDau == DiaDiem.maDiaDiem
                               where DiaDiem.tenDiaDiem == diemXuatPhat
                               select Tour;
            }
            if (diemDen != "")
            {
                danhSachTour = from Tour in danhSachTour
                               from DanhSachDiaDiemTour in db_context.DanhSachDiaDiemTours
                               from DiaDiem in db_context.DiaDiems
                               where Tour.maTour == DanhSachDiaDiemTour.maTour
                               where DiaDiem.maDiaDiem == DanhSachDiaDiemTour.maDiaDiem
                               where DiaDiem.tenDiaDiem == diemDen
                               select Tour;
            }
            foreach (var tungTour in danhSachTour)
            {
                TourTemp = new Classes.ListTour();
                TourTemp.maTour = tungTour.maTour.ToString();
                TourTemp.tenTour = tungTour.tenTour;
                TourTemp.soNgayDem = tungTour.soNgayDem;
                //TourTemp.ngayThucHien = DateTime.Parse(tungTour.ngayThucHien.ToString());
                TourTemp.giaTour = tungTour.giaTour.ToString();

                ListTour.Add(TourTemp);
            }
            return ListTour;
        }
        [WebMethod(Description = "Lấy thông tin chi tiết của 1 tour")]
        public Classes.TourInfo GetTourInfo(string maTour)
        {
            Classes.TourInfo TourInfo = new Classes.TourInfo();
            var db_context = new Travel_G08DataContext();

            var tour = from Tour in db_context.Tours
                       where Tour.maTour == int.Parse(maTour)
                       select Tour;
            var diemBatDau = from Tour in db_context.Tours
                             from DiaDiem in db_context.DiaDiems
                             where DiaDiem.maDiaDiem == Tour.maDiemBatDau
                             where Tour.maTour == int.Parse(maTour)
                             select DiaDiem;
            //Lấy danh sách địa điểm
            List<string> listDanhSachDiaDiem = new List<string>();
            var danhSachDiaDiem = from DanhSachDiaDiemTour in db_context.DanhSachDiaDiemTours
                                  where DanhSachDiaDiemTour.maTour == int.Parse(maTour)
                                  select DanhSachDiaDiemTour;
            foreach (var diaDiem in danhSachDiaDiem)
            {
                var tenTungDiaDiem = from DiaDiem in db_context.DiaDiems
                                     where diaDiem.maDiaDiem == DiaDiem.maDiaDiem
                                     select DiaDiem;
                foreach (var temp in tenTungDiaDiem)
                    listDanhSachDiaDiem.Add(temp.tenDiaDiem);
            }

            //Lấy danh sách dịch vụ
            List<string> listDanhSachDichVu = new List<string>();
            var danhSachDichVu = from DanhSachDichVuTour in db_context.DanhSachDichVuTours
                                 where DanhSachDichVuTour.maTour == int.Parse(maTour)
                                 select DanhSachDichVuTour;
            foreach (var dichVu in danhSachDichVu)
            {
                var tenDichVu = from DichVu in db_context.DichVus
                                where dichVu.maDichVu == DichVu.maDichVu
                                select DichVu;
                foreach (var temp in tenDichVu)
                    listDanhSachDichVu.Add(temp.tenDichVu);
            }

            //Lấy thông tin
            TourInfo.maTour = maTour;
            foreach (var temp in tour)
            {
                TourInfo.loaiTour = temp.loaiTour;
                TourInfo.tenTour = temp.tenTour;
                TourInfo.soNgayDem = temp.soNgayDem;
                //TourInfo.ngayThucHien = temp.ngayThucHien.ToShortDateString();
                TourInfo.trangThaiTour = temp.trangThaiTour.ToString();
                TourInfo.giaTour = temp.giaTour.ToString();
            }
            foreach (var temp in diemBatDau)
                TourInfo.diemBatDau = temp.tenDiaDiem;
            TourInfo.danhSachDiaDiem = listDanhSachDiaDiem;
            TourInfo.danhSachDichVu = listDanhSachDichVu;

            return TourInfo;
        }
        [WebMethod(Description = "Tìm kiếm tour nâng cao")]
        public List<Classes.ListTour> SearchTourAdvance(string loaiTour, string fromDate, string toDate, string fromPrice, string toPrice, string diaDiem)
        {
            var db_context = new Travel_G08DataContext();
            //Tạo list tour
            List<Classes.ListTour> ListTour = new List<Classes.ListTour>();
            //Tạo tour temp
            Classes.ListTour TourTemp = new Classes.ListTour();
            //Lấy tất cả danh sác Tour
            var danhSachTour = from Tour in db_context.Tours
                               select Tour;
            //Nếu tìm kiếm theo loại tour
            if (loaiTour != "")
            {
                danhSachTour = from Tour in danhSachTour
                               where Tour.loaiTour == loaiTour
                               select Tour;
            }
            //Nếu tìm kiếm theo ngày
            if (fromDate != "")
            {
                danhSachTour = from Tour in danhSachTour
                               where Tour.ngayThucHien >= DateTime.Parse(fromDate)
                               select Tour;
            }
            if (toDate != "")
            {
                danhSachTour = from Tour in danhSachTour
                               where Tour.ngayThucHien <= DateTime.Parse(toDate)
                               select Tour;
            }
            //Nếu tìm kiếm theo giá
            if (fromPrice != "")
            {
                danhSachTour = from Tour in danhSachTour
                               where Tour.giaTour >= int.Parse(fromPrice)
                               select Tour;
            }
            if (toPrice != "")
            {
                danhSachTour = from Tour in danhSachTour
                               where Tour.giaTour <= int.Parse(toPrice)
                               select Tour;
            }
            //Nếu tìm kiếm theo địa điểm
            if (diaDiem != "")
            {
                var temp = danhSachTour;
                temp = from Tour in danhSachTour
                       from DiaDiem in db_context.DiaDiems
                       where Tour.maDiemBatDau == DiaDiem.maDiaDiem
                       where DiaDiem.tenDiaDiem == diaDiem
                       select Tour;
                if (temp != null)
                {
                    danhSachTour = temp;
                }
                else
                {
                    danhSachTour = from Tour in danhSachTour
                                   from DanhSachDiaDiemTour in db_context.DanhSachDiaDiemTours
                                   from DiaDiem in db_context.DiaDiems
                                   where DanhSachDiaDiemTour.maTour == Tour.maTour
                                   where DanhSachDiaDiemTour.maDiaDiem == DiaDiem.maDiaDiem
                                   where DiaDiem.tenDiaDiem == diaDiem
                                   select Tour;
                }
            }
            foreach (var tungTour in danhSachTour)
            {
                TourTemp = new Classes.ListTour();
                TourTemp.maTour = tungTour.maTour.ToString();
                TourTemp.tenTour = tungTour.tenTour;
                TourTemp.soNgayDem = tungTour.soNgayDem;
                tungTour.ngayThucHien = tungTour.ngayThucHien;
                tungTour.giaTour = tungTour.giaTour;

                ListTour.Add(TourTemp);
            }
            return ListTour;
        }
    }
}
