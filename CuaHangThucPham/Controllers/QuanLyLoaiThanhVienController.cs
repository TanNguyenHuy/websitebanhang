using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangThucPham.Models;


namespace CuaHangThucPham.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class QuanLyLoaiThanhVienController : Controller
    {
        QuanLyEntities db = new QuanLyEntities();
        // GET: QuanLyLoaiThanhVien
        public ActionResult Index()
        {
            var lstLTV = db.LoaiThanhViens;
            return View(lstLTV);
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(LoaiThanhVien ltv)
        {
            db.LoaiThanhViens.Add(ltv);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //lay ncc can chinh sua
            if (id == null) {
                Response.StatusCode = 404;
                return null;
            }
            LoaiThanhVien ltv = db.LoaiThanhViens.SingleOrDefault(n => n.MaLoaiTV == id);
            if (ltv == null) {
                return HttpNotFound();
            }
            return View(ltv);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(LoaiThanhVien ltv)
        {
            db.Entry(ltv).State = System.Data.Entity.EntityState.Modified;
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
            LoaiThanhVien ltv = db.LoaiThanhViens.SingleOrDefault(n => n.MaLoaiTV == id);
            if (ltv == null) {
                return HttpNotFound();
            }
            return View(ltv);
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            LoaiThanhVien ltv = db.LoaiThanhViens.SingleOrDefault(n => n.MaLoaiTV == id);
            if (ltv == null) {
                return HttpNotFound();
            }
            db.LoaiThanhViens.Remove(ltv);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult PhanQuyen (int? id)
        {
            //lay ma loai tv dua vao id
            if (id == null) {
                Response.StatusCode = 404;
                return null;
            }
            LoaiThanhVien ltv = db.LoaiThanhViens.SingleOrDefault(n => n.MaLoaiTV == id);
            if (ltv == null) {
                return HttpNotFound();

            }
            ViewBag.MaQuyen = db.Quyens;
            ViewBag.LoaiTVQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == id);
            return View(ltv);
        }
        [HttpPost]
        public ActionResult PhanQuyen (int? MaLTV, IEnumerable<LoaiThanhVien_Quyen> lstPhanQuyen)
        {
            var lstDaPhanQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == MaLTV);
            if (lstDaPhanQuyen.Count() != 0) {
                db.LoaiThanhVien_Quyen.RemoveRange(lstDaPhanQuyen);
                db.SaveChanges();

            }
            if (lstPhanQuyen != null) {
                foreach (var item in lstPhanQuyen) {
                    item.MaLoaiTV = int.Parse(MaLTV.ToString());
                    db.LoaiThanhVien_Quyen.Add(item);



                }
                db.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}