using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers
{

    public class QuanLyDonHangController : Controller
    {
        // GET: QuanLyDonHang
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChuaThanhToan()
        {
            //Lấy danh sách các đơn hàng chưa duyệt
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == false).OrderBy(n => n.NgayDat);
            return View(lst);
        }

        public ActionResult ChuaGiao()
        {
            //Lấy danh sách các đơn hàng chưa giao
            var lstCG = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == false && n.DaThanhToan == false).OrderBy(n => n.NgayDat);
            return View(lstCG);
        }

        public ActionResult DaGiaoDaThanhToan()
        {
            //Lấy danh sách các đơn hàng đã giao đã thanh toán
            var lstDG = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == true && n.DaThanhToan == true).OrderBy(n => n.NgayDat);
            return View(lstDG);
        }

        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            //Kiểm tra đơn đặt hàng có tồn tại
            if (model == null)
            {
                return HttpNotFound();
            }
            //Lấy danh sấch chi tiết đơn đặt hàng hiển thị cho người dùng thấy
            var lstChiTietHD = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietHD = lstChiTietHD;
            return View(model);
        }

        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh, string Email)
        {
            //Truy vấn dữ liệu của đơn đặt hàng đó
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            db.SaveChanges();
            //Lấy danh sách chi tiết của đơn hàng để cho người dùng thấy
            var lstChiTietHD = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietHD = lstChiTietHD;
            string contentMail = "Đơn hàng của bạn đã được đặt thành công";
            if (ddhUpdate.DaThanhToan == false && ddhUpdate.TinhTrangGiaoHang == false)
            {
                contentMail = "Đơn hàng của bạn đã được tiếp nhận";
            }
            if (ddhUpdate.DaThanhToan == false && ddhUpdate.TinhTrangGiaoHang == true)
            {
                contentMail = "Đơn hàng của bạn đang được vận chuyển";
            }
            //Gửi email khách hàng xác nhận việc thanh toán
            GuiEmail("Xác nhận đơn hàng  của hệ thống ", Email, "quanghuy3748@gmail.com"
            , "1loinoidoingotngao", contentMail);
            return View(ddhUpdate);
        }

        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            //Gọi email
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail); //Địa chỉ nhận
            mail.From = new MailAddress(ToEmail); //Địa chỉ gửi
            mail.Subject = Title; //Tiêu đề gửi
            mail.Body = Content; //Nội dung
            mail.IsBodyHtml = true;
            SmtpClient smpt = new SmtpClient();
            smpt.Host = "smtp.gmail.com";
            smpt.Port = 587;
            smpt.UseDefaultCredentials = false;
            smpt.Credentials = new System.Net.NetworkCredential(FromEmail, PassWord);
            smpt.EnableSsl = true; // kích hoạt giao tiếp an toàn ssl
            smpt.Send(mail); //gửi email đi
        }
    }
}