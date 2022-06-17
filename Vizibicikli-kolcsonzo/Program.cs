using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Vizibicikli_kolcsonzo
{
    class Kolcsonzes
    {
        public string Nev { get; set; }
        public char Jazonosito { get; set; }
        public int Elvitel_Ora { get; set; }
        public int Elvitel_Perc { get; set; }
        public int Visszahoz_Ora { get; set; }
        public int Visszahoz_Perc { get; set; }


        public Kolcsonzes(string Sor)
        {
            string[] sorelemek = Sor.Split(';');
            this.Nev = sorelemek[0];
            this.Jazonosito = Convert.ToChar(sorelemek[1]);
            this.Elvitel_Ora = Convert.ToInt32(sorelemek[2]);
            this.Elvitel_Perc = Convert.ToInt32(sorelemek[3]);
            this.Visszahoz_Ora = Convert.ToInt32(sorelemek[4]);
            this.Visszahoz_Perc = Convert.ToInt32(sorelemek[5]);

        }
    }
    class Program
    {
        public static List<Kolcsonzes> adatok = new List<Kolcsonzes>();
        static void Main(string[] args)
        {
            Beolvasas(); 
            KolcsonzesDarabszama(); //adatok.Count-
            Nevbekeres(); // névbekérés utána XY aznap mettől meddig bérelt biciklit, kiírásnál használjon vezető 0-ákat.
            Idopontmegadas(); //megadott időpontban melyik járművek voltak a vizen, ki vezette.
            FMO8(); //Napi bevétel összege
            FMO9(); //F.txt létrehozása F járművet vki megrongálta, lehetséges elkövetők időpontok kigyűjtése
            FMO10(); //Statisztika Linq-val
            Console.ReadLine();

        }
        private static void Beolvasas()
        {
            StreamReader Olvas = new StreamReader("kolcsonzesek.txt", Encoding.UTF8);
            string fejlec = Olvas.ReadLine();
            while (!Olvas.EndOfStream)
            {
                adatok.Add(new Kolcsonzes(Olvas.ReadLine()));

            }
        }
        private static void KolcsonzesDarabszama()
        {
            Console.WriteLine($"5. feladat: Napi kölcsönzések száma:{adatok.Count}");
        }

        private static void Nevbekeres()
        {
            Console.Write($"6. feladat: Kérek egy nevet:");
            string nev = Console.ReadLine();
            bool bennevan = false;
            for (int i = 0; i < adatok.Count; i++)
            {
                if (adatok[i].Nev == nev)
                {
                    if (bennevan == false)
                    {
                        Console.WriteLine($"\t{nev} kölcsönzései: ");
                        Console.WriteLine($"\t{adatok[i].Elvitel_Ora}:{adatok[i].Elvitel_Perc}-{adatok[i].Visszahoz_Ora}:{adatok[i].Visszahoz_Perc}");
                        bennevan = true;
                    }
                    else
                    {
                        Console.WriteLine($"\t{adatok[i].Elvitel_Ora}:{adatok[i].Elvitel_Perc}-{adatok[i].Visszahoz_Ora}:{adatok[i].Visszahoz_Perc}");
                    }
                }
            }
            if (bennevan == false)
            {
                Console.WriteLine("Nem volt ilyen nevű kölcsönző");
            }


        }
        private static void Idopontmegadas()
        {
            Console.Write($"7. feladat: Adjon meg egy időpontot óra:perc alakban: ");
            string megadottido = Console.ReadLine();
            string[] darabol = megadottido.Split(':');
            int Elperc, Visszaperc, KolcsonzesIdo;
            Console.WriteLine("\tA vizen lévő járművek:");
            foreach (Kolcsonzes item in adatok)
            {
                Elperc = (item.Elvitel_Ora * 60) + item.Elvitel_Perc;
                Visszaperc = (item.Visszahoz_Ora * 60) + item.Visszahoz_Perc;
                KolcsonzesIdo = (Convert.ToInt32(darabol[0]) * 60) + Convert.ToInt32(darabol[1]);

                if (Elperc <= KolcsonzesIdo && Visszaperc >= KolcsonzesIdo)
                {
                    if (Convert.ToString(item.Elvitel_Ora).Length<2)
                    {
                        Console.Write($"\t0{item.Elvitel_Ora}:");
                    }
                    else
                    {
                        Console.Write($"\t{item.Elvitel_Ora}:");
                    }
                    if (Convert.ToString(item.Elvitel_Perc).Length < 2)
                    {
                        Console.Write($"0{item.Elvitel_Perc}-");
                    }
                    else
                    {
                        Console.Write($"{item.Elvitel_Perc}-");
                    }
                    if (Convert.ToString(item.Visszahoz_Ora).Length < 2)
                    {
                        Console.Write($"0{item.Visszahoz_Ora}:");
                    }
                    else
                    {
                        Console.Write($"{item.Visszahoz_Ora}:");
                    }
                    if (Convert.ToString(item.Visszahoz_Perc).Length < 2)
                    {
                        Console.WriteLine($"0{item.Visszahoz_Perc} : {item.Nev}");
                    }
                    else
                    {
                        Console.WriteLine($"{item.Visszahoz_Perc} : {item.Nev}");
                    }
        
                }
            
            }

        }
        public static void FMO8()
        {
            double osszesen;
            int egyseg=0;
            for (int i = 0; i < adatok.Count; i++)
            {
                osszesen = ((adatok[i].Visszahoz_Ora - adatok[i].Elvitel_Ora) * 60) + (adatok[i].Visszahoz_Perc - adatok[i].Elvitel_Perc);
                egyseg += (int)Math.Ceiling(osszesen / 30);
            }
            Console.WriteLine($"8. feladat: A napi bevétel {egyseg*2400} Ft");
        }
        public static void FMO9()
        {
            List<Kolcsonzes> f = adatok.Where(x => x.Jazonosito == 'F').ToList();
            try
            {
                StreamWriter Iro = new StreamWriter("F.txt");
                for (int i = 0; i < adatok.Count; i++)
                {
                    Iro.WriteLine($"{adatok[i].Elvitel_Ora}:{adatok[i].Elvitel_Perc} - {adatok[i].Visszahoz_Ora}:{adatok[i].Visszahoz_Perc} : {adatok[i].Nev}");
                }
                Iro.Close();
                Console.WriteLine("9. feladat: Az F.txt irása sikeresen megtörtént.");
            }
            catch (Exception e)
            { 
                Console.WriteLine(e.ToString());
            }
        }
        public static void FMO10()
        {
            Console.WriteLine("10. feladat: Statisztika");
            foreach (var item in adatok.GroupBy(x=>x.Jazonosito).OrderBy(x=>x.Key))
            {
                Console.WriteLine($"\t{item.Key} - {item.Count()}");
            }
        }

    }

    
    
}
