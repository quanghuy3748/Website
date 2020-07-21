using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    public class NhanVienController : Controller
    {
        // GET: NhanVien
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            return View(db.ThanhViens.Where(n => n.MaLoaiTV == 2));
        }

        [HttpGet]
        public ActionResult ThemNhanVien()
        {
            return View();
        }

        public ActionResult ThemNhanVien(ThanhVien tv)
        {
            db.ThanhViens.Add(tv);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //Lấy nv cần chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (tv == null)
            {
                return HttpNotFound();
            }
            return View(tv);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(ThanhVien model)
        {
            //Nếu dữ liệu chắc chắn ok
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}