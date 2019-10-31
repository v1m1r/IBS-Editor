using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Borland.Data;
//using Borland.Vcl;
//using Borland.Data.Units;
using System.Data.SqlClient;
using System.Data.Common;
using System.Threading;
using System.Diagnostics;


namespace EditIBS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.WorkerSupportsCancellation = true;
          
        }

        public DbConnection getConnection()
        {
            // DbProviderFactory factory = DbProviderFactories.GetFactory
            //            ("Borland.Data.AdoDbxClient");
        //    DbConnection c = new TAdoDbxConnection();
            //DbConnection c = factory.CreateConnection();
            string DataBase = textBox1.Text;
            //c.ConnectionString = "DriverName=Interbase;Database=" + DataBase + ";RoleName=RoleName;User_Name=sysdba;Password=masterkey;SQLDialect=3;MetaDataAssemblyLoader=Borland.Data.TDBXInterbaseMetaDataCommandFactory,Borland.Data.DbxReadOnlyMetaData,Version=11.0.5000.0,Culture=neutral,PublicKeyToken=91d62ebb5b0d1b1b;GetDriverFunc=getSQLDriverINTERBASE;LibraryName=dbxint30.dll;VendorLib=GDS32.DLL";
           // return c;
        }
     

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgw = sender as BackgroundWorker;
            DoDeleteDouble();   
        }

        void DoDeleteDouble()
        {

            
            DbConnection conn = getConnection();
            //string sql = "DELETE FROM spr_speech_table WHERE spr_speech_table.S_DATETIME IN ( SELECT S_DATETIME FROM spr_speech_table GROUP BY S_DATETIME HAVING count (S_DATETIME)>1)";
            string sql = "DELETE FROM spr_speech_table WHERE spr_speech_table.S_DATETIME IN (SELECT S_DATETIME FROM spr_speech_table GROUP BY S_DATETIME HAVING count (S_DATETIME)>1 AND S_DATETIME>'" + textBox2.Text + "' AND S_DATETIME>='" + textBox3.Text + "')";
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            conn.Open();
            DbDataReader myreader = cmd.ExecuteReader();
            myreader.Close();
          
        
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
                textBox1.Text = "localhost:"+ofd.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Путь к БД не можеть быть пустым", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
               
                button2.Enabled = false;
                if (checkBox1.Checked == true)
                {
                    checkBox1.Enabled = false;
                    textBox1.Enabled = false;
                    button1.Enabled = false;
                    label2.Visible = true;
                    pictureBox1.Visible = true;

                    DbConnection conn = getConnection();

                    string sql = "UPDATE SPR_SPEECH_TABLE SET S_REPLICATED=0";
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    conn.Open();
                    DbDataReader myreader = cmd.ExecuteReader();
                    myreader.Close();
                    MessageBox.Show("Значения S_REPLICATED обнулены", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    checkBox1.Enabled = true;
                    textBox1.Enabled = true;
                    button1.Enabled = true;

                    button2.Enabled = true;
                    label2.Visible = false;
                    pictureBox1.Visible = false;

                }
             
                if (checkBox2.Checked == true)
                {
                    checkBox2.Enabled = false;
                    textBox1.Enabled = false;
                    button1.Enabled = false;

                    label2.Visible = true;
                    pictureBox1.Visible = true;
                    DbConnection conn = getConnection();
                    string sql = "DELETE FROM SPR_HISTORY_TABLE";
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    conn.Open();
                    DbDataReader myreader = cmd.ExecuteReader();
                    myreader.Close();

                    MessageBox.Show("Таблица SPR_HISTORY_TABLE очищена", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    checkBox2.Enabled = true;
                    textBox1.Enabled = true;
                    button1.Enabled = true;

                    button2.Enabled = true;
                    label2.Visible = false;
                    pictureBox1.Visible = false;
                }


                if (checkBox3.Checked == true)
                {

                    DbConnection conn = getConnection();
                    string sql = "delete from spr_speech_table T1 where EXISTS (SELECT * FROM  spr_speech_table T2 WHERE (T2.S_DATETIME=T1.S_DATETIME OR (T2.S_DATETIME IS NULL AND T2.S_DATETIME IS NULL)) AND (T2.S_DATETIME=T1.S_DATETIME OR (T2.S_DATETIME IS NULL AND T2.S_DATETIME IS NULL)) AND (T2.RDB$DB_KEY>T1.RDB$DB_KEY))";
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    conn.Open();
                    DbDataReader myreader = cmd.ExecuteReader();
                    myreader.Close();
                    MessageBox.Show("Дубликаты успешно удалены", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    checkBox3.Enabled = false;
                    textBox1.Enabled = false;
                    button1.Enabled = false;

                    label2.Visible = true;
                    pictureBox1.Visible = true;
                    
                    backgroundWorker1.RunWorkerAsync();

                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

      

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
            MessageBox.Show("Дубликаты успешно удалены", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            checkBox3.Enabled = true;
            textBox1.Enabled = true;
            button1.Enabled = true;

            button2.Enabled = true;
            label2.Visible = false;
            pictureBox1.Visible = false;
           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

    }
}
