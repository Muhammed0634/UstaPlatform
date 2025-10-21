using System.Globalization;

namespace UstaPlatform.Infrastructure
{
    public static class ParaFormatlayici
    {
        public static string Formatla(decimal miktar)
        {
            // Bu satır, para formatının Türkiye'ye özgü (₺ simgesi ile)
            // yapılmasını sağlar.
            var cultureInfo = new CultureInfo("tr-TR");
            return string.Format(cultureInfo, "{0:C}", miktar);
        }
    }
}

