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
using Newtonsoft.Json;

namespace SmartFaceAuthAPI.Controllers
{
    [RoutePrefix("api/group")]
    public class GroupController : ApiController
    {
        ApiContext dbContext;

        public GroupController()
        {

        }

        public GroupController(DbContext context)
        {
            dbContext = (ApiContext)context;
        }

        [HttpGet]
        [Route("byid/{id:int}")]
        public JsonResult<Group> ById(int id)
        {
            Group group = dbContext.Groups.Include(x => x.PersonsList).SingleOrDefault(gr => gr.Id.Equals(id));
            return Json(group);
        }

        [HttpPost]
        [Route("byemail")]
        public JsonResult<Group> ByEmail([FromBody]string email)
        {
            Group group = dbContext.Groups.Include(x => x.PersonsList).SingleOrDefault(gr => gr.Email.Equals(email));
            if (group != null)
            {
                List<Person> validPersonsList = new List<Person>();
                foreach (int value in group.PersonsList.Select(x => x.Id))
                {
                    validPersonsList.Add(dbContext.Persons.Include(x => x.PersonImage).SingleOrDefault(pers => pers.Id.Equals(value)));
                }
                group.PersonsList = validPersonsList;
            }
            return Json(group);
        }

        [HttpGet]
        [Route("person/{id:int}")]
        public JsonResult<Person> PersonById(int id)
        {
            Person person = dbContext.Persons.Include(x => x.PersonImage).SingleOrDefault(pers => pers.Id.Equals(id));
            return Json(person);
        }

        [HttpGet]
        [Route("person/{id:Guid}")]
        public JsonResult<Person> PersonById(Guid id)
        {
            Person person = dbContext.Persons.Include(x => x.PersonImage).SingleOrDefault(pers => pers.PersonId.Equals(id));
            return Json(person);
        }

        [HttpPost]
        [Route("addgroup")]
        public JsonResult<Group> AddGroup([FromBody]Group group)
        {
            Group result = dbContext.Groups.Add(group);
            group.IsTrained = false;
            dbContext.SaveChanges();
            return Json(result);
        }

        [HttpPost]
        [Route("addperson/{groupGuid:Guid}")]
        public JsonResult<Group> AddPerson(Guid groupGuid, [FromBody]Person person)
        {
            Group group = dbContext.Groups.Include(x => x.PersonsList).SingleOrDefault(gr => gr.GroupId.Equals(groupGuid));
            group.PersonsList.Add(person);
            group.IsTrained = false;
            dbContext.SaveChanges();
            return Json(group);
        }

        [HttpPost]
        [Route("rmperson/{groupGuid:Guid}")]
        public JsonResult<bool> RemovePersonPost([FromUri]Guid groupGuid, [FromBody]Guid personGuid)
        {
            Group group = dbContext.Groups.Include(x => x.PersonsList).SingleOrDefault(gr => gr.GroupId.Equals(groupGuid));
            group.IsTrained = false;
            if (group != null && group.PersonsList != null)
            {
                Person rmPerson = group.PersonsList.SingleOrDefault(pers => pers.PersonId.Equals(personGuid));
                if (rmPerson != null)
                {
                    bool resultRemove = group.PersonsList.Remove(rmPerson);
                    dbContext.SaveChanges();
                    return Json(resultRemove);
                }
            }
            return Json(false);
        }

        [HttpGet]
        [Route("istrained/{groupGuid:Guid}")]
        public JsonResult<bool> IsTrained(Guid groupGuid)
        {
            Group group = dbContext.Groups.SingleOrDefault(x => x.GroupId.Equals(groupGuid));
            if (group != null)
            {
                return Json(group.IsTrained);
            }
            else
            {
                return Json(false);
            }
        }

        [HttpGet]
        [Route("setnontrained/{groupGuid:Guid}")]
        public JsonResult<Group> SetNonTrained(Guid groupGuid)
        {
            Group group = dbContext.Groups.SingleOrDefault(x => x.GroupId.Equals(groupGuid));
            group.IsTrained = false;
            dbContext.SaveChanges();
            return Json(group);
        }

        [HttpGet]
        [Route("settrained/{groupGuid:Guid}")]
        public JsonResult<Group> SetTrained(Guid groupGuid)
        {
            Group group = dbContext.Groups.SingleOrDefault(x => x.GroupId.Equals(groupGuid));
            group.IsTrained = true;
            dbContext.SaveChanges();
            return Json(group);
        }
    }
}
