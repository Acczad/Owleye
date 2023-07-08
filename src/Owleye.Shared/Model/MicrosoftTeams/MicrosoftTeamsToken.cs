namespace Owleye.Shared.Model.MicrosoftTeams
{
    public class MicrosoftTeamsToken
    {
        public string Token_type { get; set; }
        public string Scope { get; set; }
        public int Expires_in { get; set; }
        public int Ext_expires_in { get; set; }
        public string Access_token { get; set; }
        public string Refresh_token { get; set; }


    }
}
