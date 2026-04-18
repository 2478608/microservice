using Polly;
using Polly.Extensions.Http;
using System.Net;

namespace OrderService.Infra.Policies
{
    public static class PollyPolicies
    {
        // ✅ Retry with exponential backoff
        public static IAsyncPolicy<HttpResponseMessage> RetryPolicy =
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retry =>
                        TimeSpan.FromSeconds(Math.Pow(2, retry))
                );

        // ✅ Circuit Breaker
        public static IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy =
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromSeconds(15)
                );

        // ✅ Timeout (INNER policy)
        public static IAsyncPolicy<HttpResponseMessage> TimeoutPolicy =
            Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(2));

        // ✅ Fallback (OUTER policy)
        public static IAsyncPolicy<HttpResponseMessage> FallbackPolicy =
            Policy<HttpResponseMessage>
                .Handle<Exception>()
                .FallbackAsync(
                    new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                );

        // ✅ Correct policy order (OUTER → INNER)
        public static IAsyncPolicy<HttpResponseMessage> CombinedPolicy =
            Policy.WrapAsync(
                FallbackPolicy,
                CircuitBreakerPolicy,
                RetryPolicy,
                TimeoutPolicy
            );
    }
}