using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace DBCOST
{
    public partial class Form1 : Form
    {
        //Data Source = DESKTOP - T1738DH\SQLEXPRESS01;Initial Catalog = DBCOST; Integrated Security = True
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-T1738DH\SQLEXPRESS01;Initial Catalog=testmaliyet;Integrated Security=True");

        void malzemelist()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * from tblmalzemeler", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        void urunlist()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("Select  * from tblurunler", con);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView1.DataSource = dt2;        
        }

        void kasa()
        {
            SqlDataAdapter da3 = new SqlDataAdapter("select * from tblkasa", con);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            dataGridView1.DataSource = dt3;        
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            urunler();
            malzemeler();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            urunlist();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            malzemelist();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            kasa();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void EKLE_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into tblmalzemeler(ad,stok,fiyat,notlar) values(@p1,@p2,@p3,@p4)",con);
            cmd.Parameters.AddWithValue("@p1",MALZEMEADI.Text);
            cmd.Parameters.AddWithValue("@p2",MALZEMESTOK.Text);
            cmd.Parameters.AddWithValue("@p3",MALZEMEFİYAT.Text);
            cmd.Parameters.AddWithValue("@p4",MALZEMENOTLAR.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("malzeme eklendi","bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
            malzemelist();
        }

        private void ürünekle_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into tblfırın(urunıd,malzemeıd,miktar,maliyet) values(@p1,@p2,@p3,@p4)",con);
            cmd.Parameters.AddWithValue("@p1",cmburun.SelectedValue);
            cmd.Parameters.AddWithValue("@p2",cmbmalzeme.SelectedValue);
            cmd.Parameters.AddWithValue("@p3",txtmiktar.Text);
            cmd.Parameters.AddWithValue("@p4",txtmaliyet.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("ürün oluşturuldu", "bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            listBox1.Items.Add(cmbmalzeme.Text + "-" + txtmaliyet.Text);
        }

        void urunler()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * from tblurunler", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmburun.ValueMember = "URUNID";
            cmburun.DisplayMember ="URUNAD";
            cmburun.DataSource = dt;
            con.Close();
        }

        void malzemeler()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter("select * from tblmalzemeler", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbmalzeme.ValueMember = "MALZEMEID"; //büyük küçük harf duyarlılığı mevcuttur
            cmbmalzeme.DisplayMember = "AD";
            cmbmalzeme.DataSource = dt;
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd2 = new SqlCommand("insert into tblurunler(urunad,mfıyat,sfıyat,stok) values(@p1,@p2,@p3,@p4)", con);
            cmd2.Parameters.AddWithValue("@p1", urunad.Text);
            cmd2.Parameters.AddWithValue("@p2", urunmfiyat.Text);
            cmd2.Parameters.AddWithValue("@p3", urunsfiyat.Text);
            cmd2.Parameters.AddWithValue("@p4", urunstok.Text);
            cmd2.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("ürün eklendi", "bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            urunlist();
            
        }

        private void txtmiktar_TextChanged(object sender, EventArgs e)
        {
            double maliyet;

            if (txtmiktar.Text == "")
            {
                txtmiktar.Text = "0";
            }

            con.Open();
            SqlCommand command = new SqlCommand("Select * from tblmalzemeler where MALZEMEID=@p1",con);
            command.Parameters.AddWithValue("@p1", cmbmalzeme.SelectedValue);
            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
            {
                txtmaliyet.Text=(sqlDataReader[3].ToString());
            }
            con.Close();

            maliyet=Convert.ToDouble(txtmaliyet.Text)/1000*Convert.ToDouble(txtmiktar.Text);

            txtmaliyet.Text = maliyet.ToString();

        }

        private void cmburun_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void cmbmalzeme_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd2 = new SqlCommand("update tblurunler set urunad=@p1,mfıyat=@p2,sfıyat=@p3,stok=@p4 where urunıd=@p5", con);
            cmd2.Parameters.AddWithValue("@p1", urunad.Text);
            cmd2.Parameters.AddWithValue("@p2", urunmfiyat.Text);
            cmd2.Parameters.AddWithValue("@p3", urunsfiyat.Text);
            cmd2.Parameters.AddWithValue("@p4", urunstok.Text);
            cmd2.Parameters.AddWithValue("@p5", urunıd.Text);
            cmd2.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("ürün güncellendi", "bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            urunlist();
        }

    }
}
