using DeSchakel.Client.Mvc.Models;
using DeSchakel.Client.Mvc.Services.Interfaces;
using DeSchakel.Client.Mvc.Viewmodels;
using DeSchakelApi.Consumer.Genres;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;

namespace DeSchakel.Client.Mvc.Components
{
    public class GenreListViewComponent : ViewComponent
    {
        private readonly IGenreApiService _genreApiService;

        public GenreListViewComponent(IGenreApiService genreApiService)
        {
            _genreApiService = genreApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string area)
        {
            // get the genrelist    	// fill the model    	//pass tot the form view as selectlestitems
            var genres = await _genreApiService.GetAsync();

            GenreListViewmodel genreListModel = new GenreListViewmodel
            {
                Genres = genres.Select(g => new BaseItemModel
                {
                    Id = g.Id,
                    Name = g.Name
                }),
                SetArea = area

            };
            return View(genreListModel);
        }
    }
}
