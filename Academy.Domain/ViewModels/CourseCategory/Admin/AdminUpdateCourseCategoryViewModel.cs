using System.ComponentModel.DataAnnotations;
using Academy.Domain.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.Domain.ViewModels.CourseCategory.Admin;

public class AdminUpdateCourseCategoryViewModel
{
    public short Id { get; set; }
    
    
    [Display(Name = "عنوان")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(150, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string Title { get; set; }
    
    
    [Display(Name = "اسلاگ")]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    [MaxLength(150, ErrorMessage = ErrorMessages.MaxLengthError)]
    public string Slug { get; set; }
    
    
    [Display(Name = "دسته‌بندی")]
    public short? ParentId { get; set; }
    
    
    [Display(Name = "ترتیب نمایش")]
    [Range(1, 100, ErrorMessage = ErrorMessages.RangeError)]
    [Required(ErrorMessage = ErrorMessages.RequiredError)]
    public short? Priority { get; set; }
}