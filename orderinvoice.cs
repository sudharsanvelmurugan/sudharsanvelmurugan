using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace sample_task.Models
{
    
    public class orderinvoice
    {
        [Key]
        public int invoiceid { get; set; }
        public int orderid { get; set; }
        public string invoiceno { get; set; }
        public string invoicedate { get; set; }
        public string invoicetype { get; set; }
        public string currencycode { get; set; }
        public decimal exchangerate { get; set; }
        public decimal invoiceamount { get; set; }
        public decimal invoicelocalvalue { get; set; }
        public string? inschargetype { get; set; }
        public decimal insvalue { get; set; }
        public decimal inslocalvalue { get; set; }
        public string? othchargetype { get; set; }
        public decimal othvalue { get; set; }
        public decimal othlocalvalue { get; set; }
        public decimal costins { get; set; }
        public decimal costoth { get; set; }
        public decimal costinsoth { get; set; }
        public int userid { get; set; }
        public bool isactive { get; set; }
        public bool isvoid { get; set; }
    }
}
