using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

[Authorize(Roles = "Basic")]
public class BasicController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
[Authorize(Roles = "Admin, Basic")]
public class SomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    [Authorize(Roles = "Admin")]
    public IActionResult ManageUsers()
    {
        // فقط المستخدمين الذين لديهم دور Admin يمكنهم الوصول إلى هذه الصفحة
        return View();
    }

    [Authorize(Roles = "Basic")]
    public IActionResult ManageTransactions()
    {
        // المستخدمين الذين لديهم دور Basic يمكنهم الوصول إلى هذه الصفحة
        return View();
    }

}
