using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        
        public ActionResult Index()
        {
            //Lấy toàn bộ dữ liệu KH thông qua câu lệnh
            //Cách 1: lấy dữ liệu 1 danh sách khách hàng
          //var lstKH = from kh in db.KhachHangs select kh;
            //Cách 2:dùng phương thức hỗ trợ sẵn
            var lstKH = db.KhachHangs;
            return View(lstKH);
        }
        
        public ActionResult Index1()
        {
            var lstKH = from kh in db.KhachHangs select kh;
            return View(lstKH);
        }

        public ActionResult TruyVanDoiTuong()
        {
            //truy vấn 1 đối tượng
            var lstKH = from kh in db.KhachHangs where kh.MaKH ==1 select kh;
            //KhachHang khang = lstKH.FirstOrDefault();
            KhachHang khang = db.KhachHangs.SingleOrDefault(n => n.MaKH == 1);
            return View(khang);
        }
        public ActionResult SortDuLieu()
        {
            var lstKH = db.KhachHangs.OrderBy(n => n.TenKH).ToList();
            return View(lstKH);
        }
        public ActionResult GroupDuLieu()
        {
           List<ThanhVien> lstKH = db.ThanhViens.OrderBy(n => n.TaiKhoan).ToList();
            return View(lstKH);
        }
    }
}