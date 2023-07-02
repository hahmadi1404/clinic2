using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeopardWebService.Models
{
    public class UsersModel

    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public bool? State { get; set; }
        public string Permission { get; set; }
        public string Comment { get; set; }
    }
}