using System.ComponentModel.DataAnnotations;

namespace sample_task.Models
{
    public class itemsection
    {
        [Key]
        public int itemid { get; set; }
        public int orderid { get; set; }
        public string invoiceno { get; set; }
        public string itemcode { get; set; }
        public string itemdescription { get; set; }
        public int quantity { get; set; }
        public decimal itemamount { get; set; }
        public decimal itemlocalvalue { get; set; }
        public decimal unitprice { get; set; }
        public int userid { get; set; }
        public bool isactive { get; set; }
        public bool isvoid { get; set; }
    }
}
