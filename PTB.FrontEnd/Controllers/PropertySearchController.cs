using PTB.DataAccess;
using PTB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PTB.FrontEnd.Controllers
{
    public class PropertySearchController : ApiController
    {
        [HttpPost]
        public PropertySearchModel getSearch([FromBody]PropertySearchParameterModel filters)
        {
            try
            {
                PropertyRepository _searchRepository = new PropertyRepository();
                var propertySearchResult = _searchRepository.SearchProperty(filters.PageNumber,filters.PageSize, filters.Lot, filters.Section, filters.Block);
                return propertySearchResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public PropertyModel getPropertyById([FromBody]long id)
        {
            try
            {
                PropertyRepository _searchRepository = new PropertyRepository();
                var propertySearchResult = _searchRepository.GetPropertyById(id);
                return propertySearchResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public bool updateProperty([FromBody]PropertyModel propertyModel)
        {
            try
            {
                PropertyRepository _searchRepository = new PropertyRepository();
                var propertySearchResult = _searchRepository.UpdateProperty(propertyModel);
                return propertySearchResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public bool deleteProperty([FromBody]long id)
        {
            try
            {
                PropertyRepository _searchRepository = new PropertyRepository();
                var propertySearchResult = _searchRepository.DeleteProperty(id);
                return propertySearchResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}