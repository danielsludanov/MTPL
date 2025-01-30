using System.Windows;

namespace MTPL
{
    public partial class App : Application
    {
        public int CurrentUserID { get; set; }

        public void SetCurrentUserID (int ID)
        {
            CurrentUserID = ID;
        }
    }
}