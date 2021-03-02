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
        }

        //For simplicity purposes, we include the identity as a part of this API
        //This could be in a different server
        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
        }
    }
}