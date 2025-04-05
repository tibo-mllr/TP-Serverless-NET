class Personne
{
    public string nom { get; set; }
    public int age { get; set; }

    public Personne(string nom, int age)
    {
        this.nom = nom;
        this.age = age;
    }

    public string Hello(bool isLowerCase)
    {
        string res = "hello " + nom + ", your are " + age;

        if (isLowerCase)
            return res;
        return res.ToUpper();
    }

}