using LeopardWebService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography;
using Globals;
using Newtonsoft.Json;
using QCS_Config;
using QCS_Config.Models;

namespace LeopardWebService.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthenticateController : Controller
    {
      [HttpPost]
        public ActionResult Authenticate(string nationalCode, string phone)
        {
            var sqlState = GlobalDB.CheckSQLConnection();
            if (sqlState != null) return Problem(sqlState.Message, null, 590, "Connection Error", "DB");
            //var hashFunction = new HMACSHA512(Encoding.ASCII.GetBytes("my HashKey"));
            UsersModel user = new UsersModel();
            user.Username = phone;
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            connectionSql.Open();
            var sqlCommand = connectionSql.CreateCommand();
            sqlCommand.CommandText = $"Select ID,Name,LastName,DosierID,InsuranceID,Gender from Patients where PhoneNumber=@PhoneNumber and NationalCode=@NationalCode ";
            sqlCommand.Parameters.AddWithValue("PhoneNumber",phone );
            sqlCommand.Parameters.AddWithValue("NationalCode", nationalCode);
            var dr = sqlCommand.ExecuteReader();
            Log.Logger.Information("Action:{Action} GetData from Patients", "Authenticate");
            var patient = new Patient();
            while (dr.Read())
            {
                user.Id = Convert.ToInt32(dr["ID"]);
                user.Name = Convert.ToString(dr["Name"]) + " "+ Convert.ToString(dr["LastName"]);
                patient.InsuranceId = Convert.ToInt32(dr["InsuranceID"]);
                patient.Gender = Convert.ToInt32(dr["Gender"]);
                patient.DosierId = Convert.ToString(dr["DosierID"]);
                patient.Name = user.Name;
                patient.NationalCode = nationalCode;
                patient.PhoneNumber = phone;
                patient.Id = user.Id;
                user.Comment = JsonConvert.SerializeObject(patient);
                user.Permission = "1";
                //user.Permission = dr["ClinicId"].ToString();
                Log.Logger.Information("Action:{Action} , Name:{Name}", "login", user.Name);
            }
            dr.Close();
            connectionSql.Close();
            if (user.Name is null) return Unauthorized(Json("Invalid PhoneNumber or NationalCode"));

          
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Clinic auth111111111111111111111");
            var Subject = new ClaimsIdentity(new Claim[]
               {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Permission),
                    new Claim(ClaimTypes.UserData,user.Comment ),
               });
            var SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = Subject,
                Expires = DateTime.UtcNow.Add(new TimeSpan(20, 00, 10)),
                SigningCredentials = SigningCredentials,

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenString = tokenHandler.WriteToken(token);
            user.Pass = tokenString;
          

            return Json(user);
        }
        [HttpPost]
        public ActionResult adminAuthenticate(AuthenticateModel model)
        {
            var sqlState = GlobalDB.CheckSQLConnection();
            if (sqlState != null) return Problem(sqlState.Message, null, 590, "Connection Error", "DB");
            //var hashFunction = new HMACSHA512(Encoding.ASCII.GetBytes("my HashKey"));
            UsersModel user = new UsersModel();
            user.Username = model.Username;
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            connectionSql.Open();
            var sqlCommand = connectionSql.CreateCommand();
            sqlCommand.CommandText = $"Select UserID,Name,ClinicId from user_t where UserName=@Username and Password=@Password and Status = 0";
            sqlCommand.Parameters.AddWithValue("Username", model.Username);
            sqlCommand.Parameters.AddWithValue("Password", model.Password);
            var dr = sqlCommand.ExecuteReader();
            Log.Logger.Information("Action:{Action} GetData from user_t", "Authenticate");
            while (dr.Read())
            {
                user.Id = Convert.ToInt32(dr["UserID"]);
                user.Name = Convert.ToString(dr["Name"]);
                user.Permission = dr["ClinicId"].ToString();
                Log.Logger.Information("Action:{Action} , Name:{Name}", "login", user.Name);
            }
            dr.Close();
            connectionSql.Close();
            if (user.Name is null) return Unauthorized(Json("Invalid UserName or Password"));

          
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Clinic auth");
            var Subject = new ClaimsIdentity(new Claim[]
               {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Permission),
                    new Claim(ClaimTypes.UserData,user.State.ToString() ),
               });
            var SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = Subject,
                Expires = DateTime.UtcNow.Add(new TimeSpan(20, 00, 10)),
                SigningCredentials = SigningCredentials,

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenString = tokenHandler.WriteToken(token);
            user.Pass = tokenString;
          

            return Json(user);
        }


        [Authorize]
        [HttpGet]
        public bool AuthenticateCheck()
        {
            return true;
        }
    }
}
