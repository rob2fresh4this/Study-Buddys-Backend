using Study_Buddys_Backend.Context;

namespace Study_Buddys_Backend.Services
{
    public class CommunityServices
    {
        private readonly DataContext _dataContext;

        public CommunityServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        
    }
}