namespace CommunityBoard.BackEnd.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Announcements
        {
            //Main Routes (CRUD)
            public const string Create = Base + "/announcements";
            public const string GetAll = Base + "/announcements";
            public const string Get = Base + "/announcements/{announcementId}";
            public const string Update = Base + "/announcements/{announcementId}";
            public const string Delete = Base + "/announcements/{announcementId}";

            //Extra
            public const string GetFromUser = Base + "/announcements/user/";

            //Like this for now, may implement API Filtering (and pagination) later
            public const string GetByName = Base + "/announcements/name={announcementName}";
        }

        public static class Reports
        {
            public const string Create = Base + "/announcements/{announcementId}/reports";
            public const string GetAll = Base + "/reports";
            public const string Get = Base + "/reports/{reportId}";
            public const string Delete = Base + "/reports/{reportId}";

            //Extra
            public const string GetAllFromAnnouncement = Base + "/announcements/{announcementId}/reports";
        }

        //For simplicity purposes, we include the identity as a part of this API
        //This could (or should...) be in a different server
        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string GetUser = Base + "/identity/users/{userId}";
            public const string Refresh = Base + "/identity/refresh";
        }
        
        public static class Chats
        {
            /// <summary>
            /// Endpoint to get a chat with specified chatId
            /// </summary>
            public const string GetChat = Base + "/chats/{chatId}";
            
            //For authenticated users: 
            
            /// <summary>
            /// Endpoint to start a chat with the specified userId
            /// </summary>
            public const string CreateUserChat = Base + "/chats/user/{userId}";
            
            /// <summary>
            /// Endpoint to get chat conversation with the specified userId
            /// </summary>
            public const string GetUserChat = Base + "/chats/user/{userId}";
            
            /// <summary>
            /// Endpoint to get all chats of logged in user
            /// </summary>
            public const string GetAllUserChats = Base + "/chats/user";
            
            /// <summary>
            /// Endpoint to delete a chat with specified userId
            /// </summary>
            public const string DeleteUserChat = Base + "/chats/user/{userId}";
        }

        public static class Messages
        {
            public const string Send = Base + "/chats/user/{userId}/messages/send";
            public const string Get = Base + "/chats/user/{userId}/messages/{messageId}";
            public const string GetAllMessages = Base + "/chats/user/{userId}/messages";
        }
    }
}