using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangThucPham.Models;

namespace CuaHangThucPham.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLyDonHang,QuanLySanPham")]
    public class AdminController : Controller
    {
        QuanLyEntities db = new QuanLyEntities();

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();// lấy số người truy cập từ application
            ViewBag.SoNguoiOnline = HttpContext.Application["SoNguoiOnline"].ToString();
            ViewBag.TongThanhVien = TongThanhVien();
            ViewBag.ThongKeDoanhThu = ThongKeDoanhThu();// thống kê doanh thu
            ViewBag.ThongKeDonHang = ThongKeDonHang(); // thống kê đơn hàng
            ViewBag.ThongKeSanPham = ThongKeSanPham(); // thống kê số lượng sản phẩm t
            ViewBag.ThongKeLoaiSanPham = ThongKeLoaiSanPham();
            return View();
        }
        public decimal? ThongKeDoanhThu()
        {
            decimal? TongDoanhThu = db.ChiTietDonDatHangs.Sum(n => n.DonGia * n.SoLuong);
            return TongDoanhThu;
        }

        public double ThongKeDonHang()
        {
            //Đếm đơn đặt hàng
            double sl = db.DonDatHangs.Count();
            return sl;
        }

        public double TongThanhVien()
        {
            double slTV = db.ThanhViens.Count();
            return slTV;
        }
        public int ThongKeSanPham()
        {
            // đếm số lượng sản phẩm
            int sanpham = db.SanPhams.Sum(n => n.SoLuongTon).Value;
            return sanpham;
        }
        public double ThongKeLoaiSanPham()
        {
            // đếm đơn đặt hàng
            double slncc = db.LoaiSanPhams.Count();
            return slncc;
        }


        public decimal? ThongKeDoanhThuTheoThang(int Thang, int Nam)
        {
            var lstDDH = db.DonDatHangs.Where(n => n.NgayDat.Value.Month == Thang && n.NgayDat.Value.Year == Nam);
            decimal? TongTien = 0;
            //Duyệt chi tiết đơn hàng theo điều kiện
            foreach (var item in lstDDH) {
                TongTien += item.ChiTietDonDatHangs.Sum(n => n.DonGia * n.SoLuong);
            }
            return TongTien;
        }
        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            int Thang = Convert.ToInt32(f["txtThang"].ToString());
            int Nam = Convert.ToInt32(f["txtNam"].ToString());
            decimal? tongtien = ThongKeDoanhThuTheoThang(Thang, Nam);
            return Content(tongtien.ToString());
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