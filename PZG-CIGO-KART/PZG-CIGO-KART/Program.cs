using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PZG_CIGO_KART
{
    #region gokart_class
    public class gokart
    {
        private string vezeteknev;
        private string keresztnev;
        private DateTime szuletes;
        private bool elmult;
        private string versenyzoazonosito;
        private string email;


        public gokart(string vezeteknev, string keresztnev, DateTime szuletes, bool elmult, string versenyzoazonosito,
            string email)
        {
            this.vezeteknev = vezeteknev;
            this.keresztnev = keresztnev;
            this.szuletes = szuletes;
            this.elmult = elmult;
            this.versenyzoazonosito = versenyzoazonosito;
            this.email = email;

        }
    }
    #endregion
    public class Foglalas
    {
        public int Nap { get; set; }
        public DateTime Idopont { get; set; }
        public int Db { get; set; } 
        /*
        public string Idopont_nyolc { get; set; }
        public int Db_nyolc { get; set; }  
        
        public string Idopont_kilenc { get; set; }
        public int Db_kilenc { get; set; }          
        
        public string Idopont_tiz { get; set; }
        public int Db_tiz { get; set; }   
        
        public string Idopont_tegy { get; set; }
        public int Db_tegy { get; set; }   
        
        public string Idopont_tketto { get; set; }
        public int Db_tketto { get; set; }   
        
        public string Idopont_tharom { get; set; }
        public int Db_tharom { get; set; }   
        
        public string Idopont_tnegy { get; set; }
        public int Db_tnegy { get; set; }   
        
        public string Idopont_tot { get; set; }
        public int Db_tot { get; set; }   
        
        public string Idopont_that { get; set; }
        public int Db_that { get; set; }   
        
        public string Idopont_thet { get; set; }
        public int Db_thet { get; set; }   
        
        public string Idopont_tnyolc { get; set; }
        public int Db_tnyolc { get; set; }   
        */
    }
    internal class Program
    {
        #region ekezet_mentesito
        static int LinearisKereses(string miben, char mit)
        {
            int i = 0;
            while (i < miben.Length && miben[i] != mit)
            {
                i++;
            }

            return (i < miben.Length) ? i : -1;
        }

        static string Ekezetmentesito(string szoveg)
        {
            string ekezetmentesSzoveg = string.Empty;
            string csere = "öüóőúéáűí";
            string mire = "ouooueaui";
            for (int i = 0; i < szoveg.Length; i++)
            {
                int index = LinearisKereses(csere, szoveg[i]);
                if (index == -1)
                {
                    ekezetmentesSzoveg += szoveg[i];
                }
                else
                {
                    ekezetmentesSzoveg += mire[index];
                }
            }
            return ekezetmentesSzoveg;
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
            string[] vnev = sor.Split(',');
            foreach (var item in vnev)
            {
                vezeteknevek.Add(item);
            }

            string sor2 = kereszt.ReadLine();
            string[] knev = sor2.Split(',');
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
                int vszam = rnd.Next(0, vezeteknevek.Count());
                int kszam = rnd.Next(0, keresztnevek.Count());
                int evszam = rnd.Next(1900, DateTime.Now.Year + 1);
                int honap = rnd.Next(1, 12 + 1);
                int nap = rnd.Next(1, DateTime.DaysInMonth(evszam, honap) + 1);
                DateTime szuletes = new DateTime(evszam, honap, nap);
                bool elmult;
                if (DateTime.Now.Year - evszam >= 18)
                    elmult = true;
                else
                    elmult = false;
                string azonosito =
                    $"GO-{Ekezetmentesito(vezeteknevek[vszam])}{Ekezetmentesito(keresztnevek[kszam])}{evszam}{honap}{nap}";
                string email =
                    $"{Ekezetmentesito(vezeteknevek[vszam]).ToLower()}.{Ekezetmentesito(keresztnevek[kszam]).ToLower()}@gmail.com";
                gokart gokart = new gokart(vezeteknevek[vszam], keresztnevek[kszam], szuletes, elmult, azonosito,
                    email);
                gokartok.Add(gokart);
            }
            #endregion
            //--------------------------------------------------------------------------------------//
            List<Foglalas> foglalasok = new List<Foglalas>();
            

            int hatralevo = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day;
            //<8-20<

            for (int i = 0; i < letszam; i++)
            {
                int orak = rnd.Next(1, 2 + 1);
                int nap = rnd.Next(DateTime.Now.Day, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + 1);
                int ora = rnd.Next(8, 19 - orak + 1);

                DateTime kezd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, nap, ora, 0, 0);

                foglalasok.Add(new Foglalas {Nap = nap, Idopont = kezd, Db = orak });
            }

            




            //--------------------------------------------------------------------------------------//

            Console.WriteLine();
            Console.WriteLine("Kilépéshez nyomja meg az ENTER billentyűt!");
            Console.ReadLine();
        }
    }
}
