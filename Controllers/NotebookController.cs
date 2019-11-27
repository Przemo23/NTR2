using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NTR02.Data;
using NTR02.Models;
using static NTR02.NotebookControl;

namespace NTR02.Controllers
{
    public class NotebookController : Controller
    {
        private readonly NotebookContext _context;

        public NotebookController(NotebookContext context)
        {
            _context = context;
        }

        // GET: Notebook
        public async Task<IActionResult> Index()
        {
            return View(await _context.Note.ToListAsync());
        }

        

        // GET: Notebook/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notebook/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NoteID,Title,Description,NoteDate")] Note note,
        string Category)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category();
                NoteCategory noteCategory = new NoteCategory();
                category.Name = Category;
               
                if(_context.Note.Where(notie =>notie.Title == note.Title).Count() ==0)               
                    _context.Add(note);
                else
                    return View(note);
                _context.Add(category);
                await _context.SaveChangesAsync();
                
                 noteCategory.NoteID = _context.Note.Where(notie => notie.Title == note.Title).First().NoteID;
                noteCategory.CategoryID = _context.Category.Where(cat => cat.Name == category.Name).First().CategoryID;
                _context.Add(noteCategory);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(note);
        }

        // GET: Notebook/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        // POST: Notebook/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NoteID,NoteDate,Title,Description")] Note note)
        {
            if (id != note.NoteID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.NoteID))
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
            return View(note);
        }

        // GET: Notebook/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .FirstOrDefaultAsync(m => m.NoteID == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notebook/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Note.FindAsync(id);
            _context.Note.Remove(note);
            await _context.SaveChangesAsync();
            updatePage();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Filter(DateTime from, DateTime to, string category)
        {
            if(DateTime.Compare(from,to) > 0)
                return RedirectToAction(nameof(Index));
            fromDate = from;
            toDate = to;
            filterCat = category;
            return RedirectToAction(nameof(Index));
        }
        public IActionResult DeleteFilter()
        {
            SetDefaults();
            filteredNotesCount = _context.Note.Count();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult NextPage()
        {
            if((page)* 10 < filteredNotesCount)
                page++;
            return RedirectToAction(nameof(Index));
        }
         public IActionResult PreviousPage()
        {
            if(page > 1)
                page--;
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.NoteID == id);
        }

        private void updatePage()
        {
            if(page*10 == filteredNotesCount +9 && page != 1)
                page--;
        }
    }
}
