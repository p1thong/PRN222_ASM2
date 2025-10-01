using ASM1.Repository.Models;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    public class DealerContractController : Controller
    {
        private readonly IDealerContractService _dealerContractService;

        public DealerContractController(IDealerContractService dealerContractService)
        {
            _dealerContractService = dealerContractService;
        }

        // GET: DealerContract
        public async Task<IActionResult> Index()
        {
            var contracts = await _dealerContractService.GetAllDealerContractsAsync();
            return View(contracts);
        }

        // GET: DealerContract/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var contract = await _dealerContractService.GetDealerContractByIdAsync(id);
            if (contract == null)
            {
                return NotFound();
            }
            return View(contract);
        }

        // GET: DealerContract/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DealerContract/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DealerContract dealerContract)
        {
            if (ModelState.IsValid)
            {
                var result = await _dealerContractService.CreateDealerContractAsync(dealerContract);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Dealer contract created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "Failed to create dealer contract. Contract may already exist between this dealer and manufacturer.");
            }
            return View(dealerContract);
        }

        // GET: DealerContract/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var contract = await _dealerContractService.GetDealerContractByIdAsync(id);
            if (contract == null)
            {
                return NotFound();
            }
            return View(contract);
        }

        // POST: DealerContract/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DealerContract dealerContract)
        {
            if (id != dealerContract.DealerContractId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _dealerContractService.UpdateDealerContractAsync(dealerContract);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Dealer contract updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "Failed to update dealer contract.");
            }
            return View(dealerContract);
        }

        // GET: DealerContract/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _dealerContractService.GetDealerContractByIdAsync(id);
            if (contract == null)
            {
                return NotFound();
            }
            return View(contract);
        }

        // POST: DealerContract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _dealerContractService.DeleteDealerContractAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Dealer contract deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete dealer contract.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: DealerContract/ByDealer/5
        public async Task<IActionResult> ByDealer(int dealerId)
        {
            var contracts = await _dealerContractService.GetContractsByDealerAsync(dealerId);
            ViewBag.DealerId = dealerId;
            return View(contracts);
        }

        // GET: DealerContract/ByManufacturer/5
        public async Task<IActionResult> ByManufacturer(int manufacturerId)
        {
            var contracts = await _dealerContractService.GetContractsByManufacturerAsync(manufacturerId);
            ViewBag.ManufacturerId = manufacturerId;
            return View(contracts);
        }
    }
}
