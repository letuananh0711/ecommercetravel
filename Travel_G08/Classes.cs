using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel_G08
{
    public class Classes
    {
        public class ListTour
        {
            public string maTour { get;set;}
            public string tenTour { get; set; }
            public string soNgayDem { get; set; }
            public DateTime ngayThucHien { get; set; }
            public string giaTour { get; set; }
        }
        public class TourInfo
        {
            public string maTour { get; set; }
            public string loaiTour { get; set; }
            public string tenTour { get; set; }
            public string soNgayDem { get; set; }
            public string diemBatDau { get; set; }
            public string ngayThucHien { get; set; }
            public string trangThaiTour { get; set; }
            public string giaTour { get; set; }
            public List<string> danhSachDiaDiem;
            public List<string> danhSachDichVu;
        }
        public class Customer
        {
            public string hoTen { get; set; }
            public string ngaySinh { get; set; }
            public string diaChi { get; set; }
            public string gioiTinh { get; set; }
            public string doTuoi { get; set; }
        }
    }
}