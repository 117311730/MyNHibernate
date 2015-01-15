using System.Collections.Generic;
using System.Linq;
using MyNHibernate.Models;

namespace MyNHibernate.Services
{
    public class CityService : BaseRepository
    {
        public IEnumerable<City> GetAll()
        {
            return Session.QueryOver<City>()
                .Fetch(city => city.LocalVendor).Eager
                .Future()
                .Distinct();              
        }

        public City GetOne(int id)
        {
            return Session.Get<City>(id);
        }


    }
}