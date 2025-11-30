namespace Estra7a.Web.ViewModels
{
    public class AssignRoleVM
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleCheckVM> Roles { get; set; }
    }
}
