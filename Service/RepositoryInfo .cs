using System;

namespace Service
{
    public class RepositoryInfo
    {
        // שם הריפוזיטורי
        public string Name { get; set; }

        // שפות תכנות עיקריות
        public List<string> Languages { get; set; }


        // מספר כוכבים (stars)
        public int Stars { get; set; }

        // מספר Pull Requests פתוחים
        public int PullRequests { get; set; }

        // קישור לריפוזיטורי
        public string Link { get; set; }

        // תאריך הקומיט האחרון
        public string LastCommit { get; set; }
    }
}
