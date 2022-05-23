using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangThucPham.Models;


namespace CuaHangThucPham.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLySanPham")]
    public class PhieuNhapController : Controller
    {
        QuanLyEntities db = new QuanLyEntities();
        // GET: PhieuNhap
        [HttpGet]
        public ActionResult NhapHang()
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            return View();
        }
        public ActionResult LichSuPhieuNhap()
        {
            var lstCTPN = db.ChiTietPhieuNhaps;
            return View(lstCTPN);
        }
        [HttpPost]
        public ActionResult NhapHang(PhieuNhap model, IEnumerable<ChiTietPhieuNhap> lstModel)
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            //sau khi kiem tra tat ca du lieu dau vao
            //gan da xoa: false
            model.DaXoa = false;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();
            //savechanges de lay mapn gan cho lstchitietphieunhap
            SanPham sp;
            foreach (var item in lstModel) {
                //cap nhat so luong ton
                sp = db.SanPhams.Single(n => n.MaSP == item.MaSP);
                sp.SoLuongTon += item.SoLuongNhap;

                //gan ma phieu nhap cho tat ca chi tiet phieu nhap

                item.MaPN = model.MaPN;
            }
            db.ChiTietPhieuNhaps.AddRange(lstModel);
            db.SaveChanges();


            return View();
        }
        [HttpGet]
        public ActionResult DSSPHetHang()
        {
            //danh sach san pham gan het hang voi so luong ton be hon hoac bang 5
            var lstSP = db.SanPhams.Where(n => n.DaXoa == false && n.SoLuongTon <= 5);
            return View(lstSP);
        }
        //tao 1 view phuc vu cho viec nhap tung san pham
        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            if (id == null) {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null) {
                return HttpNotFound();
            }
            return View(sp);
        }
        [HttpPost]
        public ActionResult NhapHangDon(PhieuNhap model, ChiTietPhieuNhap ctpn)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            model.NgayNhap = DateTime.Now;
            model.DaXoa = false;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();
            //savechanges de lay mapn gan cho lstchitietphieunhap  
            ctpn.MaPN = model.MaPN;
            SanPham sp = db.SanPhams.Single(n => n.MaSP == ctpn.MaSP);
            sp.SoLuongTon += ctpn.SoLuongNhap;
            db.ChiTietPhieuNhaps.Add(ctpn);
            db.SaveChanges();
            return View(sp);
        }
        //giai phong bien cho vung nho
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