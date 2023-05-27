using Microsoft.AspNetCore.Http;
namespace BrandMatrix.PresentationLayer.Common
{
    public static class SessionExtensions
    {
        public static bool GetBool(this ISession session, string key)
        {
            var value = session.GetString(key);
            return bool.TryParse(value, out var result) && result;
        }

        public static void SetBool(this ISession session, string key, bool value)
        {
            session.SetString(key, value.ToString());
        }

    }
}
