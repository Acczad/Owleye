using System.Collections.Generic;

namespace Owleye.Shared.Model.MicrosoftTeams
{
    public class MicrosoftTeamsConversation
    {
        private readonly string baseUrl;

        public MicrosoftTeamsConversation(
            string chatType,
            string topic,
            string baseUrl,
            IEnumerable<string> usersId
            )
        {
            Members = new List<Dictionary<string, object>>();
            ChatType = chatType;
            Topic = topic;
            this.baseUrl = baseUrl;
            AddUsers(usersId);
        }

        private void AddUsers(IEnumerable<string> usersId)
        {
            foreach (var userId in usersId)
            {
                var keyValuePairs = new Dictionary<string, object>();
                keyValuePairs.Add("@odata.type", "#microsoft.graph.aadUserConversationMember");
                var rules = new List<string>() { "owner" };
                keyValuePairs.Add("roles", rules);
                keyValuePairs.Add("user@odata.bind", $"{baseUrl}/users('{userId}')");
                Members.Add(keyValuePairs);
            }
        }
        public string ChatType { get; private set; }
        public string Topic { get; set; }
        public List<Dictionary<string, object>> Members { get; set; }
    }
}
