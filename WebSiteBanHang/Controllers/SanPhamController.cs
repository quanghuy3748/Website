using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using PagedList;

namespace WebSiteBanHang.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        //Sử dụng partial view
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //Tạo 2 partial view sản phẩm 1 và 2 để hiển thị sản phẩnm
        [ChildActionOnly]
        public ActionResult DTMoiNhatPartial()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult LTMoiNhatPartial()
        {
            return PartialView();
        }

        public ActionResult DanhMucPartial()
        {
            return PartialView();
        }

        public ActionResult SPChayNhatPartial()
        {
            var lstSP = db.SanPhams.Max(n => n.SoLanMua);
            return PartialView(lstSP);
        }

        public ActionResult ThuongHieuPartial()
        {
            return PartialView();
        }

        //Xây dựng action load sản phẩm theo mã loại sản phẩm và mã nhà sản xuất
        public ActionResult SanPham(int? MaloaiSP, int? MaNSX, int? page)
        {
            //if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            if (MaloaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Load sản phẩm dựa theo 2 tiêu chí là Mã loại sản phẩm và Mã nhà sản xuất
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaloaiSP && n.MaNSX == MaNSX);
            if (lstSP.Count() == 0)
            {
                return HttpNotFound();
            }
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            //Thực hiện chức năng phân trang
            //Tạo biến số sản phẩm trên trang
            int PageSize = 6;
            //Tạo biến thứ 2: Số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaloaiSP;
            ViewBag.MaNSX = MaNSX;
            return View(lstSP.OrderBy(n => n.MaSP).ToPagedList(PageNumber, PageSize));
        }

        public ActionResult XemChiTietSP(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);

            if (sp == null)
            {
                return HttpNotFound();
            }          
            return View(sp);
        }
    }
}