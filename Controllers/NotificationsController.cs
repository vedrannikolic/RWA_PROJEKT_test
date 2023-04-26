using Integration_modul.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using Integration_modul.Models;

namespace Integrationmodule.Controlers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly RwaMoviesContext _dbContext;
    
        public NotificationsController(RwaMoviesContext dbContext)
        {
            _dbContext = dbContext;
           
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<NotificationResponse>> GetAll()
        {
            try
            {
                var allNotifications =
                    _dbContext.Notifications.Select(dbNotification =>
                    new NotificationResponse
                    {
                        Id = dbNotification.Id,
                        CreatedAt = dbNotification.CreatedAt,
                        ReceiverEmail = dbNotification.ReceiverEmail,
                        Subject = dbNotification.Subject,
                        Body = dbNotification.Body
                    });
                return Ok(allNotifications);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<NotificationResponse> Get(int id)
        {
            try
            {
                var dbNotification = _dbContext.Notifications.FirstOrDefault(x => x.Id == id);
                if (dbNotification == null)
                    return NotFound();

                return Ok(new NotificationResponse
                {
                    Id = dbNotification.Id,
                    CreatedAt = dbNotification.CreatedAt,
                    ReceiverEmail = dbNotification.ReceiverEmail,
                    Subject = dbNotification.Subject,
                    Body = dbNotification.Body
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost()]
        public ActionResult<NotificationResponse> Create(NotificationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbNotification = new Notification
                {
                    CreatedAt = DateTime.UtcNow,
                    ReceiverEmail = request.ReceiverEmail,
                    Subject = request.Subject,
                    Body = request.Body
                };

                _dbContext.Notifications.Add(dbNotification);

                _dbContext.SaveChanges();

                return Ok(new NotificationResponse
                {
                    Id = dbNotification.Id,
                    CreatedAt = dbNotification.CreatedAt,
                    ReceiverEmail = dbNotification.ReceiverEmail,
                    Subject = dbNotification.Subject,
                    Body = dbNotification.Body
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<NotificationResponse> Modify(int id, [FromBody] NotificationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbNotification = _dbContext.Notifications.FirstOrDefault(x => x.Id == id);
                if (dbNotification == null)
                    return NotFound();

                dbNotification.ReceiverEmail = request.ReceiverEmail;
                dbNotification.Subject = request.Subject;
                dbNotification.Body = request.Body;
                

                _dbContext.SaveChanges();

                return Ok(new NotificationResponse
                {
                    Id = dbNotification.Id,
                    ReceiverEmail = dbNotification.ReceiverEmail,
                    Subject = dbNotification.Subject,
                    Body = dbNotification.Body
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<NotificationResponse> Remove(int id)
        {
            try
            {
                var dbNotification = _dbContext.Notifications.FirstOrDefault(x => x.Id == id);
                if (dbNotification == null)
                    return NotFound();

                _dbContext.Notifications.Remove(dbNotification);

                _dbContext.SaveChanges();

                return Ok(new NotificationResponse
                {
                    Id = dbNotification.Id,
                    ReceiverEmail = dbNotification.ReceiverEmail,
                    Subject = dbNotification.Subject,
                    Body = dbNotification.Body
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("unsentcount")]
        public ActionResult<int> GetUnsentCount()
        {
            try
            {
                int unsentCount = _dbContext.Notifications.Count(x => x.SentAt == null);
                return Ok(unsentCount);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while fetching the data you requested");
            }
        }

        [HttpPost("[action]")]
        public ActionResult Send()
        {
            var client = new SmtpClient("127.0.0.1", 25);
            var sender = "admin@my-cool-webapi.com";
            try
            {
                var unsentNotifications = _dbContext.Notifications.Where(x => x.SentAt == null).ToList();

                foreach (var notification in unsentNotifications)
                {
                    try
                    {
                        var mail = new MailMessage(
                            from: new MailAddress(sender),
                            to: new MailAddress(notification.ReceiverEmail));

                        mail.Subject = notification.Subject;
                        mail.Body = notification.Body;

                        client.Send(mail);

                        notification.SentAt = DateTime.UtcNow;
                       
                        _dbContext.SaveChanges();

                    }
                    catch (Exception)
                    {
                        // Black hole for notification is bad handling :(
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while sending notifications");
            }
        }

    }
}
