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
            CleanNoteCategories();
            ViewBag.Notes = await _context.Note.ToListAsync();
            ViewBag.Categories = await _context.Category.ToListAsync();
            ViewBag.NoteCategories = await _context.NoteCategory.ToListAsync();
                        
            return View();
        }
        // GET: Notebook/Create
        public IActionResult Create()
        {
            ViewBag.Categories = FindNewNoteCategories(0);
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
                if(_context.Note.Where(notie =>notie.Title == note.Title).Count() ==0)               
                    _context.Add(note);
                else
                    return View(note);
                await _context.SaveChangesAsync(); 
                SubmitNoteCategories(note.NoteID);
                return RedirectToAction(nameof(Index));
            }
            return View(note);
        }

        // GET: Notebook/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            /*if (id == null)
            {
                return NotFound();
            }*/

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            ViewBag.Categories = FindNewNoteCategories(id);
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

        public async Task<IActionResult> UpdateCategory(string category, int id, string view, string submit)
        {
            if(submit == "Add")
                AddCategory(category,id);
            else if(submit == "Remove" )
                RemoveCategory(category,id);
            else
                return RedirectToAction(nameof(Index));
            
            if(view == "Create")
                return RedirectToAction(nameof(Create));
            else
                return RedirectToAction(nameof(Edit),new{id = id});
            
        }
        public async void AddCategory(String category, int id)
        {
            if(_context.Category.Where(cat =>cat.Name == category).Count() ==0)               
            {
                Category cat = new Category();
                cat.Name = category;
                _context.Add(cat);
                await _context.SaveChangesAsync();
            }
            int catID = _context.Category.Where(cat => cat.Name == category).First().CategoryID;    
            if(_context.NoteCategory.Where(cat => cat.NoteID == id && cat.CategoryID == catID).Count() == 0)
            {
                NoteCategory newNCat = new NoteCategory();            
                newNCat.CategoryID = catID;
                newNCat.NoteID =id;
                _context.Add(newNCat);
                await _context.SaveChangesAsync();
            }
            
        }
        public async void RemoveCategory(String category, int id)
        {
            if(_context.Category.Where(cat =>cat.Name == category).Count() ==0)
                return;
            int catID = _context.Category.Where(cat =>cat.Name == category).First().CategoryID;
            if(_context.NoteCategory.Where(cat => cat.NoteID == id && cat.CategoryID == catID).Count() != 0)
            {
                NoteCategory toDelete  = _context.NoteCategory.Where(cat => cat.NoteID == id && cat.CategoryID == catID).First();
                _context.NoteCategory.Remove(toDelete);
                await _context.SaveChangesAsync();
            }
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
        private List<Category> FindNewNoteCategories(int id)
        {
            List<Category> newNoteCategories = new List<Category>();
            foreach(var noteCategory in _context.NoteCategory.Where(nCat => nCat.NoteID == id))
            {
                newNoteCategories.Add(_context.Category.Find(noteCategory.CategoryID));
            }
            return newNoteCategories;
        }
        private async void CleanNoteCategories()
        {
            
            foreach(var nCatie in _context.NoteCategory.Where(nCat => nCat.NoteID ==0))
            {
                _context.NoteCategory.Remove(nCatie);
            }
            await _context.SaveChangesAsync(); 

        }
        private async void SubmitNoteCategories(int id)
        {
            foreach(var nCatie in _context.NoteCategory.Where(nCat => nCat.NoteID ==0))
            {
                
                nCatie.NoteID = id;
                _context.NoteCategory.Update(nCatie);
            }
            await _context.SaveChangesAsync(); 
        }
    }
}
