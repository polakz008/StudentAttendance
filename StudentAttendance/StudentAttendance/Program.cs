using System;
using System.Collections.Generic;
using System.IO;

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
                return;
            }

            Console.WriteLine("Lista studentów:");
            foreach (var student in studenci)
            {
                Console.WriteLine($"{student.ToString()}");
            }

            Console.WriteLine("");
            Console.Write("Podaj format zapisu txt/csv: ");
            string format = Console.ReadLine().ToUpper();

            // Ścieżka zapisu na pulpicie
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
                    // Nagłówki kolumn
                    sw.WriteLine("Imie,Nazwisko,Obecny");
                    foreach (var student in studenci)
                    {
                        // Zapisujemy dane w formacie CSV
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
                        // Zapisujemy dane w formacie tekstowym
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
    }
}

    
