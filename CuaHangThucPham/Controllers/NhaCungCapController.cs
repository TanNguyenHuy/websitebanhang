using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangThucPham.Models;

namespace CuaHangThucPham.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class NhaCungCapController : Controller
    {
        QuanLyEntities db = new QuanLyEntities();
        // GET: NhaCungCap
        public ActionResult Index()
        {
            var lstNCC = db.NhaCungCaps;
            return View(lstNCC);
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(NhaCungCap ncc)
        {
            db.NhaCungCaps.Add(ncc);
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
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            if (ncc == null) {
                return HttpNotFound();
            }
            return View(ncc);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(NhaCungCap ncc)
        {
            db.Entry(ncc).State = System.Data.Entity.EntityState.Modified;
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
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            if (ncc == null) {
                return HttpNotFound();
            }
            return View(ncc);
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            if (ncc == null) {
                return HttpNotFound();
            }
            db.NhaCungCaps.Remove(ncc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}