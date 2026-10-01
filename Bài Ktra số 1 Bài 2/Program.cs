using System;
using System.Collections.Generic;
using System.Linq;

namespace LogisticsAutoSpeed
{

    public abstract class PhuongTien
    {

        private string _maPT;
        private string _tenHang;
        private int _namSanXuat;
        private decimal _giaGoc;


        public string MaPT
        {
            get => _maPT;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _maPT = "PT000";
                else
                    _maPT = value.Trim();
            }
        }

        public string TenHang
        {
            get => _tenHang;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tên hãng không được để trống!");
                _tenHang = value.Trim();
            }
        }

        public int NamSanXuat
        {
            get => _namSanXuat;
            set
            {
                int currentYear = DateTime.Now.Year;
                if (value < 1900 || value > currentYear)
                    throw new ArgumentException("Năm sản xuất không hợp lệ!");
                _namSanXuat = value;
            }
        }

        public decimal GiaGoc
        {
            get => _giaGoc;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Giá gốc phải lớn hơn 0!");
                _giaGoc = value;
            }
        }


        public PhuongTien(string maPT, string tenHang, int namSanXuat, decimal giaGoc)
        {
            MaPT = maPT;
            TenHang = tenHang;
            NamSanXuat = namSanXuat;
            GiaGoc = giaGoc;
        }


        public abstract decimal TinhGiaLanBanh();


        public virtual string GetInfo()
        {
            return $"[Mã: {MaPT}] Hãng: {TenHang} | Năm SX: {NamSanXuat} | Giá gốc: {GiaGoc:N0} VNĐ";
        }
    }


    public class OTo : PhuongTien
    {
        private int _soChoNgoi;
        private double _dungTichDongCo;

        public int SoChoNgoi
        {
            get => _soChoNgoi;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Số chỗ ngồi phải lớn hơn 0!");
                _soChoNgoi = value;
            }
        }

        public double DungTichDongCo
        {
            get => _dungTichDongCo;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung tích động cơ phải lớn hơn 0!");
                _dungTichDongCo = value;
            }
        }

        public OTo(string maPT, string tenHang, int namSanXuat, decimal giaGoc, int soChoNgoi, double dungTichDongCo)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            SoChoNgoi = soChoNgoi;
            DungTichDongCo = dungTichDongCo;
        }

        public override decimal TinhGiaLanBanh()
        {
            if (SoChoNgoi <= 9)
            {

                return GiaGoc + (0.12m * GiaGoc) + (0.30m * GiaGoc);
            }
            else
            {

                return GiaGoc + (0.10m * GiaGoc);
            }
        }

        public override string GetInfo()
        {
            return $"{base.GetInfo()} | Loại: Ô tô | Chỗ ngồi: {SoChoNgoi} | Dung tích: {DungTichDongCo}L";
        }
    }


    public class XeMay : PhuongTien
    {
        private int _dungTichXylanh;

        public int DungTichXylanh
        {
            get => _dungTichXylanh;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Dung tích xi lanh phải lớn hơn 0!");
                _dungTichXylanh = value;
            }
        }

        public XeMay(string maPT, string tenHang, int namSanXuat, decimal giaGoc, int dungTichXylanh)
            : base(maPT, tenHang, namSanXuat, giaGoc)
        {
            DungTichXylanh = dungTichXylanh;
        }

        public override decimal TinhGiaLanBanh()
        {
            if (DungTichXylanh < 175)
            {

                return GiaGoc + (0.02m * GiaGoc);
            }
            else
            {

                return GiaGoc + (0.05m * GiaGoc);
            }
        }

        public override string GetInfo()
        {
            return $"{base.GetInfo()} | Loại: Xe máy | Dung tích: {DungTichXylanh}cc";
        }
    }


    public class QuanLyPhuongTien
    {
        private readonly List<PhuongTien> _danhSach = new List<PhuongTien>();

        public void AddPhuongTien(PhuongTien pt)
        {
            if (pt == null) throw new ArgumentNullException(nameof(pt));
            _danhSach.Add(pt);
        }

        public void DisplayAll()
        {
            if (!_danhSach.Any())
            {
                Console.WriteLine("Danh sách trống.");
                return;
            }

            foreach (var pt in _danhSach)
            {
                Console.WriteLine($"{pt.GetInfo()} => Giá lăn bánh: {pt.TinhGiaLanBanh():N0} VNĐ");
            }
        }

        public PhuongTien? FindMaxGiaLanBanh()
        {
            if (!_danhSach.Any()) return null;
            return _danhSach.OrderByDescending(pt => pt.TinhGiaLanBanh()).FirstOrDefault();
        }

        public List<PhuongTien> SearchByName(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return new List<PhuongTien>();

            return _danhSach
                .Where(pt => pt.TenHang.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== CHẠY KỊCH BẢN KIỂM THỬ (TEST CASES) ===\n");

            var qlpt = new QuanLyPhuongTien();


            Console.WriteLine("[TC01] Kiểm tra Validation Năm sản xuất = 1850:");
            try
            {
                var otoLoi = new OTo("OTO999", "Toyota", 1850, 800_000_000m, 5, 2.0);
                Console.WriteLine("FAIL: Không ném ngoại lệ.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"PASS: Ném ngoại lệ thành công -> \"{ex.Message}\"");
            }
            Console.WriteLine();


            Console.WriteLine("[TC02] Kiểm tra Giá lăn bánh Ô tô 5 chỗ, Giá gốc 1,000,000,000 VNĐ:");
            var oto5Cho = new OTo("OTO001", "Toyota Camry", 2023, 1_000_000_000m, 5, 2.5);
            decimal giaLanBanhOto = oto5Cho.TinhGiaLanBanh();
            decimal expectedOto = 1_420_000_000m;
            Console.WriteLine($"Giá tính được : {giaLanBanhOto:N0} VNĐ");
            Console.WriteLine($"Kỳ vọng       : {expectedOto:N0} VNĐ");
            Console.WriteLine(giaLanBanhOto == expectedOto ? "PASS" : "FAIL");
            Console.WriteLine();


            Console.WriteLine("[TC03] Kiểm tra Giá lăn bánh Xe máy 150cc, Giá gốc 50,000,000 VNĐ:");
            var xeMay150 = new XeMay("XM001", "Honda AirBlade", 2022, 50_000_000m, 150);
            decimal giaLanBanhXeMay = xeMay150.TinhGiaLanBanh();
            decimal expectedXeMay = 51_000_000m;
            Console.WriteLine($"Giá tính được : {giaLanBanhXeMay:N0} VNĐ");
            Console.WriteLine($"Kỳ vọng       : {expectedXeMay:N0} VNĐ");
            Console.WriteLine(giaLanBanhXeMay == expectedXeMay ? "PASS" : "FAIL");
            Console.WriteLine();


            Console.WriteLine("[TC04] Kiểm tra Đa hình khi duyệt List<PhuongTien>:");
            qlpt.AddPhuongTien(oto5Cho);
            qlpt.AddPhuongTien(xeMay150);
            qlpt.DisplayAll();
            Console.WriteLine("PASS: Runtime tự động gọi đúng TinhGiaLanBanh() và GetInfo() theo từng kiểu đối tượng con.");
            Console.WriteLine();


            Console.WriteLine("[TC05] Kiểm tra FindMaxGiaLanBanh():");
            var maxPt = qlpt.FindMaxGiaLanBanh();
            if (maxPt != null && maxPt.TinhGiaLanBanh() == expectedOto && maxPt.MaPT == "OTO001")
            {
                Console.WriteLine($"PASS: Trả về chính xác phương tiện [{maxPt.MaPT} - {maxPt.TenHang}] với giá {maxPt.TinhGiaLanBanh():N0} VNĐ.");
            }
            else
            {
                Console.WriteLine("FAIL: Không tìm đúng đối tượng có giá lăn bánh cao nhất.");
            }
        }
    }
}
