using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: ThongKe
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Lấy số lượng người truy cập
            ViewBag.SoNguoiDangOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();//Lấy số lượng người online
            ViewBag.TongDoanhThu = ThongKeTongDoanhThu();//Lấy số lượng người online
            ViewBag.TongDDH = ThongKeDonHang();
            ViewBag.TongThanhVien = ThongKeThanhVien();
            return View();
        }

        public decimal ThongKeTongDoanhThu()
        {
            //Thống kê theo tất cả doanh thu từ khi website thành lập
            decimal TongDoanhThu = db.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value;
            return TongDoanhThu;
        }

        public double ThongKeDonHang()
        {
            //Đếm đơn đặt hàng
            double slDDH = db.DonDatHangs.Count();
            return slDDH;
        }


        public double ThongKeThanhVien()
        {
            //Đếm đơn đặt hàng
            double slTV = db.ThanhViens.Count();
            return slTV;
        }

        public decimal ThongKeDoanhThuThang(int Thang, int Nam)
        {
            //Thống kê theo tất cả doanh thu từ khi website thành lập
            //List ra những đơn đặt hàng có tháng năm tương ứng
            var lstDDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Month == Thang &&
            n.NgayDat.Value.Year == Nam);
            decimal TongTien = 0;
            //Duyệt  chi tiết của đơn đặt hàng đó và lấy tổng tiền của tất cả sản phẩm đó
            foreach (var item in lstDDH)
            {
                TongTien += decimal.Parse(item.ChiTietDonDatHangs.Sum(n => n.SoLuong * n.DonGia).Value.ToString());
            }
            return TongTien;

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}