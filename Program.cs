using GD.RRHH.CheckList.AccessMigrations.EF;
using GD.RRHH.CheckList.AccessMigrations.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GD.RRHH.CheckList.AccessMigrations;

public class Program
{
    private static async Task Main(string[] args)
    {
        // --------- Register Services en Dependencies
        InfoMessage("Cargando los Servicios...");
        var serviceProvider = BuildServiceProvider();
        var accessLogService = serviceProvider.GetRequiredService<AccessLogRepository>();
        SuccessMessage("Servicios cargados!");


        while (true)
        {
            // --------- GET Data from Access DB 
            InfoMessage("Obteniendo registros del checador... (NO CIERRE ESTA VENTANA)");
            var accessLogsResult = await accessLogService.GetAccessLogs();


            // --------- Go to next if result is success
            if (accessLogsResult.IsSuccess)
            {
                // -------- Insert new records to the SQL DB if any
                if (accessLogsResult.Data.Count > 0)
                {
                    SuccessMessage($"({accessLogsResult.Data.Count}) Datos Obtenidos!");

                    InfoMessage("Insertando a la Base de Datos... (NO CIERRE ESTA VENTANA)");

                    var insersionResult = await accessLogService.InsertDataToSQL(accessLogsResult.Data); // ---------- INSERT into SQL DB

                    // ---------- Insertion result (Success or Error)
                    if (insersionResult.IsSuccess)
                        SuccessMessage($"({accessLogsResult.Data.Count}) registros insertados!");
                    else
                        ErrorMessage("ERROR al insertar los datos:", $"{insersionResult.Message}");
                }
                else
                    InfoMessage("No hay registros nuevos !");
            }
            else
                ErrorMessage("ERROR al obtener los datos:", $"{accessLogsResult.Message}");


            //Repeat ... after 15 minutes
            InfoMessage("Esperando 15 minutos para la próxima ejecución... (NO CIERRE ESTA VENTANA)");
            await Task.Delay(TimeSpan.FromMinutes(15));

            // Display countdown for the next execution
            //InfoMessage($"Esperando 15 minutos para la próxima ejecución... (NO CIERRE ESTA VENTANA)");
            //for (int i = 14; i > 0; i--)
            //{
            //    for (int j = 60; j > 0; j--)
            //    {
            //        Console.WriteLine($"Timepo restante: {i} minutos y {j} segundos");
            //        await Task.Delay(TimeSpan.FromSeconds(1));
            //    }
            //}
        }
    }

    private static ServiceProvider BuildServiceProvider()
    {
        return new ServiceCollection()
            .AddDbContext<AccessDbContext>()
            .AddTransient<AccessLogRepository>()
            .BuildServiceProvider();
    }

    private static void InfoMessage(string infoMessage)
    {
        Console.WriteLine("\n");
        Console.WriteLine(infoMessage);
    }
    
    private static void SuccessMessage(string successMessage)
    {
        Console.WriteLine("\n");
        Console.WriteLine(successMessage);
    }
    
    private static void ErrorMessage(string messageTitle, string errorMessage)
    {
        Console.WriteLine("\n");
        Console.WriteLine(messageTitle);
        Console.WriteLine($"{errorMessage}");
    }
}