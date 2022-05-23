using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangThucPham.Models;

namespace CuaHangThucPham.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLySanPham")]
    public class QuanLySanPhamController : Controller
    {
        // GET: QuanLySanPham
        QuanLyEntities db = new QuanLyEntities();
        public ActionResult Index()
        {
            return View(db.SanPhams.Where(n => n.DaXoa == false));
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            //Load dropdownlist nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase[] HinhAnh)
        {
            //Load dropdownlist nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            var loi = 0;
            for (int i = 0; i < HinhAnh.Count(); i++) {
                if (HinhAnh[i] != null) {
                    //kiem tra hinh anh ton tai trong csdl chua
                    if (HinhAnh[i].ContentLength > 0) {
                        if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/jpg") {
                            ViewBag.upload += "Hình ảnh " + i + " không hợp lệ <br/>";
                            loi++;
                        }
                        else {
                            //lay ten hinh anh
                            var fileName = Path.GetFileName(HinhAnh[i].FileName);
                            //lay hinh anh chuyen vao thu muc hinh anh
                            var path = Path.Combine(Server.MapPath("~/hinhanh"), fileName);
                            //neu thu muc chua hinh anh co roi thi xuat ra thong bao
                            if (System.IO.File.Exists(path)) {
                                ViewBag.upload1 = "Hình" + i + " đã tồn tại <br/>";
                                loi++;
                            }
                        }
                    }
                }
            }
            if (loi > 0) {
                return View(sp);
            }
            sp.HinhAnh = HinhAnh[0].FileName;
            HinhAnh[0].SaveAs(Path.Combine(Server.MapPath("~/hinhanh"), HinhAnh[0].FileName));
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //lay san pham can chinh sua vao id
            if (id == null) {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null) {
                return HttpNotFound();
            }


            //Load dropdownlist nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model, HttpPostedFileBase HinhAnh, HttpPostedFileBase HinhAnh1, HttpPostedFileBase HinhAnh2, HttpPostedFileBase HinhAnh3, HttpPostedFileBase HinhAnh4)
        {

            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;

            if (HinhAnh != null) {
                //kiem tra hinh anh ton tai trong csdl chua
                if (HinhAnh.ContentLength > 0) {
                    if (HinhAnh.ContentType != "image/jpeg" && HinhAnh.ContentType != "image/png" && HinhAnh.ContentType != "image/jpg") {
                        ViewBag.upload = "Hình ảnh không hợp lệ <br/>";
                        return View(model);
                    }
                    else {
                        model.HinhAnh = HinhAnh.FileName;

                        HinhAnh.SaveAs(Path.Combine(Server.MapPath("~/hinhanh"), HinhAnh.FileName));
                    }
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            //lay san pham can chinh sua vao id
            if (id == null) {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null) {
                return HttpNotFound();
            }


            //Load dropdownlist nhà cung cấp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            return View(sp);
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            SanPham model = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (model == null) {
                return HttpNotFound();
            }
            db.SanPhams.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}