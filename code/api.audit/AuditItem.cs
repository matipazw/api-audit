using System;
namespace middleware.api.audit
{
    public class AuditItem
    {
		public string UserId { get; set; }
        public string UserName { get; set; }
		public string UserRole { get; set; }
		public string ItemId { get; set; }
		public AuditAction Action { get; set; }
		public object Data { get; set; }
		public DateTime Date { get; set; }
		public string Entity { get;  set; }
		public string ClientId { get;  set; }
	}
}
