using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class BinhLuanController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: BinhLuan
        [HttpPost]
        public ActionResult Create(int? MaSP, string NoiDung, int? MaTV, DateTime? NgayBL)
        {
            if (MaSP == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP && n.DaXoa == false);

            if (sp == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaSP = MaSP;
            BinhLuan bl = new BinhLuan();
            bl.MaSP = MaSP;
            bl.MaThanhVien = MaTV;
            bl.NoiDungBL = NoiDung;
            bl.NgayBL = NgayBL;
            db.BinhLuans.Add(bl);
            db.SaveChanges();
            return RedirectToAction("XemChiTietSP", "SanPham", new { id = MaSP });
        }

        public ActionResult Index(int? id, int? MaTV)
        {
            ViewBag.MaSP = id;
            ViewBag.MaThanhVien = MaTV;
            return View();
        }       
    }
}