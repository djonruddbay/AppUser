using AppUser.Data;
using AppUser.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppUser.Controllers
{
    public class WebSitesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WebSitesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WebSitesModels
        public async Task<IActionResult> WebSitesIndex(
            int? profileId,
            int? webSiteMenuId,
            string webSiteMenuDescription,
            string sortOrder,
            string searchString,
            string SearchString,
            int? page)
        {
            if (String.IsNullOrEmpty(profileId.ToString()) ||
                String.IsNullOrEmpty(webSiteMenuId.ToString()) ||
                String.IsNullOrEmpty(webSiteMenuDescription))
            {
                return NotFound();
            }

            var webSitesMenuModel = await _context.WebSitesMenu
                .SingleOrDefaultAsync(m => m.Id == webSiteMenuId);

            if (webSitesMenuModel == null)
            {
                return RedirectToAction("WebSitesMenuIndex", "WebSitesMenu", new { profileId });
            }

            var webSites = from w in _context.WebSites
                .Where(m => m.WebSitesMenuId == webSiteMenuId)
                           select w;

            ViewData["ProfileId"] = profileId;
            ViewData["WebSiteMenuId"] = webSiteMenuId;
            ViewData["WebSiteMenuDescription"] = webSiteMenuDescription;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["SortOrder"] = String.IsNullOrEmpty(sortOrder) ? "description_desc" : "";
            ViewData["SortFaArrow"] = sortOrder == "description_desc" ? "fa fa-arrow-down" : "fa fa-arrow-up";
            ViewData["SearchString"] = searchString;
            ViewData["WebSitesTotal"] = webSites.Count();

            if (searchString != null)
            {
                webSites = webSites.Where(d => d.Description.ToUpper().Contains(searchString.ToUpper())
                               || d.WebSiteURL.ToUpper().Contains(searchString.ToUpper()));
                page = 1;
            }
            ViewData["WebSitesSearchCount"] = webSites.Count();

            switch (sortOrder)
            {
                case "description_desc":
                    webSites = webSites.OrderByDescending(s => s.Description);
                    break;
                default:
                    webSites = webSites.OrderBy(s => s.Description);
                    break;
            }

            int pageSize = 6;
            return View(await PaginatedList<WebSitesModel>.CreateAsync(webSites.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: WebSitesModels/WebSitesAddNew
        public IActionResult WebSitesAddNew(
            int? profileId,
            int? webSiteMenuId,
            string webSiteMenuDescription
            )
        {
            if (String.IsNullOrEmpty(profileId.ToString()) ||
                String.IsNullOrEmpty(webSiteMenuId.ToString()) ||
                String.IsNullOrEmpty(webSiteMenuDescription))
            {
                return NotFound();
            }

            ViewData["ProfileId"] = profileId;
            ViewData["WebSiteMenuId"] = webSiteMenuId;
            ViewData["WebSiteMenuDescription"] = webSiteMenuDescription;
            return View();
        }

        // POST: WebSitesModels/WebSitesAddNew
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            WebSitesAddNew([Bind("Id,Description,WebSiteURL,WebSiteMenuId")]
            int? profileId,
            int webSiteMenuId,
            string webSiteMenuDescription,
            WebSitesModel webSitesModel)
        {
            if (String.IsNullOrEmpty(profileId.ToString()) ||
                String.IsNullOrEmpty(webSiteMenuId.ToString()) ||
                String.IsNullOrEmpty(webSiteMenuDescription))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                webSitesModel.WebSitesMenuId = webSiteMenuId;
                _context.Add(webSitesModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(WebSitesController.WebSitesIndex), new { webSiteMenuId, profileId, webSiteMenuDescription });
        }

        // GET: WebSitesModels/Edit/5
        public async Task<IActionResult> WebSitesEdit(
            int? id,
            int? profileId,
            int? webSiteMenuId,
            string webSiteMenuDescription
            )
        {
            if (String.IsNullOrEmpty(id.ToString()) ||
                String.IsNullOrEmpty(profileId.ToString()) ||
                String.IsNullOrEmpty(webSiteMenuId.ToString()) ||
                String.IsNullOrEmpty(webSiteMenuDescription))
            {
                return NotFound();
            }

            ViewData["Id"] = id;
            ViewData["ProfileId"] = profileId;
            ViewData["WebSiteMenuId"] = webSiteMenuId;
            ViewData["WebSiteMenuDescription"] = webSiteMenuDescription;

            var webSitesModel = await _context.WebSites.SingleOrDefaultAsync(m => m.Id == id);
            if (webSitesModel == null)
            {
                return RedirectToAction(nameof(WebSitesController.WebSitesIndex), new { webSiteMenuId, profileId, webSiteMenuDescription });
            }

            return View(webSitesModel);
        }

        // POST: WebSitesModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WebSitesEdit(
            [Bind("Id,Description,WebSiteURL,WebSitesMenuId")]
            int? id,
            int? profileId,
            int webSiteMenuId,
            string webSiteMenuDescription,
            WebSitesModel webSitesModel)
        {
            if (String.IsNullOrEmpty(id.ToString()) ||
                String.IsNullOrEmpty(profileId.ToString()) ||
                String.IsNullOrEmpty(webSiteMenuId.ToString()) ||
                String.IsNullOrEmpty(webSiteMenuDescription))
            {
                return NotFound();
            }

            if (webSitesModel == null ||
                id != webSitesModel.Id)
            {
                return RedirectToAction(nameof(WebSitesController.WebSitesIndex), new { webSiteMenuId, profileId, webSiteMenuDescription });
            }

            if (ModelState.IsValid)
            {
                _context.Update(webSitesModel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(WebSitesController.WebSitesIndex), new { webSiteMenuId, profileId, webSiteMenuDescription });
        }

        // POST: WebSitesModels/Delete/5
        //[HttpPost ]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> WebSitesDelete(
            int id,
            int? profileId,
            int? webSiteMenuId,
            string webSiteMenuDescription,
            WebSitesModel webSitesModel)
        {
            if (String.IsNullOrEmpty(id.ToString()) ||
                String.IsNullOrEmpty(profileId.ToString()) ||
                String.IsNullOrEmpty(webSiteMenuId.ToString()) ||
                String.IsNullOrEmpty(webSiteMenuDescription))
            {
                return NotFound();
            }

            webSitesModel = await _context.WebSites.SingleOrDefaultAsync(m => m.Id == id);
            if (webSitesModel == null)
            {
                return RedirectToAction(nameof(WebSitesController.WebSitesIndex), new { webSiteMenuId, profileId, webSiteMenuDescription });
            }

            _context.WebSites.Remove(webSitesModel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(WebSitesController.WebSitesIndex), new { webSiteMenuId, profileId, webSiteMenuDescription });
        }

    }
}
