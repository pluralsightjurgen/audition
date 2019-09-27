using Microsoft.AspNetCore.Mvc;

public static async Task<IActionResult> Run(
    HttpRequest req, 
    ILogger log,
    Microsoft.Azure.WebJobs.IAsyncCollector<dynamic> outputDocuments)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    var requestUtc = DateTime.UtcNow;

    string requestBody = null;
    using (var bodyReader = new StreamReader(req.Body))
    {
        requestBody = await bodyReader.ReadToEndAsync();
    }

    log.LogInformation($"Request body: {requestBody}");

    var formValues = System.Web.HttpUtility.ParseQueryString(requestBody);

    var outputDocument = new 
    {
        requestUtc,
        partitionKey = requestUtc.ToString("yyyy-MM"),
        firstName = formValues["firstName"],
        lastName = formValues["lastName"],
        email = formValues["email"],
        dogName = formValues["dogName"],
        dogBreed = formValues["dogBreed"]
    };

    await outputDocuments.AddAsync(outputDocument);

    return new OkResult();
}
