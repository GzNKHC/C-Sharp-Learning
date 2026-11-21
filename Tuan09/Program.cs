using Lab2.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryRepository _categories;
    public CategoryController(ICategoryRepository categories) => _categories = categories;
    public IActionResult Index() => View(_categories.GetAllCategories());
}
