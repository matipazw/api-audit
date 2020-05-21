using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace middleware.api.audit
{
	public class MiddlewareAuditFilter : IAsyncActionFilter
    {
        #region Members

        readonly IAuditService _service;
        readonly IDictionary<string, AuditAction> _actions;

		#endregion

		#region Properties

		public string[] ExcludeFields { get; set; }
		public string IdPropertyName { get; set; }

		#endregion

		#region Constructors

		public MiddlewareAuditFilter(IAuditService service)
        {
            _service = service;
            _actions = InitializeAuditActions();
        }

        #endregion

        #region Methods

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as Controller;
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var identity = controller.User.Identity as ClaimsIdentity;
            
            var auditItem = BuildAuditItem(controller.Request.Method, actionDescriptor.ControllerName, context.ActionArguments, identity.Claims);

            RegisterAudit(auditItem);

            await next();
        }

        #endregion

        IDictionary<string, AuditAction> InitializeAuditActions()
        {
            return new Dictionary<string, AuditAction>
            {
                {HttpMethods.Get, AuditAction.Find},
                {HttpMethods.Post, AuditAction.Create},
                {HttpMethods.Put, AuditAction.Update},
                {HttpMethods.Delete, AuditAction.Delete}
            };
        }

        AuditItem BuildAuditItem(string httpMethod, string controllerName, IDictionary<string, object> arguments, IEnumerable<Claim> claims)
        {
            var item = new AuditItem();

            item.Action = _actions[httpMethod];
			item.Entity = controllerName;
            item.Date = DateTime.Now;
            item.UserId = GetClaimValue(claims, JwtPrivateClaimNames.UserId);
            item.UserName = GetClaimValue(claims, JwtPrivateClaimNames.UserName);
            item.UserRole = GetClaimValue(claims, JwtPrivateClaimNames.UserRole);
            item.ClientId = GetClaimValue(claims, JwtPrivateClaimNames.ClientId);

			var jsonObject = JObject.FromObject(arguments);

			item.Data = RemoveExcludeFields(jsonObject);
			item.ItemId = GetItemId(jsonObject);

            return item;
        }

		string GetItemId(JObject jsonObject) {
            
			if(String.IsNullOrEmpty(IdPropertyName))
			{
				return string.Empty;	
			}

			var descendats = jsonObject.Descendants();

			return descendats.Where(d => d.Type == JTokenType.Property)
				             .FirstOrDefault(d => d.Path.ToLower() == IdPropertyName.ToLower() ||
				                             d.Path.ToLower().EndsWith("." + IdPropertyName.ToLower(), StringComparison.Ordinal))
				             .ToObject<JProperty>()
				             .Value.ToString();
		}

        object RemoveExcludeFields(JObject jsonObject)
        {
            var descendents = jsonObject.Descendants();

			foreach (var excludeField in ExcludeFields.Where(f => !String.IsNullOrEmpty(f)))
            {
                var tokens = GetTokens(excludeField.Trim(), descendents);

                foreach (var token in tokens)
                {
                    token.Remove();
                }
            }

            return jsonObject;
        }

        IList<JToken> GetTokens(string excludeField, IEnumerable<JToken> tokens)
        {
			return tokens.Where(d => d.Path.ToLower().Contains("." + excludeField.ToLower()) || d.Path.ToLower() == excludeField.ToLower())
                         .Where(d => d.Type == JTokenType.Array
                                || d.Type == JTokenType.Object
                                || d.Type == JTokenType.Property)
                         .ToList();
        }

        string GetClaimValue(IEnumerable<Claim> claims, string claimName)
        {
            return claims.FirstOrDefault(c => c.Type == claimName)?.Value;
        }

        void RegisterAudit(AuditItem auditItem)
        {
            _service.SaveAsync(auditItem);
        }

    }
}
