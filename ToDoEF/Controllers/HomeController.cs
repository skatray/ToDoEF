using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoEF.Models;

namespace ToDoEF.Controllers
{
    
    public class HomeController : Controller
    {
         
        private readonly ModelContext _dbContext;
        ModelContext db;
        

        int _id;
        public HomeController(ModelContext context)
        {
            db = context;
           _dbContext = context;
        }
        public IActionResult Index()
        {
           var query = from l in db.TablelistSet

                        join lg in db.TablelistTablegroup
                        on l.Id equals lg.TablelistId
                        into g1
                        from x in g1.DefaultIfEmpty()
                        join g in db.TablegroupSet
                        on x.TablegroupId equals g.Id
                        into g2
                        from y in g2.DefaultIfEmpty()
                        select new ViewModel(l.Id, l.TaskName, l.DateStart, l.DateEnd,/* y == null ? "no group" : */y.Name, x.TablelistId, x.TablegroupId);


            
           ViewBag.Dropdown  = (from c in db.TablegroupSet select new { c.Id, c.Name }).Distinct();
            IQueryable fdd = (from c in db.TablegroupSet select new { c.Id, c.Name }).Distinct();
            var fd =  new SelectList(ViewBag.Dropdown, "Id", "Name");
            fd.Where(c => c.Text == "group 1");
       


            return View(query.ToList());           
        }
        public IActionResult In()
        {
            var query = from l in db.TablelistSet

                        join lg in db.TablelistTablegroup
                        on l.Id equals lg.TablelistId
                        into g1 from x in g1.DefaultIfEmpty()
                        join g in db.TablegroupSet
                        on x.TablegroupId equals g.Id
                        into g2  from y in g2.DefaultIfEmpty()
                        select new ViewModel(l.Id,l.TaskName, l.DateStart, l.DateEnd,/* y == null ? "no group" : */y.Name,x.TablelistId,x.TablegroupId);



            return View(query.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Addgroup()
        {
            List<int> listidgroup = new List<int>();
            foreach (var model in db.TablegroupSet.ToList())
            {
                listidgroup.Add(model.Id);
            }
            List<int> listidtask = new List<int>();
            foreach (var model in db.TablelistSet.ToList())
            {
                listidtask.Add(model.Id);
            }
            int id(int max)
            {
                Random number = new Random();
                int i = number.Next(0, max);
                return i;
            }
            
            Tablelistgroup Tablelistgroup;
            void newTablelistgroup()
            {
                        Tablelistgroup = new Tablelistgroup { TablegroupId = listidgroup[id(listidgroup.Count)], TablelistId = listidtask[id(listidtask.Count)] };
              
            }
            newTablelistgroup();
           
           
                List<int[]> getlistgroup = new List<int[]>();
                foreach (var model in db.TablelistTablegroup.ToList())
                {
                    getlistgroup.Add(new int[]{ model.TablelistId, model.TablegroupId });
                }
            bool ok = false;
            while (!ok)
            {
                for (int i = 0; i < getlistgroup.Count; i++)
                {
                    int i0 = getlistgroup[i][0];
                    int i1 = getlistgroup[i][1];
                    if (Tablelistgroup.TablelistId == i0 && Tablelistgroup.TablegroupId == i1)
                    {
                        ok = false;
                        newTablelistgroup();
                        break;
                    }
                    else
                    {
                        ok = true;
                    }
                   
                }
            }
            if (ok) { 
            db.TablelistTablegroup.Add(Tablelistgroup);
            db.SaveChanges();
            }
            

            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            _id = id;
          
            var task = db.TablelistSet.Find(id);
            if (task != null)
            {
                return View(task);
            }
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            _id = id;
            var task =  db.TablelistSet.Single(s=>s.Id==id);
            ViewBag.task = task.TaskName;
            DateTime time;
            if (task.DateStart != null) { 
            time = Convert.ToDateTime(task.DateStart);
            ViewBag.DateStart = time.ToString("s");
            }
            if (task.DateEnd != null)
            {
                time = Convert.ToDateTime(task.DateEnd);
                ViewBag.DateEnd = time.ToString("s");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Index(Tablelist context)
        {
            if (!string.IsNullOrEmpty(context.TaskName))
            {
                db.TablelistSet.Add(context);
                // сохраняем в бд все изменения
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (string.IsNullOrEmpty(context.TaskName))
            {
                ViewBag.task = "error";                
            }
            return View();
        }
        [HttpPost]
        public IActionResult Edit(Tablelist context)
        {
            if (!string.IsNullOrEmpty(context.TaskName))
            {
                db.TablelistSet.Update(context);
                // сохраняем в бд все изменения
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (string.IsNullOrEmpty(context.TaskName))
            {
                ViewBag.task = "error";
            }
            return View();
        }
        [HttpPost]
        public IActionResult Create(Tablelist context, int groups)
        {
            var TablelistTablegroups = new Tablelistgroup();
           

            if (!string.IsNullOrEmpty(context.TaskName))
            {
                             
                db.TablelistSet.Add(context);
                db.SaveChanges();                
                // сохраняем в бд все изменения            
            }
            if (groups != 0)
            {
                if (context.Id == 0)
                {
                    context = db.TablelistSet.Last();
                }
                
                TablelistTablegroups.TablelistId = context.Id;
                TablelistTablegroups.TablegroupId = groups;
                db.TablelistTablegroup.Add(TablelistTablegroups);
                db.SaveChanges();
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        public IActionResult Ccreate(Tablelist context, int cgroups,int cid)
        {
            var TablelistTablegroups = new Tablelistgroup();


      
            
                if (context.Id == 0)
                {
                    context = db.TablelistSet.Last();
                }

                TablelistTablegroups.TablelistId = cid;
                TablelistTablegroups.TablegroupId = cgroups;
                db.TablelistTablegroup.Add(TablelistTablegroups);
                db.SaveChanges();
            
            return RedirectToAction("index");
        }





        [HttpGet]
        public IActionResult Delete(int id)
        {
            var task = db.TablelistSet.Find(id);            
                      if (null!=task)
            {
                db.TablelistSet.Remove(task);                
             
                // сохраняем в бд все изменения
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult DelGroup(int Taskid,int Groupid)
        {
            var task = db.TablelistTablegroup.Where(s=>s.TablelistId==Taskid);
           var task1 = task.Where(s => s.TablegroupId == Groupid).Single();
            if (null != task1)
            {
                db.TablelistTablegroup.Remove(task1);

                // сохраняем в бд все изменения
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
