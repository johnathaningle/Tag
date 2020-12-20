

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tag.Common.Data;

namespace Tag.Common.Services
{
    public class UnitOfWork
    {
        public UnitOfWork()
        {
            db = new DataContext();
            db.Database.EnsureCreated();
        }
        public UnitOfWork(DataContext context)
        {
            db = context;
        }
        public int SaveChanges()
        {
            return db.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await db.SaveChangesAsync();
        }
        private DataContext db { get; set; }
        private DataService dataService { get; set; }
        public DataService DataRepository
        {
            get
            {
                if (this.dataService == null)
                    this.dataService = new DataService();

                return this.dataService;
            }
        }

        private UserService userService { get; set; }
        public UserService UserRepository
        {
            get
            {
                if (this.userService == null)
                    this.userService = new UserService(this.db);

                return this.userService;
            }
        }

        private CryptoService cryptoService { get; set; }
        public CryptoService CryptoRepository
        {
            get
            {
                if (cryptoService == null)
                    cryptoService = new CryptoService();

                return cryptoService;
            }
            set
            {
                this.CryptoRepository = value;
            }
        }
    }
}