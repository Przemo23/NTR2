using System;


namespace NTR02
{
    public static class NotebookControl
    {
        public static string filterCat;
        public static DateTime fromDate;
        public static DateTime toDate;
        public static int page;
        public static int filteredNotesCount;
    
        static NotebookControl()
        {
            SetDefaults();
            filteredNotesCount = 0;
        }
        public static void SetDefaults()
        {
            fromDate = DateTime.Now.Date;
            toDate = DateTime.Now.AddYears(5);
            filterCat = null;
            page = 1;
        }
    }
    
}