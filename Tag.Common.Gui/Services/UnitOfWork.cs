namespace Tag.Common.Gui.Services
{
    public class UnitOfWork
    {
        private LoginService loginService { get; set; }
        public LoginService LoginRepository
        {
            get
            {
                if(loginService == null)
                    loginService  = new LoginService();

                return loginService;
            }
            set
            {
                loginService = value;
            }
        }
    }
}