using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

namespace ASPCoreMVC.Web.Helpers
{
    public static class HtmlStyleHelpers
    {
        private const string StylesKey = "Vistark_Delayed_Headers";

        public static IDisposable BeginStyles(this IHtmlHelper helper)
        {
            return new StyleBlock(helper.ViewContext);
        }

        public static HtmlString PageStyles(this IHtmlHelper helper)
        {
            return new HtmlString(string.Join(Environment.NewLine, GetPageStylesList(helper.ViewContext.HttpContext)));
        }

        private static List<string> GetPageStylesList(HttpContext httpContext)
        {
            var pageStyles = (List<string>)httpContext.Items[StylesKey];
            if (pageStyles == null)
            {
                pageStyles = new List<string>();
                httpContext.Items[StylesKey] = pageStyles;
            }
            return pageStyles;
        }

        private class StyleBlock : IDisposable
        {
            private readonly TextWriter _originalWriter;
            private readonly StringWriter _StylesWriter;

            private readonly ViewContext _viewContext;

            public StyleBlock(ViewContext viewContext)
            {
                _viewContext = viewContext;
                _originalWriter = _viewContext.Writer;
                _viewContext.Writer = _StylesWriter = new StringWriter();
            }

            public void Dispose()
            {
                _viewContext.Writer = _originalWriter;
                var pageStyles = GetPageStylesList(_viewContext.HttpContext);
                pageStyles.Add(_StylesWriter.ToString());
            }
        }
    }
}
