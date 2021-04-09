/*      Rangga Aulia Rahman
        20/456849/TK/50673         */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Net;

namespace PeramalCuaca
{
    public partial class Form1 : Form    
        //contoh dari inheritance, "Form1" adalah 
        //kelas turunan dari parent class "Form"
        /*Hal ini secara tidak langsung juga menunjukkan adanya implementasi dari konsep
          Polymorphism, dimana kelas turunan bisa memiliki basis yang sama dengan parent class,
          namun memiliki implementasi secara langsung yang bermacam macam.*/
    {
        List<Panel> ListePanel = new List<Panel>(); 
        //Menggunakan beberapa Panel untuk implementasi prinsip SOLID yaitu Interface Segregation,
        //dimana interface hendaknya tidak dipenuhi terlalu banyak konten untuk user dan hendaknya
        //dibuat sederhana
        int i;
        public Form1()
        {
            InitializeComponent();
        }
        /*
        Encapsulation & Abstraction 
        Semua property yang berhubungan dengan aksi ketika user me-klik tombol tampilkan
        disatukan menjadi 1 dengan hubungan mereka yang sama, yaitu sama2 akan muncul ketika 
        user menekan tombol tampilkan di bagian kanan atas, hal ini merupakan implementasi dari
        konsep OOP Encapsulation.
        Abstraction disini diimplementasikan dengan hanya memperlihatkan proses umum tanpa
        menunjukkan detail yang terjadi didalam suatu method. Disini button1_Click mengindikasikan
        hal yang akan terjadi ketika tombol 1 di klik, walau begitu tidak perlu dipahami secara detail
        apa yang terjadi ketika tombol tersebut diklik secara detail.
        */
        private void button1_Click(object sender, EventArgs e) //<- encapsulation
        {
            detailcuaca dtl = new detailcuaca();
            //kelas diatas memiliki Prinsip SOLID : Single Responsibility Principle, karena kewajibannya
            //hanya menyimpan properti detail cuaca

            dtl.kota = txtcity.Text;

            string url = string.Format("http://api.weatherapi.com/v1/forecast.xml?key=cddd71fbe27a4522818170642210804&q={0}&days=1&aqi=no&alerts=no", dtl.kota);

            XDocument doc = XDocument.Load(url);

            string iconUrl = (string)doc.Descendants("icon").FirstOrDefault();

            WebClient client = new WebClient();

            byte[] image = client.DownloadData("http:" + iconUrl);

            MemoryStream stream = new MemoryStream(image);

            Bitmap newBitmap = new Bitmap(stream);

            dtl.maxTemp = (string)doc.Descendants("maxtemp_c").FirstOrDefault();    //mengambil data dari xml API
            dtl.minTemp = (string)doc.Descendants("mintemp_c").FirstOrDefault();    //lalu convert ke string

            dtl.maxwindm = (string)doc.Descendants("maxwind_mph").FirstOrDefault();
            dtl.maxwindk= (string)doc.Descendants("maxwind_kph").FirstOrDefault();

            dtl.humidity = (string)doc.Descendants("avghumidity").FirstOrDefault();

            dtl.country = (string)doc.Descendants("country").FirstOrDefault();

            dtl.condition = (string)doc.Descendants("text").FirstOrDefault();

            //Prinsip SOLID kedua dapat dilihat di bagian atas ini, yaitu Open/Closed principle, artinya kita dapat menambahkan
            //fitur baru (open for extension) namun kita tidak harus mengganti kode awal (closed for modification)

            Bitmap icon = newBitmap;

            txtmaxtemp.Text = dtl.maxTemp;
            txtmintemp.Text = dtl.minTemp;

            txtwindm.Text = dtl.maxwindm;
            txtwindk.Text = dtl.maxwindk;

            txthumidity.Text = dtl.humidity;

            label7.Text = dtl.condition;
            txtcountry.Text = dtl.country;

            pictureBox1.Image = icon;

            if (dtl.condition == "Clear")
            {
                txtrekomendasi.Text = "cuaca cerah, anda bisa bepergian dengan bebas";
            }
            else if (dtl.condition == "Heavy raining")
            {
                txtrekomendasi.Text = "Cuaca sedang tidak baik, stay dirumah";
            }
            else
                txtrekomendasi.Text = "Cuaca sedang tidak menentu, bawa persiapan";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string kota;

            DataTable dt = new DataTable();
            dt.Columns.Add("Negara", typeof(string));
            dt.Columns.Add("Tanggal", typeof(string));          //membuat tabel untuk ramalan
            dt.Columns.Add("Temp Max", typeof(string));        //cuaca
            dt.Columns.Add("Temp Min", typeof(string));       //beberapa hari kedepan
            dt.Columns.Add("Kec. Angin mph", typeof(string));
            dt.Columns.Add("Kec. Angin kph", typeof(string));
            dt.Columns.Add("Kelembapan", typeof(string));
            dt.Columns.Add("Kondisi", typeof(string));
            dt.Columns.Add("Icon", typeof(string));

            kota = txtcity.Text;
            string url = string.Format("http://api.weatherapi.com/v1/forecast.xml?key=cddd71fbe27a4522818170642210804&q={0}&days=7&aqi=no&alerts=no", kota);

            XDocument doc = XDocument.Load(url);
            foreach (var npc in doc.Descendants("forecastday")) //loop untuk mengambil data API.
            {
                string iconUrl = (string)npc.Descendants("icon").FirstOrDefault();

                WebClient client = new WebClient();

                byte[] image = client.DownloadData("http:" + iconUrl);

                MemoryStream stream = new MemoryStream(image);

                Bitmap newBitmap = new Bitmap(stream);

                dt.Rows.Add(new object[]
                {
                    (string)doc.Descendants("country").FirstOrDefault(),  //display negara ditulis doc karena tidak berubah seiring waktu dalam loop.
                    (string)npc.Descendants("date").FirstOrDefault(),
                    (string)npc.Descendants("maxtemp_c").FirstOrDefault(),
                    (string)npc.Descendants("mintemp_c").FirstOrDefault(),
                    (string)npc.Descendants("maxwind_mph").FirstOrDefault(),
                    (string)npc.Descendants("maxwind_kph").FirstOrDefault(),

                    (string)npc.Descendants("avghumidity").FirstOrDefault(),

                    (string)npc.Descendants("text").FirstOrDefault(),
                    newBitmap
                });
            }
            dataGridView1.DataSource = dt;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListePanel.Add(panel2);
            ListePanel.Add(panel1);
            ListePanel[i].BringToFront();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtwindk_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (i > 0)
            {
                ListePanel[--i].BringToFront();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (i < ListePanel.Count - 1)
            {
                ListePanel[++i].BringToFront();
            }
        }
    }
}



