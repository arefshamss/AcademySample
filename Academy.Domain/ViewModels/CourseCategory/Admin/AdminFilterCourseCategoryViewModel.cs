using System.ComponentModel.DataAnnotations;
using Academy.Domain.Attributes;
using Academy.Domain.Enums.Common;
using Academy.Domain.ViewModels.Common;
using Academy.Domain.Shared;

namespace Academy.Domain.ViewModels.CourseCategory.Admin;

public class AdminFilterCourseCategoryViewModel : BasePaging<AdminCourseCategoryViewModel>
{
    [Display(Name="عنوان"),FilterInput]
    public string? Title { get; set; }  
    
    
    [Display(Name = "وضعیت حذف"),FilterInput]
    public DeleteStatus DeleteStatus { get; set; } 
    
    
    public bool ParentOnly { get; set; }
}