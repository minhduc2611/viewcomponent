using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using datafirst.Models;
using Microsoft.AspNetCore.Mvc;

namespace datafirst.Controllers
{
    public class DemoController : Controller
    {
        private readonly MyeStoreContext ctx;

        public DemoController(MyeStoreContext db) {
            ctx = db;
        }

        // https://localhost:44397/demo/ThongKe
        public IActionResult ThongKe()
        {
            var data = ctx.HangHoa
                .GroupBy(p => p.MaLoaiNavigation)
                .Select(p => new {
                    p.Key.MaLoai,
                    p.Key.TenLoai,
                    p.Key.MoTa,
                    SoMatHang = p.Count(),
                    TongSoLanXem = p.Sum(s => s.SoLanXem),
                    GiaTB = p.Average(a => a.DonGia.Value)
                    });
            return Json(data);
        }
        public IActionResult DoanhThu()
        {
            var data = from cthd in ctx.ChiTietHd
                       group cthd by new
                       {
                           Thang = cthd.MaHdNavigation.NgayDat.Month,
                           Nam = cthd.MaHdNavigation.NgayDat.Year
                       } into g
                       select new
                       {
                           g.Key.Thang,
                           g.Key.Nam,
                           DoanhThu = g.Sum(p => p.SoLuong * p.DonGia * (1 - p.GiamGia))
                       };
            return Json(data);
        }
        
    }
}