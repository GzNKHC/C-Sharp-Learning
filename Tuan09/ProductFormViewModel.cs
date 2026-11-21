using System.ComponentModel.DataAnnotations;

namespace Lab2.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm.")]
    [StringLength(100, ErrorMessage = "Tên tối đa 100 ký tự.")]
    [Display(Name = "Tên sản phẩm")]
    public string Name { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01", "1000000000", ErrorMessage = "Giá phải lớn hơn 0 và không vượt quá 1 tỷ đồng.")]
    [Display(Name = "Giá (VNĐ)")]
    public decimal Price { get; set; }

    [StringLength(2000, ErrorMessage = "Mô tả tối đa 2000 ký tự.")]
    [Display(Name = "Mô tả")]
    public string Description { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục.")]
    [Display(Name = "Danh mục")]
    public int CategoryId { get; set; }

    public string? ImageUrl { get; set; }
    public List<string> ImageUrls { get; set; } = new();
}
