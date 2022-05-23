using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangThucPham.Models;

namespace CuaHangThucPham.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyEntities db = new QuanLyEntities();
        // GET: GioHang
        public List<ItemGioHang> LayGioHang()
        {
            //gio hang da ton tai
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null) {
                //neu gio hang chua ton tai thi tao gio hang
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            //kiem tra san pham co ton tai trong CSDL hay khong
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null) {
                //trang duong dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            //lay gio hang 
            List<ItemGioHang> lstGioHang = LayGioHang();
            //truong hop 1 neu san pham da ton tai trong gio hang
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null) {
                //kiem tra so luong ton truoc khi dat hang
                if (sp.SoLuongTon < spCheck.SoLuong) {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong) {
                return View("ThongBao");
            }
            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }

        public double TinhTongSoLuong()
        {
            //lay gio hang
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null) {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null) {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);

        }
        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0) {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();

            return PartialView();
        }
        public ActionResult XemGioHang()
        {
            //lay gio hang 
            List<ItemGioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return View(lstGioHang);
        }
        public ActionResult SuaGioHang(int MaSP)
        {
            //kiem tra session gio hang ton tai chua
            if (Session["GioHang"] == null) {
                return RedirectToAction("Index", "Home");
            }
            //kiem tra san pham co ton tai trong CSDL hay khong
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null) {
                //trang duong dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            //lay list gio hang tu session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //kiem tra xem san pham do co ton tai trong gio hang hay khong
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null) {
                return RedirectToAction("Index", "Home");
            }
            //lay list gio hang tao giao dien
            ViewBag.GioHang = lstGioHang;
            //neu ton tai roi
            return View(spCheck);

        }
        //xu ly cap nhat gio hang
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            //kiem tra so luong ton
            SanPham spCheck = db.SanPhams.Single(n => n.MaSP == itemGH.MaSP);
            if (spCheck.SoLuongTon < itemGH.SoLuong) {
                return View("ThongBao");
            }
            //Cap nhap so luong trong ssesion gio hang
            //Buoc1: lay list<GioHang> tu ssesion["GioHang"]
            List<ItemGioHang> lstGH = LayGioHang();
            //Buoc2 : lay san pham can cap nhat tu trong list<GioHang> ra
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            //Buoc3: cap nhat lai so luong va thanh tien
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
            return RedirectToAction("XemGioHang");
        }
        public ActionResult XoaGioHang(int MaSP)
        {
            //kiem tra session da ton tai hay chua
            if (Session["GioHang"] == null) {
                return RedirectToAction("Index", "Home");
            }
            //kiem tra san pham co ton tai trong CSDL hay khong
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null) {
                //trang duong dan khong hop le
                Response.StatusCode = 404;
                return null;
            }
            //lay list gio hang tu session
            List<ItemGioHang> lstGH = LayGioHang();

            //kiem tra xem san pham co ton tai trong gio hang hay khong
            ItemGioHang spCheck = lstGH.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null) {
                return RedirectToAction("Index", "Home");
            }
            //xoa item gio hang
            lstGH.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }
        public ActionResult DatHang(KhachHang kh)
        {
            //kiem tra session gio hang ton tai chua
            if (Session["GioHang"] == null) {
                return RedirectToAction("Index", "Home");
            }

            KhachHang khang = new KhachHang();
            if (Session["KhachHang"] == null) {
                //them khach hang doi voi khach hang chua co tai khoan
                khang = new KhachHang();
                khang = kh;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }
            else {
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khang.TenKH = tv.HoTen;
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;
                khang.SoDienThoai = tv.SoDienThoai;
                khang.MaThanhVien = tv.MaThanhVien;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }
            //them don hang
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            //them chi tiet don dat hang
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH) {
                SanPham sp = db.SanPhams.Single(n => n.MaSP == item.MaSP);
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                sp.SoLuongTon -= item.SoLuong;
                db.ChiTietDonDatHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }
    }
}