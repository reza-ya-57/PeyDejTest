using System;

namespace PeyDej.Tools
{
    public static class PeyDejHelper
    {
        public static string ActiveLinkAjax(string href, string text, bool downloadLink, bool openNewTab)
        {
            return
                $"<a {(openNewTab ? "target=\"_blank\"" : "")} href=\"{href}\" {(downloadLink ? "download" : "")}>{text}</a>";
        }

        public static string ActiveLinkAjax(string href, string text, string css, string title = "")
        {
            return $"<a href=\"{href}\" class=\"{css}\" title=\"{title}\">{text}</a>";
        }

        public static string ActionLink(string text, string action, string controller, object? id, string css,
            string title = "")
        {
            return id == null ? $"<a href=\"{controller}/{action}\" class=\"{css}\" title=\"{title}\">{text}</a>" : $"<a href=\"{controller}/{action}/{id}\" class=\"{css}\" title=\"{title}\">{text}</a>";
        }
        
        public static string ActionLink(string text, string action, string controller,string parameterName, object? id, string css,
            string title = "")
        {
            return id == null ? $"<a href=\"{controller}/{action}\" class=\"{css}\" title=\"{title}\">{text}</a>" : $"<a href=\"{controller}/{action}?{parameterName}={id}\" class=\"{css}\" title=\"{title}\">{text}</a>";
        }
    }
}