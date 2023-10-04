using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class CategoriesController : Controller
    {
      private readonly ToDoListContext _db;

      public CategoriesController(ToDoListContext db)
      {
        _db = db;
      }

      public ActionResult Index()
      {
        List<Category> model = _db.Categories.ToList();
        return View(model);
      }

      public ActionResult Create()
      {
        return View();
      }

      [HttpPost]
      public ActionResult Create(Category category)
      {
        _db.Categories.Add(category);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }

      public ActionResult Details(int id)
      {
        Category model = _db.Categories
                            .Include(category => category.Items)
                            .FirstOrDefault(category => category.CategoryId == id);
        return View(model);
      }

      public ActionResult Edit(int id)
      {
        Category model = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
        return View(model);
      }

      [HttpPost]
      public ActionResult Edit(Category category)
      {
        _db.Categories.Update(category);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }

      public ActionResult Delete(int id)
      {
        Category category = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
        _db.Categories.Remove(category);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
}