using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CuaHangThucPham.Models
{
    [MetadataTypeAttribute(typeof(ThanhVienMetadata))]
    public partial class ThanhVien
    {
        internal sealed class ThanhVienMetadata
        {
            public int MaThanhVien { get; set; }


            [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
            [StringLength(15, ErrorMessage = "{0} không được quá 15 kí tự")]
            public string TaiKhoan { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
            public string MatKhau { get; set; }
            [Required(ErrorMessage = "Vui lòng nhập họ tên")]
            public string HoTen { get; set; }
            [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
            public string DiaChi { get; set; }
            [Required(ErrorMessage = "Vui lòng nhập email")]
            [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email không hợp lệ")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
            public string SoDienThoai { get; set; }
            public string CauHoi { get; set; }
            public string CauTraLoi { get; set; }
            public Nullable<int> MaLoaiTV { get; set; }

        }
    }
}