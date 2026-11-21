using Lab2.Models;
using Lab2.Repositories;
using Lab2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab2.Controllers;

public class ProductController : Controller
{
    private const long MaxImageBytes = 5 * 1024 * 1024;
    private readonly IProductRepository _products;
    private readonly ICategoryRepository _categories;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductRepository products, ICategoryRepository categories,
        IWebHostEnvironment environment, ILogger<ProductController> logger)
    {
        _products = products;
        _categories = categories;
        _environment = environment;
        _logger = logger;
    }

    public IActionResult Index(string? search, int? categoryId)
    {
        var items = _products.GetAll();
        if (!string.IsNullOrWhiteSpace(search))
            items = items.Where(p => p.Name.Contains(search.Trim(), StringComparison.OrdinalIgnoreCase));
        if (categoryId.HasValue && categoryId.Value > 0)
            items = items.Where(p => p.CategoryId == categoryId.Value);
        ViewBag.Search = search;
        ViewBag.CategoryId = categoryId;
        ViewBag.Categories = _categories.GetAllCategories().ToList();
        return View(items.ToList());
    }

    public IActionResult Display(int id)
    {
        var product = _products.GetById(id);
        return product is null ? NotFound() : View(product);
    }

    [HttpGet]
    public IActionResult Add()
    {
        LoadCategories();
        return View(new ProductFormViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    [RequestSizeLimit(40 * 1024 * 1024)]
    public async Task<IActionResult> Add(ProductFormViewModel form)
    {
        ValidateForm(form);
        if (!ModelState.IsValid) { LoadCategories(); return View(form); }
        var created = new List<string>();
        try
        {
            var image = form.Image is null ? null : await SaveImage(form.Image, created);
            var additional = await SaveImages(form.AdditionalImages, created);
            _products.Add(new Product
            {
                Name = form.Name.Trim(), Price = form.Price, Description = form.Description?.Trim() ?? "",
                CategoryId = form.CategoryId, ImageUrl = image, ImageUrls = additional
            });
            TempData["Success"] = "Đã thêm sản phẩm.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (ex is IOException or InvalidImageException)
        {
            _logger.LogError(ex, "Không thể lưu hình ảnh sản phẩm");
            RemoveImages(created);
            ModelState.AddModelError("", "Không thể lưu ảnh. Vui lòng thử lại.");
        }
        catch
        {
            RemoveImages(created);
            throw;
        }
        LoadCategories();
        return View(form);
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var p = _products.GetById(id);
        if (p is null) return NotFound();
        LoadCategories();
        return View(new ProductFormViewModel
        {
            Id = p.Id, Name = p.Name, Price = p.Price, Description = p.Description,
            CategoryId = p.CategoryId, CurrentImageUrl = p.ImageUrl, CurrentImageUrls = p.ImageUrls
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    [RequestSizeLimit(40 * 1024 * 1024)]
    public async Task<IActionResult> Update(int id, ProductFormViewModel form)
    {
        if (id != form.Id) return BadRequest();
        var existing = _products.GetById(id);
        if (existing is null) return NotFound();
        ValidateForm(form, existing.ImageUrls.Count);
        if (!ModelState.IsValid) return UpdateWithErrors(form, existing);
        var created = new List<string>();
        try
        {
            string? replacement = form.Image is null ? null : await SaveImage(form.Image, created);
            var additional = await SaveImages(form.AdditionalImages, created);
            var updated = new Product
            {
                Id = id, Name = form.Name.Trim(), Price = form.Price,
                Description = form.Description?.Trim() ?? "", CategoryId = form.CategoryId,
                ImageUrl = replacement ?? existing.ImageUrl,
                ImageUrls = existing.ImageUrls.Concat(additional).ToList()
            };
            if (!_products.Update(updated))
            {
                RemoveImages(created);
                return NotFound();
            }
            if (replacement is not null && existing.ImageUrl is not null)
                RemoveImages(new[] { existing.ImageUrl });
            TempData["Success"] = "Đã cập nhật sản phẩm.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (ex is IOException or InvalidImageException)
        {
            _logger.LogError(ex, "Không thể lưu hình ảnh sản phẩm");
            RemoveImages(created);
            ModelState.AddModelError("", "Không thể lưu ảnh. Vui lòng thử lại.");
        }
        catch
        {
            RemoveImages(created);
            throw;
        }
        return UpdateWithErrors(form, existing);
    }

    public IActionResult Delete(int id)
    {
        var product = _products.GetById(id);
        return product is null ? NotFound() : View(product);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var p = _products.GetById(id);
        if (p is null) return NotFound();
        if (!_products.Delete(id)) return NotFound();
        RemoveImages(new[] { p.ImageUrl }.Concat(p.ImageUrls));
        TempData["Success"] = "Đã xóa sản phẩm.";
        return RedirectToAction(nameof(Index));
    }

    private IActionResult UpdateWithErrors(ProductFormViewModel form, Product existing)
    {
        form.CurrentImageUrl = existing.ImageUrl;
        form.CurrentImageUrls = existing.ImageUrls;
        LoadCategories();
        return View("Update", form);
    }

    private void LoadCategories() =>
        ViewBag.Categories = new SelectList(_categories.GetAllCategories(), "Id", "Name");

    private void ValidateForm(ProductFormViewModel form, int existingImages = 0)
    {
        if (_categories.GetById(form.CategoryId) is null)
            ModelState.AddModelError(nameof(form.CategoryId), "Danh mục không hợp lệ.");
        var uploads = (form.AdditionalImages ?? new List<IFormFile>()).ToList();
        if (existingImages + uploads.Count > 6)
            ModelState.AddModelError(nameof(form.AdditionalImages), "Mỗi sản phẩm có tối đa 6 ảnh bổ sung.");
        foreach (var file in uploads.Concat(form.Image is null ? Enumerable.Empty<IFormFile>() : new[] { form.Image }))
        {
            if (file.Length == 0 || file.Length > MaxImageBytes)
                ModelState.AddModelError("", $"Ảnh {file.FileName} phải có dung lượng từ 1 B đến 5 MB.");
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var mime = file.ContentType.ToLowerInvariant();
            if ((ext, mime) is not ((".jpg", "image/jpeg") or (".jpeg", "image/jpeg")
                or (".png", "image/png") or (".webp", "image/webp")))
                ModelState.AddModelError("", "Chỉ nhận ảnh JPG, PNG hoặc WebP.");
        }
    }

    private async Task<List<string>> SaveImages(IEnumerable<IFormFile>? files, List<string> created)
    {
        var urls = new List<string>();
        foreach (var file in files ?? Enumerable.Empty<IFormFile>())
            urls.Add(await SaveImage(file, created));
        return urls;
    }

    private async Task<string> SaveImage(IFormFile file, List<string> created)
    {
        // Kiểm tra cả chữ ký dữ liệu, không dựa riêng vào tên tệp hay MIME do trình duyệt gửi.
        byte[] header = new byte[12];
        await using (var stream = file.OpenReadStream())
        {
            int offset = 0;
            while (offset < header.Length)
            {
                int read = await stream.ReadAsync(header.AsMemory(offset, header.Length - offset));
                if (read == 0) break;
                offset += read;
            }
        }
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        bool valid = ext switch
        {
            ".jpg" or ".jpeg" => header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF,
            ".png" => header.Take(8).SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }),
            ".webp" => System.Text.Encoding.ASCII.GetString(header, 0, 4) == "RIFF"
                        && System.Text.Encoding.ASCII.GetString(header, 8, 4) == "WEBP",
            _ => false
        };
        if (!valid) throw new InvalidImageException();
        string dir = Path.Combine(_environment.WebRootPath, "images", "uploads");
        Directory.CreateDirectory(dir);
        string filename = $"{Guid.NewGuid():N}{ext}";
        string path = Path.Combine(dir, filename);
        await using (var target = new FileStream(path, FileMode.CreateNew))
        {
            created.Add($"/images/uploads/{filename}");
            await file.CopyToAsync(target);
        }
        return $"/images/uploads/{filename}";
    }

    private void RemoveImages(IEnumerable<string?> urls)
    {
        foreach (var url in urls)
        {
            if (url is null || !url.StartsWith("/images/uploads/", StringComparison.Ordinal)) continue;
            var name = Path.GetFileName(url);
            if (name != url["/images/uploads/".Length..]) continue;
            try { System.IO.File.Delete(Path.Combine(_environment.WebRootPath, "images", "uploads", name)); }
            catch (IOException ex) { _logger.LogWarning(ex, "Không thể dọn ảnh {Name}", name); }
        }
    }

    private sealed class InvalidImageException : Exception { }
}
