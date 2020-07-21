using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;
using System.Web.Helpers;
using System.Net.Mail;

namespace WebSiteBanHang.Controllers
{
    
    public class HomeController : Controller
    {        
        // GET: Home
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            //Lần lượt tạo các view bag để lấy list sản phẩm từ cơ sở dữ liệu
            //List Điện thoại mới nhất
            var lstDTM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1);
            ViewBag.ListDTM = lstDTM;

            //List Laptop mới nhất
            var lstLTM = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
            ViewBag.ListLTM = lstLTM;            
            return View();
        }

        public ActionResult MenuPartial()
        {
            //truy vấn lấy về 1 list các sản phẩm
            var lstSP = db.SanPhams;
            return PartialView(lstSP);
        }

        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            //Kiểm tra captcha 
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                if (ModelState.IsValid)
                {                
                    ViewBag.ThongBao = "Thêm thành công ";
                    db.ThanhViens.Add(tv);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.ThongBao = "Thêm thất bại";
                }                
                return View();
            }
            ViewBag.ThongBao = "Sai mã Captcha";
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }
        [HttpGet]
        public ActionResult DangKy1()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy1(ThanhVien tv)
        {
            return View();
        }

        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích ?");
            lstCauHoi.Add("Đội bóng mà bạn yêu thích ?");
            lstCauHoi.Add("Con số yêu thích của bạn ?");
            lstCauHoi.Add("Tên của người yêu bạn ?");
            return lstCauHoi;
        }

        //Xây dựng action đăng nhập
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            //Kiểm tra tên đăng nhập và mật khẩu
            string sTaiKhoan = f["txtTenDangNhap"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if (tv != null)
            {
                Session["TaiKhoan"] = tv;
                //return RedirectToAction("Index");
                return Content("<script>window.location.reload();</script>");
            }
            return Content("Tài khoản hoặc mật khẩu không chính xác");
            //string taikhoan = f["txtTenDangNhap"].ToString();
            //string matkhau = f["txtMatKhau"].ToString();
            ////truy vấn kiêm tra đăng nhập lấy thông tin thành viên
            //ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matkhau);
            //if (tv != null)
            //{
            //    Session["TaiKhoan"] = tv;
            //    //Lấy ra list quyền của thành viên tương ứng loại tv
            //    var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
            //    string Quyen = "";
            //    if (lstQuyen.Count() != 0)
            //    {
            //        foreach (var item in lstQuyen)
            //        {
            //            Quyen += item.Quyen.MaQuyen + ",";
            //            Quyen = Quyen.Substring(0, Quyen.Length - 1); //Cắt dấu ,
            //            PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
            //            return Content("<script>window.location.reload();</script>");
            //        }
            //    }
            //}
            //return Content("Tài khoản hoặc mật khẩu không chính xác");
        }

        public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                                                        TaiKhoan,
                                                        DateTime.Now,
                                                        DateTime.Now.AddHours(3),
                                                        false,
                                                        Quyen,
                                                        FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
        //Tạo trang chặn quyền truy cập
        public ActionResult LoiPhanQuyen()
        {
            return View();
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }       

        public ActionResult LienHe()
        {         
            return View();
        }

        public ActionResult GioiThieu()
        {
            return View();
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

        public ActionResult SendMail()
        {
            string subject = Request["subject"];
            string body = Request["body"];
            GuiEmail(subject, "damhuyenmina@gmail.com", "quanghuy3748@gmail.com"
                 , "1loinoidoingotngao", body);
            return View();
        }
    }
}