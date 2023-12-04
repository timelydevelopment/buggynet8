using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

public class SsrfController : Controller
{
    public const string SsrfAnswer = @"asdgf(*&^%)";
    private IOptions<WebSettings> _settings;
    private IWebHostEnvironment _host;

    public SsrfController(IOptions<WebSettings> settings, IWebHostEnvironment webHostEnvironment){
        _settings = settings;
        _host = webHostEnvironment;
    }
    public IActionResult Index()
    {
        ViewBag.Hint = _host.ContentRootPath;
        return View();
    }

    public IActionResult Page2()
    {
        return View();
    }
    [HttpPost]
    public async Task<string> GetReport(string report)
    {
        var request = WebRequest.Create(report);
        request.Headers.Add("secret", SsrfAnswer);
            // Add custom header

        var res = request.GetResponse();
        Stream sr=  res.GetResponseStream();
        StreamReader sre = new StreamReader(sr);

        return sre.ReadToEnd();
    }

    [HttpPost]
    public async Task<string> GetReportBetter(string report)
    {
        using (HttpClient client = new HttpClient())
        {
            // Add custom header
            client.DefaultRequestHeaders.Add("secret", SsrfAnswer);

            // Download the JSON data
            string jsonData = await client.GetStringAsync(report);
            return jsonData;
        }

    }
    
    [HttpPost]
    public IActionResult CheckAnswer(string header, string config){
        return Json(new {header = header == SsrfAnswer, config = config == _settings.Value.Secret});
    }
}
