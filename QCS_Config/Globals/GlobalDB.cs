using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using QCS_Config.Models;
using PersianDate.Standard;

namespace Globals
{
    public static class GlobalDB
    {
        //هر شیف و بیمه چند تا تو تاریخ خاصی داره
        public static int ReserveCount(DateTime date, int shiftId, int insuranceId)
        {
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            connectionSql.Open();
            var sqlCommand = connectionSql.CreateCommand();
            sqlCommand.CommandText = $"select count(*) from Reserve where Reserve_Date_Persian='{date.ToFa()}' and ShiftID=@shiftId and InsuranceId=@insuranceId ";
            sqlCommand.Parameters.AddWithValue("shiftId", shiftId);
            sqlCommand.Parameters.AddWithValue("insuranceId", insuranceId);
            int count = Convert.ToInt32(sqlCommand.ExecuteScalar());
            return count;
        }
        //هر شیفت و بیمه چند تا می تونه رزرو داشته باشه
        public static int  ReceptionCount(int shiftId, int insuranceId)
        {
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            connectionSql.Open();
            var sqlCommand = connectionSql.CreateCommand();
            sqlCommand.CommandText = $"select count from ReceptionCount where shiftId=@shiftId and insuranceId=@insuranceId ";
            sqlCommand.Parameters.AddWithValue("shiftId", shiftId);
            sqlCommand.Parameters.AddWithValue("insuranceId", insuranceId);
            int count =Convert.ToInt32( sqlCommand.ExecuteScalar());
            return count;
        }

        // public static string SQLConnectionString;

        public static Exception CheckSQLConnection()
        {
            try
            {
                SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
                connectionSql.Open();
                connectionSql.Close();
            }
            catch(Exception ex)
            {
                return ex;
            }
            return null;
        }
        public static void CreateTables()
        {
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            connectionSql.Open();
            var sqlCommand = connectionSql.CreateCommand();
            sqlCommand.CommandText = "SELECT name FROM sys.objects WHERE type in (N'U')";
            var vr = sqlCommand.ExecuteReader();
            List<string> all_tables_sql = new List<string>();
            while (vr.Read())
            {
                all_tables_sql.Add(vr[0].ToString().ToLower());
            }
            vr.Close();
            connectionSql.Close();

            if (all_tables_sql.Contains("Result_t".ToLower()) == false)
            {
                connectionSql.Open();
                sqlCommand = connectionSql.CreateCommand();
                
                sqlCommand.CommandText = "CREATE TABLE Result_t (ID BIGINT NOT NULL IDENTITY" +
                    ",ResultData NVARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS" +
                    ",StartProcessTime DATETIME" +
                    ",EndProcessTime DATETIME" +
                    ",Type TINYINT" +
                    ",Status TINYINT" +
                    ",Description NVARCHAR(2000) COLLATE SQL_Latin1_General_CP1_CI_AS" +
                    ",CONSTRAINT PK_Result_t PRIMARY KEY (ID));";
                // Log.Logger.Information("Action:{Action} , TableName:{TableName}", "Create Table", "Result_t");
                sqlCommand.ExecuteNonQuery();
                connectionSql.Close();
            }
            if (all_tables_sql.Contains("Request_t".ToLower()) == false)
            {
                connectionSql.Open();
                sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = "CREATE TABLE Request_t (Id BIGINT NOT NULL IDENTITY" +
                    ",UserId VARCHAR(200)" +
                    ",Name NVARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL" +
                    ",RequestData NVARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS" +
                    ",StartDateTime DATETIME" +
                    ",EndDateTime DATETIME" +
                    ",Type TINYINT DEFAULT 1 NOT NULL" +
                    ",State TINYINT" +
                    ",ResultID BIGINT" +
                    ",CreateDateTime DATETIME DEFAULT GETDATE()" +
                    ",Status TINYINT" +
                    ",ProjectName NVARCHAR(1000) COLLATE SQL_Latin1_General_CP1_CI_AS" +
                    ",Progress TINYINT" +
                    ",Description NVARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS" +
                    ",CONSTRAINT PK_Request_t PRIMARY KEY (Id)" + 
                    ",CONSTRAINT FK_Request_t_Result_t FOREIGN KEY (ResultID) REFERENCES \"Result_t\" (\"ID\"))";
                // Log.Logger.Information("Action:{Action} , TableName:{TableName}", "Create Table", "Request_t");
                sqlCommand.ExecuteNonQuery();
                connectionSql.Close();
            }
            if (all_tables_sql.Contains("Permission_t".ToLower()) == false)
            {
                connectionSql.Open();
                sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = "CREATE TABLE Permission_t (PermissionID INT NOT NULL IDENTITY" +
                    ",Name NVARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL" +
                    ",Content VARCHAR(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL" +
                    ",PRIMARY KEY (PermissionID))";
                sqlCommand.ExecuteNonQuery();
                connectionSql.Close();
                AddPermission();
            }
            if (all_tables_sql.Contains("User_t".ToLower()) == false)
            {
                connectionSql.Open();
                sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = "CREATE TABLE User_t (UserID INT NOT NULL IDENTITY" +
                    ",Name NVARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL" +
                    ",UserName NVARCHAR(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL" +
                    ",Password VARCHAR(200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL" +
                    ",CreateTime DATETIME" +
                    ",State BIT"+
                    ",isAdmin BIT"+
                    ",Owner INT DEFAULT 1 NOT NULL"+
                    ",PRIMARY KEY (UserID))";
                sqlCommand.ExecuteNonQuery();
                connectionSql.Close();

                System.Threading.Thread.Sleep(1000);
                AddUser("admin", "admin", "admin",0,true); //HARDCODE: Add Default admin User
            }
            if (all_tables_sql.Contains("UserPermission_t".ToLower()) == false)
            {
                connectionSql.Open();
                sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = "CREATE TABLE UserPermission_t (UserPermissionID INT NOT NULL IDENTITY" +
                    ",UserID INT NOT NULL" +
                    ",PermissionID INT NOT NULL" +
                    ",PRIMARY KEY (UserPermissionID))";
                sqlCommand.ExecuteNonQuery();
                connectionSql.Close();

                AddUserPermission();
            }
        }

