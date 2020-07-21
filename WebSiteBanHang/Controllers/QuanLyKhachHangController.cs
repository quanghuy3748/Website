using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{    
    public class QuanLyKhachHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyKhachHang
        public ActionResult Index()
        {
            return View(db.ThanhViens.Where(n => n.MaLoaiTV == 3 || n.MaLoaiTV == 4));
        }

        [HttpGet]
        public ActionResult ThemThanhVien()
        {            
            return View();
        }

        public ActionResult ThemThanhVien(ThanhVien tv)
        {
            db.ThanhViens.Add(tv);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //Lấy tv cần chỉnh sửa
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
        
        public ActionResult TiemNang()
        {         
            return View();
        }
    }
}