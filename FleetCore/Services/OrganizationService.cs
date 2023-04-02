using FleetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FleetCore.Services
{
    public interface IOrganizationService
    {
        Task<bool>Update(UpdateOrganizationModel model);
        Task<bool> CreateNotice(CreateNoticeModel model);
        Organization Get();
        IEnumerable<Notice> GetNotices();
    }
    public class OrganizationService : IOrganizationService
    {
        private readonly FleetCoreDbContext _dbContext;

        public OrganizationService(FleetCoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Update(UpdateOrganizationModel model)
        {
            var organization = _dbContext
                .Organizations
                .FirstOrDefault();
            if(organization is not null)
            {
                organization.Name = model.Name;
                organization.Address1 = model.Address1;
                organization.Address2 = model.Address2;
                organization.NIP = model.NIP;

               _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        public Organization Get()
        {
            return _dbContext.Organizations.First();
        }

        public IEnumerable<Notice> GetNotices()
        {
            var notices = _dbContext.Notices.OrderByDescending(x => x.CreatedAt).ToList();
            
            for(int i = 10; i < notices.Count; i++)
            {
                _dbContext.Notices.Remove(notices[i]);
            }
            _dbContext.SaveChanges();

            return _dbContext.Notices.Include(x => x.User).ToList();

        }
        public async Task<bool> CreateNotice(CreateNoticeModel model)
        {
            var user = _dbContext.Users.FirstOrDefault(x=>x.Id.ToString().Equals(model.userId));
            if (user is null) return false;
            else
            {
                var n = new Notice()
                {
                    Content=model.Content,
                    CreatedAt=DateTime.Now,
                    User= user
                };
                _dbContext.Notices.Add(n);
                _dbContext.SaveChanges();
                return true;
            }
        }
    }

}
