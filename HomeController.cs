using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sample_task.Data;
using sample_task.Models;
using System.Collections.Generic;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace sample_task.Controllers
{
    public class HomeController : Controller
    {
        private readonly application bb;

        public HomeController(application logger)
        {
            bb = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult logincheck(companyuser obj)
        {
           

            

            byte[] encData = new byte[obj.password.Length];
            encData = System.Text.Encoding.UTF8.GetBytes(obj.password);
            string encodedData = Convert.ToBase64String(encData);

            //string useremailtosearch = obj.useremail;
            //var id = (from companyuser in bb.companyuser where obj.useremail == useremailtosearch join company in bb.company on obj.companyuserid equals company.companyid select company.companyid).FirstOrDefault();

            //var companycode = bb.company.Where(c => c.companyid == obj.companyuserid).Select(c => c.companycode).FirstOrDefault();



            var data =bb.companyuser.Where(s=>s.useremail==obj.useremail).ToList();
             var data1=bb.companyuser.Where(s=>s.password==encodedData).ToList();
            int companyuserid = data[0].companyuserid;
             var data2=bb.company.Where(s=>s.companyid==companyuserid).ToList();
            string companycode = data2[0].companycode;
            int companyid= data2[0].companyid;

            TempData["id"] = companycode;
            TempData["companyid"] = companyid;
            TempData.Keep();

           
            if (data.Count == 1)
            {
                if(data1.Count==1)
                {
                   
                    return View("Privacy");
                }

            }
            else
            {
                ModelState.AddModelError("Email:", "email is not user");
            }
            
            return View("Index");
        }


       

        public IActionResult Privacy()
        {
            ViewBag.emp = TempData["id"];
            ViewBag.first = TempData["companyid"];
            TempData.Keep();
            return View();
        }
       
        public IActionResult orderentry(int id)
         {
            ViewBag.get = TempData["id"];
            ViewBag.second = TempData["companyid"];
            TempData.Keep();
            //string id = (string)TempData["id"];
            ViewBag.entryid = id;
            return View();
        }
        [HttpGet]
         public JsonResult order(string Prefix)
        {

            List<tradingparty> objlist = new List<tradingparty>();
            int companyid = Convert.ToInt32(TempData["companyid"]);
            //var orderlist = bb.tradingparty_master.Where(e => e.email.Contains(Prefix)).Select(e=>e.masterid).ToList();
            var orderlist = (from p in bb.tradingparty_master
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
        [HttpPost]
        public IActionResult save(orderentry obj1)
        {
            ViewBag.obj = TempData["id"];
            ViewBag.obj2 = TempData["companyid"];
            TempData.Keep();

            //var id = (from N in bb.tradingparty_master join M in bb.company on N.mastercompanyid equals M.companyid select N.mastercompanyid).FirstOrDefault();
          
            obj1.ordercompanyid = ViewBag.obj2;

            if(obj1.orderid!=0)
            {
                var data = bb.orderheader.FirstOrDefault(x => x.orderid == obj1.orderid);
                if (data != null)
                {
                    data.orderno = obj1.orderno;
                    data.orderdate = obj1.orderdate;
                    data.jobtype = obj1.jobtype;
                    data.initiator = obj1.initiator;
                    data.importer = obj1.importer;
                    data.exporter = obj1.exporter;
                    data.referencesno = obj1.referencesno;
                }
                else
                {
                    return Json("Data not saved");
                }
            }
            else
            {
                bb.Add(obj1);
            }

           
            bb.SaveChanges();

            int id = obj1.orderid;
            TempData["orderid"] = id;
            TempData["companyid"] = ViewBag.obj2;
           
            
            return Json(obj1);
        }


        public IActionResult invoice()
        {

            return View();
        }


        public JsonResult generateordernumber(string companyId, string selectedJobField)
        {

            string orderNumber = GenerateUniqueOrderNumber(companyId, selectedJobField);

            return Json(new { orderNumber });
        }

        private string GenerateUniqueOrderNumber(string companyId, string selectedJobField)
        {
            var ord = "ord";
            var monthLetter = GetMonthLetters();
            var year = GenerateCode();

            var jobFieldFirstLetter = selectedJobField?.Length > 0 ? selectedJobField[0].ToString().ToUpper() : " ";

            var counter = 1;

            while (true)
            {
                var paddedCounter = counter.ToString().PadLeft(5, '0');
                var orderNumber = companyId + ord + year + monthLetter + jobFieldFirstLetter + paddedCounter;


                if (!bb.orderheader.Any(o=>o.orderno==orderNumber))
                {

                    return orderNumber;
                }


                counter++;
            }
        }

        private string GetMonthLetters()
        {
            var monthIndex = DateTime.Now.Month - 1; 

            var monthLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var letter = monthLetters[monthIndex];

            return letter.ToString();
        }

        private string GenerateCode()
        {
            var currentYear = DateTime.Now.Year;

            var lastTwoDigits = (currentYear % 100).ToString("D2");

            var code = lastTwoDigits.ToUpper();

            return code;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}