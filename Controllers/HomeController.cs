using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Page2()
    {
        return View();
    }

    public IActionResult AdminView()
    {
        // Attempt to retrieve the cookie
        if (HttpContext.Request.Cookies.TryGetValue("MyVulnerableCookie", out string encodedRole))
        {
            // Decode the Base64 encoded role
            var role = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedRole));

            // Check if the role is "Admin"
            if (role == "Admin")
            {
                return View();
            }
        }

        // If the cookie is not set to "Admin", redirect to the Unauthorized page
        return RedirectToAction("Unauthorized");
    }


    public IActionResult RegularUserView()
    {
        return View();
    }

    public IActionResult Unauthorized()
    {
        return View();
    }

    public IActionResult BrokenAccessControl()
    {
        // Always redirect to the Login page when accessing the Broken Access Control demo
        return RedirectToAction("Login");
    }

   

    [HttpGet]
    public IActionResult Login()
    {
        // Return the Login view when the Login link is clicked
        return View();
    }

    [HttpPost]
    public IActionResult Login(string role)
    {
        // Create a simple cookie with the user's role, encoded in Base64
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.Now.AddDays(1)
        };

        // Simulating a more complex value by encoding the role
        string encodedRole = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(role));
        Response.Cookies.Append("MyVulnerableCookie", encodedRole, cookieOptions);

        // Redirect based on the original role
        if (role == "Admin")
        {
            return RedirectToAction("AdminView");
        }
        else if (role == "User")
        {
            return RedirectToAction("RegularUserView");
        }
        else
        {
            return RedirectToAction("Unauthorized");
        }
    }




}
