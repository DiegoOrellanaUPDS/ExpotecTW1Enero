using Microsoft.EntityFrameworkCore;
using Universidad.Models;

namespace Universidad.Data;

public static class SeedData
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        if (await context.Estudiantes.AnyAsync())
            return;

        // Add sample data
        var docentes = new List<Docente>
        {
            new() { Codigo = "DOC001", Nombre = "Juan", Apellido = "Pérez", Email = "juan@upds.edu.bo", Contrato = "Tiempo Completo", Especialidad = "Matemáticas" },
            new() { Codigo = "DOC002", Nombre = "María", Apellido = "Gómez", Email = "maria@upds.edu.bo", Contrato = "Tiempo Completo", Especialidad = "Programación" },
            new() { Codigo = "DOC003", Nombre = "Carlos", Apellido = "López", Email = "carlos@upds.edu.bo", Contrato = "Medio Tiempo", Especialidad = "Bases de Datos" }
        };
        await context.Docentes.AddRangeAsync(docentes);
        await context.SaveChangesAsync();

        var materias = new List<Materia>
        {
            new() { Codigo = "MAT101", Nombre = "Cálculo I", Sigla = "CAL1", Creditos = 4, Semestre = 1, Tipo = "Obligatoria", Area = "Ciencias Básicas" },
            new() { Codigo = "PROG101", Nombre = "Programación I", Sigla = "PRO1", Creditos = 4, Semestre = 1, Tipo = "Obligatoria", Area = "Ingeniería" },
            new() { Codigo = "MAT102", Nombre = "Cálculo II", Sigla = "CAL2", Creditos = 4, Semestre = 2, Tipo = "Obligatoria", Area = "Ciencias Básicas", PreRequisitos = "1" },
            new() { Codigo = "BD101", Nombre = "Bases de Datos I", Sigla = "BD1", Creditos = 3, Semestre = 3, Tipo = "Obligatoria", Area = "Ingeniería" },
            new() { Codigo = "WEB101", Nombre = "Tecnologías Web I", Sigla = "TW1", Creditos = 3, Semestre = 5, Tipo = "Obligatoria", Area = "Ingeniería" }
        };
        await context.Materias.AddRangeAsync(materias);
        await context.SaveChangesAsync();

        var grupos = new List<Grupo>
        {
            new() { MateriaId = 1, Paralelo = "A", Turno = "Mañana", CupoMaximo = 30, Aula = "A-101", Horarios = "Lunes 8:00-10:00, Miércoles 8:00-10:00", DocenteId = 1 },
            new() { MateriaId = 1, Paralelo = "B", Turno = "Tarde", CupoMaximo = 25, Aula = "A-102", Horarios = "Martes 14:00-16:00, Jueves 14:00-16:00", DocenteId = 1 },
            new() { MateriaId = 2, Paralelo = "A", Turno = "Mañana", CupoMaximo = 25, Aula = "LAB-201", Horarios = "Lunes 10:00-12:00, Miércoles 10:00-12:00", DocenteId = 2 },
            new() { MateriaId = 3, Paralelo = "A", Turno = "Mañana", CupoMaximo = 30, Aula = "A-103", Horarios = "Martes 8:00-10:00, Jueves 8:00-10:00", DocenteId = 1 },
            new() { MateriaId = 4, Paralelo = "A", Turno = "Noche", CupoMaximo = 20, Aula = "LAB-202", Horarios = "Lunes 19:00-21:00, Miércoles 19:00-21:00", DocenteId = 3 },
            new() { MateriaId = 5, Paralelo = "A", Turno = "Mañana", CupoMaximo = 25, Aula = "LAB-203", Horarios = "Viernes 8:00-11:00", DocenteId = 2 }
        };
        await context.Grupos.AddRangeAsync(grupos);
        await context.SaveChangesAsync();

        var estudiantes = new List<Estudiante>
        {
            new() { Codigo = "20230001", Nombre = "Joel", Apellido = "Cerrogrande", CI = "1234567", Email = "tj.joel.cerrogrande.o@upds.net.bo", Telefono = "77788899", Carrera = "Ingeniería de Sistemas", Semestre = 5, Promedio = 75.5m },
            new() { Codigo = "20230002", Nombre = "Ana", Apellido = "Rodríguez", CI = "7654321", Email = "ana@upds.net.bo", Telefono = "77711122", Carrera = "Ingeniería de Sistemas", Semestre = 3, Promedio = 82.0m },
            new() { Codigo = "20230003", Nombre = "Luis", Apellido = "Martínez", CI = "9876543", Email = "luis@upds.net.bo", Telefono = "77733344", Carrera = "Ingeniería Civil", Semestre = 1, Promedio = 68.5m }
        };
        await context.Estudiantes.AddRangeAsync(estudiantes);
        await context.SaveChangesAsync();

        Console.WriteLine("✅ Base de datos inicializada con datos de prueba");
    }
}
