using coderush.Pages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush
{
    public static class HtmlHelpers
    {

        public static string IsSelected(this IHtmlHelper html, string controller = null, string action = null, string cssClass = null, string module = null)
        {
            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            if (!String.IsNullOrEmpty(module))
            {
                string path = "/"+currentController+"/"+currentAction+"";
                string selectedModule = GetModuleNameByPath(path);

                return module.Equals(selectedModule) ?
                cssClass : String.Empty;
            }
            else
            {
                return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
            }

            
        }

        private static string GetModuleNameByPath(string path)
        {
            string result = "";
            try
            {
                Dictionary<string, string> modulePath = new Dictionary<string, string>();
                Type t = typeof(MainMenu);
                foreach (Type item in t.GetNestedTypes())
                {
                    string key = "";
                    string val = "";

                    foreach (var itm in item.GetFields())
                    {
                        if (itm.Name.Contains("Module"))
                        {
                            string value = (string)itm.GetValue(item);
                            val = value;
                        }

                        if (itm.Name.Contains("Path"))
                        {
                            string value = (string)itm.GetValue(item);
                            key = value;
                        }
                    }

                    modulePath.Add(key, val);
                }
                if (!modulePath.TryGetValue(path, out result))
                {
                    result = string.Empty;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

        public static string PageClass(this IHtmlHelper htmlHelper)
        {
            string currentAction = (string)htmlHelper.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

    }
}
