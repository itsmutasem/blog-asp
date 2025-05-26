using Microsoft.AspNetCore.Mvc;
using BlogApp.Models;
using BlogApp.TempStore;
using Microsoft.AspNetCore.Authorization;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(TempStore.TempData.Posts);
        }

        [Authorize]
        public IActionResult Create() => View();

        [Authorize, HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post post)
        {
            if (!ModelState.IsValid)
                return View(post);

            post.Id = TempStore.TempData.Posts.Count > 0 ? TempStore.TempData.Posts.Max(p => p.Id) + 1 : 1;
            post.Author = User.Identity?.Name ?? "Anonymous";
            post.Date = DateTime.Now;
            TempStore.TempData.Posts.Add(post);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var post = TempStore.TempData.Posts.FirstOrDefault(p => p.Id == id);
            return post == null ? NotFound() : View(post);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var post = TempStore.TempData.Posts.FirstOrDefault(p => p.Id == id);
            return post == null ? NotFound() : View(post);
        }

        [Authorize, HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Post updated)
        {
            if (!ModelState.IsValid)
                return View(updated);

            var post = TempStore.TempData.Posts.FirstOrDefault(p => p.Id == updated.Id);
            if (post == null) return NotFound();

            post.Title = updated.Title;
            post.Description = updated.Description;
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var post = TempStore.TempData.Posts.FirstOrDefault(p => p.Id == id);
            return post == null ? NotFound() : View(post);
        }

        [Authorize, HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var post = TempStore.TempData.Posts.FirstOrDefault(p => p.Id == id);
            if (post != null)
            {
                TempStore.TempData.Posts.Remove(post);
            }
            return RedirectToAction("Index");
        }
    }
}