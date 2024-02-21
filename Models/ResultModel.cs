using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.RRHH.CheckList.AccessMigrations.Models;

public class ResultModel<T>
{
    public ResultModel()
    {
        Message = string.Empty;
    }
    public ResultModel(T data)
    {
        Data = data;
    }

    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public int ResultType { get; set; }
}
