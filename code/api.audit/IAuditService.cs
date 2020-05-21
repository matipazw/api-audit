using System.Threading.Tasks;

namespace middleware.api.audit
{
	public interface IAuditService
    {
		Task SaveAsync(AuditItem item);
    }
}
