using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ServerSide.Models;

namespace ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var dblist = dbClient.GetDatabase("myproject").GetCollection<Department>("Department").AsQueryable();

            return new JsonResult(dblist);
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Post(Department dep)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            int lastDepartmentId = dbClient.GetDatabase("myproject").GetCollection<Department>("Department").AsQueryable().Count();
            dep.DepartmentId = lastDepartmentId + 1;
            dbClient.GetDatabase("myproject").GetCollection<Department>("Department").InsertOne(dep);
            return new JsonResult("Added Seccessfully");
        }
        [HttpPut]
        public JsonResult Put(Department dep)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            var filter = Builders<Department>.Filter.Eq("DepartmentId", dep.DepartmentId);
            var update = Builders<Department>.Update.Set("DepartmentName", dep.DepartmentName);
            
            dbClient.GetDatabase("myproject").GetCollection<Department>("Department").InsertOne(dep);
            return new JsonResult("Update Seccessfully");
        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            var filter = Builders<Department>.Filter.Eq("DepartmentId", id);
            dbClient.GetDatabase("myproject").GetCollection<Department>("Department").DeleteOne(filter);
            return new JsonResult("Delete Seccessfully");
        }
    }
}