        public static void AddPermission()
        {
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            connectionSql.Open();
            var sqlCommand = connectionSql.CreateCommand();
            sqlCommand.CommandText = "insert into Permission_t (Name,Content) values ('View','Kavosh.CommonRegion.View')";
            sqlCommand.ExecuteNonQuery();
            connectionSql.Close();
        }

        public static void AddUserPermission()
        {
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            connectionSql.Open();
            var sqlCommand = connectionSql.CreateCommand();
            sqlCommand.CommandText = "insert into UserPermission_t (UserID,PermissionID) values (1,1)";
            sqlCommand.ExecuteNonQuery();
            connectionSql.Close();
        }

        public static string AddUser(string name, string userName, string password ,int owner ,bool isAdmin=false )
        {
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                var sqlCommand1 = connectionSql.CreateCommand();
                connectionSql.Open();
                sqlCommand1.CommandText = $"select count(*) from User_t where UserName='{userName}' and state=0";
                if (Convert.ToInt32(sqlCommand1.ExecuteScalar()) > 0)
                {
                    connectionSql.Close();
                    return "user exist";
                }
                //connectionSql.Close();

                var hashFunction = new HMACSHA512(Encoding.ASCII.GetBytes("my HashKey"));
                //connectionSql.Open();
                var sqlCommand = connectionSql.CreateCommand();
                sqlCommand.CommandText = $"insert into User_t (Name,UserName,Password,isAdmin,owner,CreateTime,state) values (@name,@userName,@password,@isAdmin,@owner,@CreateTime,0)";
                sqlCommand.Parameters.AddWithValue("name", name);
                sqlCommand.Parameters.AddWithValue("isAdmin", isAdmin);
                sqlCommand.Parameters.AddWithValue("userName", userName);
                sqlCommand.Parameters.AddWithValue("owner", owner);
                sqlCommand.Parameters.AddWithValue("password", string.Concat(hashFunction.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(b => b.ToString("X2")).ToArray()));
                sqlCommand.Parameters.AddWithValue("CreateTime", DateTime.Now);
                sqlCommand.ExecuteNonQuery();
                connectionSql.Close();
            }
            catch (Exception e)
            {
                connectionSql.Close();
                return "DB Error";
            }

            return "OK";
        }

        public static int GetUserID(string userName)
        {
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                var sqlCommand1 = connectionSql.CreateCommand();
                connectionSql.Open();
                sqlCommand1.CommandText = $"select max(UserID) from User_t where UserName='{userName}' and state=0";
                int userID = Convert.ToInt32(sqlCommand1.ExecuteScalar());
                connectionSql.Close();
                return userID;
            }
            catch (Exception e)
            {
                connectionSql.Close();
            }
            return -1;
        }

        public static int GetPermissionID(string permission)
        {
            SqlConnection connectionSql = new SqlConnection(Config.All.sqlConnectionString);
            try
            {
                var sqlCommand1 = connectionSql.CreateCommand();
                connectionSql.Open();
                sqlCommand1.CommandText = $"select max(PermissionID) from Permission_t where Content='{permission}'";
                int permissionID = Convert.ToInt32(sqlCommand1.ExecuteScalar());
                connectionSql.Close();
                return permissionID;
            }
            catch (Exception e)
            {
                connectionSql.Close();
            }
            return -1;
        }
    }
}