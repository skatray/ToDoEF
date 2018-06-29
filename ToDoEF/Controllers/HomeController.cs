using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

            


            return View(db.TablelistSet.ToList());           
        }
        public async Task<IActionResult> In()
        { 
            var query = from l in db.TablelistSet
                        join lg in db.TablelistTablegroup
                        on l.Id equals lg.TablelistId
                        join g in db.TablegroupSet
                        on lg.TablegroupId equals g.Id
                        select new
                        {
                            Nameoftask = l.TaskName,
                            DateStart = l.DateStart,
                            DateEnd = l.DateEnd,
                            Groups = g.Name
                        };
            /*   var dbs = _dbContext.TablelistSet.Join(_dbContext.TablelistTablegroup,
                 l => l.Id,
                 lg => lg.TablelistId,
                 (l, lg) => new {
                     Id = l.Id,
                     Name = l.TaskName,
                     Groupsid = lg.TablelistId                
                 }); */

            //var task = db.TablelistSet.Where(s => s.Id == 10);
            ViewBag.task = "huy";
            string text=null;
            foreach(var d in db.TablelistTablegroup)
            {                
                text += d.Tablegroup+" "+d.Tablelist+"\n";               
            }


            ViewBag.task = text;
            return View(query.ToList());
        }
        public IActionResult Create()
        {
            return View();
        } 
        public async Task<IActionResult> Details(int id)
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
