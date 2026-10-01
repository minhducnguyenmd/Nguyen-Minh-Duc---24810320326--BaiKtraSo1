using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace hoccsharp
{
    public partial class Form1 : Form
    {
        private readonly BindingList<SanPham> _danhSachSanPham = new BindingList<SanPham>();
        private readonly BindingSource _bindingSource = new BindingSource();
        private string _duongDanAnhChon = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            KhoiTaoDanhMuc();
            KhoiTaoDataBinding();
            CapNhatThongKeStatus();
        }

        private void KhoiTaoDanhMuc()
        {
            List<DanhMucItem> danhSachDanhMuc = new List<DanhMucItem>
            {
                new DanhMucItem("CAT01", "Điện thoại"),
                new DanhMucItem("CAT02", "Laptop"),
                new DanhMucItem("CAT03", "Phụ kiện")
            };

            cboCategory.DataSource = danhSachDanhMuc;
            cboCategory.DisplayMember = "TenDanhMuc";
            cboCategory.ValueMember = "TenDanhMuc";
        }

        private void KhoiTaoDataBinding()
        {
            _bindingSource.DataSource = _danhSachSanPham;
            dgvProducts.DataSource = _bindingSource;
        }

        private bool KiemTraHopLe()
        {
            bool hopLe = true;
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                errorProvider1.SetError(txtProductName, "Tên sản phẩm không được để trống!");
                hopLe = false;
            }

            if (!decimal.TryParse(txtUnitPrice.Text.Trim(), out decimal donGia) || donGia <= 0)
            {
                errorProvider1.SetError(txtUnitPrice, "Đơn giá phải là số lớn hơn 0!");
                hopLe = false;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int soLuong) || soLuong < 0)
            {
                errorProvider1.SetError(txtQuantity, "Số lượng phải là số nguyên lớn hơn hoặc bằng 0!");
                hopLe = false;
            }

            return hopLe;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!KiemTraHopLe()) return;

            string maSP = string.IsNullOrWhiteSpace(txtProductId.Text) 
                ? $"SP{_danhSachSanPham.Count + 1:D3}" 
                : txtProductId.Text.Trim();

            if (_danhSachSanPham.Any(p => p.MaSP.Equals(maSP, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Mã sản phẩm này đã tồn tại trong hệ thống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SanPham spMoi = new SanPham
            {
                MaSP = maSP,
                TenSP = txtProductName.Text.Trim(),
                DanhMuc = cboCategory.SelectedValue?.ToString() ?? string.Empty,
                DonGia = decimal.Parse(txtUnitPrice.Text.Trim()),
                SoLuong = int.Parse(txtQuantity.Text.Trim()),
                DuongDanAnh = _duongDanAnhChon
            };

            _danhSachSanPham.Add(spMoi);
            CapNhatThongKeStatus();
            XoaTrangOInput();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null || !(dgvProducts.CurrentRow.DataBoundItem is SanPham spDangChon))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần cập nhật từ bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!KiemTraHopLe()) return;

            spDangChon.TenSP = txtProductName.Text.Trim();
            spDangChon.DanhMuc = cboCategory.SelectedValue?.ToString() ?? string.Empty;
            spDangChon.DonGia = decimal.Parse(txtUnitPrice.Text.Trim());
            spDangChon.SoLuong = int.Parse(txtQuantity.Text.Trim());
            spDangChon.DuongDanAnh = _duongDanAnhChon;

            _bindingSource.ResetBindings(false);
            CapNhatThongKeStatus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null || !(dgvProducts.CurrentRow.DataBoundItem is SanPham spDangChon))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa từ bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm [{spDangChon.TenSP}] không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                _danhSachSanPham.Remove(spDangChon);
                CapNhatThongKeStatus();
                XoaTrangOInput();
            }
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            if (openFileDialogImage.ShowDialog() == DialogResult.OK)
            {
                _duongDanAnhChon = openFileDialogImage.FileName;
                picAvatar.ImageLocation = _duongDanAnhChon;
            }
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow != null && dgvProducts.CurrentRow.DataBoundItem is SanPham sp)
            {
                txtProductId.Text = sp.MaSP;
                txtProductName.Text = sp.TenSP;
                cboCategory.SelectedValue = sp.DanhMuc;
                txtUnitPrice.Text = sp.DonGia.ToString("G0");
                txtQuantity.Text = sp.SoLuong.ToString();
                _duongDanAnhChon = sp.DuongDanAnh;

                if (!string.IsNullOrEmpty(_duongDanAnhChon) && File.Exists(_duongDanAnhChon))
                {
                    picAvatar.ImageLocation = _duongDanAnhChon;
                }
                else
                {
                    picAvatar.Image = null;
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string tuKhoa = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(tuKhoa))
            {
                _bindingSource.DataSource = _danhSachSanPham;
            }
            else
            {
                List<SanPham> ketQuaTimKiem = _danhSachSanPham
                    .Where(p => p.TenSP.ToLower().Contains(tuKhoa))
                    .ToList();
                _bindingSource.DataSource = new BindingList<SanPham>(ketQuaTimKiem);
            }

            CapNhatThongKeStatus();
        }

        private void menuXuatCsv_Click(object sender, EventArgs e)
        {
            if (saveFileDialogCsv.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Mã SP,Tên SP,Danh Mục,Đơn Giá,Số Lượng");

                    foreach (SanPham sp in _danhSachSanPham)
                    {
                        sb.AppendLine($"\"{sp.MaSP}\",\"{sp.TenSP}\",\"{sp.DanhMuc}\",{sp.DonGia},{sp.SoLuong}");
                    }

                    File.WriteAllText(saveFileDialogCsv.FileName, sb.ToString(), new UTF8Encoding(true));
                    MessageBox.Show("Xuất danh sách ra file CSV thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Xảy ra lỗi khi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void menuThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CapNhatThongKeStatus()
        {
            lblTongSoSP.Text = $"Tổng số sản phẩm: {dgvProducts.Rows.Count}";
        }

        private void XoaTrangOInput()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            picAvatar.Image = null;
            _duongDanAnhChon = string.Empty;
            errorProvider1.Clear();
        }
    }
}