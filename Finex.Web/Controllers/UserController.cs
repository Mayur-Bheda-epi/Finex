using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finex.Buisness.IBuisnessHelper;
using Finex.Dto.Dtos;
using Finex.Utility;

namespace Finex.Web.Controllers
{
    public class UserController : Controller
    {
        private IUserHelper _iUserHelper;
        public UserController()
        {
            _iUserHelper = DependencyResolver.GetUserInstance();
        }

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View("Login");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LoginUser(FormCollection frm)
        {
            var userDto = new UserLoginDto
            {
                UserName = frm.GetValue("txtUserName").AttemptedValue,
                Password = frm.GetValue("txtPassword").AttemptedValue
            };
            var user = _iUserHelper.GetLogin(userDto);
            if (user != null && user.UserId > 0)
            {
                Session["UserId"] = user.UserId;
                Session["User"] = user;
                Session["UserTypeId"] = user.UserTypeId;
                if (user.UserTypeId == 3)
                    return RedirectToAction("GetClaimForInsuer", "Insurer");
                return RedirectToAction("Index", "Claims");
            }
            ViewBag.Incorrectlogin = "Please enter correct username and password";
            return View("Login");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        public ActionResult GetUsers()
        {
            return View("Index", _iUserHelper.GetUsers());
        }

        public ActionResult AddUser()
        {
            var userDto = new UserMasterDto();
            userDto.UserTypesDtos = _iUserHelper.GetUserTypes();
            return View("AddUser", userDto);
        }

        public ActionResult EditUser(string userId)
        {
            var usId = Decryption.Decrypt(userId, true);
            var userDto = _iUserHelper.GetUserByUserId(Convert.ToInt32(usId));
            userDto.UserTypesDtos = _iUserHelper.GetUserTypes();
            return View("EditUser", userDto);
        }

        public ActionResult UpdateUser(UpdateUserMasterDto userMasterDto)
        {
            _iUserHelper.UpdateUser(userMasterDto);
            return RedirectToAction("GetUsers");
        }

        public ActionResult SaveUser(UserMasterDto userMasterDto)
        {

            _iUserHelper.AddUser(userMasterDto);

            return RedirectToAction("GetUsers");
        }

        public ActionResult UserPost(string btnUserPost, FormCollection frm)
        {
            var chkUser = frm.GetValues("chkUser");
            var userId = new List<int>();
            switch (btnUserPost)
            {
                case "Delete":
                    if (chkUser != null)
                    {
                        for (var i = 0; i < chkUser.Length; i++)
                        {
                            userId.Add(Convert.ToInt32(chkUser[i]));
                        }

                    }
                    _iUserHelper.DeleteUser(userId, Convert.ToInt32(Session["UserId"]));
                    break;
                case "Edit":
                    if (chkUser != null)
                    {
                        return RedirectToAction("EditUser", new { userId = Encryption.Encrypt(chkUser[0], true) });
                    }

                    break;
            }

            return RedirectToAction("GetUsers");
        }
    }
}
