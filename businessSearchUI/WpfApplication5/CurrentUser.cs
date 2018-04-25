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
        private CurrentUser()
        {

        }
        public CurrentUser getCurrentUser()
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
        private CurrentBusiness()
        {

        }
        public CurrentBusiness getCurrentUser()
        {
            if (current == null)
            {
                current = new CurrentBusiness();
            }
            return current;
        }
    }

}
