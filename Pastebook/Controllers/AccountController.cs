﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pastebook.Mapper;
using Pastebook.ViewModels;
using PastebookBusinessLogicLibrary;
using PastebookEntityLibrary;

namespace Pastebook.Controllers
{
    public class AccountController : Controller
    {
        PasswordManager passwordManager = new PasswordManager();
        UserDataAccess userDataAccess = new UserDataAccess();
        CountryDataAccess countryDataAccess = new CountryDataAccess();
        MapperManager mapperManager = new MapperManager();
        //
        // GET: /Account/
        public ActionResult Index()
        {
            Session["user_id"] = 1;
            Session["username"] = "jayquiambao";
            return View();
        }

        public ActionResult Register()
        {
            IEnumerable<SelectListItem> countryListItems;

            countryListItems = countryDataAccess.GetCountryList().Select(i => new SelectListItem()
            {
                Value = i.ID.ToString(),
                Text = i.COUNTRY
            });

            ViewBag.CountryList = countryListItems;
            return View();
        }

        [HttpPost]
        public ActionResult RegisterUser(USER model, string CONFIRM_PASSWORD)
        {
            if (model.PASSWORD != CONFIRM_PASSWORD)
            {
                ModelState.AddModelError("PASSWORD", "Password do not match");
            }
            if (userDataAccess.CheckUsername(model.USER_NAME))
            {
                ModelState.AddModelError("USER_NAME", "Username already exists.");
            }

            if (userDataAccess.CheckEmail(model.EMAIL_ADDRESS))
            {
                ModelState.AddModelError("EMAIL_ADDRESS", "Email Address already exists.");
            }

            if (ModelState.IsValid)
            {
                string salt = null;

                model.PASSWORD = passwordManager.GeneratePasswordHash(model.PASSWORD, out salt);
                model.SALT = salt;

                //userDataAccess.SaveUser(mapperManager.RegisterViewModelToUSER(model));

                return RedirectToAction("Register");
            }

            IEnumerable<SelectListItem> countryListItems;

            countryListItems = countryDataAccess.GetCountryList().Select(i => new SelectListItem()
            {
                Value = i.ID.ToString(),
                Text = i.COUNTRY
            });

            model.PASSWORD = null;
            ViewBag.CountryList = countryListItems;

            return View("Register", model);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            var user = new USER();

            if (ModelState.IsValid)
            {
                user = userDataAccess.GetUser(model.EmailAddress, null);
                if (user != null)
                {
                    if (passwordManager.IsPasswordMatch(model.Password, user.SALT, user.PASSWORD))
                    {
                        Session["user_id"] = user.ID;
                        Session["username"] = user.USER_NAME;

                        return RedirectToAction("Index", "Pastebook");
                    }
                }
            }

            ModelState.AddModelError("Username", "Username/Password is incorrect.");

            return View("Index", model);
        }

        public JsonResult CheckUsername(string username)
        {
            bool result = userDataAccess.CheckUsername(username);

            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckEmail(string email)
        {
            bool result = userDataAccess.CheckEmail(email);

            return Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }
    }
}