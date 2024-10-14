using System.Configuration;
using DataLayer;
using Microsoft.Extensions.Configuration;

namespace ServiceLayer
{
    public class MyService
    {
        private readonly Context _dbContext;

        // Ändra MyService så den får Context via DI
        public MyService(Context dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
