using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangThucPham.Models;

namespace CuaHangThucPham.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class QuanLyTaiKhoanController : Controller
    {
        QuanLyEntities db = new QuanLyEntities();
        // GET: QuanLyTaiKhoan
        public ActionResult Index()
        {
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.TenLoai), "MaLoaiTV", "TenLoai");
            var lstTV = db.ThanhViens;
            return View(lstTV);
        }

        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //lay ncc can chinh sua
            if (id == null) {
                Response.StatusCode = 404;
                return null;
            }
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (tv == null) {
                return HttpNotFound();
            }
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.TenLoai), "MaLoaiTV", "TenLoai");
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View(tv);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(ThanhVien tv)
        {
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.TenLoai), "MaLoaiTV", "TenLoai");
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            db.Entry(tv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            //lay ncc can chinh sua
            if (id == null) {
                Response.StatusCode = 404;
                return null;
            }
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (tv == null) {
                return HttpNotFound();
            }
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.TenLoai), "MaLoaiTV", "TenLoai");
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View(tv);
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (tv == null) {
                return HttpNotFound();
            }
            db.ThanhViens.Remove(tv);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích?");
            lstCauHoi.Add("Ca sỹ mà bạn yêu thích");
            lstCauHoi.Add("Hiện tại bạn đang làm công việc gì?");
            return lstCauHoi;
        }
    }
}