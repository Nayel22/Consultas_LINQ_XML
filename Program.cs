using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace Consultas_LINQ_XML
{
    // Definición de la clase Empleado
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string Departamento { get; set; }
        public decimal Salario { get; set; }

        public override string ToString()
        {
            return $"ID: {Id}, Nombre: {Nombre}, Edad: {Edad}, Departamento: {Departamento}, Salario: ${Salario}";
        }
    }

    internal class Program
    {
        private static List<Empleado> empleados;
        private static string xmlFilePath = "empleados.xml";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Inicializar la lista de empleados
            InicializarEmpleados();

            // Crear archivo XML si no existe
            if (!File.Exists(xmlFilePath))
            {
                CrearArchivoXML();
                Console.WriteLine($"Archivo XML creado: {xmlFilePath}");
            }

            bool salir = false;
            while (!salir)
            {
                MostrarMenu();
                string opcion = Console.ReadLine();
                Console.WriteLine();

                switch (opcion)
                {
                    case "1":
                        ConsultarEmpleadosMayores30();
                        break;
                    case "2":
                        ConsultarEmpleadosSalarioMayor50000();
                        break;
                    case "3":
                        CalcularPromedioSalarioPorDepartamento();
                        break;
                    case "4":
                        EncontrarEmpleadoSalarioMasAlto();
                        break;
                    case "5":
                        ContarEmpleadosPorDepartamento();
                        break;
                    case "6":
                        LeerXMLyCargarEmpleados();
                        break;
                    case "7":
                        FiltrarEmpleadosXMLSalarioMayor50000();
                        break;
                    case "8":
                        ActualizarSalarioEmpleadoXML();
                        break;
                    case "9":
                        EliminarEmpleadoXML();
                        break;
                    case "10":
                        AgregarNuevoEmpleadoXML();
                        break;
                    case "0":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente nuevamente.");
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        private static void MostrarMenu()
        {
            Console.WriteLine("===== SISTEMA DE GESTIÓN DE EMPLEADOS =====");
            Console.WriteLine("1. Consultar empleados mayores de 30 años");
            Console.WriteLine("2. Consultar empleados con salario mayor a 50000");
            Console.WriteLine("3. Ver promedio de salarios por departamento");
            Console.WriteLine("4. Ver empleado con salario más alto");
            Console.WriteLine("5. Contar empleados por departamento");
            Console.WriteLine("6. Leer archivo XML y cargar empleados");
            Console.WriteLine("7. Filtrar empleados XML con salario mayor a 50000");
            Console.WriteLine("8. Actualizar salario de empleado en XML");
            Console.WriteLine("9. Eliminar empleado del XML por ID");
            Console.WriteLine("10. Agregar nuevo empleado al XML");
            Console.WriteLine("0. Salir");
            Console.Write("\nSeleccione una opción: ");
        }

        #region Inicialización de Datos
        private static void InicializarEmpleados()
        {
            empleados = new List<Empleado>
            {
                new Empleado { Id = 1, Nombre = "Juan Pérez", Edad = 45, Departamento = "TI", Salario = 55000 },
                new Empleado { Id = 2, Nombre = "Ana González", Edad = 34, Departamento = "Marketing", Salario = 45000 },
                new Empleado { Id = 3, Nombre = "Luis Rodríguez", Edad = 29, Departamento = "Ventas", Salario = 38000 },
                new Empleado { Id = 4, Nombre = "María López", Edad = 42, Departamento = "RRHH", Salario = 52000 },
                new Empleado { Id = 5, Nombre = "Carlos Sánchez", Edad = 31, Departamento = "TI", Salario = 60000 },
                new Empleado { Id = 6, Nombre = "Laura Martínez", Edad = 27, Departamento = "Marketing", Salario = 42000 },
                new Empleado { Id = 7, Nombre = "Roberto Fernández", Edad = 38, Departamento = "Ventas", Salario = 65000 },
                new Empleado { Id = 8, Nombre = "Patricia Ramírez", Edad = 33, Departamento = "RRHH", Salario = 48000 },
                new Empleado { Id = 9, Nombre = "Miguel Álvarez", Edad = 50, Departamento = "TI", Salario = 75000 },
                new Empleado { Id = 10, Nombre = "Silvia Torres", Edad = 36, Departamento = "Marketing", Salario = 51000 }
            };
        }

        private static void CrearArchivoXML()
        {
            XDocument xdoc = new XDocument(
                new XElement("Empleados",
                    from emp in empleados
                    select new XElement("Empleado",
                        new XElement("Id", emp.Id),
                        new XElement("Nombre", emp.Nombre),
                        new XElement("Edad", emp.Edad),
                        new XElement("Departamento", emp.Departamento),
                        new XElement("Salario", emp.Salario)
                    )
                )
            );

            xdoc.Save(xmlFilePath);
        }
        #endregion

        #region Consultas LINQ a Objetos
        private static void ConsultarEmpleadosMayores30()
        {
            Console.WriteLine("=== EMPLEADOS MAYORES DE 30 AÑOS ===");

            var resultado = empleados.Where(e => e.Edad > 30).ToList();

            foreach (var emp in resultado)
            {
                Console.WriteLine(emp);
            }

            Console.WriteLine($"\nTotal: {resultado.Count} empleados");
        }

        private static void ConsultarEmpleadosSalarioMayor50000()
        {
            Console.WriteLine("=== EMPLEADOS CON SALARIO MAYOR A 50000 ===");

            var resultado = empleados.Where(e => e.Salario > 50000).ToList();

            foreach (var emp in resultado)
            {
                Console.WriteLine(emp);
            }

            Console.WriteLine($"\nTotal: {resultado.Count} empleados");
        }

        private static void CalcularPromedioSalarioPorDepartamento()
        {
            Console.WriteLine("=== PROMEDIO DE SALARIO POR DEPARTAMENTO ===");

            var resultado = empleados
                .GroupBy(e => e.Departamento)
                .Select(g => new {
                    Departamento = g.Key,
                    PromedioSalario = g.Average(e => e.Salario)
                });

            foreach (var grupo in resultado)
            {
                Console.WriteLine($"Departamento: {grupo.Departamento}, Promedio Salario: ${Math.Round(grupo.PromedioSalario, 2)}");
            }
        }

        private static void EncontrarEmpleadoSalarioMasAlto()
        {
            Console.WriteLine("=== EMPLEADO CON SALARIO MÁS ALTO ===");

            var empleadoSalarioMasAlto = empleados.OrderByDescending(e => e.Salario).First();

            Console.WriteLine(empleadoSalarioMasAlto);
        }

        private static void ContarEmpleadosPorDepartamento()
        {
            Console.WriteLine("=== CANTIDAD DE EMPLEADOS POR DEPARTAMENTO ===");

            var resultado = empleados
                .GroupBy(e => e.Departamento)
                .Select(g => new {
                    Departamento = g.Key,
                    CantidadEmpleados = g.Count()
                });

            foreach (var grupo in resultado)
            {
                Console.WriteLine($"Departamento: {grupo.Departamento}, Cantidad: {grupo.CantidadEmpleados}");
            }
        }
        #endregion

        #region Manipulación de XML con LINQ
        private static void LeerXMLyCargarEmpleados()
        {
            Console.WriteLine("=== EMPLEADOS CARGADOS DESDE XML ===");

            XDocument xdoc = XDocument.Load(xmlFilePath);

            var empleadosXML = xdoc.Descendants("Empleado")
                .Select(e => new Empleado
                {
                    Id = int.Parse(e.Element("Id").Value),
                    Nombre = e.Element("Nombre").Value,
                    Edad = int.Parse(e.Element("Edad").Value),
                    Departamento = e.Element("Departamento").Value,
                    Salario = decimal.Parse(e.Element("Salario").Value)
                }).ToList();

            foreach (var emp in empleadosXML)
            {
                Console.WriteLine(emp);
            }

            Console.WriteLine($"\nTotal: {empleadosXML.Count} empleados cargados desde XML");
        }

        private static void FiltrarEmpleadosXMLSalarioMayor50000()
        {
            Console.WriteLine("=== EMPLEADOS XML CON SALARIO MAYOR A 50000 ===");

            XDocument xdoc = XDocument.Load(xmlFilePath);

            var resultado = xdoc.Descendants("Empleado")
                .Where(e => decimal.Parse(e.Element("Salario").Value) > 50000)
                .Select(e => new {
                    Id = int.Parse(e.Element("Id").Value),
                    Nombre = e.Element("Nombre").Value,
                    Salario = decimal.Parse(e.Element("Salario").Value)
                });

            foreach (var emp in resultado)
            {
                Console.WriteLine($"ID: {emp.Id}, Nombre: {emp.Nombre}, Salario: ${emp.Salario}");
            }

            Console.WriteLine($"\nTotal: {resultado.Count()} empleados");
        }

        private static void ActualizarSalarioEmpleadoXML()
        {
            Console.Write("Ingrese el ID del empleado a actualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido");
                return;
            }

            Console.Write("Ingrese el nuevo salario: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal nuevoSalario))
            {
                Console.WriteLine("Salario inválido");
                return;
            }

            XDocument xdoc = XDocument.Load(xmlFilePath);

            var empleado = xdoc.Descendants("Empleado")
                .FirstOrDefault(e => int.Parse(e.Element("Id").Value) == id);

            if (empleado == null)
            {
                Console.WriteLine($"No se encontró ningún empleado con ID {id}");
                return;
            }

            // Guardar datos anteriores para mostrar cambio
            string nombre = empleado.Element("Nombre").Value;
            decimal salarioAnterior = decimal.Parse(empleado.Element("Salario").Value);

            // Actualizar salario
            empleado.Element("Salario").Value = nuevoSalario.ToString();

            // Guardar cambios
            xdoc.Save(xmlFilePath);

            Console.WriteLine($"\nSalario actualizado para {nombre}:");
            Console.WriteLine($"Salario anterior: ${salarioAnterior}");
            Console.WriteLine($"Nuevo salario: ${nuevoSalario}");
        }

        private static void EliminarEmpleadoXML()
        {
            Console.Write("Ingrese el ID del empleado a eliminar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("ID inválido");
                return;
            }

            XDocument xdoc = XDocument.Load(xmlFilePath);

            var empleado = xdoc.Descendants("Empleado")
                .FirstOrDefault(e => int.Parse(e.Element("Id").Value) == id);

            if (empleado == null)
            {
                Console.WriteLine($"No se encontró ningún empleado con ID {id}");
                return;
            }

            // Guardar nombre para mostrar mensaje
            string nombre = empleado.Element("Nombre").Value;

            // Eliminar empleado
            empleado.Remove();

            // Guardar cambios
            xdoc.Save(xmlFilePath);

            Console.WriteLine($"\nEmpleado eliminado: {nombre} (ID: {id})");
        }

        private static void AgregarNuevoEmpleadoXML()
        {
            Empleado nuevoEmpleado = new Empleado();

            Console.WriteLine("=== AGREGAR NUEVO EMPLEADO ===");

            // Buscar el ID más alto en el XML para asignar uno nuevo
            XDocument xdoc = XDocument.Load(xmlFilePath);
            int maxId = 0;

            if (xdoc.Descendants("Empleado").Any())
            {
                maxId = xdoc.Descendants("Empleado")
                    .Max(e => int.Parse(e.Element("Id").Value));
            }

            nuevoEmpleado.Id = maxId + 1;

            Console.Write("Nombre: ");
            nuevoEmpleado.Nombre = Console.ReadLine();

            Console.Write("Edad: ");
            if (!int.TryParse(Console.ReadLine(), out int edad))
            {
                Console.WriteLine("Edad inválida");
                return;
            }
            nuevoEmpleado.Edad = edad;

            Console.Write("Departamento: ");
            nuevoEmpleado.Departamento = Console.ReadLine();

            Console.Write("Salario: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal salario))
            {
                Console.WriteLine("Salario inválido");
                return;
            }
            nuevoEmpleado.Salario = salario;

            // Crear nuevo elemento XML y agregarlo al documento
            XElement nuevoElemento = new XElement("Empleado",
                new XElement("Id", nuevoEmpleado.Id),
                new XElement("Nombre", nuevoEmpleado.Nombre),
                new XElement("Edad", nuevoEmpleado.Edad),
                new XElement("Departamento", nuevoEmpleado.Departamento),
                new XElement("Salario", nuevoEmpleado.Salario)
            );

            xdoc.Element("Empleados").Add(nuevoElemento);

            // Guardar cambios
            xdoc.Save(xmlFilePath);

            Console.WriteLine($"\nEmpleado agregado con éxito:");
            Console.WriteLine(nuevoEmpleado);
        }
        #endregion
    }
}