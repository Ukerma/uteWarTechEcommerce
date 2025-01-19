using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models.Db;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class SettingsController : Controller
    {
        private readonly OnlineShopContext _context;

        public SettingsController(OnlineShopContext context)
        {
            _context = context;
        }

      
        
        // GET: Admin/Settings/Edit/5
        public async Task<IActionResult> Edit()
        {
           
            var setting = await _context.Settings.FirstAsync();
            if (setting == null)
            {
                return NotFound();
            }
            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
    [Bind("Id,Shipping,Title,Address,Email,Phone,Copyright,Instagram,Facebook,GooglePlus,Youtube,Twitter,Logo")]
       Setting setting, IFormFile? newLogo)
        {
            if (id != setting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (newLogo != null)
                    {

                        string d = Directory.GetCurrentDirectory();
                        string path = d + "\\wwwroot\\images\\" + setting.Logo;
                        //------------------------------------------------
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        //------------------------------------------------
                        setting.Logo = Guid.NewGuid() + Path.GetExtension(newLogo.FileName);
                        path = d + "\\wwwroot\\images\\" + setting.Logo;
                        //------------------------------------------------

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            newLogo.CopyTo(stream);
                        }
                    }

                    _context.Update(setting);
                    await _context.SaveChangesAsync();

                    TempData["message"] = "Setting saved";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettingExists(setting.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return Redirect($"/admin/Settings/Edit");
        }


        private bool SettingExists(int id)
        {
            return _context.Settings.Any(e => e.Id == id);
        }

    }
}
