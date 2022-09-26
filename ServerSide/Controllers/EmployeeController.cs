using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using ServerSide.Models;
using System.Text;

namespace ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));

            var dblist = dbClient.GetDatabase("myproject").GetCollection<Employee>("Employee").AsQueryable();

            return new JsonResult(dblist);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            int lastEmployeeId = dbClient.GetDatabase("myproject").GetCollection<Employee>("Employee").AsQueryable().Count();
            emp.EmployeeId = lastEmployeeId + 1;
            dbClient.GetDatabase("myproject").GetCollection<Employee>("Department").InsertOne(emp);
            return new JsonResult("Added Seccessfully");
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            var filter = Builders<Employee>.Filter.Eq("EmployeeId", emp.EmployeeId);
            var update = Builders<Employee>.Update.Set("EmployeeName", emp.EmployeeName)
                                                                       .Set("Department", emp.Department)
                                                                       .Set("DateOfJoining", emp.DateOfJoining)
                                                                       .Set("PhotoFileName", emp.PhotoFileName)
                                                                      
                ;

            dbClient.GetDatabase("myproject").GetCollection<Employee>("Employee").InsertOne(emp);
            return new JsonResult("Update Seccessfully");
        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("EmployeeAppCon"));
            var filter = Builders<Employee>.Filter.Eq("EmployeeId", id);
            dbClient.GetDatabase("myproject").GetCollection<Employee>("Employee").DeleteOne(filter);
            return new JsonResult("Delete Seccessfully");
        }
    }
}

