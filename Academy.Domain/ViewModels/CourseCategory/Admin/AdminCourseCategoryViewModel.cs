using System.ComponentModel.DataAnnotations;

namespace Academy.Domain.ViewModels.CourseCategory.Admin;

public class AdminCourseCategoryViewModel
{
    public short Id { get; set; }
    
    
    [Display(Name = "عنوان")]
    public string Title { get; set; }
    
    public short? ParentId { get; set; }
    
    [Display(Name = "دسته‌بندی والد")]
    public string? ParentTitle { get; set; }
    
    
    [Display(Name = "ترتیب نمایش")]
    public short Priority { get; set; }
    
    public bool IsDeleted { get; set; }
    
    
    public DateTime CreatedDate { get; set; }   
}