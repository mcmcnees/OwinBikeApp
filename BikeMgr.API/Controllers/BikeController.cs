using BikeMgr.API.DTO;
using BikeMgr.API.Helpers;
using BikeMgr.Core;
using BikeMgr.Core.Interface.Services;
using BikeMgr.Core.Interface.Utilities;
using BikeMgr.Core.Models;
using BikeMgrAPI.Filters;
using BikeMgrAPI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace BikeMgr.API.Controllers
{
    [Authorize]
    public class BikeController : ApiController
    {
        private readonly IBikeService _bikeService;

        public BikeController(IBikeService bikeService)
        {
            _bikeService = bikeService ?? throw new ArgumentNullException(nameof(IBikeService));
        }

        // GET: api/Bike
        [HttpGet]
        [ResponseType(typeof(Page<Bike>))]
        public async Task<IHttpActionResult> Get(string sortOrder, string search, int? page = 1, int? pageSize = 10)
        {
            int pageSize2 = pageSize ?? 10;
            if (pageSize2 > 40) { pageSize2 = 40; }
            int pageNo = (page ?? 1);
            Page<Bike> bikeList = await _bikeService.GetBikes(sortOrder, search, new PageParams() { PageNo = pageNo, PageSize = pageSize2 });
            return base.Ok(bikeList);
        }

        // GET: api/Bike/5
        [HttpGet]
        [ResponseType(typeof(Bike))]
        public IHttpActionResult Get(int id)
        {
            Bike bike = _bikeService.GetBikeByID(id);
            if (!String.IsNullOrWhiteSpace(bike.ImageLocation))
                bike.ImageLocation = @"/Images/Bikes/" + Path.GetFileName(bike.ImageLocation);
            else
                bike.ImageLocation = "";
            return Ok(bike);
        }

        // POST: api/Bike
        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            var bikeDTO = ModelBinder.Bind<BikeDTO>();
            if(!bikeDTO.TryValidate(out var errors))
            {
                var errorMessage = string.Join(", ", errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var bike = Mapper.Map(bikeDTO);
            await _bikeService.CreateBike(bike);
            return Ok(bikeDTO);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put()
        {
            var bikeDTO = ModelBinder.Bind<BikeDTO>();
            if (!bikeDTO.TryValidate(out var errors))
            {
                var errorMessage = string.Join(", ", errors.Select(e => e.ErrorMessage));
                return BadRequest(errorMessage);
            }
            var bike = Mapper.Map(bikeDTO);
            await _bikeService.UpdateBike(bike.ID, bike);
            return Ok(bikeDTO);
        }

        // DELETE: api/Bike/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            _bikeService.DeleteBike(id);
            return Ok();
        }

        // GET: api/BikeType
        [Route("api/BikeType")]
        [HttpGet]
        [ResponseType(typeof(BikeType[]))]
        public IHttpActionResult GetBikeTypes()
        {
            return Ok(_bikeService.GetBikeTypes());
        }
    }
}
