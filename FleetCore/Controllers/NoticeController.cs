using FleetCore.Models;
using FleetCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace FleetCore.Controllers
{
    [ApiController]
    [Route("api/notice")]
    public class NoticeController : ControllerBase
    {
        private readonly INoticeService _noticeService;

        public NoticeController(INoticeService noticeService)
        {
            _noticeService = noticeService;
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_noticeService.GetAll());
        }
        [HttpPost("create")]
        public ActionResult Create([FromBody]CreateNoticeModel model)
        {
            return Ok(_noticeService.Create(model));
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var result = _noticeService.Delete(id);
            if (result is false) return NoContent();
            else return Ok(result);
        }

    }
}
