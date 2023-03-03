using FleetCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FleetCore.Services
{
    public interface IBonusService
    {
        int Create(CreateBonusModel model);
        IEnumerable<Bonus> Get(SearchBonusModel model);
    }
    public class BonusService : IBonusService
    {
        private readonly FleetCoreDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BonusService(FleetCoreDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public int Create(CreateBonusModel model)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users.Include(x => x.Organization).FirstOrDefault(x => x.Id == userId);

            var bonus = new Bonus()
            {
                Content = model.Content,
                User = user,
                CreatedAt= DateTime.Now
            };
            _dbContext.Bonuses.Add(bonus);
            _dbContext.SaveChanges();
            return bonus.Id;
        }
        public IEnumerable<Bonus> Get(SearchBonusModel model)
        {
            var bonuses = _dbContext
                .Bonuses
                .Include(x => x.User)
                .Where(x => x.User.FullName == model.FullName && x.CreatedAt.Month == model.Month && x.CreatedAt.Year == model.Year)
                .ToList();

            return bonuses;
        }
    }
}
