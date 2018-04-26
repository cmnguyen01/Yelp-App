using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication5
{
    class CurrentUser
    {
        private static CurrentUser current;
        public string UserID { get; set; }
        public string UserName { get; set; }
        private CurrentUser()
        {

        }
        public static CurrentUser getCurrentUser()
        {
            if(current == null)
            {
                current = new CurrentUser();
            }
            return current;
        }
    }
    class CurrentBusiness
    {
        private static CurrentBusiness current;
        public string UserID { get; set; }
        public string BusinessName { get; set; }
        private CurrentBusiness()
        {

        }
        public static CurrentBusiness getCurrentUser()
        {
            if (current == null)
            {
                current = new CurrentBusiness();
            }
            return current;
        }
    }

}
