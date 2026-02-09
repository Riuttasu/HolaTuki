namespace Main
{
    class MainClass
    {
        static Random rnd = new Random(); // generador de aleatorios
        const int NUM_MONTONES = 5, MAX_PALILLOS = 4;
        public static void Main()
        {
            string[] jugadores = { "Roberto", "Berto", "Rigoberto", "Humano" };
            int[] montones = new int[NUM_MONTONES];
            int mon = 0, pals = 0, turno = 0;
            bool Hayjuego = true;
            Console.Write("¿Quieres cargar una partida? (si para cargar, cualquiero otra cosa para no cargar) : ");
            if (Console.ReadLine() == "si") LeeArchivo(montones, out turno);
            else Inicializa(montones, jugadores, out turno);
            Render(montones, jugadores, turno, pals, mon);
            while (Hayjuego)
            {
                if (jugadores[turno % 4] == "Humano")
                {
                    do
                    {
                        JuegaHumano(montones, out mon, out pals);
                        if (mon == -1) Hayjuego = false;
                        else
                        {
                            QuitaPalillos(montones, pals, mon);
                            Render(montones, jugadores, turno, pals, mon);
                        }
                    } while (Palindromo(montones) && Hayjuego && !FinJuego(montones));
                    turno++;
                }
                else
                {
                    do
                    {
                        JuegaMaquina(montones, out mon, out pals);
                        QuitaPalillos(montones, pals, mon);
                        Render(montones, jugadores, turno, pals, mon);
                    } while (Palindromo(montones) && !FinJuego(montones));
                    turno++;
                }
                if (Hayjuego) Hayjuego = !FinJuego(montones);
            }
            if (mon == -1)
            {
                Console.Write("¿Quieres guardar la partida? (si para guardar, cualquiero otra cosa para no guardar) : ");
                if (Console.ReadLine() == "si") GuardaPartida(montones, turno);
            }
            else Console.WriteLine($"Ha ganado el/la jugador(a): {jugadores[(turno - 1) % 4]}");
        }
        static void Inicializa(int[] montones, string[] jugadores, out int turno)
        {
            for (int i = 0; i < NUM_MONTONES; i++)
            {
                montones[i] = rnd.Next(1, MAX_PALILLOS + 1);
            }
            turno = rnd.Next(0, jugadores.Length);

        }
        static void Render(int[] montones, string[] jugadores, int turno, int num, int mon)
        {
            if (num == 0) Console.WriteLine("Empieza el juego: ");
            else if (num > 0)
            {
                Console.WriteLine($"{jugadores[turno % 4]} ha quitado {num} palillo(s) del montón {mon}.");
            }
            for (int i = 0; i < NUM_MONTONES; i++)
            {
                Console.Write($"{i}: ");
                for (int j = 0; j < montones[i]; j++)
                {
                    Console.Write("| ");
                }
                Console.WriteLine();
            }
        }
        static void JuegaHumano(int[] montones, out int mon, out int pals)
        {
            Console.WriteLine("Humano, tu turno: ");
            do
            {
                mon = PideDato(-1, montones.Length, "Montón (-1 para terminar): ");
            } while (mon != -1 && montones[mon] == 0);
            if (mon != -1) pals = PideDato(1, montones[mon], "Palillos a sacar: ");
            else pals = 0;
        }
        static int PideDato(int min, int max, string mensaje)
        {
            int resultado = 0; bool valido = false;
            while (!valido)
            {
                Console.Write(mensaje);
                resultado = int.Parse(Console.ReadLine());
                if (resultado >= min && resultado <= max) valido = true;
            }
            return resultado;
        }
        static void JuegaMaquina(int[] montones, out int mon, out int pals)
        {
            bool valido = false; mon = -1;
            while (!valido)
            {
                mon = rnd.Next(0, NUM_MONTONES);
                if (montones[mon] != 0) valido = true;
            }
            pals = rnd.Next(1, montones[mon] + 1);
        }
        static void QuitaPalillos(int[] montones, int pals, int mon)
        {
            montones[mon] -= pals;
        }
        static bool FinJuego(int[] montones)
        {
            bool fin = true; int i = 0;
            while (i < montones.Length && fin)
            {
                if (montones[i] != 0) fin = false;
                i++;
            }
            return fin;
        }
        static void GuardaPartida(int[] montones, int turno)
        {
            StreamWriter guardado = new StreamWriter("minguardado.txt");
            for (int i = 0; i < montones.Length; i++)
            {
                guardado.Write($"{montones[i]} ");
            }
            guardado.WriteLine();
            guardado.WriteLine(turno);
            guardado.Close();
        }
        static void LeeArchivo(int[] montones, out int turno)
        {
            StreamReader guardado = new StreamReader("minguardado.txt");
            string s = guardado.ReadLine();
            string[] a = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (a.Length == montones.Length)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    montones[i] = int.Parse(a[i]);
                }
            }
            else Console.WriteLine("No coincide el número de montones");
            turno = int.Parse(guardado.ReadLine()) - 1;
            guardado.Close();
        }
        static bool Palindromo(int[] montones)
        {
            bool pal = true; int i = 0;
            while (i <= montones.Length / 2 && pal)
            {
                if (montones[i] != montones[montones.Length - i - 1]) pal = false;
                i++;
            }
            return pal;
        }
    }
}