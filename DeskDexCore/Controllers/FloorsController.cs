using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeskDexCore.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DeskDexCore.Controllers
{
    public class FloorsController : Controller
    {
        private readonly DeskContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;


        public FloorsController(DeskContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Floors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Floors.ToListAsync());
        }

        // GET: Floors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var floor = await _context.Floors
                .SingleOrDefaultAsync(m => m.ID == id);
            if (floor == null)
            {
                return NotFound();
            }

            return View(floor);
        }

        // GET: Floors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Floors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FloorViewModel fvm)
        {
            if (ModelState.IsValid)
            {
                // store file
                try
                {
                    if (fvm.File.Length > 0)
                    {
                        if (Path.GetExtension(fvm.File.FileName).ToLower() == ".svg")
                        {
                            // get file name
                            string _FileName = $@"{Guid.NewGuid()}.svg";
                            string _Path = Path.Combine(_hostingEnvironment.WebRootPath, "Floors", _FileName);

                            using (var stream = new FileStream(_Path, FileMode.Create))
                            {
                                await fvm.File.CopyToAsync(stream);
                                fvm.Floor.FilePath = "/Floors/" + _FileName;
                            }
                        }
                        else
                        {
                            ViewBag.Message("Invalid file uploaded. Please select an SVG file.");
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message("Unable to upload file.");
                }

                // commit changes
                _context.Floors.Add(fvm.Floor);

                return RedirectToAction(nameof(Index));
            }
            return View(fvm);
        }

        // GET: Floors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var floor = await _context.Floors.SingleOrDefaultAsync(m => m.ID == id);
            if (floor == null)
            {
                return NotFound();
            }
            return View(new FloorViewModel { Floor = floor });
        }

        // POST: Floors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FloorViewModel fvm)
        {
            if (id != fvm.Floor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // get old entry
                var oldEntry = _context.Floors.Find(fvm.Floor.ID);
                var ogImage = oldEntry.FilePath;

                // update values
                _context.Entry(oldEntry).CurrentValues.SetValues(fvm.Floor);

                // store file
                try
                {
                    if (fvm.File.Length > 0)
                    {
                        if (Path.GetExtension(fvm.File.FileName).ToLower() == ".svg")
                        {
                            // get file name
                            string _FileName = $@"{Guid.NewGuid()}.svg";
                            string _Path = Path.Combine(_hostingEnvironment.WebRootPath, "Floors", _FileName);

                            using (var stream = new FileStream(_Path, FileMode.Create))
                            {
                                await fvm.File.CopyToAsync(stream);
                                oldEntry.FilePath = "/Floors/" + _FileName;
                            }
                        } else
                        {
                            ViewBag.Message("Invalid file uploaded. Please select an SVG file.");
                            oldEntry.FilePath = ogImage;
                        }

                    } else
                    {
                        // no file uploaded
                        oldEntry.FilePath = ogImage;
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message("Unable to upload file.");
                    oldEntry.FilePath = ogImage;
                }

                // commit changes
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(fvm);
        }

        // GET: Floors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var floor = await _context.Floors
                .SingleOrDefaultAsync(m => m.ID == id);
            if (floor == null)
            {
                return NotFound();
            }

            return View(floor);
        }

        // POST: Floors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var floor = await _context.Floors.SingleOrDefaultAsync(m => m.ID == id);
            _context.Floors.Remove(floor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FloorExists(int id)
        {
            return _context.Floors.Any(e => e.ID == id);
        }
    }
}
