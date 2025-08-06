using Microsoft.AspNetCore.Http;

namespace Academy.Domain.ViewModels.User.Client;

public class ClientUpdateUserAvatarViewModel
{
    public int UserId { get; set; }
    
    public IFormFile Avatar { get; set; }
    
    public string AvatarImageName { get; set; }     
}