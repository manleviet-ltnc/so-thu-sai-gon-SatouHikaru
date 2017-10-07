using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SaigonZoo
{
    public partial class frmSaigonZoo : Form
    {
        public frmSaigonZoo()
        {
            InitializeComponent();
        }

        private void ListBox_MouseDown(object sender, MouseEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            int index = lb.IndexFromPoint(e.X, e.Y);

            if (index != -1)
                lb.DoDragDrop(lb.Items[index].ToString(), DragDropEffects.Copy);
        }

        private void ListBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.Move;
        }

        bool isItemChanged = false;
        private void lstDanhSach_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                if (!lstDanhSach.Items.Contains(lstThuMoi.SelectedItem))
                {
                    int newIndex = lstDanhSach.IndexFromPoint(lstDanhSach.PointToClient(new Point(e.X, e.Y)));
                    object selectedItem = e.Data.GetData(DataFormats.Text);

                    lstDanhSach.Items.Remove(selectedItem);
                    if (newIndex != -1)
                        lstDanhSach.Items.Insert(newIndex, selectedItem);
                    else
                        lstDanhSach.Items.Insert(lstDanhSach.Items.Count, selectedItem);

                    isItemChanged = true;
                }
        }

        bool isSave = true;
        private void Save(object sender, EventArgs e)
        {
            // Mở tập tin
            StreamWriter write = new StreamWriter("danhsachthu.txt");

            if (write == null)
                return;

            foreach (var item in lstDanhSach.Items)
                write.WriteLine(item.ToString());

            isSave = false;
            write.Close();
        }

        bool isLoad = false;
        private void mnuLoad_Click(object sender, EventArgs e)
        {
            StreamReader reader = new StreamReader("thumoi.txt");

            if (reader == null)
                return;

            string input;
            while ((input = reader.ReadLine()) != null)
                lstThuMoi.Items.Add(input);
            reader.Close();

            using (StreamReader rs = new StreamReader("danhsachthu.txt"))
            {
                input = null;
                while ((input = rs.ReadLine()) != null)
                    lstDanhSach.Items.Add(input);
            }

            isLoad = true;
        }

        private void mnuClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            while (lstDanhSach.SelectedIndex != -1)
                lstDanhSach.Items.RemoveAt(lstDanhSach.SelectedIndex);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            mnuDelete_Click(sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = string.Format("Bây giờ là {0}:{1}:{2} ngày {3} tháng {4} năm {5}",
                                         DateTime.Now.Hour,
                                         DateTime.Now.Minute,
                                         DateTime.Now.Second,
                                         DateTime.Now.Day,
                                         DateTime.Now.Month,
                                         DateTime.Now.Year);
        }

        private void frmSaigonZoo_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isLoad == true)
            {
                if (isSave && isItemChanged)
                {
                    DialogResult result = MessageBox.Show("Bạn có muốn lưu danh sách?",
                                                          "",
                                                          MessageBoxButtons.YesNoCancel,
                                                          MessageBoxIcon.None);
                    if (result == DialogResult.Yes)
                    {
                        Save(sender, e);
                        e.Cancel = false;
                    }
                    else if (result == DialogResult.No)
                        e.Cancel = false;
                    else
                        e.Cancel = true;
                }
            }
            else
                mnuClose_Click(sender, e);
        }
    }
}