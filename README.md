# Registro y Consulta desde Checador Facial con Inserción SQL

## Descripción del Proyecto

Este repositorio alberga una aplicación especializada en la consulta de registros almacenados en una base de datos Access. Estos registros son recopilados mediante un sistema integrado de checador facial y lector de tarjetas. Posteriormente, la aplicación facilita la inserción de los datos consultados en una base de datos SQL, proporcionando una solución integral para la gestión eficiente de la información proveniente de estos dispositivos.

## Instrucciones de Configuración

1. Abre el archivo "AccessDbContext.cs" y actualiza las variables de conexión a la base de datos Access con tus credenciales y detalles de conexión.

```csharp
// AccessDbContext.cs

static readonly string _source = "Ubicacion_de_tu_archivo_.mbd";
static readonly string _provider = "Microsoft.ACE.OLEDB.12.0";
```

2. Accede al archivo "AccessLogRepository.cs" y modifica la cadena de conexión a la base de datos SQL con los detalles correspondientes.

```csharp
// AccessLogRepository.cs

static readonly string _connSQL = "tu_cadena_de_conexion_access";
```
3. Asegúrate de tener el SDK de .NET 7 instalado en tu sistema (Arquitectura 64-Bit)
4. Verifica que el Motor de Bases de Datos Ole DB 12 esté instalado.
