using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangThucPham.Models;


namespace CuaHangThucPham.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLySanPham")]
    public class QuanLyLoaiSanPhamController : Controller
    {
        QuanLyEntities db = new QuanLyEntities();
        // GET: QuanLyLoaiSanPham
        public ActionResult Index()
        {
            var lstLSP = db.LoaiSanPhams;
            return View(lstLSP);
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(LoaiSanPham lsp)
        {
            db.LoaiSanPhams.Add(lsp);
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
            LoaiSanPham lsp = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == id);
            if (lsp == null) {
                return HttpNotFound();
            }
            return View(lsp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(LoaiSanPham lsp)
        {
            db.Entry(lsp).State = System.Data.Entity.EntityState.Modified;
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
            LoaiSanPham lsp = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == id);
            if (lsp == null) {
                return HttpNotFound();
            }
            return View(lsp);
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            LoaiSanPham lsp = db.LoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == id);
            if (lsp == null) {
                return HttpNotFound();
            }
            db.LoaiSanPhams.Remove(lsp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}