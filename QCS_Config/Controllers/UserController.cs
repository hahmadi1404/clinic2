using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System.Text;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Linq;
using Globals;
using Models;
using QCS_Config.Models;


namespace UserController
{
    [Authorize]
    [ApiController]
    [Route("user-management/[action]")]
    public class UserController : Controller
    {
        private int getUserId()
        {
            // if ((User.Claims.First(a => a.Type.ToLower().Contains("role")).Value).ToLower().Contains("admin"))
                return Convert.ToInt32(User.Claims.First(a => a.Type.ToLower().Contains("nameidentifier")).Value);
            // return -1;
            
        }

        //[HttpGet]
        private int GetUserID(string userName)
        {
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "GetUserID");
            return GlobalDB.GetUserID(userName);
        }

        //[HttpGet]
        private int GetPermissionID(string permission)
        {
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "GetPermissionID");
            return GlobalDB.GetPermissionID(permission);
        }

        [HttpGet]
        public ActionResult PermissionCheck(string permissionName)
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "PermissionCheck");
            return Json(User.Claims.First(a => a.Type.Contains("role")).Value.Split(',').Any(a => a == permissionName));
        }

        [HttpPost]
        public ActionResult AddUserWithPermissions(UserPermissionsModel permissionsModel)
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            string name = permissionsModel.Name, userName = permissionsModel.UserName, password = permissionsModel.password;
            List<string> permissions = permissionsModel.Permissions.ToList();

            string addResult = GlobalDB.AddUser(name, userName, password,getUserId(),permissionsModel.isAdmin);
            if (addResult == "OK") 
            {
                long UserID = GlobalDB.GetUserID(userName);
                foreach (var permission in permissions)
                {
                    int permissionID = GlobalDB.GetPermissionID(permission);
                    if (permissionID != -1)
                        if (setUserPermissions(UserID, permissionID) == false)
                            addResult = "Permission Error";
                }
            }
            return Json(addResult);
        }

        [HttpGet]
        public ActionResult AddUser(string name, string userName, string password,bool isAdmin =false )
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "AddUser");
            return Json(GlobalDB.AddUser(name, userName, password,getUserId(),isAdmin));
        }

        //[HttpGet]
        //public bool DeleteUser(long UserID)
        //{
        //    Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "DeleteUser");
        //    SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
        //    try
        //    {
        //        connectionSql.Open();
        //        var sqlCommand = connectionSql.CreateCommand();
        //        sqlCommand.CommandText = $"UPDATE User_t SET state = 1 where userID = @userID";
        //        sqlCommand.Parameters.AddWithValue("userID", UserID);
        //        sqlCommand.ExecuteNonQuery();
        //        connectionSql.Close();
        //        Log.Logger.Information("UserID:{UserID} , Action:{Action}, UserID:{UserID} Deleted ", getUserId(), "DeleteUser", UserID);
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "DeleteUser", e.Message);
        //        connectionSql.Close();
        //        return false;
        //    }
        //    return true;
        //}

        [HttpGet]
        public ActionResult DeleteUser(string userName)
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            long UserID = GetUserID(userName);
            if (UserID == 1)
            {
                return Unauthorized(Json("Permission Denied")); 
            }
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "DeleteUser");
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = $"UPDATE User_t SET state = 1 where userID = @userID";
                sqlCommand.Parameters.AddWithValue("userID", UserID);
                sqlCommand.ExecuteNonQuery();
                connectionSql.Close();
                // Log.Logger.Information("UserID:{UserID} , Action:{Action}, UserID:{UserID} Deleted ", getUserId(), "DeleteUser", UserID);
            }
            catch (Exception e)
            {
                // Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "DeleteUser", e.Message);
                connectionSql.Close();
                return Json(false);
            }
            return Json(true);
        }

        [HttpGet]
        public ActionResult UpdateUser(string name, string userName, bool isAdmin)
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            long userID = GetUserID(userName);
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "UpdateUser");
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                var hashFunction = new HMACSHA512();
                connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = $"UPDATE User_t SET name = @name, userName = @userName, isAdmin = @isAdmin where userID = @userID and state=0 and owner={getUserId()}";
                sqlCommand.Parameters.AddWithValue("userID", userID);
                sqlCommand.Parameters.AddWithValue("name", name);
                sqlCommand.Parameters.AddWithValue("userName", userName);
                sqlCommand.Parameters.AddWithValue("isAdmin", isAdmin);
                sqlCommand.ExecuteNonQuery();
                connectionSql.Close();
                // Log.Logger.Information("UserID:{UserID} , Action:{Action}, UserName:{UserName} Updated ", getUserId(), "UpdateUser", userName);
            }
            catch (Exception e)
            {
                // Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "UpdateUser", e.Message);
                connectionSql.Close();
                return Json(false);
            }
            return Json(true);
        }

        [HttpPost]
        public ActionResult UpdateUserWithPermissions(UserPermissionsModel updateModel)
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            long userID = GetUserID(updateModel.UserName);
            string name = updateModel.Name, userName = updateModel.UserName, password = updateModel.password;
            List<string> permissions = updateModel.Permissions.ToList();
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "UpdateUser");
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                var hashFunction = new HMACSHA512();
                connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                if (updateModel.password == null || updateModel.password == "")
                    sqlCommand.CommandText = $"UPDATE User_t SET name = @name where userName = @userName";
                else
                    sqlCommand.CommandText = $"UPDATE User_t SET name = @name, isAdmin = @isAdmin where userName = @userName and state=0 and owner={getUserId()}";
                sqlCommand.Parameters.AddWithValue("userID", userID);
                sqlCommand.Parameters.AddWithValue("name", name);
                sqlCommand.Parameters.AddWithValue("userName", userName);
                sqlCommand.Parameters.AddWithValue("isAdmin", updateModel.isAdmin);
                var o=sqlCommand.ExecuteNonQuery();
                sqlCommand.Parameters.Clear();
                if (o>0)
                {
                    
                // Log.Logger.Information("UserID:{UserID} , Action:{Action}, UserName:{UserName} Updated ", getUserId(), "UpdateUser", userName);

                sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = $"delete from UserPermission_t where UserID = @UserID and UserID={getUserId()}";
                sqlCommand.Parameters.AddWithValue("UserID", userID);
                sqlCommand.ExecuteNonQuery();
                // Log.Logger.Information("UserID:{UserID} , Action:{Action} permissions Deleted ", userID, "DeleteUserPermission");

                foreach (var permission in permissions)
                {
                    int permissionID = GlobalDB.GetPermissionID(permission);
                    if (permissionID != -1)
                        setUserPermissions(userID, permissionID);
                }
                }

                connectionSql.Close();
            }
            catch (Exception e)
            {
                // Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "UpdateUser", e.Message);
                connectionSql.Close();
                return Json(false);
            }
            return Json(true);
        }

        //[HttpGet]
        //public bool UpdatePassword(long userID, string password)
        //{
        //    Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "UpdatePassword");
        //    SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
        //    try
        //    {
        //        var hashFunction = new HMACSHA512();
        //        connectionSql.Open();
        //        var sqlCommand = connectionSql.CreateCommand();
        //        sqlCommand.CommandText = $"UPDATE User_t SET password = @password where userID = @userID";
        //        sqlCommand.Parameters.AddWithValue("userID", userID);
        //        sqlCommand.Parameters.AddWithValue("password", Convert.ToBase64String(hashFunction.ComputeHash(Encoding.ASCII.GetBytes(password))));
        //        sqlCommand.ExecuteNonQuery();
        //        connectionSql.Close();
        //        Log.Logger.Information("UserID:{UserID} , Action:{Action}, userID:{userID} password Updated ", getUserId(), "UpdatePassword", userID);
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "UpdatePassword", e.Message);
        //        connectionSql.Close();
        //        return false;
        //    }
        //    return true;
        //}

        [HttpGet]
        public ActionResult UpdatePassword(string userName, string password)
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            long userID = GetUserID(userName);
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "UpdatePassword");
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                var hashFunction = new HMACSHA512(Encoding.ASCII.GetBytes("my HashKey"));
                connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = $"UPDATE User_t SET password = @password where userID = @userID and state=0";
                sqlCommand.Parameters.AddWithValue("userID", userID);
                sqlCommand.Parameters.AddWithValue("password", string.Concat(hashFunction.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(b => b.ToString("X2")).ToArray()));
                sqlCommand.ExecuteNonQuery();
                connectionSql.Close();
                // Log.Logger.Information("UserID:{UserID} , Action:{Action}, userID:{userID} password Updated ", getUserId(), "UpdatePassword", userID);
            }
            catch (Exception e)
            {
                // Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "UpdatePassword", e.Message);
                connectionSql.Close();
                return Json(false);
            }
            return Json(true);
        }

        [HttpGet]
        public ActionResult getUsers()
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "getUsers");
            List<UserModel> users = new List<UserModel>();
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                // sqlCommand.CommandText = $"select UserID,Name,UserName,CreateTime,isAdmin,owner from User_t where state=0 ";
                sqlCommand.CommandText = $"select UserID,Name,UserName,CreateTime,isAdmin,owner from User_t ";
                var dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    users.Add(new UserModel
                    {
                        UserID = Convert.ToInt32(dr["UserID"]),
                        Name = Convert.ToString(dr["Name"]),
                        UserName = Convert.ToString(dr["UserName"]),
                        CreateTime = Convert.ToDateTime(dr["CreateTime"]),
                        isAdmin = Convert.ToBoolean(dr["isAdmin"]),
                        owner = Convert.ToInt32(dr["owner"])
                    });
                }
                connectionSql.Close();
                // Log.Logger.Information("UserID:{UserID} , Action:{Action} UserCount:{UserCount} ", getUserId(), "getUsers", users.Count);
            }
            catch (Exception e)
            {
                // Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "getUsers", e.Message);
                connectionSql.Close();
                return Conflict();
            }
            return Json(users);
        }

        [HttpGet]
        public ActionResult getUsersWithPermissions()
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "getUsers");
            List<UserPermissionModel> users = new List<UserPermissionModel>();
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                // sqlCommand.CommandText = $"SELECT  U.isAdmin,U.Name,U.UserName,U.CreateTime,U.Owner,P.Name as PermissionName FROM UserPermission_t as UP inner join Permission_t as P on p.PermissionID = up.PermissionID inner join User_t as U on UP.UserID = U.UserID where U.state!=1 and owner ={getUserId()} ";
                if (getUserId() == 1)
                {
                    sqlCommand.CommandText = $"SELECT distinct U.UserId ,U.isAdmin,U.Name,U.UserName,U.CreateTime,U.Owner,P.Name as PermissionName FROM UserPermission_t as UP inner join Permission_t as P on p.PermissionID = up.PermissionID inner join User_t as U on UP.UserID = U.UserID where U.state!=1 ";

                }
                else
                {
                    sqlCommand.CommandText = $"SELECT distinct U.UserId,U.isAdmin,U.Name,U.UserName,U.CreateTime,U.Owner,P.Name as PermissionName FROM UserPermission_t as UP inner join Permission_t as P on p.PermissionID = up.PermissionID inner join User_t as U on UP.UserID = U.UserID where U.state!=1 and owner ={getUserId()}";
                }
                var dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    users.Add(new UserPermissionModel
                    {
                        UserId = Convert.ToInt64(dr["UserId"]),
                        Name = Convert.ToString(dr["Name"]),
                        UserName = Convert.ToString(dr["UserName"]),
                        CreateTime = Convert.ToDateTime(dr["CreateTime"]),
                        Permission = Convert.ToString(dr["PermissionName"]),
                        isAdmin = Convert.ToBoolean(dr["isAdmin"]),
                        Owner = Convert.ToInt32(dr["Owner"])
                    });
                }
                connectionSql.Close();
                // Log.Logger.Information("UserID:{UserID} , Action:{Action} UserCount:{UserCount} ", getUserId(), "getUsers", users.Count);
            }
            catch (Exception e)
            {
                // Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "getUsers", e.Message);
                connectionSql.Close();
                return Conflict();
            }
            return Json(users);
        }

        //[HttpGet]
        //public ActionResult getUser(long userID)
        //{
        //    Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "getUsers");
        //    List<UserModel> users = new List<UserModel>();
        //    SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
        //    try
        //    {
        //        connectionSql.Open();
        //        var sqlCommand = connectionSql.CreateCommand();
        //        sqlCommand.CommandText = $"select UserID,Name,UserName,CreateTime from User_t where userID = @userID and state!=1";
        //        sqlCommand.Parameters.AddWithValue("userID", userID);
        //        var dr = sqlCommand.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            users.Add(new UserModel
        //            {
        //                UserID = Convert.ToInt64(dr["UserID"]),
        //                Name = Convert.ToString(dr["Name"]),
        //                UserName = Convert.ToString(dr["UserName"]),
        //                CreateTime = Convert.ToDateTime(dr["CreateTime"]),
        //            });
        //        }
        //        connectionSql.Close();
        //        Log.Logger.Information("UserID:{UserID} , Action:{Action} UserCount:{UserCount} ", getUserId(), "getUsers", users.Count);
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "getUsers", e.Message);
        //        connectionSql.Close();
        //        return Conflict();
        //    }
        //    return Json(users);
        //}

        [HttpGet]
        public ActionResult getUser(string userName)
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            long userID = GetUserID(userName);
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "getUsers");
            List<UserModel> users = new List<UserModel>();
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = $"select UserID,Name,UserName,CreateTime,isAdmin from User_t where userID = @userID and state!=1 and owner ={getUserId()}";
                sqlCommand.Parameters.AddWithValue("userID", userID);
                var dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    users.Add(new UserModel
                    {
                        //UserID = Convert.ToInt64(dr["UserID"]),
                        Name = Convert.ToString(dr["Name"]),
                        UserName = Convert.ToString(dr["UserName"]),
                        CreateTime = Convert.ToDateTime(dr["CreateTime"]),
                        isAdmin = Convert.ToBoolean(dr["isAdmin"])
                    });
                }
                connectionSql.Close();
                // Log.Logger.Information("UserID:{UserID} , Action:{Action} UserCount:{UserCount} ", getUserId(), "getUsers", users.Count);
            }
            catch (Exception e)
            {
                // Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "getUsers", e.Message);
                connectionSql.Close();
                return Conflict();
            }
            return Json(users);
        }

        [HttpGet]
        public ActionResult getUserById(int userId)
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "getUsers");
            List<UserModel> users = new List<UserModel>();
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = $"select UserID,Name,UserName,CreateTime,isAdmin,owner from User_t where userID = @userID and state!=1 and owner ={getUserId()}";
                sqlCommand.Parameters.AddWithValue("userID", userId);
                var dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    users.Add(new UserModel
                    {
                        //UserID = Convert.ToInt64(dr["UserID"]),
                        Name = Convert.ToString(dr["Name"]),
                        UserName = Convert.ToString(dr["UserName"]),
                        CreateTime = Convert.ToDateTime(dr["CreateTime"]),
                        isAdmin = Convert.ToBoolean(dr["isAdmin"]),
                        owner = Convert.ToInt32(dr["owner"])
                    });
                }
                connectionSql.Close();
                // Log.Logger.Information("UserID:{UserID} , Action:{Action} UserCount:{UserCount} ", getUserId(), "getUsers", users.Count);
            }
            catch (Exception e)
            {
                // Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "getUsers", e.Message);
                connectionSql.Close();
                return Conflict();
            }
            return Json(users);
        }
        
        //[HttpGet]
        //public ActionResult getUserWithPermissions(long userID)
        //{
        //    Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "getUsers");
        //    List<UserPermissionModel> users = new List<UserPermissionModel>();
        //    SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
        //    try
        //    {
        //        connectionSql.Open();
        //        var sqlCommand = connectionSql.CreateCommand();
        //        sqlCommand.CommandText = $"SELECT U.UserID,U.Name,U.UserName,U.CreateTime,P.Name as PermissionName FROM UserPermission_t as UP inner join Permission_t as P on p.PermissionID = up.PermissionID inner join User_t as U on UP.UserID = U.UserID where U.userID = @userID and U.state!=1";
        //        sqlCommand.Parameters.AddWithValue("userID", userID);
        //        var dr = sqlCommand.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            users.Add(new UserPermissionModel
        //            {
        //                UserID = Convert.ToInt64(dr["UserID"]),
        //                Name = Convert.ToString(dr["Name"]),
        //                UserName = Convert.ToString(dr["UserName"]),
        //                CreateTime = Convert.ToDateTime(dr["CreateTime"]),
        //                Permission = Convert.ToString(dr["PermissionName"])
        //            });
        //        }
        //        dr.Close();
        //        connectionSql.Close();
        //        Log.Logger.Information("UserID:{UserID} , Action:{Action} UserCount:{UserCount} ", getUserId(), "getUsers", users.Count);
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "getUsers", e.Message);
        //        connectionSql.Close();
        //        return Conflict();
        //    }
        //    return Json(users);
        //}

        [HttpGet]
        public ActionResult getUserWithPermissions(string userName)
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            long userID = GetUserID(userName);
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "getUsers");
            List<UserPermissionModel> users = new List<UserPermissionModel>();
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                if (getUserId() == 1)
                {
                    sqlCommand.CommandText = $"SELECT U.isAdmin,U.UserID,U.Name,U.UserName,U.CreateTime,P.Name as PermissionName FROM UserPermission_t as UP inner join Permission_t as P on p.PermissionID = up.PermissionID inner join User_t as U on UP.UserID = U.UserID where U.userID = @userID and U.state!=1 ";

                }
                else
                {
                    sqlCommand.CommandText = $"SELECT U.isAdmin,U.UserID,U.Name,U.UserName,U.CreateTime,P.Name as PermissionName FROM UserPermission_t as UP inner join Permission_t as P on p.PermissionID = up.PermissionID inner join User_t as U on UP.UserID = U.UserID where U.userID = @userID and U.state!=1 and owner ={getUserId()}";
                }
                sqlCommand.Parameters.AddWithValue("userID", userID);
                var dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    users.Add(new UserPermissionModel
                    {
                        //UserID = Convert.ToInt64(dr["UserID"]),
                        Name = Convert.ToString(dr["Name"]),
                        UserName = Convert.ToString(dr["UserName"]),
                        CreateTime = Convert.ToDateTime(dr["CreateTime"]),
                        Permission = Convert.ToString(dr["PermissionName"]),
                        isAdmin = Convert.ToBoolean(dr["isAdmin"])
                    });
                }
                dr.Close();
                connectionSql.Close();
                // Log.Logger.Information("UserID:{UserID} , Action:{Action} UserCount:{UserCount} ", getUserId(), "getUsers", users.Count);
            }
            catch (Exception e)
            {
                // Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "getUsers", e.Message);
                connectionSql.Close();
                return Conflict();
            }
            return Json(users);
        }

        [HttpGet]
        public ActionResult getPermissions()
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "getPermissions");
            List<PermissionModel> permissions = new List<PermissionModel>();
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = $"select PermissionID,Name,Content from Permission_t";
                var dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    permissions.Add(new PermissionModel
                    {
                        PermissionID = Convert.ToInt64(dr["PermissionID"]),
                        Name = Convert.ToString(dr["Name"]),
                        Content = Convert.ToString(dr["Content"]),
                    });
                }
                dr.Close();
                connectionSql.Close();
                // Log.Logger.Information("UserID:{UserID} , Action:{Action} PermissionCount:{PermissionCount} ", getUserId(), "getPermissions", permissions.Count);
            }
            catch (Exception e)
            {
                // Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "getPermissions", e.Message);
                connectionSql.Close();
                return Conflict();
            }
            return Json(permissions);
        }

        //[HttpGet]
        //public ActionResult getUserPermissions(long userID)
        //{
        //    Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "getUserPermissions");
        //    List<long> permissions = new List<long>();
        //    SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
        //    try
        //    {
        //        connectionSql.Open();
        //        var sqlCommand = connectionSql.CreateCommand();
        //        sqlCommand.CommandText = $"select PermissionID from UserPermission_t where userID = {userID}";
        //        var dr = sqlCommand.ExecuteReader();
        //        while (dr.Read())
        //        {
        //            permissions.Add(Convert.ToInt64(dr["PermissionID"]));
        //        }
        //        dr.Close();
        //        connectionSql.Close();
        //        Log.Logger.Information("UserID:{UserID} , Action:{Action} PermissionCount:{PermissionCount} ", getUserId(), "getUserPermissions", permissions.Count);
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "getUserPermissions", e.Message);
        //        connectionSql.Close();
        //        return Conflict();
        //    }
        //    return Json(permissions);
        //}

        [HttpGet]
        public ActionResult getUserPermissions(string userName)
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            long userID = GetUserID(userName);
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "getUserPermissions");
            List<long> permissions = new List<long>();
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = $"select PermissionID from UserPermission_t where userID = {userID}";
                var dr = sqlCommand.ExecuteReader();
                while (dr.Read())
                {
                    permissions.Add(Convert.ToInt64(dr["PermissionID"]));
                }
                dr.Close();
                connectionSql.Close();
                // Log.Logger.Information("UserID:{UserID} , Action:{Action} PermissionCount:{PermissionCount} ", getUserId(), "getUserPermissions", permissions.Count);
            }
            catch (Exception e)
            {
                // Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "getUserPermissions", e.Message);
                connectionSql.Close();
                return Conflict();
            }
            return Json(permissions);
        }

        //[HttpGet]
        private bool setUserPermissions(long userID, long permissionID)
        {
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "setUserPermissions");
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = $"insert into UserPermission_t (UserID,PermissionID) values ({userID},{permissionID})";
                var dr = sqlCommand.ExecuteNonQuery();
                connectionSql.Close();
                // Log.Logger.Information("UserID:{UserID} , Action:{Action} Permission Added ", getUserId(), "setUserPermissions");
            }
            catch (Exception e)
            {
                // Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "setUserPermissions", e.Message);
                connectionSql.Close();
                return false;
            }
            return true;
        }

        [HttpGet]
        public ActionResult setUserPermissions(string userName, string permissionName)
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            long userID = GetUserID(userName);
            long permissionID = GetPermissionID(permissionName);
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "setUserPermissions");
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = $"insert into UserPermission_t (UserID,PermissionID) values ({userID},{permissionID})";
                var dr = sqlCommand.ExecuteNonQuery();
                connectionSql.Close();
                // Log.Logger.Information("UserID:{UserID} , Action:{Action} Permission Added ", getUserId(), "setUserPermissions");
            }
            catch (Exception e)
            {
                // Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "setUserPermissions", e.Message);
                connectionSql.Close();
                return Json(false);
            }
            return Json(true);
        }

        //[HttpGet]
        //public bool DeleteUserPermission(long UserID, long PermissionID)
        //{
        //    Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "DeleteUserPermission");
        //    SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
        //    try
        //    {
        //        connectionSql.Open();
        //        var sqlCommand = connectionSql.CreateCommand();
        //        sqlCommand.CommandText = $"delete from UserPermission_t where UserID = @UserID and PermissionID=@PermissionID";
        //        sqlCommand.Parameters.AddWithValue("UserID", UserID);
        //        sqlCommand.Parameters.AddWithValue("PermissionID", PermissionID);
        //        sqlCommand.ExecuteNonQuery();
        //        connectionSql.Close();
        //        Log.Logger.Information("UserID:{UserID} , Action:{Action}, UserID:{UserID} , permissionID:{permissionID} Deleted ", getUserId(), "DeleteUserPermission", UserID, PermissionID);
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "DeleteUserPermission", e.Message);
        //        connectionSql.Close();
        //        return false;
        //    }
        //    return true;
        //}

        [HttpGet]
        public ActionResult DeleteUserPermission(string userName, string permissionName)
        {
            if (getUserId() == -1) return Unauthorized(Json("Access Denied"));
            long UserID = GetUserID(userName);
            long PermissionID = GetPermissionID(permissionName);
            // Log.Logger.Information("UserID:{UserID} , Action:{Action} ", getUserId(), "DeleteUserPermission");
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = $"delete from UserPermission_t where UserID = @UserID and PermissionID = @PermissionID";
                sqlCommand.Parameters.AddWithValue("UserID", UserID);
                sqlCommand.Parameters.AddWithValue("PermissionID", PermissionID);
                sqlCommand.ExecuteNonQuery();
                connectionSql.Close();
                // Log.Logger.Information("UserID:{UserID} , Action:{Action}, UserID:{UserID} , permissionID:{permissionID} Deleted ", getUserId(), "DeleteUserPermission", UserID, PermissionID);
            }
            catch (Exception e)
            {
                // Log.Logger.Error("UserID:{UserID} , Action:{Action} , Exception:{Exception}", getUserId(), "DeleteUserPermission", e.Message);
                connectionSql.Close();
                return Json(false);
            }
            return Json(true);
        }

        [HttpGet]
        public ActionResult GetProjectName()
        {
            return Json("QCS_Config");
        }
    }
}