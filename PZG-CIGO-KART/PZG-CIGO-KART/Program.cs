using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PZG_CIGO_KART
{
    #region gokart_class
    public class gokart
    {
        public string vezeteknev;
        public string keresztnev;
        public DateTime szuletes;
        public bool elmult;
        public string versenyzoazonosito;
        public string email;
        public DateTime datum;
        public int idopont;
        public int orak;
        public gokart(string vezeteknev, string keresztnev, DateTime szuletes, bool elmult, string versenyzoazonosito, string email, DateTime datum, int idopont, int orak)
        {
            this.vezeteknev = vezeteknev;
            this.keresztnev = keresztnev;
            this.szuletes = szuletes;
            this.elmult = elmult;
            this.versenyzoazonosito = versenyzoazonosito;
            this.email = email;
            this.datum = datum;
            this.idopont = idopont;
            this.orak = orak;
        }
        public override string ToString()
        {
            return $"{szuletes:yyyyMMdd}";
        }
    }
    #endregion
    
    internal class Program
    {
        #region ekezet_mentesito
        static string Ekezetmentesito(string szoveg)
        {
            return szoveg
                .Replace("á", "a")
                .Replace("é", "e")
                .Replace("í", "i")
                .Replace("ó", "o")
                .Replace("ö", "o")
                .Replace("ő", "o")
                .Replace("ú", "u")
                .Replace("ü", "u")
                .Replace("ű", "u");
        }
        #endregion

        #region Tabla_kiiro
        static void TablaKiiro(List<Dictionary<DateTime, List<int>>> tabla)
        {
            Console.WriteLine("\n\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("██ - Nincs elég versenyző");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("██ - Van elég versenyző");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("██ - Tele van");
            Console.ResetColor();
            
            Console.WriteLine("\n\t\t8-9 \t 9-10 \t 10-11 \t 11-12 \t 12-13 \t 13-14 \t 14-15 \t 15-16 \t 16-17 \t 17-18 \t 18-19");

            foreach (var item in tabla)
            {
                foreach (var item2 in item)
                {
                    Console.Write(item2.Key.ToString("yyyy/MM/dd") + " |\t ");
                    foreach (var item3 in item2.Value)
                    {
                        if (item3 < 8)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(item3 + " \t ");
                        }
                        if (item3 >= 8 && item3 < 20)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(item3 + " \t ");
                        }
                        if (item3 == 20)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(item3 + " \t ");
                        }
                        Console.ResetColor();
                    }

                    Console.WriteLine();
                }
            }
            Console.WriteLine("\n\n");
        }
        
        #endregion

        #region  Foglalas
        static (List<gokart>, List<Dictionary<DateTime, List<int>>>) Foglalas(List<Dictionary<DateTime, List<int>>> tabla, List<gokart> gokart, string beazonosito, int hatralevo, int mod)
        {
            List<string> orak = new List<string>() {"8-9", "9-10", "10-11", "11-12", "12-13", "13-14", "14-15", "15-16", "16-17", "17-18", "18-19"};

            DateTime idopont = DateTime.Now;
            DateTime athelyezett_idopont = DateTime.Now;
            string ora = "";
            string athelyezett_ora = "";
            int hossz = 0;
            int oraindex = 0;
            int athelyezett_oraindex = 0;
            
            #region Datum
            bool valos_datum = false;
            bool valos_athelyezett_datum = false;
            while (!valos_datum && !valos_athelyezett_datum)
            {
                switch (mod)
                {
                    case 0:
                        Console.Write("\nHova szeretne foglalni (yyyy.MM.dd): ");
                        idopont = Convert.ToDateTime(Console.ReadLine());
                        break;
                    case 1:
                        Console.Write("\nHonnan szeretne módosítani (yyyy.MM.dd): ");
                        idopont = Convert.ToDateTime(Console.ReadLine());

                        Console.Write("\nHova szeretné áthelyezni (yyyy.MM.dd): ");
                        athelyezett_idopont = Convert.ToDateTime(Console.ReadLine());
                        break;
                    case 2:
                        Console.Write("\nHonnan szeretne törölni (yyyy.MM.dd): ");
                        idopont = Convert.ToDateTime(Console.ReadLine());
                        break;
                }
                if (idopont >= DateTime.Now.Date &&  idopont <= DateTime.Now.AddDays(hatralevo))
                {
                    valos_datum = true; 
                }
                if (athelyezett_idopont >= DateTime.Now.Date &&  athelyezett_idopont <= DateTime.Now.AddDays(hatralevo))
                {
                    valos_athelyezett_datum = true; 
                }
                else
                    Console.WriteLine("Helytelen!");
            }
            #endregion
            
            #region Ora
            bool valos_ora = false;
            bool athelyezett_valos_ora = false;
            while (!valos_ora && !athelyezett_valos_ora)
            {
                switch (mod)
                {
                    case 0:
                        Console.Write("\nMelyik időpont (8-9 9-10 10-11 11-12 12-13 13-14 14-15 15-16 16-17 17-18 18-19): ");
                        ora = Console.ReadLine();
                        break;
                    case 1:
                        Console.Write("\nMelyik időpontról (8-9 9-10 10-11 11-12 12-13 13-14 14-15 15-16 16-17 17-18 18-19): ");
                        ora = Console.ReadLine();
                        
                        Console.Write("\nMelyik időpontra (8-9 9-10 10-11 11-12 12-13 13-14 14-15 15-16 16-17 17-18 18-19): ");
                        athelyezett_ora = Console.ReadLine();
                        
                        if (athelyezett_ora == ora)
                        {
                            Console.WriteLine("Nem helyezheted át ugyanarra az időpontra!");
                        }
                        else
                        {
                            athelyezett_valos_ora = true;
                            switch (athelyezett_ora)
                            {
                                case "8-9": athelyezett_oraindex = 0; break;
                                case "9-10": athelyezett_oraindex = 1; break;
                                case "10-11": athelyezett_oraindex = 2; break;
                                case "11-12": athelyezett_oraindex = 3; break;
                                case "12-13": athelyezett_oraindex = 4; break;
                                case "13-14": athelyezett_oraindex = 5; break;
                                case "14-15": athelyezett_oraindex = 6; break;
                                case "15-16": athelyezett_oraindex = 7; break;
                                case "16-17": athelyezett_oraindex = 8; break;
                                case "17-18": athelyezett_oraindex = 9; break;
                                case "18-19": athelyezett_oraindex = 10; break;
                                default:
                                    athelyezett_valos_ora = false;
                                    Console.WriteLine("Hibás időpontot adtál meg!");
                                    break;
                            }
                        }

                        break;
                    case 2:
                        Console.Write("\nMelyik időpont (8-9 9-10 10-11 11-12 12-13 13-14 14-15 15-16 16-17 17-18 18-19): ");
                        ora = Console.ReadLine();
                        break;
                }
                
                if (orak.Contains(ora))
                {
                    valos_ora = true;
                    switch (ora)
                    {
                        case "8-9": oraindex = 0; break;
                        case "9-10": oraindex = 1; break;
                        case "10-11": oraindex = 2; break;
                        case "11-12": oraindex = 3; break;
                        case "12-13": oraindex = 4; break;
                        case "13-14": oraindex = 5; break;
                        case "14-15": oraindex = 6; break;
                        case "15-16": oraindex = 7; break;
                        case "16-17": oraindex = 8; break;
                        case "17-18": oraindex = 9; break;
                        case "18-19": oraindex = 10; break;
                        default:
                            athelyezett_valos_ora = false;
                            Console.WriteLine("Hibás időpontot adtál meg!");
                            break;
                    }
                }
                else
                    Console.WriteLine("Helytelen!");
            }
            #endregion
            
            #region Hossz
            bool valos_hossz = false;
            while (!valos_hossz)
            {
                switch (mod)
                {
                    case 0:
                        Console.Write("\nMennyi időre (1/2): ");
                        hossz = Convert.ToInt32(Console.ReadLine());
                        break;
                    case 1:
                        Console.Write("\nMennyi időt (1/2): ");
                        hossz = Convert.ToInt32(Console.ReadLine());
                        break;
                    case 2:
                        Console.Write("\nMennyi időt (1/2): ");
                        hossz = Convert.ToInt32(Console.ReadLine());
                        break;
                }
                
                if (hossz == 1 || (hossz == 2 && oraindex != 10 && athelyezett_oraindex != 10))
                {
                    valos_hossz = true;
                }
                else
                    Console.WriteLine("Helytelen!");
            }
            #endregion
            
            #region Tabla modositas
            foreach (var item in tabla)
            {
                foreach (var item2 in item)
                {
                    switch (mod)
                    {
                        case 0:
                            if (item2.Key.Date == idopont.Date) //hulye masodpercek
                            {
                                if (hossz == 1 && item2.Value[oraindex] < 20)
                                {
                                    item2.Value[oraindex]++;
                                }
                                else if (hossz == 2 && item2.Value[oraindex] < 20 && item2.Value[oraindex + 1] < 20)
                                {
                                    item2.Value[oraindex]++;
                                    item2.Value[oraindex + 1]++;
                                }
                            }
                            break;
                        case 1:
                            if (item2.Key.Date == idopont.Date) //hulye masodpercek
                            {
                                if (hossz == 1 && item2.Value[oraindex] < 20 && item2.Value[athelyezett_oraindex] < 20)
                                {
                                    item2.Value[oraindex]--;
                                }
                                else if (hossz == 2 && item2.Value[oraindex] < 20 && item2.Value[oraindex + 1] < 20 && item2.Value[athelyezett_oraindex] < 20 && item2.Value[athelyezett_oraindex + 1] < 20)
                                {
                                    item2.Value[oraindex]--;
                                    item2.Value[oraindex + 1]--;
                                }
                            }
                            else if (item2.Key.Date == athelyezett_idopont.Date)
                            {
                                if (hossz == 1 && item2.Value[oraindex] < 20 && item2.Value[athelyezett_oraindex] < 20)
                                {
                                    item2.Value[athelyezett_oraindex]++;
                                }
                                else if (hossz == 2 && item2.Value[oraindex] < 20 && item2.Value[oraindex + 1] < 20 && item2.Value[athelyezett_oraindex] < 20 && item2.Value[athelyezett_oraindex + 1] < 20)
                                {
                                    item2.Value[athelyezett_oraindex]++;
                                    item2.Value[athelyezett_oraindex + 1]++;
                                }
                            }
                            break;
                        case 2:
                            if (item2.Key.Date == idopont.Date) //hulye masodpercek
                            {
                                if (hossz == 1 && item2.Value[oraindex] < 20)
                                {
                                    item2.Value[oraindex]--;
                                }
                                else if (hossz == 2 && item2.Value[oraindex] < 20 && item2.Value[oraindex + 1] < 20)
                                {
                                    item2.Value[oraindex]--;
                                    item2.Value[oraindex + 1]--;
                                }
                            }
                            break;
                    }
                }
            }
            #endregion
            
            #region Versenyzo modositas
            switch (mod)
            {
                case 0:
                    foreach (var item in gokart)
                    {
                        foreach (var item2 in tabla)
                        {
                            foreach (var item3 in item2)
                            {
                                if (beazonosito == item.versenyzoazonosito && item3.Value[oraindex] < 20 && item3.Value[oraindex + 1] < 20)
                                {
                                    item.datum = idopont;
                                    item.idopont = oraindex;
                                    item.orak = hossz;
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    foreach (var item in gokart)
                    {
                        foreach (var item2 in tabla)
                        {
                            foreach (var item3 in item2)
                            {
                                if (beazonosito == item.versenyzoazonosito && item3.Value[oraindex] < 20 && item3.Value[oraindex + 1] < 20)
                                {
                                    item.datum = athelyezett_idopont;
                                    item.idopont = athelyezett_oraindex;
                                    item.orak = hossz;
                                }
                            }
                        }
                    }
                    break;
                case 2:
                    foreach (var item in gokart)
                    {
                        item.datum = new DateTime(1900,01,01);
                        item.idopont = 0;
                        item.orak = 0;
                    }
                    break;
            }
            #endregion
            
            Console.WriteLine("Módosítva!");
            return (gokart, tabla);
        }
        #endregion
        
        #region GokartKiiras
        static void GokartKiiras(List<gokart> gokartok)
        {
            foreach (var item in gokartok)
            {
                Console.Write($"{item.versenyzoazonosito, -30}");
                if (item.datum != new DateTime(1900, 01, 01))
                {
                    Console.WriteLine($"\t{item.datum.ToString("yyyy.MM.dd")}. napon - {item.idopont + 1}. oszlop - {item.orak} óra");
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }
        #endregion
        static void Main(string[] args)
        {

            #region fejlec

            CultureInfo culture = new CultureInfo("hu-HU"); //Mac-Windows file kezelés
            Thread.CurrentThread.CurrentCulture = culture; // "." és "," ugyan úgy működjön
            Thread.CurrentThread.CurrentUICulture = culture; //using System.Globalization; kell
            //using System.Threading; kell
            /*

            PZG - CIGO-KART
            PZG - 2025.09.15

            Gokart időpontfoglaló - Egyéni kisprojekt

            */

            string fejlec = "PZG - CIGO-KART";
            Console.WriteLine(fejlec);
            for (int i = 0; i < fejlec.Length; i++) Console.Write('-');
            Console.WriteLine();

            #endregion

            //--------------------------------------------------------------------------------------//
            #region adatok
            string nev = "ciGO-KART";
            string cim = "7586 Sárhányóköz, Jocói körút 4.";
            string tel = "+362030507090";
            string webcim = "www.cigo-kart.hu";
            Console.WriteLine($"{nev}\n{cim}\n{tel}\n{webcim}");
            for (int i = 0; i < fejlec.Length; i++) Console.Write('-');
            Console.WriteLine();
            #endregion
            //--------------------------------------------------------------------------------------//
            #region gokartok_feltoltes

            List<gokart> gokartok = new List<gokart>();
            List<string> vezeteknevek = new List<string>();
            List<string> keresztnevek = new List<string>();

            StreamReader vezetek = new StreamReader("vezeteknevek.txt", Encoding.UTF8);
            StreamReader kereszt = new StreamReader("keresztnevek.txt", Encoding.UTF8);

            string sor = vezetek.ReadLine();
            string temp_sor = sor.Replace(" ", "");
            string[] vnev = temp_sor.Split(',');
            foreach (var item in vnev)
            {
                vezeteknevek.Add(item);
            }

            string sor2 = kereszt.ReadLine();
            string temp_sor2 = sor2.Replace(" ", "");
            string[] knev = temp_sor2.Split(',');
            foreach (var item in knev)
            {
                keresztnevek.Add(item);
            }

            vezetek.Close();
            kereszt.Close();
            //--------------------------------------------------------------------------------------//
            Random rnd = new Random();
            int letszam = rnd.Next(1, 150 + 1);
            for (int i = 0; i < letszam; i++)
            {
                DateTime temp_datum = new DateTime(1900, 01, 01);
                int temp_szam = 0;            
                int vszam = rnd.Next(0, vezeteknevek.Count());
                int kszam = rnd.Next(0, keresztnevek.Count());
                int evszam = rnd.Next(1900, DateTime.Now.Year + 1);
                int honap = rnd.Next(1, 12 + 1);
                int nap = rnd.Next(1, DateTime.DaysInMonth(evszam, honap) + 1);
                DateTime szuletes = new DateTime(evszam, honap, nap);
                bool elmult = DateTime.Now >= szuletes.AddYears(18);
                string azonosito = $"GO-{Ekezetmentesito(vezeteknevek[vszam])}{Ekezetmentesito(keresztnevek[kszam])}-{evszam}{honap:D2}{nap:d2}";
                string email =
                    $"{Ekezetmentesito(vezeteknevek[vszam]).ToLower()}.{Ekezetmentesito(keresztnevek[kszam]).ToLower()}@gmail.com";
                gokart gokart = new gokart(vezeteknevek[vszam], keresztnevek[kszam], Convert.ToDateTime(szuletes.ToString("yyyy/MM/dd")), elmult, azonosito, email, temp_datum, temp_szam, temp_szam);
                gokartok.Add(gokart);
            }
            #endregion
            //--------------------------------------------------------------------------------------//
            #region tablaletrehozas
            List<Dictionary<DateTime, List<int>>> tabla = new List<Dictionary<DateTime, List<int>>>();
            int hatralevo = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day +1;
            //<8-20<

            for (int i = 0; i < hatralevo; i++)
            {
                List<int> empty = new List<int>() {0,0,0,0,0,0,0,0,0,0,0};
                DateTime ido = DateTime.Now.AddDays(i);
                Dictionary<DateTime, List<int>> temp = new Dictionary<DateTime, List<int>>();
                temp.Add(ido, empty);
                tabla.Add(temp);
            }
            #endregion
            //--------------------------------------------------------------------------------------//
            #region Menu
            string valasztas = "";
            while (valasztas != "Kilépés" && valasztas != "Kilepes" && valasztas != "kilépés" && valasztas != "kilepes" && valasztas != "Exit" && valasztas != "exit")
            {
                valasztas = "";
                int mod = -1;
                int case_mod = -1;
                Console.WriteLine("Foglalás || Módosítás || Törlés");
                Console.Write("Válasszon egy módot (Kilépéshez: 'Kilépés'): ");
                valasztas = Console.ReadLine();
                if (valasztas == "Foglalás" || valasztas == "Foglalas" || valasztas == "foglalás" || valasztas == "foglalas")
                {
                    case_mod = 0;
                }
                else if (valasztas == "Módosítás" || valasztas == "Modositas" || valasztas == "modósítás" || valasztas == "modositas")
                {
                    case_mod = 1;
                }
                else if (valasztas == "Törlés" || valasztas == "törlés" || valasztas == "Torles" || valasztas == "torles")
                {
                    case_mod = 2;
                }
                string beazonosito = "";
                switch (case_mod)
                {
                    case 0:
                        mod = 0;
                        while (beazonosito != "Kilépés" && beazonosito != "Kilepes" && beazonosito != "kilépés" && beazonosito != "kilepes" && beazonosito != "Exit" && beazonosito != "exit")
                        { 
                            Console.Clear();
                            GokartKiiras(gokartok);
                            TablaKiiro(tabla);
                
                            bool letezik = false; 
                            Console.Write("Adjon meg egy azonosítot (Kilépéshez: 'Kilépés'): ");
                            beazonosito = Console.ReadLine();
                            foreach (var item in gokartok)
                            {
                                if (item.versenyzoazonosito == beazonosito && item.datum == new DateTime(1900,01,01))
                                {
                                    letezik = true;
                                    (gokartok, tabla) = Foglalas(tabla, gokartok, beazonosito, hatralevo, mod);
                                }
                            } 
                            if (!letezik)
                                Console.WriteLine("Nincs ilyen azonosító, vagy már van időpontja!"); 
                        }
                        break;
                    case 1:
                        mod = 1;
                        while (beazonosito != "Kilépés" && beazonosito != "Kilepes" && beazonosito != "kilépés" && beazonosito != "kilepes" && beazonosito != "Exit" && beazonosito != "exit")
                        { 
                            Console.Clear();
                            GokartKiiras(gokartok);
                            TablaKiiro(tabla);
                
                            bool letezik = false; 
                            Console.Write("Adjon meg egy azonosítot (Kilépéshez: 'Kilépés'): ");
                            beazonosito = Console.ReadLine();
                            foreach (var item in gokartok)
                            {
                                if (item.versenyzoazonosito == beazonosito)
                                {
                                    letezik = true;
                                    (gokartok, tabla) = Foglalas(tabla, gokartok, beazonosito, hatralevo, mod);
                                }
                            } 
                            if (!letezik)
                                Console.WriteLine("Nincs ilyen azonosító!"); 
                        }
                        break;
                    case 2:
                        mod = 2;
                        while (beazonosito != "Kilépés" && beazonosito != "Kilepes" && beazonosito != "kilépés" && beazonosito != "kilepes" && beazonosito != "Exit" && beazonosito != "exit")
                        { 
                            Console.Clear();
                            GokartKiiras(gokartok);
                            TablaKiiro(tabla);
                
                            bool letezik = false; 
                            Console.Write("Adjon meg egy azonosítot (Kilépéshez: 'Kilépés'): ");
                            beazonosito = Console.ReadLine();
                            foreach (var item in gokartok)
                            {
                                if (item.versenyzoazonosito == beazonosito)
                                {
                                    letezik = true;
                                    (gokartok, tabla) = Foglalas(tabla, gokartok, beazonosito, hatralevo, mod);
                                }
                            } 
                            if (!letezik)
                                Console.WriteLine("Nincs ilyen azonosító!"); 
                        }
                        break;
                }
            }
            #endregion
            //--------------------------------------------------------------------------------------//

            Console.WriteLine();
            Console.WriteLine("Kilépéshez nyomja meg az ENTER billentyűt!");
            Console.ReadLine();
        }
    }
}
