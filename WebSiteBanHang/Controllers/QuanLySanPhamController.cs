using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{
    
    public class QuanLySanPhamController  : Controller
    {
        
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            return View(db.SanPhams.Where(n => n.DaXoa == false).OrderByDescending(n => n.MaSP));
        }
        

        [HttpGet]
        public ActionResult TaoMoi()
        {
            //Load dropdownlist nahf cung cấp, loại sản phảm và mã nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp,HttpPostedFileBase[] HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            //Kiểm tra hình ảnh đã tồn tại
            if (HinhAnh[0].ContentLength > 0)
            {
                //Lấy tên hình ảnh
                var fileName = Path.GetFileName(HinhAnh[0].FileName);
                //Lấy hình ảnh chuyển vào thư mục
                var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), fileName);
                //Nếu thư mục chứa hình ảnh đó rồi xuất ra thông báo
                if (System.IO.File.Exists(path))
                {
                    ViewBag.upload = "Hình ảnh đã tồn tại";
                    return View();
                }
                else
                {
                    //Lấy hình ảnh đưa vào HinhAnhSP
                    HinhAnh[0].SaveAs(path);
                    sp.HinhAnh = fileName;
                }              

            }
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //Lấy sản phẩm cần chỉnh sửa
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", model.MaNSX);
            //Nếu dữ liệu chắc chắn ok
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");           
        }

        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }

        [HttpPost]
        public ActionResult Xoa(int id)
        {
            //Lấy sản phẩm cần chỉnh sửa
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham model = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.SanPhams.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SPBanChay()
        {
            //return View(db.SanPhams.Where(n => n.DaXoa == false).OrderByDescending(n => n.MaSP));
            return View(db.SanPhams.OrderByDescending(n => n.SoLanMua));
        }

        public ActionResult TimKiemSP(string sTuKhoa, int? page)
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
            return View(lstSP.OrderBy(n => n.TenSP).ToPagedList(PageNumber, PageSize));
        }

        [HttpPost]
        public ActionResult LayTuKhoa(string sTuKhoa)
        {
            //Gọi hàm về get tìm kiếm
            return RedirectToAction("TimKiemSP", new { @sTuKhoa = sTuKhoa });
        }
    }
}