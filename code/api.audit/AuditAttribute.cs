using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace middleware.api.audit
{
	public class AuditAttribute : Attribute, IFilterFactory
    {
		public string ExcludeFields { get; set; } = "";
		public string IdPropertyName { get; set; }

        public bool IsReusable => false;

		public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
		{
			var filter = serviceProvider.GetService<MiddlewareAuditFilter>();

			filter.ExcludeFields = ExcludeFields?.Split(',');
			filter.IdPropertyName = IdPropertyName;

			return filter;
		}
	}
}
