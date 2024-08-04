using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeSchakelApi.Consumer
{
    public static class ApiRoutes
    {
        public const string Base = "https://localhost:44326/api";
        public const string Events = Base + "/events/";
        public const string Locations = Base + "/locations/";
        public const string Companies = Base + "/companies/";
        public const string Genres = Base + "/genres/";
        public const string ActionLinks = Base + "/ActionLinks/";
        public const string Accounts = Base + "/authentication/";
        public const string Auth = Base + "/auth/";
        public const string Email = Accounts + "ByEmail";
        public const string Roles = Base + "/roles/";
    }
}
