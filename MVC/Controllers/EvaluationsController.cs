using BLL.Controllers.Bases;
using BLL.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers
{
    [Authorize]
    public class EvaluationsController : MvcController
    {
        private readonly IService<Evaluation, EvaluationModel> _evaluationService;
        private readonly IService<User, UserModel> _userService;
        private readonly IService<Evaluated, EvaluatedModel> _evaluatedService;


        public EvaluationsController(IService<Evaluation, EvaluationModel> evaluationService, IService<User, UserModel> userService, IService<Evaluated, EvaluatedModel> EvaluatedService)
        {
            _evaluationService = evaluationService;
            _userService = userService;
            _evaluatedService = EvaluatedService;
        }
        protected void SetViewData (int? currentEvaluatedId = null)
        {
            // Kullanıcılar için dropdown
            ViewBag.UserIds = new SelectList(_userService.Query().ToList(), "Record.Id", "UserName");

            // Evaluated listesi için dropdown
            // Evaluated dropdown
            var evaluateds = _evaluatedService.Query()
                .Select(e => new
                {
                    e.Record.Id,
                    FullName = (e.Record.Name ?? "Unknown") + " " + (e.Record.Surname ?? "Unknown")
                })
                .ToList();

            ViewBag.EvaluatedIds = new SelectList(evaluateds, "Id", "FullName", currentEvaluatedId);

        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var evaluations = _evaluationService.Query().ToList();
            return View(evaluations);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var evaluation = _evaluationService.Query().SingleOrDefault(e => e.Record.Id == id);
            if (evaluation == null)
            {
                return NotFound();
            }
            return View(evaluation);
        }

        [Authorize]
        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(EvaluationModel evaluation)
        {
            if (ModelState.IsValid)
            {
                // Evaluated IDs'leri Evaluation nesnesine bağlama
                foreach (var evaluatedId in evaluation.SelectedEvaluatedIds)
                {
                    var evaluated = new EvaluatedEvaluation
                    {
                        EvaluationId = evaluation.Record.Id,
                        EvaluatedId = evaluatedId
                    };
                    evaluation.Record.EvaluatedEvaluations.Add(evaluated);
                }

                var result = _evaluationService.Create(evaluation.Record);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            SetViewData();
            return View(evaluation);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var evaluation = _evaluationService.Query().SingleOrDefault(q => q.Record.Id == id);

            if (evaluation == null)
            {
                return NotFound();
            }

            // Evaluated bilgilerini SetViewData'ya gönder
            SetViewData(currentEvaluatedId: evaluation.Record.EvaluatedEvaluations.FirstOrDefault()?.EvaluatedId);

            return View(evaluation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(EvaluationModel evaluation)
        {
            if (ModelState.IsValid)
            {
                // Update item service logic:
                var result = _evaluationService.Update(evaluation.Record);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = evaluation.Record.Id });
                }
                ModelState.AddModelError("", result.Message);
            }
            SetViewData();
            return View(evaluation);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var evaluation = _evaluationService.Query().SingleOrDefault(e => e.Record.Id == id);
            if (evaluation == null)
            {
                return NotFound();
            }
            return View(evaluation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _evaluationService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
