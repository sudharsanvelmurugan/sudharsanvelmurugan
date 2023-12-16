using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace sample_task.Models
{
    public class companyuser
    {
        public int userid { get; set; }
        public string useremail { get; set; }
        public string password { get; set; }
        public int companyuserid { get; set; }
        public bool isactive { get; set; }
        public bool isdelete { get; set; }
    }
    
    
   
    public class tradingparty
    {
        [Key]
        public int masterid { get; set; }
        public int mastercompanyid { get; set; }
        public string email { get; set; }
        public bool isinitiator { get; set; } 
        public bool isimporter { get; set; }
        public bool isexporter { get; set; }
        public int parentid { get; set; }
        public bool isactive { get; set; }
        public bool isvoid { get; set; }
    }

    public class itemmaster
    {
        [Key]
        public int itemid { get; set; }
        public int companyiditem { get; set; }
        public string itemcode { get; set; }
        public string itemdescription { get; set; }
        public int quantity { get; set; }
        public decimal itemamount { get; set; }
        public bool isactive { get; set; }
        public bool isvoid { get; set; }
    }
       
}
