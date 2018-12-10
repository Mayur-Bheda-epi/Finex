using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Finex.Api
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private const string BasicAuthResponseHeader = "WWW-Authenticate";
        private const string BasicAuthResponseHeaderValue = "Basic";
        //readonly DataContext Context = new DataContext();

        public string UsersConfigKey { get; set; }
        public string RolesConfigKey { get; set; }

        //protected CustomPrincipal CurrentUser
        //{
        //    get { return Thread.CurrentPrincipal as CustomPrincipal; }
        //    set { Thread.CurrentPrincipal = value as CustomPrincipal; }
        //}

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                AuthenticationHeaderValue authValue = actionContext.Request.Headers.Authorization;

                if (authValue != null && !String.IsNullOrWhiteSpace(authValue.Parameter) && authValue.Scheme == BasicAuthResponseHeaderValue)
                {
                    Credentials parsedCredentials = ParseAuthorizationHeader(authValue.Parameter);

                    if (parsedCredentials != null)
                    {

                        if (parsedCredentials.Username == "NIA" && parsedCredentials.Password == "Nia@123")
                        {

                            return;

                        }
                        else
                        {
                            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                            actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                return;

            }
        }

        private Credentials ParseAuthorizationHeader(string authHeader)
        {
            string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader)).Split(new[] { ':' });

            if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[1]))
                return null;

            return new Credentials() { Username = credentials[0], Password = credentials[1], };
        }
    }
    //Client credential
    public class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}