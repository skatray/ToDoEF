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


            SelectList groups = new SelectList(db.TablegroupSet.ToList(), "id", "Name");
            //  Models.ViewModel.AddList(groups);
            ViewBag.Groups = groups;




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


            SelectList groups = new SelectList(db.TablegroupSet.ToList(),"id","Name");
          //  Models.ViewModel.AddList(groups);
              ViewBag.Groups = groups;


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

            //    var Tablelistgroup = new Tablelistgroup { TablegroupId = listidgroup[id(listidgroup.Count)], TablelistId = listidtask[id(listidtask.Count)] };
            Tablelistgroup Tablelistgroup;
            void newTablelistgroup()
            {
                        Tablelistgroup = new Tablelistgroup { TablegroupId = listidgroup[id(listidgroup.Count)], TablelistId = listidtask[id(listidtask.Count)] };
              //  Tablelistgroup = new Tablelistgroup { TablelistId = 3,  TablegroupId = 1};
                //      db.TablelistTablegroup.Add(Tablelistgroup);
                //       db.SaveChanges();
            }
            newTablelistgroup();
           
            /*      var connect = new Tablelistgroup[]
               {
                    new Tablelistgroup{TablegroupId=listidgroup[id(listidgroup.Count)],TablelistId=listidtask[id(listidtask.Count)]}                
                };*/
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
                    //    Tablelistgroup = getlistgroup[i];
                }
            }
            if (ok) { 
            db.TablelistTablegroup.Add(Tablelistgroup);
            db.SaveChanges();
            }
            /*   foreach (Tablelistgroup s in connect)
               {
                   db.TablelistTablegroup.Add(s);
               }
               */

            return RedirectToAction("In");
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
        public IActionResult Create(Tablelist context)
        {           
        
          if (!string.IsNullOrEmpty(context.TaskName))
            {
                db.TablelistSet.Add(context);
                // сохраняем в бд все изменения
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           return RedirectToAction("Create");
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
                return RedirectToAction("In");
            }
            return RedirectToAction("In");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
