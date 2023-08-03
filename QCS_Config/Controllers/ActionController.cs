using Globals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using PersianDate.Standard;
using QCS_Config.Models;
using System.Data.SqlClient;

namespace QCS_Config.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Produces("application/json")]
    public class ActionController : ControllerBase
    {
        [HttpGet]
        public  ActionResult<string> GetFirstReserveDate(int shiftId, int insuranceId)
        {

            var receptionCount = GlobalDB.ReceptionCount( shiftId, insuranceId);
            var date = DateTime.Now;
            while (GlobalDB.ReserveCount(date,shiftId,insuranceId) == receptionCount)
            {
                date=date.AddDays(1);
            }
            return date.ToFa();
        }
    }
}
