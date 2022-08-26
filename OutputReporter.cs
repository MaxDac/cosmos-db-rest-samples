using Microsoft.Extensions.Logging;

namespace CosmosDbRestSamples;

public class OutputReporter
{
    private readonly ILogger<OutputReporter> m_logger;

    public OutputReporter(ILogger<OutputReporter> logger)
    {
        m_logger = logger;
    }

    public async Task ReportOutput(string methodName, HttpResponseMessage httpResponse)
    {
        var responseContent = await httpResponse.Content.ReadAsStringAsync();
        if (httpResponse.IsSuccessStatusCode)
        {
            m_logger.LogInformation("{MethodName}: SUCCESS\n    {ResponseContent}\n\n", methodName, responseContent);
        }
        else
        {
            m_logger.LogError("{MethodName}: FAILED -> {StatusCode}: {ReasonPhrase}.\n    {ResponseContent}\n\n",
                methodName,
                (int)httpResponse.StatusCode,
                httpResponse.ReasonPhrase,
                responseContent
            );
        }
    }
}
