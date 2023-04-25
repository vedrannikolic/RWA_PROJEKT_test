using Integration_modul.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

 

namespace Integrationmodule.Controlers

{


    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class VideosController : ControllerBase

    {



        private readonly RwaMoviesContext _context;



        public VideosController(RwaMoviesContext context)

        {

            _context = context;

        }



        [HttpGet("action")]


        public async Task<ActionResult<IEnumerable<Video>>> GetVideos(

        [FromQuery] string search, [FromQuery] string orderBy, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)

        {

            IQueryable<Video> videos = _context.Videos.Include(v => v.Genre).Include(v => v.VideoTags);



            if (!string.IsNullOrEmpty(search))

            {

                videos = videos.Where(v => v.Name.Contains(search));

            }



            if (!string.IsNullOrEmpty(orderBy))

            {

                switch (orderBy.ToLower())

                {

                    case "id":

                        videos = videos.OrderBy(v => v.Id);

                        break;

                    case "id_desc":

                        videos = videos.OrderByDescending(v => v.Id);

                        break;

                    case "name":

                        videos = videos.OrderBy(v => v.Name);

                        break;

                    case "name_desc":

                        videos = videos.OrderByDescending(v => v.Name);

                        break;

                    case "totaltime":

                        videos = videos.OrderBy(v => v.TotalSeconds);

                        break;

                    case "totaltime_desc":

                        videos = videos.OrderByDescending(v => v.TotalSeconds);

                        break;

                    default:

                        videos = videos.OrderBy(v => v.Id);

                        break;

                }

            }



            return await videos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        }

    }

}



