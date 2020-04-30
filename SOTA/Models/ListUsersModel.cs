using Microsoft.EntityFrameworkCore;
using SOTA.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SOTA.Models
{
    public class ListMo
    {
        public List<Users> ListUsersMo { get; set; }

    }
}