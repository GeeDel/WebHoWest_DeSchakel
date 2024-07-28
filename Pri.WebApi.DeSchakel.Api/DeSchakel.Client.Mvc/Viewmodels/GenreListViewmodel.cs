
using DeSchakel.Client.Mvc.Models;
using System.Web.Mvc;

namespace DeSchakel.Client.Mvc.Viewmodels
{
    public class GenreListViewmodel
    {
        public IEnumerable<BaseItemModel> Genres { get; set; }
        public string SetArea { get; set; }

    }
}
