using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Dapper;
using System.Net.Http.Headers;

public class InjectionController : Controller
{
    public const string SsrfAnswer = @"asdgf(*&^%)";
    private IOptions<WebSettings> _settings;
    private IWebHostEnvironment _host;
    private const string _adminUser = @"administrator@security-training.com";
    private const string _password = @"textbooks";

    public InjectionController(IOptions<WebSettings> settings, IWebHostEnvironment webHostEnvironment){
        _settings = settings;
        _host = webHostEnvironment;
    }
    public IActionResult Index()
    {
        ViewBag.Hint = _host.ContentRootPath;
        return View();
    }

    public IActionResult Search(string term)
    {
        var con =  new SqliteConnection("Filename=:memory:"); 
        con.BuildDatabase(_adminUser, _password);

        var products = con.Query<Product>("SELECT * FROM Product where name LIKE '%" + term + "%'");

        return Json(products);
    }    

    [HttpPost]
    public IActionResult Check(string database, string admin, string password){
        return Json(new {database = string.Equals(database, "Sqlite", StringComparison.InvariantCultureIgnoreCase), admin = admin == _adminUser, password = password == _password});
    }

}

public class Product
{
    public string Name {get;set;}
    public double Price {get;set;}
}