# Skype4COM-Extensions
Skype4COM is outdated, broken and limited. This aims to fix small bugs in the API. This is by no means a new API, only extensions for it which allow it to do certain tasks that would be very difficult or glitchy in the original API.

# (YOU SHOULD STOP USING SKYPE4COM NOW. AND MOVE OVER TO SKYPE4SHARP) [https://github.com/lin-e/Skype4Sharp]

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
Adding a contact (by username)
```
Skype4COM_Fix.AddContact("foobar", "Hello world!"); // Sends a contact request to the specified user with the specified message
```
Adding a contact (by User type)
```
// This is only an example; it will add whoever sends you a message (which may accidentally remove them if they are already added)
static void skypeVar_MessageStatus(ChatMessage pMessage, TChatMessageStatus Status) // Skype's generated event for messages
{
    pMessage.Sender.AddContact("Hello world!"); // Adds said contact with the specified message
}
```
