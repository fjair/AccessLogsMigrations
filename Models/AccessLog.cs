using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.RRHH.CheckList.AccessMigrations.Models;

public class AccessLog
{
    [Key]
    public int LogID { get; set; }
    public int CardHolderID { get; set; }
    public DateTime LocalTime { get; set; }
}
