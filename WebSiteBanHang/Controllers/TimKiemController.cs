using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using PagedList;

namespace WebSiteBanHang.Controllers
{
    public class TimKiemController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: TimKiem
        [HttpGet]
        public ActionResult KQTimKiem(string sTuKhoa, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            //Thực hiện chức năng phân trang
            //Tạo biến số sản phẩm trên trang
            int PageSize = 5;
            //Tạo biến thứ 2: Số trang hiện tại
            int PageNumber = (page ?? 1);
            //Tìm kiếm theo tên sản phẩm
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return View(lstSP.OrderBy(n=>n.TenSP).ToPagedList(PageNumber, PageSize));
        }

        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string sTuKhoa)
        {
            //Gọi hàm về get tìm kiếm
            return RedirectToAction("KQTimKiem", new { @sTuKhoa = sTuKhoa});
        }

        public ActionResult KQTimKiemPartial(string sTuKhoa)
        {
            var lstsanpham = db.SanPhams.Where(s => s.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return PartialView(lstsanpham.OrderBy(s => s.DonGia));
        }
    }
}