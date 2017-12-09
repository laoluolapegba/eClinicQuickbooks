using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ganss.Excel;
namespace DataImport
{
    public class CustomerObject
    {
        [Column(1)]
        public string Id { get; set; }

        [Column(2)]
        public string ListID { get; set; }
        [Column(3)]
        public string TimeCreated { get; set; }
        [Column(4)]
        public string TimeModified { get; set; }
        [Column(5)]
        public string EditSequence { get; set; }
        [Column(6)]
        public string Name { get; set; }
        [Column(7)]
        public string Fullname { get; set; }
        [Column(8)]
        public string IsActive { get; set; }
        [Column(9)]
        public string ClassRefListID { get; set; }
        [Column(10)]
        public string ClassrefFullName { get; set; }
        [Column(11)]
        public string parentrefListID { get; set; }
        [Column(12)]
        public string ParentRefFullName { get; set; }
        [Column(13)]
        public string Sublevel { get; set; }
        [Column(14)]
        public string CompanyName { get; set; }
        [Column(15)]
        public string Salutation { get; set; }
        [Column(16)]
        public string Firstname { get; set; }
        [Column(17)]
        public string Middlename { get; set; }
        [Column(18)]
        public string Lastname { get; set; }
        public string BillAddressAddr { get; set; }
        public string BillAddressAddr2 { get; set; }
        public string BillAddressAddr3 { get; set; }
        public string BillAddressAddr4 { get; set; }
        public string BillAddressAddr5 { get; set; }
        public string BillAddressCity { get; set; }
        public string BillAddressState { get; set; }
        public string BillAddressProvince { get; set; }
        public string BillAddressCounty { get; set; }
        public string BillAddressPostalCode { get; set; }
        public string BillAddressCountry { get; set; }
        public string BillAddressNote { get; set; }
        public string BillAddressBlockAddr1 { get; set; }
        public string BillAddressBlockAddr2 { get; set; }
        public string BillAddressBlockAddr3 { get; set; }
        public string BillAddressBlockAddr4 { get; set; }
        public string BillAddressBlockAddr5 { get; set; }
        public string ShipAddressAddr1 { get; set; }
        public string ShipAddressAddr2 { get; set; }
        public string ShipAddressAddr3 { get; set; }
        public string ShipAddressAddr4 { get; set; }
        public string ShipAddressAddr5 { get; set; }
        public string ShipAddressCity { get; set; }
        public string ShipAddressState { get; set; }
        public string ShipAddressProvince { get; set; }
        public string ShipAddressCounty { get; set; }
        public string ShipAddressPostalCode { get; set; }
        public string ShipAddressCountry { get; set; }
        public string ShipAddressNote { get; set; }
        public string ShipAddressBlockAddr1 { get; set; }
        public string ShipAddressBlockAddr2 { get; set; }
        public string ShipAddressBlockAddr3 { get; set; }
        public string ShipAddressBlockAddr4 { get; set; }
        public string ShipAddressBlockAddr5 { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Cc { get; set; }
        public string Contact { get; set; }
        public string AltContact { get; set; }
        public string CustomerTypeRefListID { get; set; }
        public string CustomerTypeRefFullName { get; set; }
        public string TermsRefListID { get; set; }
        public string TermsRefFullName { get; set; }
        public string SalesRepRefListID { get; set; }
        public string SalesRreprefFullName { get; set; }
        public string Balance { get; set; }
        public string TotalBalance { get; set; }
        public string OpenBalance { get; set; }
        public string OpenBalanceDate { get; set; }
        public string SalesTaxcoderefListID { get; set; }
        public string SalesTaxcoderefFullname { get; set; }
        public string TaxCodeRefListID { get; set; }
        public string TaxCodeRefFullname { get; set; }
        public string ItemSalesTaxRefListID { get; set; }
        public string ItemSalesTaxRefFullName { get; set; }
        public string SalesTaxCountry { get; set; }
        public string RefSalesNumber { get; set; }
        public string AccountNumber { get; set; }
        public string BusinessNUmber { get; set; }
        public string CreditLimit { get; set; }
        public string PrefpaymthdRefListID { get; set; }
        public string PrefpaymthdRefFullname { get; set; }
        public string CCCnumber { get; set; }
        public string CCExpiryYr { get; set; }
        public string CCexpiryMnth { get; set; }
        public string CCnameonCard { get; set; }
        public string CCAddress { get; set; }
        public string CCpostalCode { get; set; }
        public string Jobstatus { get; set; }
        public string JobStartDate { get; set; }
        public string JobProjectedenddate { get; set; }
        public string JobEnddate { get; set; }
        public string JobDesc { get; set; }
        public string JobTypereflistID { get; set; }
        public string JobTyperefFullname { get; set; }
        public string Notes { get; set; }
        public string PrefDeliv { get; set; }
        public string isUsingCustoms { get; set; }
        public string PriceLevelrefLIst { get; set; }
        public string ExternalGUID { get; set; }
        public string TaxRegistration { get; set; }
        public string CurrencyRefList { get; set; }
        public string CurrencyRefFullname { get; set; }

    }
}
