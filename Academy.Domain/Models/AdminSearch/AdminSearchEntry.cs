using Academy.Domain.Models.Common;

namespace Academy.Domain.Models.AdminSearch;

public class AdminSearchEntry : MongoDBBaseEntity
{
    public string Title { get; set; }

    public string Url { get; set; }

    public string? PermissionName { get; set; }
    
}