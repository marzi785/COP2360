// Nested Types - Basic Usage
using System;

// Object initialization syntax.

Bunny b1 = new Bunny { Name = "Bo", LikesCarrots = true, LikesHumans = false };
Bunny b2 = new Bunny ("Bo")     { LikesCarrots=true, LikesHumans=false };

Console.WriteLine(b1.Name);
Console.WriteLine(b1.LikesCarrots);
Console.WriteLine (b1.LikesHumans);

Console.WriteLine(b2.Name);
Console.WriteLine(b2.LikesCarrots);
Console.WriteLine (b2.LikesHumans);

public class Bunny
{
    public string Name;
    public Bunny() { }
    public Bunny(string n) { Name = n; }
}

public class Carrots : Bunny   // inherits from Bunny
{
	public bool LikesCarrots;
}

public class Humans : Bunny  // inherits from Bunny
{
    public bool LikesHumans;
}
