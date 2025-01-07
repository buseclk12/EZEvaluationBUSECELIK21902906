using BLL.Controllers.Bases;
using BLL.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.Models;
using BLL.Services.Bases;

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EvaluatedsController : MvcController
    {
        private readonly IService<Evaluated, EvaluatedModel> _evaluatedService;

        public EvaluatedsController(IService<Evaluated, EvaluatedModel> evaluatedService)
        {
            _evaluatedService = evaluatedService;
        }

        public IActionResult Index()
        {
            var evaluateds = _evaluatedService.Query().ToList();
            return View(evaluateds);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EvaluatedModel evaluated)
        {
            if (ModelState.IsValid)
            {
                var result = _evaluatedService.Create(evaluated.Record);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(evaluated);
        }
    }
}