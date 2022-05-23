using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangThucPham.Models;
using PagedList;

namespace CuaHangThucPham.Controllers
{
    public class TimKiemController : Controller
    {
        QuanLyEntities db = new QuanLyEntities();
        // GET: TimKiem
        [HttpGet]
        public ActionResult KQTimKiem(string sTuKhoa, int? page)
        {
            if (Request.HttpMethod != "GET") {
                page = 1;
            }
            //thuc hien chuc nang phan trang
            //tao bien so san pham tren trang
            int PageSize = 6;
            //so trang hien tai
            int PageNumber = (page ?? 1);
            //tim kiem theo ten san pham
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return View(lstSP.OrderBy(n => n.TenSP).ToPagedList(PageNumber, PageSize));
        }
        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string sTuKhoa)
        {
            //goi ve ham get tim kiem
            return RedirectToAction("KQTimKiem", new { @sTuKhoa = sTuKhoa });
        }
    }
}