namespace Oyooni.Server.Services.Cache
{
    public class ActiveCall
    {
        public string VolunteerConnectionId { get; set; }
        public string VIConnectionId { get; set; }

        public ActiveCall() { }
        public ActiveCall(string volunteerConnectionId, string vIConnectionId)
            => (VolunteerConnectionId, VIConnectionId) = (volunteerConnectionId, vIConnectionId);
    }
}
