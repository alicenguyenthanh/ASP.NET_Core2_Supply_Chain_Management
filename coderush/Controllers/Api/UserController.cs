using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using coderush.Models.AccountViewModels;
using coderush.Models.SyncfusionViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using coderush.Services;

namespace coderush.Controllers.Api
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IFunctional _functionalService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IFunctional functionalService,
                        UserManager<ApplicationUser> userManager,
                        RoleManager<IdentityRole> roleManager)
        {
            _functionalService = functionalService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult GetUser()
        {
            List<UserProfile> Items = new List<UserProfile>();
            Items = _functionalService.GetList<UserProfile>().ToList();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetByApplicationUserId([FromRoute]string id)
        {
            UserProfile userProfile = _functionalService.GetList<UserProfile>().Where(x => x.ApplicationUserId.Equals(id)).FirstOrDefault();
            List<UserProfile> Items = new List<UserProfile>();
            if (userProfile != null)
            {
                Items.Add(userProfile);
            }
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Insert([FromBody]CrudViewModel<UserProfile> payload)
        {
            UserProfile register = payload.value;
            if (register.Password.Equals(register.ConfirmPassword))
            {
                ApplicationUser user = new ApplicationUser() { Email = register.Email, UserName = register.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    register.Password = user.PasswordHash;
                    register.ConfirmPassword = user.PasswordHash;
                    register.ApplicationUserId = user.Id;
                    _functionalService.Insert<UserProfile>(register);
                }
                
            }
            return Ok(register);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<UserProfile> payload)
        {
            UserProfile profile = payload.value;
            _functionalService.Update<UserProfile>(profile);
            return Ok(profile);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword([FromBody]CrudViewModel<UserProfile> payload)
        {
            UserProfile profile = payload.value;
            if (profile.Password.Equals(profile.ConfirmPassword))
            {
                var user = await _userManager.FindByIdAsync(profile.ApplicationUserId);
                var resultRemove = await _userManager.RemovePasswordAsync(user);
                if (resultRemove.Succeeded)
                {
                    var resultAdd = await _userManager.AddPasswordAsync(user, profile.Password);
                }
                
            }
            profile = _functionalService.GetList<UserProfile>()
                .Where(x => x.ApplicationUserId.Equals(profile.ApplicationUserId))
                .FirstOrDefault();
            return Ok(profile);
        }
        
        [HttpPost("[action]")]
        public IActionResult ChangeRole([FromBody]CrudViewModel<UserProfile> payload)
        {
            UserProfile profile = payload.value;
            return Ok(profile);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Remove([FromBody]CrudViewModel<UserProfile> payload)
        {
            int key = Convert.ToInt32(payload.key);
            var userProfile = _functionalService.GetById<UserProfile>(key);
            if (userProfile != null)
            {
                var user = await _userManager.FindByIdAsync(userProfile.ApplicationUserId);
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    _functionalService.Delete<UserProfile>(userProfile);
                }
                
            }
            
            return Ok();

        }
        
        
    }
}