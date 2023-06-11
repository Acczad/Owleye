using System.Collections.Generic;
using System;

namespace Owleye.Shared.Model.MicrosoftTeams
{
    //request
    public class GraphSendMessageInChat
    {
        public GraphSendMessageInChat(string content)
        {
            body = new GraphMessageBody(content);

        }
        public GraphMessageBody body { get; set; }
    }

    public class GraphMessageBody
    {
        public GraphMessageBody(string content, string contentType = "html")
        {
            this.content = content;
            this.contentType = contentType;
        }

        public string content { get; set; }
        public string contentType { get; set; }
    }

    //response
    public class GraphSendMessageInChatResponse
    {

        public string OdataContext { get; set; }
        public string id { get; set; }
        public object replyToId { get; set; }
        public string etag { get; set; }
        public string messageType { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public object lastEditedDateTime { get; set; }
        public object deletedDateTime { get; set; }
        public object subject { get; set; }
        public object summary { get; set; }
        public string chatId { get; set; }
        public string importance { get; set; }
        public string locale { get; set; }
        public object webUrl { get; set; }
        public object channelIdentity { get; set; }
        public object policyViolation { get; set; }
        public object eventDetail { get; set; }
        public ChatFrom from { get; set; }
        public ChatBody body { get; set; }
        public List<object> attachments { get; set; }
        public List<object> mentions { get; set; }
        public List<object> reactions { get; set; }
    }
    public class ChatBody
    {
        public string contentType { get; set; }
        public string content { get; set; }
    }
    public class ChatFrom
    {
        public object application { get; set; }
        public object device { get; set; }
        public UserInChat user { get; set; }
    }
    public class UserInChat
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string userIdentityType { get; set; }
    }
}
