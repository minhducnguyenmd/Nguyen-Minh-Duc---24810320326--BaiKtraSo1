namespace hoccsharp
{
    public class DanhMucItem
    {
        public string MaDanhMuc { get; set; } = string.Empty;
        public string TenDanhMuc { get; set; } = string.Empty;

        public DanhMucItem(string maDanhMuc, string tenDanhMuc)
        {
            MaDanhMuc = maDanhMuc;
            TenDanhMuc = tenDanhMuc;
        }
    }
}