using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Web.Http;
using System.Web.Http.Results;
using System.Configuration;
using System.IO;

namespace WebAPI.Controllers
{

    public class Person
    {
        public string LastName
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string Sex
        {
            get;
            set;
        }

        public string FavoriteColor
        {
            get;
            set;
        }

        public DateTime BirthDate
        {
            get;
            set;
        }
    }

    //[Authorize]
    [RoutePrefix("records")]
    public class RecordsController : ApiController
    {

        private List<Person> GetPeopleFromFile()
        {

            string path = ConfigurationManager.AppSettings["PeopleFilePath"];

            List<Person> people = new List<Person>();

            if (File.Exists(path))
            {

                using (StreamReader r = new StreamReader(path))
                {

                    string json = r.ReadToEnd();

                    people = JsonConvert.DeserializeObject<List<Person>>(json);
                }
            }

            return people;
        }

        // records/gender
        [HttpGet]
        [Route("gender")]
        public JsonResult<List<Person>> Gender()
        {

            List<Person> results = this.GetPeopleFromFile().OrderBy(d => d.Sex).ToList();

            return Json(results);
        }

        [HttpGet]
        [Route("birthdate")]
        public JsonResult<List<Person>> BirthDate()
        {

            List<Person> results = this.GetPeopleFromFile().OrderBy(d => d.BirthDate).ToList();

            return Json(results);
        }

        [HttpGet]
        [Route("name")]
        public JsonResult<List<Person>> Name()
        {

            List<Person> results = this.GetPeopleFromFile().OrderBy(d => d.LastName).ToList();

            return Json(results);
        }

        [HttpGet]
        [Route("add")]
        public JsonResult<Boolean> Add([FromBody]string record)
        {

            var cells = record.Split(',');

            Person MyPerson = new Person()
            {
                LastName = cells[0].ToString(),
                FirstName = cells[1].ToString(),
                Sex = cells[2].ToString(),
                FavoriteColor = cells[3].ToString(),
                BirthDate = DateTime.Parse(cells[4].ToString())
            };

            // logic to add person

            return Json(true);
        }
    }
}
