namespace Orchard.Tools.Helpers.Html {
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Orchard.Tools.Helpers.Strings;

    public static class HtmlHelperExtensions {
        /// <summary>Replaces all occurences of CRLF with a <![CDATA[<br/>]]> tag.</summary>
        /// <param name="helper">A HTMLHelper instance.</param>
        /// <param name="inputString">The string to transform.</param>
        /// <param name="doHtmlEncode">Wether to htmlEcnode the result.</param>
        public static MvcHtmlString ReplaceCrLf(this HtmlHelper helper, string inputString, bool doHtmlEncode = true) {
            return ReplaceCrLfWithHtmlBreak(inputString, doHtmlEncode);
        }

        public static MvcHtmlString ReplaceCrLfWithHtmlBreak(string inputString, bool doHtmlEncode) {
            var s = (doHtmlEncode ? HttpUtility.HtmlEncode(inputString) : inputString)
                .GetAllLines()
                .Where(x => x != null)
                .Aggregate(
                    new StringBuilder(),
                    (sb, line) => sb.Append(line + "<br />"),
                    sb => sb.ToString());

            return MvcHtmlString.Create(s);
        }
    }
}