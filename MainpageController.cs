using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using sample_task.Data;
using sample_task.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace sample_task.Controllers
{
    public class MainpageController : Controller
    {
        private readonly application re;

        public MainpageController(application logger)
        {
            re = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult getorder(string date1,string date2,string orderno,string jobtype)
        {
           
            if (orderno == null)
            {
                var data = re.orderheader.Where(x => string.Compare(x.orderdate, date1) >= 0 && string.Compare(x.orderdate, date2) <= 0).ToList();
                var order = new List<object>();
                for (int i = 0; i < data.Count; i++)
                {
                    var data1 = data[i].orderid;
                    var data2 = re.orderinvoice.Where(x => x.orderid == data1).ToList();
                    var data3 = data2.Count;
                    //var invoicelocal = re.orderinvoice.Where(x => x.orderid == data1).Select(x => x.invoicelocalvalue).ToList();
                    //var data4 = re.itemsections.Where(x => x.orderid == data1).ToList();
                    //var data5 = data4.Count;
                    //var itemlocal = re.itemsections.Where(x => x.orderid == data1).Select(x => x.itemlocalvalue).ToList();
                    var data4 = re.orderheader.Where(x => x.orderid == data1).Select(x => x.ordercompanyid).ToList();
                    var data5 = re.tradingparty_master.Where(x => data4.Contains(x.mastercompanyid)).Select(x => x.email).ToList();

                    var datajoin = new
                    {
                        order = data[i],
                        invoicecount = data3,
                        email=data5
                       
                    };
                    order.Add(datajoin);
                }

                return Json(order);
            }
            else
            {
                var data = re.orderheader.Where(x => string.Compare(x.orderdate, date1) >= 0 && string.Compare(x.orderdate, date2) <= 0 && x.orderno==orderno && x.jobtype==jobtype).ToList();
                var order = new List<object>();
                for (int i = 0; i < data.Count; i++)
                {
                    var data1 = data[i].orderid;
                    var data2 = re.orderinvoice.Where(x => x.orderid == data1).ToList();
                    var data3 = data2.Count;
                    //var invoicelocal = re.orderinvoice.Where(x => x.orderid == data1).Select(x => x.invoicelocalvalue).ToList();
                    //var data4 = re.itemsections.Where(x => x.orderid == data1).ToList();
                    //var data5 = data4.Count;
                    //var itemlocal = re.itemsections.Where(x => x.orderid == data1).Select(x => x.itemlocalvalue).ToList();
                    var data4 = re.orderheader.Where(x => x.orderid == data1).Select(x => x.ordercompanyid).ToList();
                    var data5 = re.tradingparty_master.Where(x => data4.Contains(x.mastercompanyid)).Select(x => x.email).ToList();
                    var datajoin = new
                    {
                        order = data[i],
                        invoicecount = data3,
                        email=data5
                       
                    };
                    order.Add(datajoin);
                }

                return Json(order);
            }

            return Json(orderno);
        }

        public IActionResult getorderno(string prefix)
        {
            int companyid = Convert.ToInt32(TempData["companyid"]);
            var data = re.orderheader.Where(x => x.orderno.Contains(prefix) && x.ordercompanyid == companyid).ToList();
            TempData["companyid"] = companyid;
            return Json(data);
        }

        public IActionResult getvalue(string Prefix)
        {
            int companyid = Convert.ToInt32(TempData["companyid"]);
           
            var orderlist = (from p in re.tradingparty_master
                             where (p.email).Contains(Prefix)

                             select p).Take(10).ToList();
            var jsonData = orderlist.Select(item => new
            {
                email = string.Format("{0}", item.email),
                masterid = item.masterid

            });
            var commonparentid = orderlist[0].parentid;
            var filteredList =orderlist.Where(p => p.parentid == commonparentid && p.mastercompanyid == companyid).ToList();

            TempData["companyid"] = companyid;
            return Json(filteredList);
        }

        public IActionResult editorderdata(int orderid)
        {
            var data = re.orderheader.FirstOrDefault(x => x.orderid == orderid);
            return Json(data);
        }
        public IActionResult editdata(int orderid)
        {
            var order = re.orderinvoice.FirstOrDefault(x => x.orderid == orderid);
            return Json(order);
        }

        public IActionResult edititemdata(int orderid)
        {
            var item = re.itemsections.FirstOrDefault(x => x.orderid == orderid);
            return Json(item);
        }

        public IActionResult delete(int orderid)
        {
            re.Database.ExecuteSqlRaw("delete from itemsections where orderid={0}", orderid);
            re.Database.ExecuteSqlRaw("delete from orderinvoice where orderid={0}", orderid);
            re.Database.ExecuteSqlRaw("delete from orderheader where orderid={0}", orderid);
            return Json("Deleted successfully");
        }

        public IActionResult edit(int id)
        {
            return View();
        }

        public IActionResult orderdata(int orderid)
        {
            var data = re.orderheader.Where(x => x.orderid == orderid).Select(p => p.ordercompanyid).ToList();
            var data1 = re.tradingparty_master.Where(x => data.Contains(x.mastercompanyid)).Select(x => x.email).ToList();
           
            
            return Json(data1);
        }

        public IActionResult data(string orderno,int orderid)
        {
            //var data = re.orderheader.Where(x => x.orderid == orderid).FirstOrDefault();
            //data.orderno = orderno;
            //data.orderid = 0;
            //re.Add(data);
            //re.SaveChanges();

            using (re)
            {
                



                var data = re.Database.ExecuteSqlRaw(" EXEC UpdateOrder @OrderId, @OrderNo",
                    new SqlParameter("@OrderId", orderid),
                    new SqlParameter("@OrderNo", orderno));



                var data2 = re.Database.ExecuteSqlRaw(" EXEC updateinvoice @OrderId, @OrderNo",
                   new SqlParameter("@OrderId", orderid),
                   new SqlParameter("@OrderNo", orderno));

                var data3 = re.Database.ExecuteSqlRaw(" EXEC updateitem @OrderId, @OrderNo",
                   new SqlParameter("@OrderId", orderid),
                   new SqlParameter("@OrderNo", orderno));


                re.SaveChanges();
            }
           

            //var data1 = re.orderinvoice.Where(x => x.orderid == orderid).ToList();
            //foreach (var item in data1)
            //{
            //    item.orderid = 0;
            //    data1.Add(new orderinvoice { orderid = data.orderid });
            //}


            //re.Add(data1);
            //re.SaveChanges();



            return Json(orderno);
        }

        public IActionResult invoicecopy(int invoiceid,string invoiceno)
        {
           
                    using (re)
                    {

                        var data1 = re.Database.ExecuteSqlRaw(" EXEC invoicecopy @invoiceno,@invoiceid",
                            new SqlParameter("@invoiceno",invoiceno),
                            new SqlParameter("@invoiceid",invoiceid));

                          re.SaveChanges();
                          
                        if(data1>=0)
                        {
                             var data2 = re.Database.ExecuteSqlRaw(" EXEC itemcopy @invoiceno,@invoiceid",
                             new SqlParameter("@invoiceno", invoiceno),
                             new SqlParameter("@invoiceid", invoiceid));

                             re.SaveChanges();

                              return Json("Invoice copied");
                        }
                        else
                        {
                             return Json("Invoiceno already exsits");
                        }
                    }
             
            
            return Json("Invoice copied");
        }

        public IActionResult invoicecopydata(int invoiceid)
        {
            var data = re.orderinvoice.Where(x => x.invoiceid == invoiceid).Select(x => x.orderid).FirstOrDefault();
            var data1 = re.orderinvoice.Where(x => x.orderid ==data).ToList();
            return Json(data1);
        }
        public IActionResult itemcopydata(int invoiceid)
        {
            var data = re.orderinvoice.Where(x => x.invoiceid == invoiceid).Select(x => x.orderid).FirstOrDefault();
            var data1 = re.itemsections.Where(x => x.orderid == data).ToList();
            return Json(data1);
        }

        public IActionResult orderitemcopy(int itemid)
        {
            using (re)
            {
                var data = re.Database.ExecuteSqlRaw(" EXEC itemordercopy @itemid",
                    new SqlParameter("@itemid",itemid));
                re.SaveChanges();
                if(data>=0)
                {
                    return Json("Item copied successfully");
                }
                else
                {
                    return Json("Data itemamount is more than invoicevalue");
                }
            }
                return Json(data);
        }

        public IActionResult itemcopy(int itemid)
        {
            var data = re.itemsections.Where(x => x.itemid == itemid).Select(x => x.orderid).FirstOrDefault();

            var data2 = re.itemsections.Where(x => x.orderid == data).ToList();
            return Json(data2);
        }
    }
}
