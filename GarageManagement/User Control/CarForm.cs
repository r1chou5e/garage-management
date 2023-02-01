﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAL;

namespace GarageManagement.User_Control
{
    public partial class CarForm : UserControl
    {
        public DataTable customerList;

        public CarForm()
        {
            InitializeComponent();
        }

        private void CarForm_Load(object sender, EventArgs e)
        {
            DataTable allBrand = HIEUXE_DAL.Instance.LoadBrandList();
            for (int i = 0; i < allBrand.Rows.Count; i++)
            {
                brandCb.Items.Add(allBrand.Rows[i]["TenHieuXe"].ToString());
            }
            brandCb.SelectedIndex = 0;
            LoadCutomerList();
        }

        void LoadCutomerList()
        {
            customerList = KHACHHANG_DAL.Instance.LoadCustomerList();
            customerLv.Columns.Add("STT", 50);
            customerLv.Columns.Add("Họ và tên", 225);
            customerLv.Columns.Add("Số điện thoại", 215);
            for (int i = 0; i < customerList.Rows.Count; i++)
            {
                ListViewItem item = new ListViewItem(i + 1 + "");
                customerLv.Items.Add(item);

                // add customerName to carLv
                string customerName = customerList.Rows[i]["TenKH"].ToString();
                ListViewItem.ListViewSubItem customerNameItem = new ListViewItem.ListViewSubItem(item, customerName);
                item.SubItems.Add(customerNameItem);

                // add phoneNumber to carLv
                string phoneNumber = customerList.Rows[i]["DienThoai"].ToString();
                ListViewItem.ListViewSubItem phoneNumberItem = new ListViewItem.ListViewSubItem(item, phoneNumber);
                item.SubItems.Add(phoneNumberItem);
            }
        }
    }
}
