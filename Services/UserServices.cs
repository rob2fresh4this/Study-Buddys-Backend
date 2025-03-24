
using Study_Buddys_Backend.Context;

namespace Study_Buddys_Backend.Services
{
    public class UserServices
    {
        private readonly DataContext _dataContext;

        public UserServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        
    }
}