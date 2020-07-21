using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class QLDonHangController : Controller
    {
        // GET: QLDonHang
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult DonHang()
        {
            return View();
        }

        public ActionResult DangXuLy(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
            DonDatHang dh = db.DonDatHangs.SingleOrDefault(n => n.MaKH == id && n.DaThanhToan == false && n.TinhTrangGiaoHang == false);
            if (dh == null)
            {
                return HttpNotFound();
            }
            return View(dh);
        }

        public ActionResult DangVanChuyen(int? id)
        {
            return View();
        }

        public ActionResult ThanhCong(int? id)
        {
            return View();
        }
    }
}