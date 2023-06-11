using System.Collections.Generic;

namespace Owleye.Shared.Model.MicrosoftTeams
{
    public class SendTeamsUsersMessageRequest
    {
        public List<string> Emails { get; set; }
        public string Message { get; set; }
    }
}
