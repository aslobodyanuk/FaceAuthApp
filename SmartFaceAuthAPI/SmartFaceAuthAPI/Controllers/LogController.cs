using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Newtonsoft.Json.Serialization;
using System.Data.Entity;
using SmartFaceAuthAPI.Models;
using SmartFaceAuthAPI.Context;

namespace SmartFaceAuthAPI.Controllers
{
    [RoutePrefix("api/log")]
    public class LogController : ApiController
    {
        ApiContext dbContext;

        public LogController()
        {

        }

        public LogController(DbContext context)
        {
            dbContext = (ApiContext)context;
        }

        [HttpGet]
        [Route("byid/{id:int:min(1)}")]
        public JsonResult<Log> ById(int id)
        {
            Log result = dbContext.Logs.Include(x => x.AuthImage).SingleOrDefault(log => log.Id == id);
            return Json(result);
        }

        [HttpGet]
        [Route("byguid/{id:Guid}")]
        public JsonResult<Log[]> ByGuid(Guid id)
        {
            Log[] result = dbContext.Logs.Include(x => x.AuthImage).Where(log => log.UserGuid.Equals(id)).ToArray();
            return Json(result);
        }

        [HttpPost]
        [Route("addlog")]
        public JsonResult<Log> AddLog([FromBody]Log log)
        {
            Log result = dbContext.Logs.Add(log);
            dbContext.SaveChanges();
            return Json(result);
        }

        [HttpGet]
        [Route("listall")]
        public JsonResult<Log[]> List()
        {
            Log[] result = dbContext.Logs.ToList().ToArray();
            return Json(result);
        }

        [HttpGet]
        [Route("list/{groupGuid:Guid}")]
        public JsonResult<Log[]> List(Guid groupGuid)
        {
            Log[] result = dbContext.Logs.Include(x => x.AuthImage).Where(g => g.UserGuid.Equals(groupGuid)).ToList().ToArray();
            return Json(result);
        }

        //[HttpGet]
        //[Route("GetJson")]
        //public JsonResult<Log> GetJson()
        //{
        //    Log test = new Log();
        //    test.Id = 5;
        //    test.Message = "wdwdwadawd";
        //    return Json(test);
        //}
    }
}