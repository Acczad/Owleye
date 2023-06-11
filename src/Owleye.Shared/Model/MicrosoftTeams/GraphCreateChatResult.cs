using System;

namespace Owleye.Shared.Model.MicrosoftTeams
{
    public class GraphCreateChatResult
    {
        public string OdataContext { get; set; }
        public string Id { get; set; }
        public object Topic { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string ChatType { get; set; }
        public string WebUrl { get; set; }
        public string TenantId { get; set; }
    }
}
