﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TFSPraise.Abstract;
using TFSPraise.Models;
using TFSPraise.Entities;
using TFSPraise.Concrete;
using Newtonsoft.Json;

namespace TFSPraise.Controllers
{
    public class LikeController : Controller
    {
        private RepositoryBase<Like> likeRepo;
        private RepositoryBase<UserProfile> userRepo;
        readonly int PageSize = 4;

        public LikeController(RepositoryBase<Like> _likeRepo, RepositoryBase<UserProfile> _userRepo)
        {
            likeRepo = _likeRepo;
            userRepo = _userRepo;
        }

        public ActionResult LikeList(int page = 1)
        {
            ListViewModel<Like> praiseListViewModel = new ListViewModel<Like>
            {
                List = likeRepo.GetAll().Skip(PageSize * (page - 1)).Take(PageSize).ToList(),
                PageInfo = new PageInfo
                {
                    TotalItems = likeRepo.GetAll().ToList().Count,
                    CurrentPage = page,
                    ItemsPerPage = PageSize
                }
            };
            return View(praiseListViewModel);
        }

        public ActionResult LikeRanking()
        {
            var usersOrdered = userRepo.GetAll().OrderByDescending(u=>u.Likes.Count).ToList();
            ViewBag.CurrentUserName = ((UserRepository)userRepo).GetCurrentUser().Identity.DispalyName;

            return PartialView(usersOrdered);
        }

        public ActionResult LikeChart()
        {
            string[] months = new string[] {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var likeGroups = likeRepo.GetAll().GroupBy(l => l.Date.Month);
            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach (var group in likeGroups)
            {
                dataPoints.Add(new DataPoint(group.Key, group.Count(), months[group.Key - 1]));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return PartialView();
        }

        [ChildActionOnly]
        public int LikesWarning()
        {
            return likeRepo.GetAll().ToList().Count;
        }
    }
}