using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using middleware.api.audit;
using Newtonsoft.Json;

namespace api.audit.aws.sqs
{
	public class SQSAuditService : IAuditService
	{
		readonly IAmazonSQS _queueProvider;
        readonly IConfiguration _configuration;

		public SQSAuditService(IAmazonSQS queueProvider, IConfiguration configuration)
        {
            _queueProvider = queueProvider;
            _configuration = configuration;
        }

		public async Task SaveAsync(AuditItem item)
		{
			var queueUrl = GetUrlQueue();
			var sendRequest = GetAuditMessage(queueUrl, item);
            
            await _queueProvider.SendMessageAsync(sendRequest);           
		}

		string GetUrlQueue()
        {
			return _configuration.GetSection("api.audit:AWS:SQS:queueName").Value;
        }

		SendMessageRequest GetAuditMessage(string queueUrl, AuditItem item)
        {
            var sendRequest = new SendMessageRequest();
            sendRequest.QueueUrl = queueUrl;
			sendRequest.MessageBody = JsonConvert.SerializeObject(item);
            return sendRequest;
        }      
	}
}
