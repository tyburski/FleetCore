using FleetCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FleetCore.Services
{
    public interface INoticeService
    {
        int Create(CreateNoticeModel model);
        IEnumerable<Notice> GetAll();
        bool Delete(int id);
    }
    public class NoticeService : INoticeService
    {
        private readonly FleetCoreDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NoticeService(FleetCoreDbContext dbContext, IHttpContextAccessor httpContextAccessor) 
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public int Create(CreateNoticeModel model)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _dbContext.Users.Include(x => x.Organization).FirstOrDefault(x => x.Id == userId);

            var notice = new Notice()
            {
                Content= model.Content,
                User = user,
                CreatedAt= DateTime.Now
            };
            _dbContext.Notices.Add(notice);
            _dbContext.SaveChanges();
            return notice.Id;
        }
        public IEnumerable<Notice> GetAll() 
        {
           var notices = _dbContext
                .Notices
                .Include(x=>x.User)
                .OrderByDescending(x=>x.CreatedAt)
                .ToList();
            return notices;
        }
        public bool Delete(int id)
        {
            var notice = _dbContext
                .Notices
                .FirstOrDefault(x=> x.Id == id);
            if (notice is null) return false;
            _dbContext.Notices.Remove(notice);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
