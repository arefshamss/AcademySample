using Academy.Domain.Models.Common;

namespace Academy.Domain.Models.Category;

public class CourseCategory : BaseEntity<short>
{
    #region Properties

    public string Title { get; set; }
    
    public string Slug { get; set; }
    
    public short? ParentId { get; set; }
    
    public short Priority { get; set; }

    #endregion

    #region Relations

    public CourseCategory? Parent { get; set; }
    
    public List<CourseCategory> Children { get; set; }

    #endregion
}