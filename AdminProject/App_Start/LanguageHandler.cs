using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AdminProject.App_Start
{
    public class LanguageHandler : DelegatingHandler
    {
        readonly HashSet<string> _languages = new HashSet<string>()
        {
            "tr", "en", "ru", "fr"
        };

        private readonly string _defaultLanguage = "tr";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var language = GetLanguage(request);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

            return base.SendAsync(request, cancellationToken);
        }

        private string GetLanguage(HttpRequestMessage request)
        {
            var language = _defaultLanguage;
            var header = request.Headers.AcceptLanguage.FirstOrDefault();
            if (header != null)
            {
                var value = header.Value;
                if (_languages.Contains(value))
                    language = value;
            }
            return language;
        }
    }
}