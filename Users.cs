using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BusinessUser
    {
        private int businessID;
        private String password;

    }


public class RegisteredUser
    {
        private String userID;
        private String name;
        private List<String> friendsList;
        private List<String> reviewsList;
        private double stars;
        private String password;

        public void AddFriend(String name)
        {
            friendsList.Add(name);
        }

        public void RemoveFriend(String name)
        {
            friendsList.Remove(name);
        }
        

    }

class UserReview
    {
        private string UserID;
        private string BusinessID;
        private int Likes;
        public string ReviewText{get;}
    }