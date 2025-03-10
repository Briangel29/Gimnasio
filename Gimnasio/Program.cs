using System;
using System.IO;
using System.Net.Http.Json;
using System.Text;

namespace Gimnasio
{
    public class Program
    {
        public static void Main()
        {
            string cliente = Path.Combine(Environment.CurrentDirectory, "cliente.txt");

            int opcion = 7;

            do
            {
                Menu();

                try
                {
                    opcion = int.Parse(Console.ReadLine()!);

                    // 1. Da de alta un cliente
                    if (opcion.Equals(1))
                    {
                        Console.Clear();
                        Console.WriteLine("\t***Registro de clientes del Gimnasio***\n\n");
                        Console.Write("Digite el nombre: ");

                        string nombre = Console.ReadLine()!;

                        Console.Write("\nDigite el numero de identidad: ");

                        string identidad = Console.ReadLine()!;

                        Console.Write("\nDigite la observacion: ");

                        string? observacion = Console.ReadLine();

                        int idCliente = GeneraId(cliente);

                        if (!idCliente.Equals(0))
                        {
                            string[] informacionCliente = new string[] { idCliente.ToString(), nombre, identidad, observacion};

                            string registroCliente = string.Join("|", informacionCliente);

                            Escritura(cliente, registroCliente, 1);

                            Console.WriteLine("\n\nHan sido registrado los datos");
                            Console.WriteLine("\nPrecione enter para volver al menu...");
                        }
                        else
                        {
                            Console.WriteLine("Ocurrio un error generando el Id, revisar el metodo 'GeneraId'");
                        }

                    }
                    // 2. Mostrar detalles de un cliente
                    else if (opcion.Equals(2))
                    {
                        Console.Clear();
                        Console.WriteLine("\t***Registro de clientes del Gimnasio***\n\n");
                        Console.WriteLine("Detalle");
                        Console.Write("Digite el id del ciente o precione 0 para volver: ");

                        int idCliente = int.Parse(Console.ReadLine()!);

                        if (!idCliente.Equals(0))
                        {
                            IEnumerable<string>? listadoClientes = Lectura(cliente);

                            if (listadoClientes is not null)
                            {
                                (bool, string?) informacionCliente = buscaCliente(listadoClientes, idCliente);

                                bool existeCliente = informacionCliente.Item1;

                                if (existeCliente)
                                {
                                    string[] partes = informacionCliente.Item2!.Split('|');
                                    int id;

                                    if (!int.TryParse(partes[0], out id))
                                    {
                                        Console.WriteLine($"Ocurrio un error, no se identifico el id del cliente, " +
                                            $"favor verificar el registro de clientes en el txt");
                                    }

                                    string nombre = partes[1];
                                    string identificacion = partes[2];
                                    string observacion = partes[3];

                                    Console.WriteLine($"\n\nID: {id}\nNombre: {nombre}\nIdentificacion: {identificacion}\nObservacion: {observacion}");
                                    Console.WriteLine("\n\nPresione enter para volver al menu....");
                                }
                                else Console.WriteLine("El ID no pertenece a ningun cliente");
                            }
                            else
                            {
                                Console.WriteLine("No hay ningun cliente registrado hasta el momento");
                            }
                        }
                    }
                    // 3. Listar clientes
                    else if (opcion.Equals(3))
                    {
                        Console.Clear();
                        Console.WriteLine("\t***Registro de clientes del Gimnasio***\n\n");
                        Console.WriteLine("Lista de clientes: \n");

                        IEnumerable<string>? existeCliente = Lectura(cliente);

                        if (existeCliente is not null)
                        {
                            string[,] listaClientes = buscarTodosLosClientes(existeCliente)!;

                            for (int i = 0; i < listaClientes.GetLength(0); i++)
                            {
                                for (int j = 0; j < listaClientes.GetLength(1); j++)
                                {
                                    Console.Write($"{listaClientes[i, j]} \t");
                                }
                                Console.WriteLine();
                            }

                            Console.WriteLine("\n\nPresione enter para volver al menu....");
                        }
                        else
                        {
                            Console.WriteLine("No hay ningun cliente registrado hasta el momento");
                        }
                    }
                    // 4. Buscar cliente (Nombre)
                    else if (opcion.Equals(4))
                    {
                        Console.Clear();
                        Console.WriteLine("\t***Registro de clientes del Gimnasio***\n\n");
                        Console.Write("Digite el nombre del cliente: ");

                        string nombreCliente = Console.ReadLine()!;

                        IEnumerable<string>? listadoClientes = Lectura(cliente);

                        if (listadoClientes is not null)
                        {
                            (bool, string?) existeCliente = buscaCliente(listadoClientes, nombreCliente);

                            if (existeCliente.Item1)
                            {
                                string[] informacionCliente = existeCliente.Item2!.Split('|');

                                Console.WriteLine($"{informacionCliente[0]}  -  {informacionCliente[1]}");
                            }
                            else Console.WriteLine(existeCliente.Item2);
                        }
                        else Console.WriteLine("No se encontro ningun cliente");

                        Console.WriteLine("\n\nPresione enter para volver al menu....");
                    }
                    // 5. Dar de baja un cliente
                    else if (opcion.Equals(5))
                    {
                        Console.Clear();
                        Console.WriteLine("\t***Registro de clientes del Gimnasio***\n\n");
                        Console.Write("Ingrese el Id del cliente: ");

                        int idCliente = int.Parse(Console.ReadLine()!);

                        if (!idCliente.Equals(0))
                        {
                            string mensaje = EliminarCliente(cliente, idCliente);

                            Console.WriteLine(mensaje);
                        }
                        else Console.WriteLine("El valor ingresado debe ser numerico");
                        Console.WriteLine("\n\nPresione enter para volver al menu....");
                    }
                    // 6. Modificar un cliente
                    else if (opcion.Equals(6))
                    {

                    }
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e);
                }

                Console.ReadLine();
                Console.Clear();
            } while (!opcion.Equals(7));
        }
        /*
            mensaje: la informacion escrita en el txt
            ruta: la ubicacion del archivo que estara en la misma carpeta del proyecto
            tipoAccion: 0 si sera reescrito el archivo completo, 1 si solo se agregara informacion adicional al txt
         */
        public static void Escritura(string ruta, string mensaje, int tipoAccion)
        {
            if (tipoAccion.Equals(0))
            {
                StreamWriter escritura = new StreamWriter(ruta, false, Encoding.UTF8);

                escritura.WriteLine(mensaje);
                escritura.Close();
            }
            else if(tipoAccion.Equals(1))
            {
                StreamWriter escritura = new StreamWriter(ruta, true, Encoding.UTF8);

                escritura.WriteLine(mensaje);
                escritura.Close();
            }
            else
            {
                Console.WriteLine("El tipo de accion no es valido");
            }
        }
        public static IEnumerable<string>? Lectura(string ruta)
        {
            if (File.Exists(ruta))
            {
                using (StreamReader lectura = new StreamReader(ruta, Encoding.UTF8))
                {

                    List<string>? mensajePorLinea = new List<string>();

                    while (lectura.Peek() >= 0)
                    {
                        mensajePorLinea.Add(lectura.ReadLine()!);
                    }

                    IEnumerable<string> mensaje = mensajePorLinea.ToList();

                    return mensaje;
                }
            }
            else
            {
                return null;
            }
        }
        public static void Menu(int? opcion = null)
        {
            if (opcion is null)
            {
                Console.WriteLine("\t***Registro de clientes del Gimnasio***\n\n");
                Console.WriteLine("1. Da de alta un cliente");
                Console.WriteLine("2. Mostrar detalles de un cliente");
                Console.WriteLine("3. Listar clientes");
                Console.WriteLine("4. Buscar cliente (Nombre)");
                Console.WriteLine("5. Dar de baja un cliente");
                Console.WriteLine("6. Modificar un cliente");
                Console.WriteLine("7. Salir");
            }
        }
        public static int GeneraId(string ruta)
        {
            IEnumerable<string>? lista = Lectura(ruta);

            if (lista is not null)
            {
                string[]? caracter = lista.Select(x => x.ToString().Split('|')).LastOrDefault();

                int ultimoId;
                int.TryParse(caracter[0], out ultimoId);

                return ultimoId + 1;
            }
            else return 1;
        }
        public static (bool, string?) buscaCliente(IEnumerable<string>? clientes, int? idCliente)
        {
            string cliente = clientes?.FirstOrDefault(x => x.ToString().Split('|')[0].Equals(idCliente.ToString()))!;

            if (cliente is not null) return (true, cliente);
            else return (false, "No hay personas registradas hasta el momento");
        }
        public static (bool, string?) buscaCliente(IEnumerable<string>? clientes, string? nombreCliente)
        {
            string cliente = clientes?.FirstOrDefault(x => x.ToString().Split('|')[1].Equals(nombreCliente))!;

            if (cliente is not null) return (true, cliente);
            else return (false, "No hay personas registradas hasta el momento");
        }
        public static string[,]? buscarTodosLosClientes(IEnumerable<string>? clientes)
        {
            if (clientes is not null)
            {
                List<string> filas = clientes!.ToList();

                string[] columnas = filas.First().Split('|');

                string[,] listadoCliente = new string[filas.Count(), columnas.Length];

                for (int i = 0; i < listadoCliente.GetLength(0); i++)
                {
                    string[] partes = filas[i].Split('|');
                    for (int j = 0; j < columnas.Length; j++)
                    {
                        listadoCliente[i, j] = partes[j];
                    }
                }

                return listadoCliente;
            }
            else
            {
                return null;
            }
        }
        public static string EliminarCliente(string ruta, int id)
        {
            IEnumerable<string>? listaClientes = Lectura(ruta);

            if (listaClientes is null) return $"No hay ningun cliente perteneciente al id {id}";

            List<string> nuevaListaClientes = listaClientes.ToList();

            Escritura(ruta, "", 0);

            return "Fue eliminado el cliente";
        }
    }
}