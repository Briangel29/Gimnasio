using System;
using System.IO;
using System.Text;

namespace Gimnasio
{
    public class Program
    {
        public static void Main()
        {
            string cliente = Path.Combine(Environment.CurrentDirectory, "cliente.txt");

            Console.WriteLine("\t***Registro de clientes del Gimnasio***\n\n");

            int opcion;

            do
            {
                Menu();

                try
                {
                    opcion = int.Parse(Console.ReadLine()!);

                    if (opcion.Equals(1))
                    {
                        Console.Clear();
                        Console.WriteLine("\t***Registro de clientes del Gimnasio***\n\n");
                        Console.Write("Digite el id del cliente a dar de alta o digite 0 para volver atras: ");
                        int idClienteOpcion;

                        try
                        {
                            idClienteOpcion = int.Parse(Console.ReadLine()!);
                            if (idClienteOpcion.Equals(0)) break;
                            else
                            {
                                List<string> listaClientes = Lectura(cliente).ToList();

                                if (listaClientes.Any())
                                {
                                    List<string> id = new List<string>();

                                    foreach (string i in listaClientes)
                                    {
                                        if(string.Join("|", i.Split('|').Take(1)).Equals(idClienteOpcion))
                                        {
                                            
                                        }
                                    }

                                    
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    }

                }catch(NullReferenceException e)
                {
                    Console.WriteLine(e);
                }
            } while (!opcion.Equals(7));

            Escritura("", cliente);
        }
        /*
            mensaje: la informacion escrita en el txt
            ruta: la ubicacion del archivo que estara en la misma carpeta del proyecto
            tipoAccion: 0 si sera reescrito el archivo completo, 1 si solo se agregara informacion adicional al txt
         */
        public static void Escritura(string mensaje, string ruta, int tipoAccion)
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
        public static IEnumerable<string> Lectura(string ruta)
        {
            StreamReader lectura = new StreamReader(ruta, Encoding.UTF8);

            List<string>? mensajePorLinea = new List<string>();

            while (lectura.Peek() >= 0)
            {
                mensajePorLinea.Add(lectura.ReadLine()!);
            }

            IEnumerable<string> mensaje = mensajePorLinea.ToList();

            lectura.Close();

            return mensaje;
        }
        public static void Menu(int? opcion = null)
        {
            if (opcion is null)
            {
                Console.WriteLine("1. Da de alta un cliente");
                Console.WriteLine("2. Mostrar detalles de un cliente");
                Console.WriteLine("3. Listar clientes");
                Console.WriteLine("4. Buscar cliente (Nombre)");
                Console.WriteLine("5. Dar de baja un cliente");
                Console.WriteLine("6. Modificar un cliente");
                Console.WriteLine("7. Salir");
            }
        }
    }
}