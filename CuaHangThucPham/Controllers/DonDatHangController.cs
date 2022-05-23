using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using CuaHangThucPham.Models;
using System.Net;

namespace CuaHangThucPham.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class DonDatHangController : Controller
    {
        QuanLyEntities db = new QuanLyEntities();
        // GET: DonDatHang
        public ActionResult ChuaThanhToan()
        {
            //lay danh sach don hang chua duyet
            var lst = db.DonDatHangs.Where(n => n.DaThanhToan == false).OrderBy(n => n.NgayDat);
            return View(lst);
        }
        public ActionResult ChuaGiao()
        {
            //lay danh sach don hang chua giao
            var lstDSDHCG = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == false && n.DaThanhToan == true).OrderBy(n => n.NgayGiao);
            return View(lstDSDHCG);
        }
        public ActionResult DaGiaoDaThanhToan()
        {
            //lay danh sach don hang
            var lstDSDH = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == true && n.DaThanhToan == true).OrderBy(n => n.NgayGiao);
            return View(lstDSDH);
        }
        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            //kiem tra xem id hop le khong
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            //kiem tra don hang co ton tai hay khong
            if (model == null) {
                return HttpNotFound();
            }
            //lay danh sach chi tiet don hang de hien thi cho nguoi dung thay
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }

        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
            //truy van lay ra du lieu cua don hang do
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            db.SaveChanges();

            //lay danh sach chi tiet don hang de hien thi nguoi dung thay
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = lstChiTietDH;
            //gui khach hang 1 mail de xac nhan viec thanh toan
            /*GuiMail("Xác nhận đơn hàng của hệ thống", "mail@gmail.com", "mailnhan@gmail.com", "pw", "noi dung");*/
            return View(ddhUpdate);
        }
        public void GuiMail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            //goi mail
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail);//Dia chi nhan
            mail.From = new MailAddress(ToEmail); //dia chi goi
            mail.Subject = Title;//tieu de
            mail.Body = Content;//noi dung
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";//host goi cua gmail
            smtp.Port = 587;//port của mail
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(FromEmail, PassWord); //tai khoan pw nguoi goi
            smtp.EnableSsl = true;//kich hoat giao tiep an toan
            smtp.Send(mail);//gui mail di
        }
    }
}