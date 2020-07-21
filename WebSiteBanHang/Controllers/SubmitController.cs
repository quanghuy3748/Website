using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanHang.Models;


namespace WebSiteBanHang.Controllers
{
    public class SubmitController : Controller
    {
        // GET: Submit
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(PhieuNhap pn,IEnumerable<ChiTietPhieuNhap> ModelList)
        {
            return View();
        }
    }
}