using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangThucPham.Models;
using System.Net;
using PagedList;

namespace CuaHangThucPham.Controllers
{
    public class SanPhamController : Controller
    {
        QuanLyEntities db = new QuanLyEntities();


        [ChildActionOnly]
        public ActionResult SanPhamParital()
        {
            return PartialView();
        }
        public ActionResult XemChiTiet(int? id, string tensp)
        {
            //kiem tra tham so truyen vao co rong hay khong
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //neu khong thi truy xuat csdl lay ra sp tuong ung
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            if (sp == null) {
                //thong bao neu nhu khong co sp do
                return HttpNotFound();
            }
            return View(sp);
        }
        public ActionResult SanPham(int? MaLoaiSP, int? MaNCC, int? page)
        {
            //kiem tra tham so truyen vao co rong hay khong
            if (MaLoaiSP == null || MaNCC == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //load san pham dua theo 2 tieu chi la ma loai san pham va ma san xuat (2 truong san pham)
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNCC == MaNCC);
            if (lstSP.Count() == 0) {
                return HttpNotFound();
            }
            //thuc hien phan trang
            //tao bien so san pham tren trang
            int PageSize = 6;
            //tao bien so trang hien tai
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNCC = MaNCC;
            return View(lstSP.OrderBy(n => n.MaSP).ToPagedList(PageNumber, PageSize));
        }
    }
}