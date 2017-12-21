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
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

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

            string input = File.ReadAllText(@"c:\u01\rspxml.txt");
            //string input = System.Text.Encoding.UTF8.GetString(all_bytes);
            // File.WriteAllText(@"c:\u01\tmprsp.txt", input);
            string newimput = File.ReadAllText(@"c:\u01\tmprsp.txt");
            string xml = input.Substring(input.IndexOf("requestID"));
            
            xml = input.Substring(input.IndexOf("requestID"));

                       

            int resultIndex = xml.IndexOf("statusMessage");
            if (resultIndex != -1)
            {
                xml = xml.Substring(0, resultIndex);
            }
            xml = Regex.Unescape(xml);
            xml = xml.Replace(@"\", "");
            
            //xmlHeader = @"<? xml version ="1.0" ?>";
            xml = "<RootNode> <RspObj " + xml + "></RspObj> </RootNode>";
            xml.Replace("\\\"", "\"");

            //string xml2 = File.ReadAllText(@"c:\u01\rspxml2.txt");
            XmlSerializer serializer = new XmlSerializer(typeof(RootNode));
            serializer.UnknownNode += new
            XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
            XmlAttributeEventHandler(serializer_UnknownAttribute);
            StringReader sr = new StringReader(xml);
            // Declare an object variable of the type to be deserialized.
            RootNode responseObject;
            responseObject = (RootNode)serializer.Deserialize(sr);



            string pause = "jhgjjjhjhj";
            //int index = input.IndexOf("statusMessage");

            //if (index > 0)
            //    xml = xml.Substring(0, index);
            

            //var value =
            //input
            //.Split(',')
            //.Select(
            //    pair => pair.Split('='))
            //.ToDictionary(
            //    keyValue => keyValue[0].Trim(),
            //    keyValue => keyValue[1].Trim())
            //["requestID"];
            //string requestID = GetValueFromMessage("requestID", input);


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
        private void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private void serializer_UnknownAttribute
        (object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }
        private string GetValueFromMessage(string identifier, string respnseXML)
        {
            identifier = " " + identifier + "=";
            int index = respnseXML.IndexOf(identifier) + identifier.Length;

            if (index != -1)
            {
                int index2 = respnseXML.IndexOf(",", index);
                if (index2 == -1)
                {
                    index2 = respnseXML.Length;
                }
                return respnseXML.Substring(index, index2 - index);
            }

            return null;
        }
    }
    [XmlRootAttribute("RootNode", IsNullable = false)]
    public class RootNode
    {
        [XmlElementAttribute("RspObj", IsNullable = false)]
        public RspObj rspObj;
    }
    public class RspObj    {        

        [XmlAttribute(AttributeName = "requestID")]
        public string requestID { get; set; }
        [XmlAttribute(AttributeName = "statusCode")]
        public string statusCode { get; set; }
        [XmlAttribute(AttributeName = "statusSeverity")]
        public string statusSeverity { get; set; }
    }
    
}
