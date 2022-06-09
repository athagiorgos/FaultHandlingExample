using Polly;
using Polly.Retry;

namespace RequestService.Policies
{
    public class ClientPolicy
    {
        public AsyncRetryPolicy<HttpResponseMessage> ImmediateHttpRetry { get; set; }
        public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetry { get; set; }
        public AsyncRetryPolicy<HttpResponseMessage> ExponentialHttpRetry { get; set; }

        public ClientPolicy()
        {
            ImmediateHttpRetry =
            Policy.HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
            .RetryAsync(5);

            LinearHttpRetry =
            Policy.HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(3));

            ExponentialHttpRetry =
            Policy.HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}