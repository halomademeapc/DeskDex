using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DeskDexCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeskDexCore.Controllers
{
    [Authorize(Policy = "Admins")]
    public class StationController : Controller
    {
        private DeskContext db;
        private readonly IHostingEnvironment _hostingEnvironment;

        public StationController(DeskContext context, IHostingEnvironment hostingEnvironment)
        {
            db = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Stations
        public async Task<IActionResult> Index()
        {
            ViewBag.UserName = ((ClaimsIdentity)User.Identity).Claims;
            return View(await db.Stations.OrderByDescending(s => s.ID).Include(stat => stat.Type).Include(stat => stat.Floor).ToListAsync());
        }

        // GET: Stations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Station station = await db.Stations.Include(stat => stat.Type).Include(stat => stat.StationEquipments).ThenInclude(se => se.Equipment).Include(se => se.Floor).FirstOrDefaultAsync(s => s.ID == id);
            if (station == null)
            {
                return NotFound();
            }
            return View(station);
        }

        // GET: Stations/Create
        public async Task<IActionResult> Create()
        {
            var stationViewModel = new StationViewModel
            {
                Station = new Station()

            };

            var _equipment = db.Equipment.ToListAsync();
            var _types = db.WorkStyles.ToListAsync();
            var _floors = db.Floors.ToListAsync();

            var allEquipmentList = await _equipment;
            stationViewModel.AllEquipment = allEquipmentList.Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.ID.ToString()
            });

            var allTypesList = await _types;
            stationViewModel.AllWorkStyles = allTypesList.Select(w => new SelectListItem
            {
                Text = w.Name,
                Value = w.ID.ToString()
            });

            var allFloorsList = await _floors;
            stationViewModel.AllFloors = allFloorsList.OrderBy(f => f.SortName).Select(f => new SelectListItem
            {
                Text = f.Name,
                Value = f.ID.ToString()
            });

            return View(stationViewModel);
        }

        // POST: Stations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StationViewModel stationViewModel)
        {
            if (ModelState.IsValid)
            {
                var station = stationViewModel.Station;

                // update model
                //station.Equipment = db.Equipment.Where(o => stationViewModel.SelectedEquipment.Contains(o.ID)).ToList();
                station.StationEquipments = new List<StationEquipment>();

                foreach (var equip in stationViewModel.SelectedEquipment)
                {
                    station.StationEquipments.Add(new StationEquipment
                    {
                        StationId = station.ID,
                        EquipmentId = equip
                    });
                }

                station.Type = await db.WorkStyles.FirstOrDefaultAsync(w => w.ID == stationViewModel.selectedWorkStyle);
                station.Floor = await db.Floors.FirstOrDefaultAsync(f => f.ID == stationViewModel.selectedFloor);

                // handle image
                try
                {
                    if (stationViewModel.File.Length > 0)
                    {
                        // get file name
                        string _FileName = $@"{Guid.NewGuid()}.svg";
                        string _Folder = Path.Combine(_hostingEnvironment.WebRootPath, "Floors");
                        string _Path = Path.Combine(_Folder, _FileName);

                        // make folder if needed
                        if (!Directory.Exists(_Folder))
                        {
                            Directory.CreateDirectory(_Folder);
                        }

                        using (var memoryStream = new MemoryStream())
                        {
                            stationViewModel.File.OpenReadStream().CopyTo(memoryStream);
                            Image scaled = Resize(Image.FromStream(memoryStream), 600, 600);

                            // convert image
                            scaled.Save(_Path, ImageFormat.Jpeg);

                            station.FilePath = "/Uploaded/" + _FileName;

                            scaled.Dispose();
                        }

                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = "File upload failed.";
                }

                db.Stations.Add(station);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(stationViewModel);
        }

        // GET: Stations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var stationViewModel = new StationViewModel
            {
                Station = await db.Stations.Include(s => s.StationEquipments).Include(s => s.Floor).FirstOrDefaultAsync(s => s.ID == id)
            };
            if (stationViewModel.Station == null)
            {
                return NotFound();
            }

            var allEquipmentList = db.Equipment.ToListAsync();
            var allTypesList = db.WorkStyles.ToListAsync();
            var allFloorsList = db.Floors.ToListAsync();

            stationViewModel.AllEquipment = (await allEquipmentList).Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.ID.ToString()
            });

            stationViewModel.AllWorkStyles = (await allTypesList).Select(w => new SelectListItem
            {
                Text = w.Name,
                Value = w.ID.ToString()
            });

            stationViewModel.AllFloors = (await allFloorsList).OrderBy(f => f.SortName).Select(f => new SelectListItem
            {
                Text = f.Name,
                Value = f.ID.ToString()
            });

            return View(stationViewModel);
        }

        // POST: Stations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StationViewModel stationViewModel)
        {
            if (ModelState.IsValid)
            {
                await db.StationEquipments
                    .Where(se => se.StationId == stationViewModel.Station.ID)
                    .ForEachAsync(old => db.StationEquipments.Remove(old));
                await db.SaveChangesAsync();

                var station = stationViewModel.Station;

                var oldEntry = await db.Stations.Include(s => s.StationEquipments).FirstOrDefaultAsync(s => s.ID == stationViewModel.Station.ID);

                var ogImage = oldEntry.FilePath;

                // update old row
                db.Entry(oldEntry).CurrentValues.SetValues(station);

                // handle image
                try
                {
                    if (stationViewModel.File.Length > 0)
                    {
                        // get file name
                        string _FileName = $@"{Guid.NewGuid()}.jpg";
                        string _Path = Path.Combine(_hostingEnvironment.WebRootPath, "Uploaded", _FileName);

                        using (var memoryStream = new MemoryStream())
                        {
                            stationViewModel.File.OpenReadStream().CopyTo(memoryStream);
                            Image scaled = Resize(Image.FromStream(memoryStream), 600, 600);

                            // convert image
                            scaled.Save(_Path, ImageFormat.Jpeg);

                            oldEntry.FilePath = "/Uploaded/" + _FileName;

                            scaled.Dispose();

                            // delete old image
                            string delTarget = !String.IsNullOrEmpty(ogImage) ? _hostingEnvironment.WebRootPath + ogImage.Replace("/", "\\") : String.Empty;
                            if (!String.IsNullOrEmpty(delTarget) && System.IO.File.Exists(delTarget))
                            {
                                System.IO.File.Delete(delTarget);
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    ViewBag.Message = "File upload failed.";
                    // restore previous image
                    oldEntry.FilePath = ogImage;
                }

                //oldEntry.Equipment = db.Equipment.Where(o => stationViewModel.SelectedEquipment.Contains(o.ID)).ToList();
                station.StationEquipments = new List<StationEquipment>();

                foreach (var equip in stationViewModel.SelectedEquipment)
                {
                    oldEntry.StationEquipments.Add(new StationEquipment
                    {
                        StationId = station.ID,
                        EquipmentId = equip
                    });
                }

                var _type = db.WorkStyles.FirstOrDefaultAsync(t => t.ID == stationViewModel.selectedWorkStyle);
                var _floor = db.Floors.FirstOrDefaultAsync(f => f.ID == stationViewModel.selectedFloor);

                oldEntry.Type = await _type;
                oldEntry.Floor = await _floor;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(stationViewModel);
        }

        // GET: Stations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Station station = await db.Stations.FirstOrDefaultAsync(s => s.ID == id);
            if (station == null)
            {
                return NotFound();
            }
            return View(station);
        }

        // POST: Stations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Station station = await db.Stations.FirstOrDefaultAsync(s => s.ID == id);
            string ogImage = station.FilePath;
            db.Stations.Remove(station);
            await db.SaveChangesAsync();

            // delete old image
            string delTarget = !String.IsNullOrEmpty(ogImage) ? _hostingEnvironment.WebRootPath + ogImage.Replace("/", "\\") : String.Empty;
            if (!String.IsNullOrEmpty(delTarget) && System.IO.File.Exists(delTarget))
            {
                System.IO.File.Delete(delTarget);
            }

            return RedirectToAction("Index");
        }

        public Image Resize(Image current, int maxWidth, int maxHeight)
        {
            /* resizes an image to fit within provided size limitations
             */

            int width, height;
            #region reckon size 
            if (current.Width > current.Height)
            {
                width = maxWidth;
                height = Convert.ToInt32(current.Height * maxHeight / (double)current.Width);
            }
            else
            {
                width = Convert.ToInt32(current.Width * maxWidth / (double)current.Height);
                height = maxHeight;
            }
            #endregion

            #region get resized bitmap 
            var canvas = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(canvas))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(current, 0, 0, width, height);
            }

            return canvas;
            #endregion
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
