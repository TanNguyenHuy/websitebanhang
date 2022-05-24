using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangThucPham.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace CuaHangThucPham.Controllers
{
    public class HomeController : Controller
    {
       
        QuanLyEntities db = new QuanLyEntities();
        public ActionResult Index()
        {
            //tao viewbag de lay san pham moi tu co so du lieu
            var lstSPM = db.SanPhams.Where(n => n.Moi == 1 && n.DaXoa == false);
            //gan vao viewbag
            ViewBag.ListSPM = lstSPM;
            return View();
        }
        public ActionResult MenuPartial()
        {
            var lstSP = db.SanPhams;
            return PartialView(lstSP);
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            tv.MaLoaiTV = 4;
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            //kiểm tra captcha
            if (this.IsCaptchaValid("captcha is not valid")) {
                if (ModelState.IsValid) {
                    if (CheckTaiKhoan(tv.TaiKhoan)) {
                        ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                    }
                    else if (CheckEmail(tv.Email)) {
                        ModelState.AddModelError("", "Email đã tồn tại");
                    }
                    else if (CheckSDT(tv.SoDienThoai)) {
                        ModelState.AddModelError("", "Số điện thoại đã tồn tại");
                    }
                    else {
                        ViewBag.ThongBao = "Tạo tài khoản thành công";
                        //thêm khách hàng vào csdl
                        db.ThanhViens.Add(tv);
                        db.SaveChanges();
                    }
                    
                }
                else {
                    ViewBag.ThongBao = "Tạo tài khoản thất bại";
                }
                return View();
            }
            ViewBag.ThongBao = "Sai mã Captcha";
            return View();
        }
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích?");
            lstCauHoi.Add("Ca sỹ mà bạn yêu thích");
            lstCauHoi.Add("Hiện tại bạn đang làm công việc gì?");
            return lstCauHoi;
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        //xay dung action dang nhap
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            //kiem tra ten dang nhap va mat khau
            /* string sTaiKhoan = f["txtTenDangNhap"].ToString();
             string sMatKhau = f["txtMatKhau"].ToString();
             ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
             if (tv != null) {
                 Session["TaiKhoan"] = tv;
                 return RedirectToAction("Index");
             }
             ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
             return View();*/
            string sTaiKhoan = f["txtTenDangNhap"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();
            //truy van kiem tra dang nhap lay thong tin thanh vien
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if (tv != null) {
                //lay ra list quyen cua thanh vien tuong ung voi loai thanh vien
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                //duyet list quyen
                string Quyen = "";
                foreach (var item in lstQuyen) {
                    Quyen += item.Quyen.MaQuyen + ",";
                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1);//cat di dau "," o cuoi 
                PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
                Session["TaiKhoan"] = tv;
                return RedirectToAction("Index");
            }
            ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
            return View();

        }
        public void PhanQuyen (string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                                                    TaiKhoan, //user
                                                    DateTime.Now, //begin
                                                    DateTime.Now.AddHours(3),//timeout
                                                    false, //remember
                                                    Quyen,
                                                    FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
        //tao trang ngan chan quyen truy cap
        public ActionResult LoiPhanQuyen()
        {
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
        public bool CheckTaiKhoan (string taiKhoan)
        {
            return db.ThanhViens.Count(n => n.TaiKhoan == taiKhoan) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.ThanhViens.Count(n => n.Email == email) > 0;
        }
        public bool CheckSDT(string sdt)
        {
            return db.ThanhViens.Count(n => n.SoDienThoai == sdt) > 0;
        }
    }

}