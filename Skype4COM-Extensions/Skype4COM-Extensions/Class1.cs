using System;
using System.Linq;
using SKYPE4COMLib;
namespace Skype4COM_Extensions
{
    public static class Skype4COM_Fix
    {
        public static Skype mainSkype;
        #region Broken PM Fix
        /* [BROKEN PM FIX]
        On occasion, with Skype4COM, there's the chance that the chat ID (Chat.Name) is simply the sender's username (in PM). This causes Skype4COM to be unable to send a message to said chat, due to the obvious error, and therefore cause it to throw an exception. The code below detects if it has a faulty chat name, and then fixes it by sending the message directly as a PM, instead of trying to send it to a group. I've also highlighted support for cloud chats, so if one were to figure out how to add support for that (possible - I've done it in C#), they  could add it in.
        */
        public static ChatMessage SendMessage_Fixed(this Chat originalChat, string newText)
        {
            ChatType[] cloudChats = { ChatType.Cloud, ChatType.CloudHost };
            ChatType currentType = getChatType(originalChat.Name);
            if (cloudChats.Contains(currentType))
            {
                throw new Exception("Unsupported chat format");
            }
            if (currentType == ChatType.BrokenPrivateMessage)
            {
                return mainSkype.SendMessage(originalChat.Name, newText);
            }
            return originalChat.SendMessage(newText);
        }
        public static ChatType getChatType(string checkChat)
        {
            string chatName = checkChat;
            if (chatName.EndsWith("@thread.skype"))
            {
                return ChatType.Cloud;
            }
            try
            {
                if (chatName.Split('/')[1].StartsWith("$*T"))
                {
                    return ChatType.CloudHost;
                }
            } catch { }
            if ((chatName.Contains("#")) && (chatName.Contains("$")))
            {
                if (chatName.Contains(";"))
                {
                    return ChatType.PrivateMessage;
                }
                else
                {
                    return ChatType.PeerToPeer;
                }
            }
            else
            {
                return ChatType.BrokenPrivateMessage;
            }
        }
        public enum ChatType { Cloud, PeerToPeer, PrivateMessage, BrokenPrivateMessage, CloudHost }; // Credit to Les and I for figuring out the 5 main 'chats'
        #endregion
        #region Sending Contact Requests
        /* [SENDING CONTACT REQUESTS]
        As Skype4COM is horribly documented, a feature that's not well known to most is sending a contact request with a message, this should make it incredibly simple to do so.
        */
        public static void AddContact(this User toAdd, string requestBody)
        {
            mainSkype.Property["USER", toAdd.Handle, "BUDDYSTATUS"] = string.Format("{0} {1}", (int)TBuddyStatus.budPendingAuthorization, requestBody);
        }
        public static void AddContact(string newContactName, string requestBody)
        {
            mainSkype.Property["USER", newContactName, "BUDDYSTATUS"] = string.Format("{0} {1}", (int)TBuddyStatus.budPendingAuthorization, requestBody);
        }
        #endregion
    }
}