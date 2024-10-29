using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StudentAttendance
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Ścieżka do pliku wejściowego na pulpicie
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string inputFilePath = Path.Combine(desktopPath, "Obecni.txt");

            // Import studentów
            List<Students> studenci = ImportStudentow(inputFilePath);

            if (studenci.Count == 0)
            {
                Console.WriteLine("Plik nie zawiera studentów lub nie istnieje.");
            }
            else
            {
                //podanie daty
                Console.WriteLine("Podaj dzisiejszą datę:");
                DateTime data = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Lista studentów:");
                foreach (var student in studenci)
                {
                    Console.WriteLine($"{student.ToString()}");
                }
            }

            // Funkcja sprawdzania obecności
            Console.Write("Czy chcesz sprawdzić obecność studentów? (tak/nie): ");
            if (Console.ReadLine().ToLower() == "tak")
            {
                SprawdzObecnosc(studenci);
            }

            // Dodawanie nowego studenta
            Console.Write("Czy chcesz dodać nowego studenta? (tak/nie): ");
            if (Console.ReadLine().ToLower() == "tak")
            {
                DodajNowegoStudenta(studenci);
            }

            //Edycja listy obecnosci
            Console.WriteLine("Czy chcesz edytować listę obecności? (tak/nie) ");
            if (Console.ReadLine().ToLower() == "tak")
            {
                SprawdzObecnosc(studenci);
            }

            // Eksport do formatu wybranego przez użytkownika
            Console.Write("Podaj format zapisu txt/csv: ");
            string format = Console.ReadLine().ToUpper();
            string outputFilePath = Path.Combine(desktopPath, format == "CSV" ? "Obecni_Export.csv" : "Obecni_Export.txt");

            if (format == "CSV")
            {
                EksportDoCsv(studenci, outputFilePath);
            }
            else if (format == "TXT")
            {
                EksportDoTxt(studenci, outputFilePath);
            }
            else
            {
                Console.WriteLine("Nieznany format.");
            }

            Console.WriteLine($"Zapisano plik na pulpicie: {outputFilePath}");
            Console.ReadKey();
        }

        static List<Students> ImportStudentow(string filePath)
        {
            List<Students> studenci = new List<Students>();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    string[] data = line.Split(' ');
                    if (data.Length >= 2)
                    {
                        studenci.Add(new Students(data[0], data[1]));
                    }
                }
            }
            else
            {
                Console.WriteLine("Plik nie istnieje.");
            }

            return studenci;
        }

        static void EksportDoCsv(List<Students> studenci, string filePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine("Imie,Nazwisko,Obecny");
                    foreach (var student in studenci)
                    {
                        sw.WriteLine(student.ToCsv());
                    }
                }
                Console.WriteLine($"Zapisano plik CSV: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy zapisie pliku CSV: {ex.Message}");
            }
        }

        static void EksportDoTxt(List<Students> studenci, string filePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    foreach (var student in studenci)
                    {
                        sw.WriteLine(student.ToString());
                    }
                }
                Console.WriteLine($"Zapisano plik TXT: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy zapisie pliku TXT: {ex.Message}");
            }
        }

        // Funkcja do dodawania nowego studenta
        static void DodajNowegoStudenta(List<Students> studenci)
        {
            Console.Write("Podaj imię studenta: ");
            string imie = Console.ReadLine();
            Console.Write("Podaj nazwisko studenta: ");
            string nazwisko = Console.ReadLine();
            //funkcja sprawdzania obecności
            Console.Write("Czy student jest obecny? (tak/nie): ");
            bool obecnosc = Console.ReadLine().ToLower() == "tak";

            Students nowyStudent = new Students(imie, nazwisko, obecnosc);
            studenci.Add(nowyStudent);

            Console.WriteLine($"Dodano nowego studenta: {nowyStudent.ToString()}");
        }
        static void SprawdzObecnosc(List<Students> studenci)
        {
            foreach (var student in studenci)
            {
                Console.Write($"Czy {student.Imie} {student.Nazwisko} jest obecny? (tak/nie): ");
                bool obecnosc = Console.ReadLine().ToLower() == "tak";

                // aktualizacja obecności
                student.Obecnosc = obecnosc;
            }
        }
    }

    public class Students
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public bool Obecnosc { get; set; }

        public Students(string imie, string nazwisko, bool obecnosc)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            Obecnosc = obecnosc; //funkcja sprawdzania obecnosci
        }
        public Students(string imie, string nazwisko)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            Obecnosc = false; //funckja sprawdzania obecnosci
        }

        public override string ToString()
        {
            return $"{Imie} {Nazwisko} - {(Obecnosc ? "Obecny" : "Nieobecny")}";
        }

        public string ToCsv()
        {
            return $"{Imie},{Nazwisko},{(Obecnosc ? "Nieobecny" : "Obecny")}";
        }
    }

}


