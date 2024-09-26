namespace eventsapi.Models
{
    public class RolesModel
    {
        private string _roleName;
        
        public static RolesModel User = new RolesModel("USER");
        public static RolesModel Admin = new RolesModel("ADMIN");

        public RolesModel(string roleName) 
        {
            this._roleName = roleName;
        }

        public override string ToString() 
        {
            return _roleName;
        }
    }
}
