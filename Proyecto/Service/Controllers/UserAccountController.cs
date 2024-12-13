using BLL;
using Entities;
using SLC;
using System.Collections.Generic;
using System.Web.Http;

namespace Service.Controllers
{
    [RoutePrefix("useraccount")]
    public class UserAccountController : ApiController, IUserAccount
    {
        private readonly UserAccountLogic _logic;

        public UserAccountController()
        {
            _logic = new UserAccountLogic();
        }

        [HttpPost]
        [Route("create")]
        public UserAccount CreateUserAccount(UserAccount newUserAccount)
        {
            return _logic.CreateUserAccount(newUserAccount);
        }

        [HttpGet]
        [Route("delete/{id}")]
        public bool DeleteUserAccount(int id)
        {
            return _logic.DeleteUserAccount(id);
        }

        [HttpGet]
        [Route("get/{id}")]
        public UserAccount GetUserAccountByID(int id)
        {
            return _logic.GetUserAccountByID(id);
        }

        [HttpPost]
        [Route("update")]
        public bool UpdateUserAccount(UserAccount userAccountToUpdate)
        {
            return _logic.UpdateUserAccount(userAccountToUpdate);
        }

        [HttpGet]
        [Route("all")]
        public List<UserAccount> GetAllUserAccounts()
        {
            return _logic.GetAllUserAccounts();
        }

        [HttpGet]
        [Route("email")]
        public UserAccount GetUserByEmail([FromUri] string email)
        {
            return _logic.GetUserByEmail(email);
        }
    }
}