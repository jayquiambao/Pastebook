﻿using PastebookEntityLibrary;
using System.Collections.Generic;
using System.Linq;

namespace PastebookDataAccessLibrary
{
    public class PostDataAccess
    {
        public List<POST> GetUserTimeline(string username)
        {
            var listOfPosts = new List<POST>();

            using (var context = new PastebookEntities())
            {
                listOfPosts = context.POSTs.Include("LIKEs").Include("COMMENTs").Include("USER").Include("COMMENTs.USER").Include("LIKEs.USER").Where(p => p.USER1.USER_NAME == username).OrderByDescending(d => d.CREATED_DATE).Take(100).ToList();
            }

            return listOfPosts;
        }

        public List<POST> GetUserNewsFeed(List<int> listOfFriendsID)
        {
            var listOfPosts = new List<POST>();

            using (var context = new PastebookEntities())
            {
                listOfPosts = context.POSTs.Include("LIKEs").Include("COMMENTs").Include("USER").Include("USER1").Include("COMMENTs.USER").Include("LIKEs.USER").Where(p => listOfFriendsID.Contains(p.POSTER_ID)).OrderByDescending(d => d.CREATED_DATE).ToList();
            }

            return listOfPosts;
        }

        public int GetProfileOwnerID(int postID)
        {
            POST post = new POST();

            using (var context = new PastebookEntities())
            {
                post = context.POSTs.FirstOrDefault(p => p.ID == postID);
            }

            return post.POSTER_ID;
        }

        public POST GetPost(int postID)
        {
            POST post = new POST();

            using (var context = new PastebookEntities())
            {
                post = context.POSTs.Include("LIKEs").Include("COMMENTs").Include("USER").Include("USER1").Include("COMMENTs.USER").Include("LIKEs.USER").FirstOrDefault(p => p.ID == postID);
            }

            return post;
        }
    }
}
