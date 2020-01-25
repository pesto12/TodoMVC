using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TodoMVC.Data;
using TodoMVC.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace TodoMVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly TodoMVCContext _context;

        public TaskController(TodoMVCContext context)
        {
            _context = context;
        }

        // GET: Task
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.TodoTask.ToListAsync();
            tasks = tasks.Select(t => {
                t.Topic = _context.Topic.FirstOrDefault(topic => topic.Id == t.TopicId);
                return t;
                }).ToList();

            return View(tasks);
        }

        // GET: Task/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoTask = await _context.TodoTask
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoTask == null)
            {
                return NotFound();
            }
            todoTask.Topic = await _context.Topic.FirstOrDefaultAsync(topic => topic.Id == todoTask.TopicId);

            return View(todoTask);
        }

        // GET: Task/Create
        public IActionResult Create()
        {
            PopulateTopicsDropDownList();
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Deadline,TopicId")] TodoTask todoTask)
        {
            try {
                if (ModelState.IsValid)
                {
                    _context.Add(todoTask);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }catch (RetryLimitExceededException){
               ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            
            PopulateTopicsDropDownList(todoTask.TopicId);
            return View(todoTask);
        }

        // GET: Task/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoTask = await _context.TodoTask.FindAsync(id);
            if (todoTask == null)
            {
                return NotFound();
            }

            PopulateTopicsDropDownList(todoTask.TopicId);
            return View(todoTask);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Deadline,TopicId")] TodoTask todoTask)
        {
            if (id != todoTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todoTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoTaskExists(todoTask.Id))
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
            PopulateTopicsDropDownList(todoTask.TopicId);
            return View(todoTask);
        }


        private async void PopulateTopicsDropDownList(object selectedTopic = null)
        {
            var topics = await _context.Topic.ToListAsync();
            ViewData["Topics"] = new SelectList(topics, "Id", "Name");
            // topics.Select(t => new SelectListItem(t.Id.ToString(), t.Name)).ToList();
        }

        // GET: Task/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todoTask = await _context.TodoTask
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todoTask == null)
            {
                return NotFound();
            }

            todoTask.Topic = await _context.Topic.FirstOrDefaultAsync(topic => topic.Id == todoTask.TopicId);

            return View(todoTask);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todoTask = await _context.TodoTask.FindAsync(id);
            _context.TodoTask.Remove(todoTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoTaskExists(int id)
        {
            return _context.TodoTask.Any(e => e.Id == id);
        }
    }
}
