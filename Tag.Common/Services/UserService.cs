
using System.Linq;
using Tag.Common.Data;
using Tag.Common.Models;

namespace Tag.Common.Services
{
    public class UserService
    {
        private DataContext db { get; set; }
        public UserService(DataContext context)
        {
            db = context;
        }

        public IQueryable<User> Users_Get()
        {
            
            return db.Users;
        }

        public void Users_Add(User u)
        {
            db.Users.Add(u);
        }
    }
}