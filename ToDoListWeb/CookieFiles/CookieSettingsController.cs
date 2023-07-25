using Microsoft.AspNetCore.Mvc;
using System.Web;


namespace ToDoListWeb.CookieFiles
{
    public class CookieSettingsController : Controller
    {
        public IActionResult CookieThemeSettings(string ThemeSettings)
        {
            if (!string.IsNullOrEmpty(ThemeSettings))
            {
                Response.Cookies.Append("SettingsTheme", ThemeSettings, new CookieOptions
                {
                    // Настройки куки (например, время жизни и доступность)
                    Expires = DateTime.Now.AddDays(7) // Здесь можно установить желаемое время жизни куки
                });
            } 
            return View();
        }

    }
}
