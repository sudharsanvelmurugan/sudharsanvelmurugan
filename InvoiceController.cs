using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using sample_task.Data;
using sample_task.Models;

namespace sample_task.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly application gg;

        public InvoiceController(application logger)
        {
            gg = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult saveinvoice( orderinvoice obj)
        {

            ViewBag.id = TempData["orderid"];

            obj.orderid = ViewBag.id;

            if (obj.invoiceid!=0)
            {
                var existingInvoice = gg.orderinvoice.FirstOrDefault(x => x.invoiceid == obj.invoiceid);

                if (existingInvoice != null)
                {

                    existingInvoice.invoiceno = obj.invoiceno;
                    existingInvoice.invoicetype = obj.invoicetype;
                    existingInvoice.currencycode = obj.currencycode;
                    existingInvoice.exchangerate = obj.exchangerate;
                    existingInvoice.invoiceamount = obj.invoiceamount;
                    existingInvoice.invoicelocalvalue = obj.invoicelocalvalue;
                    existingInvoice.inschargetype = obj.inschargetype;
                    existingInvoice.insvalue = obj.insvalue;
                    existingInvoice.inslocalvalue = obj.inslocalvalue;
                    existingInvoice.othchargetype = obj.othchargetype;
                    existingInvoice.othvalue = obj.othvalue;
                    existingInvoice.othlocalvalue = obj.othlocalvalue;
                    existingInvoice.costins = obj.costins;
                    existingInvoice.costoth = obj.costoth;
                    existingInvoice.costinsoth = obj.costinsoth;

                }
                else
                {
                    return Json(new { success = false, message = "Invoice not found" });
                }
               
            }
            else
            {
                gg.Add(obj);
            }

            
            gg.SaveChanges();
            TempData["orderid"] = ViewBag.id;
            return Json(obj);
        }

        public IActionResult update(int invoiceid)
        {

            var model = gg.orderinvoice.FirstOrDefault(x => x.invoiceid == invoiceid);
            return Json(model);

            //List<orderinvoice> invoice = new List<orderinvoice>();

            //invoice = gg.orderinvoice.FromSqlRaw("select * from orderinvoice where invoiceid={0}", invoiceid).ToList();

            //orderinvoice obj = new orderinvoice();
            //foreach (var item in invoice)
            //{
            //    obj.invoiceid = item.invoiceid;
            //    obj.invoiceno = item.invoiceno;
            //    obj.invoicedate = item.invoicedate;
            //    obj.currencycode = item.currencycode;
            //    obj.exchangerate = item.exchangerate;
            //    obj.invoiceamount = item.invoiceamount;
            //    obj.invoicelocalvalue = item.invoicelocalvalue;
            //    obj.inschargetype = item.inschargetype;
            //    obj.insvalue = item.insvalue;
            //    obj.inslocalvalue = item.inslocalvalue;
            //    obj.othchargetype = item.othchargetype;
            //    obj.othvalue = item.othvalue;
            //    obj.othlocalvalue = item.othlocalvalue;
            //    obj.costins = item.costins;
            //    obj.costoth = item.costoth;
            //    obj.costinsoth = item.costinsoth;
            //}

            //return Json(obj);
        }



        public IActionResult delete(int invoiceid)
        {
            //var data = gg.orderinvoice.Find(invoiceid);
            //if (data != null)
            //{
            //    gg.orderinvoice.Remove(data);
            //    gg.SaveChanges();
            //}

            var data1 = gg.orderinvoice.Where(x => x.invoiceid == invoiceid).ToList();

            int data = data1[0].orderid;

            gg.Database.ExecuteSqlRaw("delete from orderinvoice where invoiceid={0}", invoiceid);

            var selectdata = gg.orderinvoice.Where(x => x.orderid == data).ToList();

            var deletedata = selectdata[0].orderid;

            return Json(deletedata);
        }

        public IActionResult getdata(orderinvoice obj)
        {

            var data = gg.orderinvoice.Where(x => x.orderid == obj.orderid).ToList();
            return Json(data);
        }

        public IActionResult getdatadelete(int orderid)
        {
            var data = gg.orderinvoice.Where(x => x.orderid == orderid).ToList();
            return Json(data);
        }
        public IActionResult itemsection()
        {
            return View();
        }

        public IActionResult iteminvoiceno(string prefix)
        {

            //ViewBag.companyiditem = TempData["companyid"];
            int companyId = Convert.ToInt32(TempData["companyid"]);
            List<itemmaster> data = new List<itemmaster>();

            var list = (from p in gg.itemmaster where (p.itemcode).Contains(prefix) && p.companyiditem==companyId   select p).Take(3).ToList();
            var jsonData = list.Select(item => new
            {
                itemcode = string.Format("{0}", item.itemcode),
                companyiditem = item.companyiditem,

            });

            //ViewBag.dataid = TempData["orderid"];
            //TempData.Keep();
            TempData["companyid"] = companyId;
            return Json(jsonData);
        }

        public IActionResult getdbdata(int companyiditem,string itemcode)
        {
            //var data = gg.orderheader.Where(x => x.orderid == orderid).ToList();

            //var data1 = data[0].ordercompanyid;

           
            var data2 = gg.itemmaster.Where(x => x.companyiditem ==companyiditem && x.itemcode==itemcode).ToList();

            return Json(data2);
        }

        public IActionResult itemsave(itemsection obj)
        {
            ViewBag.itemid = TempData["orderid"];

            obj.orderid = ViewBag.itemid;

             if (obj.itemid!=0)
            {
                var existingItem = gg.itemsections.FirstOrDefault(x => x.itemid == obj.itemid);

                if (existingItem != null)
                {

                    existingItem.invoiceno = obj.invoiceno;
                    existingItem.itemcode = obj.itemcode;
                    existingItem.itemdescription = obj.itemdescription;
                    existingItem.quantity = obj.quantity;
                    existingItem.itemamount = obj.itemamount;
                    existingItem.itemlocalvalue = obj.itemlocalvalue;
                    existingItem.unitprice = obj.unitprice;

                }
                else
                {
                    return Json(new { success = false, message = "Invoice not found" });
                }
               
            }
            else
            {
                gg.Add(obj);
            }

           
            gg.SaveChanges();

            TempData["orderid"] = ViewBag.itemid;
            return Json(obj);
        }

        public IActionResult invoiceno(string invoiceno)
        {
            var number = gg.orderinvoice.Where(x => x.invoiceno == invoiceno).Select(x => x.invoicelocalvalue).ToList();
           
            return Json(number);
        }

        public IActionResult getdataitem(itemsection obj)
        {
            var data = gg.itemsections.Where(x => x.orderid == obj.orderid).ToList();
            return Json(data);
        }

        public IActionResult updateitem(int itemid)
        {
            var data = gg.itemsections.FirstOrDefault(x => x.itemid == itemid);
            return Json(data);
        }

        public IActionResult deleteitem(int itemid)
        {
            var id = gg.itemsections.Where(x => x.itemid == itemid).ToList();
            var data = id[0].orderid;
            gg.Database.ExecuteSqlRaw("delete from itemsections where itemid={0}",itemid);
            var selectdata = gg.itemsections.Where(x => x.orderid == data).ToList();
            if (selectdata.Count == 0)
            {
                return Json("This order id completely deleted");
            }
            var deleteitem = selectdata[0].orderid;
            return Json(deleteitem);
        }

        public IActionResult getitemdelete(int orderid)
        {
            var item = gg.itemsections.Where(x => x.orderid ==orderid).ToList();
            return Json(item);
        }

        public IActionResult getdropdown()
        {
            int dropdown = Convert.ToInt32(TempData["orderid"]);
            var data = gg.orderinvoice.Where(x => x.orderid == dropdown).ToList();
            var data1 = data[0].orderid;
            TempData["orderid"] = dropdown;
            return Json(data);
        }
    }
}
