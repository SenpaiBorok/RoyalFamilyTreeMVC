using Microsoft.AspNetCore.Mvc;
using RoyalFamily.Models;
using RoyalFamily.Services;

namespace RoyalFamily.Controllers
{
    public class HomeController : Controller
    {
        private readonly FamilyTreeService _tree;
        public HomeController(FamilyTreeService tree) { _tree = tree; }

        public IActionResult Index(string? q)
        {
            ViewBag.Query = q;
            ViewBag.Match = string.IsNullOrWhiteSpace(q) ? null : _tree.Find(q);
            ViewBag.Succession = _tree.GetSuccessionOrder();
            return View(_tree.Root);
        }

        [HttpPost]
        public IActionResult AddChild(Guid parentId, string name, DateOnly dob, bool isAlive)
        {
            if (string.IsNullOrWhiteSpace(name))
                ModelState.AddModelError("name", "Name required");

            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            _tree.AddChild(parentId, name.Trim(), dob, isAlive);
            return RedirectToAction(nameof(Index), new { q = name });
        }

        public IActionResult Bfs() =>
            View("Traversal", _tree.BreadthFirstOrder(true).Select(n => n.Member));

        public IActionResult Dfs() =>
            View("Traversal", _tree.DepthFirstOrder(true).Select(n => n.Member));
    }
}
