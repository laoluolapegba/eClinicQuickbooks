using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using FCQB.Data;
using System.Windows.Forms;
using log4net;
using CsvHelper;
using System.IO;
using CsvHelper.Configuration;
using Ganss.Excel;
using FCQB.Data;
using System.Globalization;

namespace DataImport
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            
        }
        private static readonly ILog log = LogManager.GetLogger(typeof(frmMain));
        eClatModel db;
        private void btnCust_Click(object sender, EventArgs e)
        {
            string x = "10000";
            decimal txnamount = decimal.Parse(x);
            string tranamount = string.Format("{0:0.00}", Convert.ToDecimal(txnamount) );
            db = new eClatModel();
            string excelfile = @"C:\LaoluOlapegba\Myprojects\femi\FCWBWebConnSvc\DataImport\in\book2.xlsx";
            //Configuration config = new Configuration();
            //using (TextReader rdr = File.OpenText(@"C:\LaoluOlapegba\Myprojects\femi\FCWBWebConnSvc\DataImport\in\customers.csv"))
            //{
            //    string text = rdr.ReadToEnd();
            //    Console.WriteLine(text.Length);
            //    var csv = new CsvReader(rdr);
            //    var records = csv.GetRecords<CustomerObject>();
            //}
            var customers = new ExcelMapper(excelfile) { HeaderRow = false }.Fetch<CustomerObject>();
            foreach (var item in customers)
            {
                //Console.WriteLine(item.ListID);
                //insert into DB
                qb_customers entity = new qb_customers();
                entity.Name = item.Name;
                entity.ListID = item.ListID;
                entity.IsActive = item.IsActive;
                entity.FullName = item.Fullname;
                entity.Balance = item.Balance;
                entity.EditSequence = item.EditSequence;
                entity.Sublevel = item.Sublevel;
                entity.Firstname = item.Firstname;
                entity.LastName = item.Lastname;
                entity.Middlename = item.Middlename;
                //entity.TimeCreated = DateTime.ParseExact(item.TimeCreated, "yyyy-MM-dd'T'HH:mm:sszzz", CultureInfo.InvariantCulture);

                //entity.TimeModified = DateTime.ParseExact(item.TimeModified, "yyyy-MM-dd'T'HH:mm:sszzz", CultureInfo.InvariantCulture);

                //entity.TotalBalance = item.TotalBalance;

                db.qb_customers.Add(entity);
            }
            
            db.SaveChanges();
        }
    }
}
