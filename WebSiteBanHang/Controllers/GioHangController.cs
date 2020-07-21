using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
namespace WebSiteBanHang.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        //Lấy giỏ hàng
        public List<ItemGioHang> LayGioHang()
        {
            //Giỏ hàng đã tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang == null)
            {
                //nếu giỏ hàng chưa tồn tại
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
                
            }
            return lstGioHang;
        } 

        //Thêm giỏ hàng (Load lại trang)
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            //KIểm tra sản phẩn có tồn tại
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            //lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();

            //TH1: sản phẩm đã tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck != null)
            {
                //Kiểm tra số lượng tồn
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
           
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }

            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }

        //Tính tổng số lượng
        public double TinhTongSoLuong()
        {
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if(lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
        //Tính tổng tiền
        public decimal TinhTongTien()
        {
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }

        // GET: GioHang
        public ActionResult XemGioHang()
        {
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }

        //Chỉ sửa giỏ hàng
        public ActionResult SuaGioHang(int MaSP)
        {
            //Kiểm tra session giỏ hàng tồn tại
            if(Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //KIểm tra sản phẩn có tồn tại trong CSDL
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }

            //Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiểm tra sp có tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if(spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Lấy list giỏ hàng
            ViewBag.GioHang = lstGioHang;
            //Nếu tồn tại
            return View(spCheck);
        }
        
        //Xử lý cập nhật giửo hàng
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            //Kiểm tra số lượng tồn
            SanPham spCheck = db.SanPhams.Single(n => n.MaSP == itemGH.MaSP);
            if (spCheck.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            //Cập nhật số lượng trong session giỏ hàng
            //B1: lấy list từ session
            List<ItemGioHang> lstGH = LayGioHang();
            //B2: lấy sp cần cập nhật từ trong list
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            //B3: tiến hành cập nhật 
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
            return RedirectToAction("XemGioHang");
        }

         public ActionResult XoaGioHang( int MaSP)
        {
            //Kiểm tra session giỏ hàng tồn tại
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //KIểm tra sản phẩn có tồn tại trong CSDL
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang đường dẫn không hợp lệ
                Response.StatusCode = 404;
                return null;
            }

            //Lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiểm tra sp có tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Xoá item trong giỏ hàng
            lstGioHang.Remove(spCheck);

            return RedirectToAction("XemGioHang");
        }
        
        //Xây dựng chức năng đặt hàng
        public ActionResult DatHang(KhachHang kh)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khang = new KhachHang();
            if (Session["TaiKhoan"] == null)
            {
                //Thêm khách hàng vào bảng khách hàng đối với kh vãng lai   
               
                khang = kh;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }
            else
            {
                //Đối với khách hàng là thành viên
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khang.TenKH = tv.HoTen;
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;
                khang.SoDienThoai = tv.SoDienThoai;
                khang.MaThanhVien = tv.MaLoaiTV;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }
            //Thêm đơn hàng 
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            //Thêm chi tiết đơn đặt hàng
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);                
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }
        public ActionResult TheoDoi()
        {
            return View();
        }
    }       
}