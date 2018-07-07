using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoEF.Models;

namespace ToDoEF.Controllers
{
    public class TablegroupsController : Controller
    {
    
        private readonly ModelContext _context;

        public TablegroupsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Tablegroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.TablegroupSet.ToListAsync());
        }
        [HttpPost]
        public IActionResult Create(Tablegroup context)
        {

            if (!string.IsNullOrEmpty(context.Name))
            {
                _context.Add(context);
                // сохраняем в бд все изменения
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }

        // GET: Tablegroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           




            var tablegroup = await _context.TablegroupSet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tablegroup == null)
            {
                return NotFound();
            }

            return View(tablegroup);
        }

        // GET: Tablegroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tablegroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*   [HttpPost]
       [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([Bind("Id,Name")] Tablegroup tablegroup)
         {
             if (ModelState.IsValid)
             {
                 _context.Add(tablegroup);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }
             return View(tablegroup);
         }
         */
        // GET: Tablegroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tablegroup = await _context.TablegroupSet.FindAsync(id);
            if (tablegroup == null)
            {
                return NotFound();
            }
            return View(tablegroup);
        }

        // POST: Tablegroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Tablegroup tablegroup)
        {
            if (id != tablegroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tablegroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TablegroupExists(tablegroup.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tablegroup);
        }

        // GET: Tablegroups/Delete/5
        public  IActionResult Delete(int? id)
        {
            var task = _context.TablegroupSet.Find(id);
            if (null != task)
            {
                _context.TablegroupSet.Remove(task);

                // сохраняем в бд все изменения
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        /* POST: Tablegroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tablegroup = await _context.TablegroupSet.FindAsync(id);
            _context.TablegroupSet.Remove(tablegroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */
        private bool TablegroupExists(int id)
        {
            return _context.TablegroupSet.Any(e => e.Id == id);
        }
    }
}
