using TagConsumer.Data;

namespace Tag.Consumer.Services
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