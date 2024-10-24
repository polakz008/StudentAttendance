public class Students
{
    public string Imie { get; set; }
    public string Nazwisko { get; set; }
    public bool Obecny { get; set; }

    public Students(string imie, string nazwisko)
    {
        Imie = imie;
        Nazwisko = nazwisko;
        Obecny = false; 
    }

    public override string ToString()
    {
        return $"{Imie} {Nazwisko} | {(Obecny ? "Obecny" : "Nieobecny")}";
    }

    public string ToCsv()
    {
        return $"{Imie},{Nazwisko},{(Obecny ? "Obecny" : "Nieobecny")}";
    }
}


