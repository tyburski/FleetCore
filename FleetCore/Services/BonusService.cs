using FleetCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FleetCore.Services
{
    public interface IBonusService
    {
        Task<bool> Create(CreateBonusModel model);
        IEnumerable<Bonus> GetAll(string fullname);
    }
    public class BonusService : IBonusService
    {
        private readonly FleetCoreDbContext _dbContext;

        public BonusService(FleetCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> Create(CreateBonusModel model)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Id.ToString().Equals(model.userId));

            if(user is not null)
            {
                var bonus = new Bonus()
                {
                    Content = model.Content,
                    User = user,
                    CreatedAt = DateTime.Now
                };
                _dbContext.Bonuses.Add(bonus);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
            
            
        }

        public IEnumerable<Bonus> GetAll(string fullname)
        {
            var oldDate = DateTime.Now.AddMonths(-2);
            var dates = _dbContext.Bonuses.Where(x => x.CreatedAt.Month <= oldDate.Month);
            _dbContext.Bonuses.RemoveRange(dates);
            _dbContext.SaveChanges();

            return _dbContext.Bonuses.Where(x => x.User.FullName.Equals(fullname)).ToList();
        }
    }
}
