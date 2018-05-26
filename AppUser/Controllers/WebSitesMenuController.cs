using AppUser.Data;
using AppUser.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppUser
{
    [Authorize(Roles = "AppUser")]

    public class WebSitesMenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WebSitesMenuController(ApplicationDbContext context)
        {
            _context = context;
        }
        public int ProfileId { get; set; }


        public async Task<IActionResult> WebSitesMenuIndex(int? profileId)
        {
            if (profileId == null)
            {
                return NotFound();
            }

            ViewData["ProfileId"] = profileId;

            var WebSitesMenu = _context.WebSitesMenu
                .Where(w => w.ProfileId == profileId)
                .OrderBy(o => o.SequenceNumber);

            return View(await WebSitesMenu.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFirst(
            int profileId,
            string inputDescription,
            WebSitesMenuModel webSitesMenuModel)
        {
            if (String.IsNullOrEmpty(profileId.ToString()))
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(inputDescription))
            {
                webSitesMenuModel.SequenceNumber = 1;
                webSitesMenuModel.ProfileId = profileId;
                webSitesMenuModel.Description = inputDescription;
                _context.Add(webSitesMenuModel);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(WebSitesMenuIndex), new { profileId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNew(
            int profileId,
            string inputDescription,
            string currId,
            string addLocation,
            WebSitesMenuModel webSitesMenuModel)
        {
            if (String.IsNullOrEmpty(profileId.ToString()) ||
                String.IsNullOrEmpty(currId) ||
                String.IsNullOrEmpty(addLocation))
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(inputDescription))
            {
                return RedirectToAction(nameof(WebSitesMenuIndex), new { profileId });
            }

            var menuItems = _context.WebSitesMenu
                .Where(w => w.ProfileId == profileId)
                .OrderBy(o => o.SequenceNumber);

            var seqNo = 0;
            var addAmt = 1;

            foreach (var item in menuItems)
            {
                seqNo = seqNo + addAmt;
                addAmt = 1;

                if (item.Id.ToString() == currId)
                {
                    webSitesMenuModel.ProfileId = profileId;
                    webSitesMenuModel.Description = inputDescription;

                    if (addLocation == "Above")
                    {
                        webSitesMenuModel.SequenceNumber = seqNo;
                        seqNo += 1;
                    }
                    if (addLocation == "Below")
                    {
                        webSitesMenuModel.SequenceNumber = seqNo + 1;
                        addAmt = 2;
                    }

                    _context.Add(webSitesMenuModel);
                }

                item.SequenceNumber = seqNo;
                var updateItem = await _context.WebSitesMenu.SingleOrDefaultAsync(m => m.Id == item.Id);
                _context.Update(updateItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(WebSitesMenuIndex), new { profileId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItem(
            int? currId,
            int? profileId,
            string inputDescription)
        {
            if (String.IsNullOrEmpty(profileId.ToString()) ||
                String.IsNullOrEmpty(currId.ToString()) ||
                String.IsNullOrEmpty(inputDescription))
            {
                return NotFound();
            }

            var webSitesMenuModel = await _context.WebSitesMenu
                .SingleOrDefaultAsync(m => m.Id == currId);

            if (webSitesMenuModel != null)
            {
                if (!string.IsNullOrEmpty(inputDescription))
                {
                    webSitesMenuModel.Description = inputDescription;
                    _context.Update(webSitesMenuModel);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(WebSitesMenuIndex), new { profileId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int currId, int profileId)
        {
            if (String.IsNullOrEmpty(profileId.ToString()) ||
                String.IsNullOrEmpty(currId.ToString()))
            {
                return NotFound();
            }

            var deleteItem = await _context.WebSitesMenu.SingleOrDefaultAsync(m => m.Id == currId);
            if (deleteItem != null)
            {
                _context.WebSitesMenu.Remove(deleteItem);
            }

            var seqNo = 0;

            foreach (var item in _context.WebSitesMenu
                .Where(p => p.ProfileId == profileId)
                .Where(i => i.Id != currId)
                .OrderBy(o => o.SequenceNumber))
            {
                seqNo += 1;
                var updateItem = await _context.WebSitesMenu.SingleOrDefaultAsync(m => m.Id == item.Id);
                updateItem.SequenceNumber = seqNo;
                _context.Update(updateItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(WebSitesMenuIndex), new { profileId });
        }

        public async Task<IActionResult> MoveItem(
            int? currId,
            int? profileId,
            string moveDirection,
            WebSitesMenuModel webSitesMenuModel)
        {
            if (String.IsNullOrEmpty(profileId.ToString()) ||
                String.IsNullOrEmpty(currId.ToString()) ||
                String.IsNullOrEmpty(moveDirection))
            {
                return NotFound();
            }
            var seqNo = 1;
            var increment = 1;

            if (moveDirection == "Up")
            {
                increment = -1;
            }

            var origIncrement = increment;
            var origSeqNo = seqNo;

            var menuItems = _context.WebSitesMenu
                .Where(w => w.ProfileId == profileId)
                .OrderBy(o => o.SequenceNumber * increment)
                .Include(w => w.Profile);

            if (moveDirection == "Up")
            {
                seqNo = menuItems.Count();
            }

            foreach (var item in menuItems)
            {
                origSeqNo = item.SequenceNumber;
                item.SequenceNumber = seqNo;

                seqNo = seqNo + increment;

                if (item.Id == currId)
                {
                    item.SequenceNumber = item.SequenceNumber + increment;
                    seqNo = seqNo - increment;
                    increment = increment * 2;
                }
                else
                {
                    increment = origIncrement;
                }

                if (item.SequenceNumber != origSeqNo)
                {
                    var updateItem = await _context.WebSitesMenu.SingleOrDefaultAsync(m => m.Id == item.Id);
                    updateItem.SequenceNumber = item.SequenceNumber;
                    _context.Update(updateItem);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(WebSitesMenuIndex), new { profileId });
        }

        protected override void Dispose(bool disposing)
        { if (disposing) { _context.Dispose(); } }

    }
}