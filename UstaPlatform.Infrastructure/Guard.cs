namespace UstaPlatform.Infrastructure
{
    // GEREKLİLİK 3.B.5: Statik Yardımcı Sınıf
    // Programın çökmemesi için doğrulama yapar
    public static class Guard
    {
        public static void AgainstNull(object value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        public static void AgainstNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Değer null veya boş olamaz", name);
        }
    }
}