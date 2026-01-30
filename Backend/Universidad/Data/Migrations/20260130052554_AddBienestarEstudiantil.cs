using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Universidad.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBienestarEstudiantil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProyectosExtension",
                table: "ProyectosExtension");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActividadesExtension",
                table: "ActividadesExtension");

            migrationBuilder.DropColumn(
                name: "AreaProyectoExtension",
                table: "ProyectosExtension");

            migrationBuilder.DropColumn(
                name: "CodigoProyectoExtension",
                table: "ProyectosExtension");

            migrationBuilder.DropColumn(
                name: "DescripcionProyectoExtension",
                table: "ProyectosExtension");

            migrationBuilder.DropColumn(
                name: "EstadoProyectoExtension",
                table: "ProyectosExtension");

            migrationBuilder.DropColumn(
                name: "FechaFinProyectoExtension",
                table: "ProyectosExtension");

            migrationBuilder.DropColumn(
                name: "NombreProyectoExtension",
                table: "ProyectosExtension");

            migrationBuilder.DropColumn(
                name: "ObservacionesProyectoExtension",
                table: "ProyectosExtension");

            migrationBuilder.DropColumn(
                name: "ResponsableCIProyectoExtension",
                table: "ProyectosExtension");

            migrationBuilder.DropColumn(
                name: "UbicacionProyectoExtension",
                table: "ProyectosExtension");

            migrationBuilder.DropColumn(
                name: "IdRegistroActividadExtension",
                table: "ActividadesExtension");

            migrationBuilder.DropColumn(
                name: "CodigoActividadExtension",
                table: "ActividadesExtension");

            migrationBuilder.DropColumn(
                name: "DescripcionActividadExtension",
                table: "ActividadesExtension");

            migrationBuilder.DropColumn(
                name: "EstadoActividadExtension",
                table: "ActividadesExtension");

            migrationBuilder.DropColumn(
                name: "HoraFinActividadExtension",
                table: "ActividadesExtension");

            migrationBuilder.DropColumn(
                name: "HoraInicioActividadExtension",
                table: "ActividadesExtension");

            migrationBuilder.DropColumn(
                name: "LugarActividadExtension",
                table: "ActividadesExtension");

            migrationBuilder.DropColumn(
                name: "MaterialesActividadExtension",
                table: "ActividadesExtension");

            migrationBuilder.DropColumn(
                name: "NombreActividadExtension",
                table: "ActividadesExtension");

            migrationBuilder.DropColumn(
                name: "NumeroParticipantesActividadExtension",
                table: "ActividadesExtension");

            migrationBuilder.DropColumn(
                name: "ObservacionesActividadExtension",
                table: "ActividadesExtension");

            migrationBuilder.DropColumn(
                name: "TipoActividadExtension",
                table: "ActividadesExtension");

            migrationBuilder.RenameTable(
                name: "ProyectosExtension",
                newName: "ProyectoExtensions");

            migrationBuilder.RenameTable(
                name: "ActividadesExtension",
                newName: "ActividadExtensions");

            migrationBuilder.RenameColumn(
                name: "PresupuestoProyectoExtension",
                table: "ProyectoExtensions",
                newName: "Presupuesto");

            migrationBuilder.RenameColumn(
                name: "FechaRegistroProyectoExtension",
                table: "ProyectoExtensions",
                newName: "FechaInicio");

            migrationBuilder.RenameColumn(
                name: "FechaInicioProyectoExtension",
                table: "ProyectoExtensions",
                newName: "FechaCreacion");

            migrationBuilder.RenameColumn(
                name: "IdRegistroProyectoExtension",
                table: "ProyectoExtensions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ProyectoIdActividadExtension",
                table: "ActividadExtensions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FechaActividadExtension",
                table: "ActividadExtensions",
                newName: "FechaCreacion");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UltimoAccesoUsuarioExtuniv",
                table: "UsuariosExtuniv",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacionUsuarioExtuniv",
                table: "UsuariosExtuniv",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaUltimoAcceso",
                table: "UsuariosBecas",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "UsuariosBecas",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "expiracion",
                table: "usuarioCIITs",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechasolicitud",
                table: "SolicitudProduccions",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechalimite",
                table: "SolicitudProduccions",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaSolicitud",
                table: "SolicitudesBecas",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaRespuesta",
                table: "SolicitudesBecas",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                table: "Solicitudes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "Profesores",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldDefaultValueSql: "CURRENT_DATE");

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Profesores",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechapublicacion",
                table: "ProduccionAudiovisuals",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechainicio",
                table: "ProduccionAudiovisuals",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechaprestamo",
                table: "Prestamos",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechadevolucion",
                table: "Prestamos",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaEncontrado",
                table: "ObjetosPerdidos",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechacambio",
                table: "HistorialCambioses",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fecha_emision",
                table: "Facturas",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaContabilidad",
                table: "ContabilidadReportesIngresos",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaContabilidad",
                table: "ContabilidadPeticionDepartamentos",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacionUsuarioContabilidad",
                table: "ContabilidadLoginOauth",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechacancelacion",
                table: "Cancelaciones",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "Becas",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaInicio",
                table: "ActividadesDepa",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaFin",
                table: "ActividadesDepa",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "Beneficiarios",
                table: "ProyectoExtensions",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoProyecto",
                table: "ProyectoExtensions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "ProyectoExtensions",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "ProyectoExtensions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaFin",
                table: "ProyectoExtensions",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Responsable",
                table: "ProyectoExtensions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "ProyectoExtensions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ActividadExtensions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "CodigoActividad",
                table: "ActividadExtensions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "ActividadExtensions",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "ActividadExtensions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaActividad",
                table: "ActividadExtensions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Lugar",
                table: "ActividadExtensions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Participantes",
                table: "ActividadExtensions",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Responsable",
                table: "ActividadExtensions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoActividad",
                table: "ActividadExtensions",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "ActividadExtensions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProyectoExtensions",
                table: "ProyectoExtensions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActividadExtensions",
                table: "ActividadExtensions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ActividadBienestars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoActividad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TipoActividad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FechaActividad = table.Column<DateOnly>(type: "date", nullable: false),
                    HoraInicio = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HoraFin = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Lugar = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UsuarioBienestarId = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CapacidadMaxima = table.Column<int>(type: "integer", nullable: false),
                    InscritosActuales = table.Column<int>(type: "integer", nullable: false),
                    MaterialesRequeridos = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CertificadoDisponible = table.Column<bool>(type: "boolean", nullable: false),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActividadBienestars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "biblioteca_usuarios_oauth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Rol = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true),
                    DiscordId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_biblioteca_usuarios_oauth", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarrerasAsistente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Facultad = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    DuracionSemestres = table.Column<int>(type: "integer", nullable: false),
                    CreditosTotales = table.Column<int>(type: "integer", nullable: false),
                    Activa = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    JefeCarreraId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrerasAsistente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentosAdjuntosAsistente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreArchivo = table.Column<string>(type: "text", nullable: false),
                    RutaArchivo = table.Column<string>(type: "text", nullable: false),
                    TipoArchivo = table.Column<string>(type: "text", nullable: false),
                    TamanioBytes = table.Column<long>(type: "bigint", nullable: false),
                    FechaSubida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SolicitudId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentosAdjuntosAsistente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EpisodioPodcasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoEpisodio = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Categoria = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    UsuarioPodcastId = table.Column<int>(type: "integer", nullable: false),
                    FechaGrabacion = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaPublicacion = table.Column<DateOnly>(type: "date", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Invitados = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DuracionMinutos = table.Column<int>(type: "integer", nullable: false),
                    AudioUrl = table.Column<string>(type: "text", nullable: true),
                    ImagenUrl = table.Column<string>(type: "text", nullable: true),
                    NotasEpisodio = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DisponibleDescarga = table.Column<bool>(type: "boolean", nullable: false),
                    Reproducciones = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpisodioPodcasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstudianteBeneficiarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CI = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NombreCompleto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Carrera = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Semestre = table.Column<int>(type: "integer", nullable: false),
                    Correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FechaRegistro = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TieneBeca = table.Column<bool>(type: "boolean", nullable: false),
                    TipoApoyo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Observaciones = table.Column<string>(type: "text", nullable: true),
                    UsuarioBienestarId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstudianteBeneficiarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventosAsistente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titulo = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ubicacion = table.Column<string>(type: "text", nullable: false),
                    CupoMaximo = table.Column<int>(type: "integer", nullable: false),
                    RequiereConfirmacion = table.Column<bool>(type: "boolean", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OrganizadorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosAsistente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventosParticipantesAsistente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EstadoAsistencia = table.Column<string>(type: "text", nullable: false),
                    FechaConfirmacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Comentarios = table.Column<string>(type: "text", nullable: false),
                    EventoId = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosParticipantesAsistente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoogleLogins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GoogleId = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Picture = table.Column<string>(type: "text", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    LoginDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleLogins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorariosAsistente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoMateria = table.Column<string>(type: "text", nullable: false),
                    NombreMateria = table.Column<string>(type: "text", nullable: false),
                    Aula = table.Column<string>(type: "text", nullable: false),
                    Seccion = table.Column<string>(type: "text", nullable: false),
                    Cupos = table.Column<int>(type: "integer", nullable: false),
                    CuposDisponibles = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DocenteId = table.Column<int>(type: "integer", nullable: false),
                    MateriaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosAsistente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HorariosDiasAsistente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiaSemana = table.Column<string>(type: "text", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "interval", nullable: false),
                    HoraFin = table.Column<TimeSpan>(type: "interval", nullable: false),
                    HorarioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorariosDiasAsistente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InformeVicerrectorados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoInforme = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TipoInforme = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FechaGeneracion = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaAprobacion = table.Column<DateOnly>(type: "date", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UsuarioVicerrectoradoId = table.Column<int>(type: "integer", nullable: false),
                    PeriodoAcademico = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DocumentoUrl = table.Column<string>(type: "text", nullable: true),
                    Confidencial = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformeVicerrectorados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MateriasAsistente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Creditos = table.Column<int>(type: "integer", nullable: false),
                    Semestre = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    Activa = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CarreraId = table.Column<int>(type: "integer", nullable: false),
                    DocenteId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriasAsistente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MateriasPrerequisitosAsistente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MateriaId = table.Column<int>(type: "integer", nullable: false),
                    PrerequisitoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriasPrerequisitosAsistente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModalidadesGrado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModalidadesGrado", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgramacionPodcasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoProgramacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TipoPrograma = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    UsuarioPodcastId = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TotalEpisodios = table.Column<int>(type: "integer", nullable: false),
                    EpisodiosPublicados = table.Column<int>(type: "integer", nullable: false),
                    Temas = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PublicoObjetivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ImagenProgramaUrl = table.Column<string>(type: "text", nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramacionPodcasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecursoPodcasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoRecurso = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nombre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TipoRecurso = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    UsuarioPodcastId = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Marca = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Modelo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Serial = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FechaAdquisicion = table.Column<DateOnly>(type: "date", nullable: true),
                    Valor = table.Column<decimal>(type: "numeric", nullable: true),
                    Caracteristicas = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ImagenRecursoUrl = table.Column<string>(type: "text", nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FechaRegistro = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecursoPodcasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportesAsistente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    Periodo = table.Column<string>(type: "text", nullable: false),
                    PromedioGeneral = table.Column<decimal>(type: "numeric", nullable: false),
                    PorcentajeDesercion = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalEstudiantes = table.Column<int>(type: "integer", nullable: false),
                    EstudiantesAprobados = table.Column<int>(type: "integer", nullable: false),
                    EstudiantesReprobados = table.Column<int>(type: "integer", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: false),
                    FechaGeneracion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GeneradoPorId = table.Column<int>(type: "integer", nullable: false),
                    CarreraId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportesAsistente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResolucionVicerrectorados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NumeroResolucion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TipoResolucion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Contenido = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    FechaEmision = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaVigencia = table.Column<DateOnly>(type: "date", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UsuarioVicerrectoradoId = table.Column<int>(type: "integer", nullable: false),
                    Fundamentos = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Alcance = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DocumentoUrl = table.Column<string>(type: "text", nullable: true),
                    RequiereFirmaDigital = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResolucionVicerrectorados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReunionVicerrectorados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoReunion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TipoReunion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Agenda = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FechaReunion = table.Column<DateOnly>(type: "date", nullable: false),
                    HoraInicio = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HoraFin = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Lugar = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UsuarioVicerrectoradoId = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Participantes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Acuerdos = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MinutaUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FechaCreacion = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReunionVicerrectorados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocioExtensionExtensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreOrganizacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TipoOrganizacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Contacto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AreaColaboracion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FechaRegistro = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocioExtensionExtensions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudApoyos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoSolicitud = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EstudianteBeneficiarioId = table.Column<int>(type: "integer", nullable: false),
                    TipoSolicitud = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FechaSolicitud = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaAtencion = table.Column<DateOnly>(type: "date", nullable: true),
                    Prioridad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    UsuarioBienestarId = table.Column<int>(type: "integer", nullable: true),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CanalAtencion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SeguimientoRequiere = table.Column<bool>(type: "boolean", nullable: false),
                    ProximaSesion = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudApoyos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudesAsistente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    MotivoRechazo = table.Column<string>(type: "text", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaRevision = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EstudianteId = table.Column<int>(type: "integer", nullable: false),
                    RevisadoPorId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesAsistente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioBienestars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoUsuario = table.Column<string>(type: "text", nullable: false),
                    NombreUsuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NombreCompleto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Rol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FechaCreacion = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Especialidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FotoUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioBienestars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioPodcasts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoUsuario = table.Column<string>(type: "text", nullable: false),
                    NombreUsuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NombreCompleto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Rol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FechaCreacion = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Especialidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    FotoUrl = table.Column<string>(type: "text", nullable: true),
                    Biografia = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioPodcasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosAsistente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cedula = table.Column<string>(type: "text", nullable: false),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Apellido = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: false),
                    Rol = table.Column<string>(type: "text", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosAsistente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosBecasOAuth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscordId = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    TokenExpiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaUltimoAcceso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosBecasOAuth", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosRectorado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Correo = table.Column<string>(type: "text", nullable: false),
                    Rol = table.Column<string>(type: "text", nullable: false),
                    TokenSesion = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosRectorado", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioVicerrectorados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoUsuario = table.Column<string>(type: "text", nullable: false),
                    NombreUsuario = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NombreCompleto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Rol = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FechaCreacion = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Departamento = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Cargo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioVicerrectorados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoluntarioExtensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NombreCompleto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AreaInteres = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HorasVoluntariado = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FechaRegistro = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoluntarioExtensions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActividadBienestars");

            migrationBuilder.DropTable(
                name: "biblioteca_usuarios_oauth");

            migrationBuilder.DropTable(
                name: "CarrerasAsistente");

            migrationBuilder.DropTable(
                name: "DocumentosAdjuntosAsistente");

            migrationBuilder.DropTable(
                name: "EpisodioPodcasts");

            migrationBuilder.DropTable(
                name: "EstudianteBeneficiarios");

            migrationBuilder.DropTable(
                name: "EventosAsistente");

            migrationBuilder.DropTable(
                name: "EventosParticipantesAsistente");

            migrationBuilder.DropTable(
                name: "GoogleLogins");

            migrationBuilder.DropTable(
                name: "HorariosAsistente");

            migrationBuilder.DropTable(
                name: "HorariosDiasAsistente");

            migrationBuilder.DropTable(
                name: "InformeVicerrectorados");

            migrationBuilder.DropTable(
                name: "MateriasAsistente");

            migrationBuilder.DropTable(
                name: "MateriasPrerequisitosAsistente");

            migrationBuilder.DropTable(
                name: "ModalidadesGrado");

            migrationBuilder.DropTable(
                name: "ProgramacionPodcasts");

            migrationBuilder.DropTable(
                name: "RecursoPodcasts");

            migrationBuilder.DropTable(
                name: "ReportesAsistente");

            migrationBuilder.DropTable(
                name: "ResolucionVicerrectorados");

            migrationBuilder.DropTable(
                name: "ReunionVicerrectorados");

            migrationBuilder.DropTable(
                name: "SocioExtensionExtensions");

            migrationBuilder.DropTable(
                name: "SolicitudApoyos");

            migrationBuilder.DropTable(
                name: "SolicitudesAsistente");

            migrationBuilder.DropTable(
                name: "UsuarioBienestars");

            migrationBuilder.DropTable(
                name: "UsuarioPodcasts");

            migrationBuilder.DropTable(
                name: "UsuariosAsistente");

            migrationBuilder.DropTable(
                name: "UsuariosBecasOAuth");

            migrationBuilder.DropTable(
                name: "UsuariosRectorado");

            migrationBuilder.DropTable(
                name: "UsuarioVicerrectorados");

            migrationBuilder.DropTable(
                name: "VoluntarioExtensions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProyectoExtensions",
                table: "ProyectoExtensions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActividadExtensions",
                table: "ActividadExtensions");

            migrationBuilder.DropColumn(
                name: "Beneficiarios",
                table: "ProyectoExtensions");

            migrationBuilder.DropColumn(
                name: "CodigoProyecto",
                table: "ProyectoExtensions");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "ProyectoExtensions");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "ProyectoExtensions");

            migrationBuilder.DropColumn(
                name: "FechaFin",
                table: "ProyectoExtensions");

            migrationBuilder.DropColumn(
                name: "Responsable",
                table: "ProyectoExtensions");

            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "ProyectoExtensions");

            migrationBuilder.DropColumn(
                name: "CodigoActividad",
                table: "ActividadExtensions");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "ActividadExtensions");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "ActividadExtensions");

            migrationBuilder.DropColumn(
                name: "FechaActividad",
                table: "ActividadExtensions");

            migrationBuilder.DropColumn(
                name: "Lugar",
                table: "ActividadExtensions");

            migrationBuilder.DropColumn(
                name: "Participantes",
                table: "ActividadExtensions");

            migrationBuilder.DropColumn(
                name: "Responsable",
                table: "ActividadExtensions");

            migrationBuilder.DropColumn(
                name: "TipoActividad",
                table: "ActividadExtensions");

            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "ActividadExtensions");

            migrationBuilder.RenameTable(
                name: "ProyectoExtensions",
                newName: "ProyectosExtension");

            migrationBuilder.RenameTable(
                name: "ActividadExtensions",
                newName: "ActividadesExtension");

            migrationBuilder.RenameColumn(
                name: "Presupuesto",
                table: "ProyectosExtension",
                newName: "PresupuestoProyectoExtension");

            migrationBuilder.RenameColumn(
                name: "FechaInicio",
                table: "ProyectosExtension",
                newName: "FechaRegistroProyectoExtension");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "ProyectosExtension",
                newName: "FechaInicioProyectoExtension");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ProyectosExtension",
                newName: "IdRegistroProyectoExtension");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "ActividadesExtension",
                newName: "FechaActividadExtension");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ActividadesExtension",
                newName: "ProyectoIdActividadExtension");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UltimoAccesoUsuarioExtuniv",
                table: "UsuariosExtuniv",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacionUsuarioExtuniv",
                table: "UsuariosExtuniv",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaUltimoAcceso",
                table: "UsuariosBecas",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "UsuariosBecas",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "expiracion",
                table: "usuarioCIITs",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechasolicitud",
                table: "SolicitudProduccions",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechalimite",
                table: "SolicitudProduccions",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaSolicitud",
                table: "SolicitudesBecas",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaRespuesta",
                table: "SolicitudesBecas",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                table: "Solicitudes",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "Profesores",
                type: "date",
                nullable: false,
                defaultValueSql: "CURRENT_DATE",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Profesores",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechapublicacion",
                table: "ProduccionAudiovisuals",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechainicio",
                table: "ProduccionAudiovisuals",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechaprestamo",
                table: "Prestamos",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechadevolucion",
                table: "Prestamos",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaEncontrado",
                table: "ObjetosPerdidos",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechacambio",
                table: "HistorialCambioses",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fecha_emision",
                table: "Facturas",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaContabilidad",
                table: "ContabilidadReportesIngresos",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaContabilidad",
                table: "ContabilidadPeticionDepartamentos",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacionUsuarioContabilidad",
                table: "ContabilidadLoginOauth",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fechacancelacion",
                table: "Cancelaciones",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaCreacion",
                table: "Becas",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaInicio",
                table: "ActividadesDepa",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FechaFin",
                table: "ActividadesDepa",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "AreaProyectoExtension",
                table: "ProyectosExtension",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CodigoProyectoExtension",
                table: "ProyectosExtension",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescripcionProyectoExtension",
                table: "ProyectosExtension",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EstadoProyectoExtension",
                table: "ProyectosExtension",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FechaFinProyectoExtension",
                table: "ProyectosExtension",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "NombreProyectoExtension",
                table: "ProyectosExtension",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ObservacionesProyectoExtension",
                table: "ProyectosExtension",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsableCIProyectoExtension",
                table: "ProyectosExtension",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UbicacionProyectoExtension",
                table: "ProyectosExtension",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "ProyectoIdActividadExtension",
                table: "ActividadesExtension",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "IdRegistroActividadExtension",
                table: "ActividadesExtension",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "CodigoActividadExtension",
                table: "ActividadesExtension",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescripcionActividadExtension",
                table: "ActividadesExtension",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EstadoActividadExtension",
                table: "ActividadesExtension",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraFinActividadExtension",
                table: "ActividadesExtension",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraInicioActividadExtension",
                table: "ActividadesExtension",
                type: "time without time zone",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "LugarActividadExtension",
                table: "ActividadesExtension",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaterialesActividadExtension",
                table: "ActividadesExtension",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NombreActividadExtension",
                table: "ActividadesExtension",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumeroParticipantesActividadExtension",
                table: "ActividadesExtension",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ObservacionesActividadExtension",
                table: "ActividadesExtension",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoActividadExtension",
                table: "ActividadesExtension",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProyectosExtension",
                table: "ProyectosExtension",
                column: "IdRegistroProyectoExtension");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActividadesExtension",
                table: "ActividadesExtension",
                column: "IdRegistroActividadExtension");
        }
    }
}
