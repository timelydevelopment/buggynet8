using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Newtonsoft.Json;

public class IntegrityController : Controller
{
    private string fileName;

    private static int counter = 100;

    public void SetFileName()
    {
     
        this.fileName = $"{counter}.txt";
   
    }
 

     
    public IActionResult Index()
    {
        counter++;
        SetFileName();

        ViewBag.FileName = fileName;

        using (StreamWriter sw = System.IO.File.CreateText(fileName))
            {
                // Write content to the file
                sw.WriteLine("Example text file created for!");
                sw.WriteLine(fileName);
            }
        
        return View();
    }

    [HttpGet]
    public string Format(string json)
    {
        var vuln = "{'Object':" + json + "}";
        var serialized = JsonConvert.DeserializeObject<BadWrapper>(vuln, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });
        Console.WriteLine(serialized.Object.ToString());
        Console.WriteLine(serialized.Object.GetType().ToString());
        
        //format the json nicely
        return JsonConvert.SerializeObject(serialized.Object, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented
        });   
    }

    [HttpPost]
    public bool Check()
    {
            SetFileName();

   // Get the file attributes
            FileAttributes attributes = System.IO.File.GetAttributes(fileName);

            // Check if the file is read-only
            return (attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
  

    }
}

public class BadWrapper {
    public dynamic Object {get;set;}
}
