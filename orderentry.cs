using System.ComponentModel.DataAnnotations;

namespace sample_task.Models
{
    public class orderentry
    {
        [Key]
        public int orderid { get; set; }
        public string orderno { get; set; }
        public int ordercompanyid { get; set; }
        public string orderdate { get; set; }
        public string jobtype { get; set; }
        public int initiator { get; set; }
        public int importer { get; set; }
        public int exporter { get; set; }
        public string referencesno { get; set; }
        public int userid { get; set; }
        public bool isactive { get; set; }
        public bool isvoid { get; set; }
    }
}
