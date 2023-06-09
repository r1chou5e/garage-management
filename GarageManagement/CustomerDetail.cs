﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;

namespace GarageManagement
{
    public partial class CustomerDetail : Form
    {
        string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\customer_image\";

        public OpenFileDialog opFile = new OpenFileDialog();

        bool imageExist = false;

        private int MaKH;
        public bool deleted = false;

        public CustomerDetail(int MaKH)
        {
            this.MaKH = MaKH;
            InitializeComponent();
            imageExist = CheckImageExist();
            LoadCustomerImage();
        }

        private void CustomerDetail_Load(object sender, EventArgs e)
        {
            DataTable customerDetail = KHACHHANG_DAL.Instance.GetCustomerDetail(MaKH);

            string TenKH = customerDetail.Rows[0]["TenKH"].ToString();
            string GioiTinh = customerDetail.Rows[0]["GioiTinh"].ToString();
            string DienThoai = customerDetail.Rows[0]["DienThoai"].ToString();
            string DiaChi = customerDetail.Rows[0]["DiaChi"].ToString();
            string ThoiGianDangKy = customerDetail.Rows[0]["ThoiGianDangKy"].ToString();

            this.Text = TenKH + " | " + DienThoai;

            nameTb.Text = TenKH;
            genderCb.SelectedIndex = GioiTinh == "Nam" ? 0 : 1;
            phoneNumberTb.Text = DienThoai;
            addressTb.Text = DiaChi;
            createdTb.Text = ThoiGianDangKy;
        }

        void SaveCustomerImage()
        {
            try
            {
                if (Directory.Exists(appPath) == false)
                {
                    Directory.CreateDirectory(appPath);
                }
                string iName = "image" + MaKH + ".jpg";
                string filepath = opFile.FileName;

                if (imageExist)
                {
                    File.Delete(appPath + iName);
                }
                File.Copy(filepath, appPath + iName);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Cannot save image !! \n " + exp.Message);
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            string TenKH = nameTb.Text;
            string GioiTinh = genderCb.SelectedIndex == 0 ? "Nam" : "Nữ";
            string DienThoai = phoneNumberTb.Text;
            string DiaChi = addressTb.Text;
            
            if (KHACHHANG_DAL.Instance.UpdateCustomer(MaKH, TenKH, DienThoai, GioiTinh, DiaChi))
            {
                if (File.Exists(opFile.FileName)) SaveCustomerImage();
                MessageBox.Show("Cập nhật khách hàng thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật khách hàng thất bại");
            }
            this.Close();
        }

        void LoadCustomerImage()
        {
            string iName = "image" + MaKH + ".jpg";
            if (imageExist)
            {
                noImgLb.Visible = false;
                using (FileStream fs = new FileStream(appPath + iName, FileMode.Open))
                {
                    Image image = Image.FromStream(fs);
                    customerImg.Image = image;
                    fs.Close();
                }
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn chắc chắn sẽ xóa khách hàng này ?", "delete customer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                if (XE_DAL.Instance.SoftDeleteAllCarOfCustomer(this.MaKH) && KHACHHANG_DAL.Instance.SoftDeleteCustomer(this.MaKH))
                {
                    MessageBox.Show("Xóa khách hàng thành công!!");
                    deleted = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Xóa khách hàng thất bại !! Thử lại");
                }
            }
        }

        bool CheckImageExist()
        {
            string iName = "image" + MaKH + ".jpg";
            return File.Exists(appPath + iName);
        }

        private void chooseImgBtn_Click(object sender, EventArgs e)
        {
            opFile.Title = "Select a Image";
            opFile.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";                                                                                   // <---

            if (opFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    customerImg.Image = new Bitmap(opFile.OpenFile());
                    noImgLb.Visible = false;
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Unable to open file " + exp.Message);
                }
            }
            else
            {
                opFile.Dispose();
            }
        }
    }
}
