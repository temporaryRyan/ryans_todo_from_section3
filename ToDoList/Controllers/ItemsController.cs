using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoList.Controllers
{
  public class ItemsController : Controller
  {
    private readonly ToDoListContext _db;

    public ItemsController(ToDoListContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Item> model = _db.Items
                          .Include(item => item.Category)
                          .ToList();
      return View(model);
    }

    public ActionResult Details(int id)
    {
      Item item = _db.Items
                          .Include(item => item.Category)
                          .FirstOrDefault(item => item.ItemId == id);
      return View(item);
    }

    public ActionResult Create()
    {
      SelectList selectList = new SelectList(_db.Categories, "CategoryId", "Name");
      ViewBag.CategoryId = selectList;
      ViewBag.Thing = "hi";
      return View();
    }

    [HttpPost]
    public ActionResult Create(Item item)
    {
      if (item.CategoryId == 0)
      {
        return RedirectToAction("Create");
      }
      _db.Items.Add(item);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Edit(int id)
    {
      Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      SelectList selectList = new SelectList(_db.Categories, "CategoryId", "Name");
      ViewBag.CategoryId = selectList;
      return View(thisItem);
    }

    [HttpPost]
    public ActionResult Edit(Item item)
    {
      _db.Items.Update(item);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
      return View(thisItem);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
       Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
       _db.Items.Remove(thisItem);
       _db.SaveChanges();
       return RedirectToAction("Index");
    }
  }
}