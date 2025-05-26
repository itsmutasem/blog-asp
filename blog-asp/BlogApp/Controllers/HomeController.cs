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
            return View(TempData.Posts);
        }

        [Authorize]
        public IActionResult Create() => View();

        [Authorize, HttpPost]
        public IActionResult Create(Post post)
        {
            post.Id = TempData.Posts.Count + 1;
            post.Author = User.Identity.Name;
            TempData.Posts.Add(post);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var post = TempData.Posts.FirstOrDefault(p => p.Id == id);
            return post == null ? NotFound() : View(post);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var post = TempData.Posts.FirstOrDefault(p => p.Id == id);
            return post == null ? NotFound() : View(post);
        }

        [Authorize, HttpPost]
        public IActionResult Edit(Post updated)
        {
            var post = TempData.Posts.FirstOrDefault(p => p.Id == updated.Id);
            if (post == null) return NotFound();

            post.Title = updated.Title;
            post.Description = updated.Description;
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var post = TempData.Posts.FirstOrDefault(p => p.Id == id);
            return post == null ? NotFound() : View(post);
        }

        [Authorize, HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var post = TempData.Posts.FirstOrDefault(p => p.Id == id);
            if (post != null) TempData.Posts.Remove(post);
            return RedirectToAction("Index");
        }
    }
}