
using Tag.Common.Data;

namespace Tag.Common.Services
{
    public class UserService
    {
        private DataContext db { get; set; }
        public UserService(DataContext context)
        {
            db = context;
        }
    }
}