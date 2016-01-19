# Skype4COM-Extensions
Skype4COM is outdated, broken and limited. This aims to fix small bugs in the API. This is by no means a new API, only extensions for it which allow it to do certain tasks that would be very difficult or glitchy in the original API.
# Usage
Starting usage
```
Skype skypeVar = new Skype(); // Honestly, this should be a global variable. 
skypeVar.Attach(); // Attaches to Skype client (basic Skype4COM)
Skype4COM_Fix.mainSkype = skypeVar; // Sets Skype in the Fix
```
Sending a message
```
static void skypeVar_MessageStatus(ChatMessage pMessage, TChatMessageStatus Status) // Skype's generated event for messages
{
    ChatMessage rMessage = pMessage.Chat.SendMessage_Fixed("Hello world!"); // Replies to the message with 'Hello world!'. Notice that it should no longer bug in PMs
}
```
