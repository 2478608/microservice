using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Timeout;
using System.Net;

namespace OrderService.Infra.Policies
{
    public static class PollyPolicies
    {

        //retry with exponstional backoouff
        public static IAsyncPolicy<HttpResponseMessage> RetryPolicy =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) //2,4,8
                );

        //circuit breaker policy
        public static IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy =>
           HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 2,
                durationOfBreak: TimeSpan.FromSeconds(30)
               );

        //timeout
        public static IAsyncPolicy<HttpResponseMessage> TimeoutPolicy =>
                    Policy.TimeoutAsync<HttpResponseMessage>(
                        TimeSpan.FromSeconds(2)
                    );


        // Bulkhead Isolation
        public static IAsyncPolicy<HttpResponseMessage> BulkheadPolicy =>
            Policy.BulkheadAsync<HttpResponseMessage>(
                maxParallelization: 5,
                maxQueuingActions: 10
            );



        // ✅ CORRECT Fallback (IMPORTANT)
        public static IAsyncPolicy<HttpResponseMessage> FallbackPolicy =
            Policy<HttpResponseMessage>
                .Handle<Exception>()
                .FallbackAsync(
                    new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                );

        // ✅ COMBINED POLICY
        public static IAsyncPolicy<HttpResponseMessage> CombinedPolicy =
            Policy.WrapAsync(
                FallbackPolicy,
                BulkheadPolicy,
                RetryPolicy,
                CircuitBreakerPolicy,
                TimeoutPolicy
            );

    }
}
