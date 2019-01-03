using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using AhwanamAPI;
using MaaAahwanam.Service;
using MaaAahwanam.Models;


namespace AhwanamAPI.Providers
{
    public class OAuthProvider : OAuthAuthorizationServerProvider
        {
            #region[GrantResourceOwnerCredentials]
            public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
            {
            UserLogin userlogin = new UserLogin();
            return Task.Factory.StartNew(() =>
                {
                    //var userName = context.UserName;
                    //var password = context.Password;
                    userlogin.UserName = context.UserName;
                    userlogin.Password = context.Password;
                    var userService = new ResultsPageService(); // our created one
                    var user = userService.GetUserLogin(userlogin);
                    if (user != null)
                    {
                        var claims = new List<Claim>()
                    {
                        //new Claim(ClaimTypes.Sid, Convert.ToString(user.UserLoginId)),
                        new Claim(ClaimTypes.Name, user.UserName),
                        //new Claim(ClaimTypes.UserData, user.UserType)
                    };
                        ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims,
                                    Startup.OAuthOptions.AuthenticationType);

                        var properties = CreateProperties(user.UserName,user.UserType,user.UserLoginId);
                        var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                       //var token1 = Startup.OAuthOptions.AccessTokenFormat.Protect(ticket);
                       //var token = Startup.OAuthOptions.AccessTokenFormat.Unprotect(token1);
                       // context.Validated(token);

                        context.Validated(ticket);
                      
                    }
                    else
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect");
                    }
                });
            }
            #endregion

            #region[ValidateClientAuthentication]
            public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
            {
                if (context.ClientId == null)
                    context.Validated();

                return Task.FromResult<object>(null);
            }
            #endregion

            #region[TokenEndpoint]
            public override Task TokenEndpoint(OAuthTokenEndpointContext context)
            {
                foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
                {
                    context.AdditionalResponseParameters.Add(property.Key, property.Value);
                }

                return Task.FromResult<object>(null);
            }
            #endregion

            #region[CreateProperties]
            public static AuthenticationProperties CreateProperties(string userName,string userType,long userLoginId)
            {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }, {"userType" , userType },{"userLoginId" , userLoginId.ToString() },
            };
                return new AuthenticationProperties(data);
            }
            #endregion
        }
    }
