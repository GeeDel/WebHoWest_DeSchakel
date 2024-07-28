using DeSchakel.Client.Mvc.Models;
using DeSchakel.Client.Mvc.Services.Interfaces;
using DeSchakel.Client.Mvc.Viewmodels;
using DeSchakelApi.Consumer.Navigations;
using Microsoft.AspNetCore.Mvc;

namespace DeSchakel.Client.Mvc.Components
{
    public class NavigationLinksViewComponent : ViewComponent
    {
        // declare ActionLinkBuilder class + ctor	
        private readonly INavigationService _navigationService;

        public NavigationLinksViewComponent(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string area)
        {
            // get the action links    	// fill the model    	//pass tot the view



            var actionLinks = _navigationService.GetAsync(area);
            if (actionLinks.IsFaulted)
            {
                return View("Error");
            }
            var result = actionLinks.Result.Select(a => new ActionLink
            { Action = a.Action,
            Controller = a.Controller,
            Name = a.Name,
            Position = a.Position});
                var navigationLinksViewModel = new NavigationLinksViewModel
            {
                     ActionLinks =  result
                 };
            return View(navigationLinksViewModel);
                }
    }
}
