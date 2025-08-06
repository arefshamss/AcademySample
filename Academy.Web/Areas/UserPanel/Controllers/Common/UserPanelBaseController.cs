using Academy.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Web.Areas.UserPanel.Controllers.Common;

[Area("UserPanel")]
[Authorize]
public class UserPanelBaseController : BaseController;