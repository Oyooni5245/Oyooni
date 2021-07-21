using System.Linq;

namespace Oyooni.Server.Constants
{
    public static class RedisPrefixes
    {
        public const string Volunteer = "VOLUNTEER";
        public const string VI = "VISUALLYIMPAIRED";
        public const string InCall = "INCALL";
        public const string NotInCall = "NOTINCALL";
        public const string ActiveCall = "ACTIVECALL";
        public const string HelpRequest = "HELPREQUEST";

        public static string VolunteerKey(string connectionId, bool inCall = false)
            => $"{Volunteer}@{(inCall ? InCall : NotInCall)}@{connectionId}";

        public static string VIKey(string connectionId)
            => $"{VI}@{connectionId}";

        public static string HelpRequestKey(string vIConnectionId)
            => $"{HelpRequest}@{vIConnectionId}";

        public static string ActiveCallKey(string connectionId)
            => $"{ActiveCall}@{connectionId}";

        public static (string, string) ActiveCallKeys(string connectionId1, string connectionId2)
            => ($"{ActiveCall}@{connectionId1}", $"{ActiveCall}@{connectionId2}");

        public static string VolunteerNotInCallPattern() => $"{Volunteer}@{NotInCall}*";

        public static string ExtractConnectionId(string prefix)
            => prefix.Split(new char[] { '@' }).Last();
    }
}
