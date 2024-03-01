using my_books.Data.Models;

namespace my_books.Data.Services
{
    public class LogsService
    {
        //inject the appdb context

        private AppDbContext _context;

        public LogsService(AppDbContext context)
        {
              _context = context;  
        }

        //Getting logs form db
        public List<Log> GetAllLogsFromDb()
        {
            return _context.Logs.ToList();
        }
        
    }
}
