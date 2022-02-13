using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using algotest.api.Helpers;
using algotest.core.models;
using algotest.core.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using swg.mining.core;

namespace algotest.api.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly ILogger<TestController> logger;
        private readonly IStoreService storeService;

        public TestController(
            ILogger<TestController> logger,
            IStoreService storeService)
        {
            this.logger = logger;
            this.storeService = storeService;
        }

        private static string GenerateJwtToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "xxx")
                   }
                ),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        protected UserModel UserInfo()
        {
            UserModel ret = (UserModel)HttpContext.Items["User"];
            return ret;
        }

        [AllowAnonymous]
        [HttpPost("Login", Name = "Login")]
        public ResponseModel<string> Login(string email, string password)
        {
            var token = string.Empty;
            var ret = storeService.Login(email, password, out string oMessage);
            if (string.IsNullOrEmpty(oMessage))
                token = GenerateJwtToken(ret);
            return new ResponseModel<string>
            {
                IsSuccess = string.IsNullOrEmpty(oMessage),
                ReturnMessage = oMessage,
                Data = token,
            };
        }
        [AllowAnonymous]
        [HttpPost("Register", Name = "Register")]
        public ResponseModel<string> Register(UserAddModel data)
        {
            var ret = storeService.Register(data, out string oMessage);
            return new ResponseModel<string>
            {
                IsSuccess = string.IsNullOrEmpty(oMessage),
                ReturnMessage = oMessage,
                Data = ret
            };
        }

        [AuthorizeRoles]
        [HttpGet("GetInfoUser", Name = "GetInfoUser")]
        public ResponseModel<UserModel> GetInfoUser()
        {
            return new ResponseModel<UserModel>
            {
                IsSuccess = true,
                ReturnMessage = string.Empty,
                Data = UserInfo()
            };
        }

        #region MANAGE PRODUCT
        [AuthorizeRoles]
        [HttpGet("GetProducts", Name = "GetProducts")]
        public ResponseModel<List<ProductModel>> GetProducts()
        {
            var ret = storeService.GetProducts(out string oMessage);
            return new ResponseModel<List<ProductModel>>
            {
                IsSuccess = string.IsNullOrEmpty(oMessage),
                ReturnMessage = oMessage,
                Data = ret
            };
        }
        [AuthorizeRoles]
        [HttpGet("GetProduct", Name = "GetProduct")]
        public ResponseModel<ProductModel> GetProduct(int productId)
        {
            var ret = storeService.GetProduct(productId, out string oMessage);
            return new ResponseModel<ProductModel>
            {
                IsSuccess = string.IsNullOrEmpty(oMessage),
                ReturnMessage = oMessage,
                Data = ret
            };
        }
        [AuthorizeRoles]
        [HttpPost("AddProduct", Name = "AddProduct")]
        public ResponseModel<string> AddProduct(ProductAddModel data)
        {
            var ret = storeService.AddProduct(data, out string oMessage);
            return new ResponseModel<string>
            {
                IsSuccess = string.IsNullOrEmpty(oMessage),
                ReturnMessage = oMessage,
                Data = ret
            };
        }
        [AuthorizeRoles]
        [HttpPost("UpdateProduct", Name = "UpdateProduct")]
        public ResponseModel<string> UpdateProduct(ProductUpdateModel data)
        {
            var ret = storeService.UpdateProduct(data, out string oMessage);
            return new ResponseModel<string>
            {
                IsSuccess = string.IsNullOrEmpty(oMessage),
                ReturnMessage = oMessage,
                Data = ret
            };
        }
        [AuthorizeRoles]
        [HttpGet("DeleteProduct", Name = "DeleteProduct")]
        public ResponseModel<string> DeleteProduct(int productId)
        {
            var ret = storeService.DeleteProduct(productId, out string oMessage);
            return new ResponseModel<string>
            {
                IsSuccess = string.IsNullOrEmpty(oMessage),
                ReturnMessage = oMessage,
                Data = ret
            };
        }
        #endregion

        [AuthorizeRoles]
        [HttpPost("AddOrder", Name = "AddOrder")]
        public ResponseModel<string> AddOrder(OrderAddModel data)
        {
            var ret = storeService.AddOrder(UserInfo().Id, data, out string oMessage);
            return new ResponseModel<string>
            {
                IsSuccess = string.IsNullOrEmpty(oMessage),
                ReturnMessage = oMessage,
                Data = ret
            };
        }
    }
}
