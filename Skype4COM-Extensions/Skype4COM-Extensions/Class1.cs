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
        public static ChatMessage SendMessage_Fixed(this Chat originalChat, string Text)
        {
            ChatType[] cloudChats = { ChatType.Cloud, ChatType.CloudHost };
            ChatType currentType = getChatType(originalChat.Name);
            if (cloudChats.Contains(currentType))
            {
                throw new Exception("Unsupported chat format");
            }
            if (currentType == ChatType.BrokenPrivateMessage)
            {
                return mainSkype.SendMessage(originalChat.Name, Text);
            }
            return originalChat.SendMessage(Text);
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
    }
}