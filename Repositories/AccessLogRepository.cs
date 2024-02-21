using GD.RRHH.CheckList.AccessMigrations.EF;
using GD.RRHH.CheckList.AccessMigrations.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GD.RRHH.CheckList.AccessMigrations.Repositories;

public class AccessLogRepository
{
    private readonly AccessDbContext _context;
    static readonly string _connSQL = "server=192.168.7.242; database=dbAssistances; User id=EMY; Password=0545696s; TrustServerCertificate=true;";

    public AccessLogRepository(AccessDbContext context)
    {
        _context = context;
    }

    public async Task<ResultModel<List<AccessLog>>> GetAccessLogs()
    {
        ResultModel<List<AccessLog>> result = new() { Data = new List<AccessLog>() };
        try
        {
            int lastLogID = GetLastAccessLog();

            var lstAccessLogs = await _context.AccessLogs
                .Where(x => x.LogID > 0 && x.LogID > lastLogID && x.CardHolderID > 0)
                .Select(x => new { x.LogID, x.CardHolderID, x.LocalTime })
                .ToListAsync();

            foreach( var log in lstAccessLogs )
            {
                AccessLog model = new();
                model.LogID = log.LogID;
                model.CardHolderID = log.CardHolderID;
                model.LocalTime = log.LocalTime;

                result.Data.Add(model);
            }

            result.IsSuccess = true;
        }
        catch (Exception ex)
        {
            result.Data = null;
            result.IsSuccess = false;
            result.Message = ex.Message;
        }

        return result;
    }


    static int GetLastAccessLog()
    {
        int result = 0;
        using (var conn = new SqlConnection(_connSQL))
        {
            conn.Open();

            SqlCommand cmd;
            cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 LogID FROM tblChecks ORDER BY LogID DESC";
            
            using(SqlDataReader reader = cmd.ExecuteReader())
            {
                if(reader.Read())
                    result = Convert.ToInt32(reader["LogID"]);
            }

            conn.Close();
        }

        return result;
    }


    public async Task<ResultModel<bool>> InsertDataToSQL(List<AccessLog> accessLogs)
    {
        ResultModel<bool> result = new();
        try
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("LogID", typeof(int));
            dataTable.Columns.Add("CardHolderID", typeof(int));
            dataTable.Columns.Add("LocalTime", typeof(DateTime));

            foreach (var log in accessLogs)
            {
                DataRow row = dataTable.NewRow();
                row["ID"] = 0;
                row["LogID"] = Convert.ToInt32(log.LogID);
                row["CardHolderID"] = Convert.ToInt32(log.CardHolderID);
                row["LocalTime"] = Convert.ToDateTime(log.LocalTime);

                dataTable.Rows.Add(row);
            }

            using (var conn = new SqlConnection(_connSQL))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
                    {
                        try
                        {
                            bulkCopy.DestinationTableName = "tblChecks";
                            bulkCopy.WriteToServer(dataTable);
                            transaction.Commit();

                            result.IsSuccess = true;
                        }
                        catch(Exception ex)
                        {
                            transaction.Rollback();
                            result.IsSuccess = false;
                            result.Message = ex.Message;
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Data = false;
            result.Message = ex.Message;
        }

        return result;
    }

}
