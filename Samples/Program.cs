﻿using TeenyMapper;

var b =  Mapper.Map<B>(new A { Id = 2, Name = "Nicolas" });
Console.WriteLine($"Id {b.Id} \nName : {b.Name}");

public class A
{
    public int Id { get; set; } = 1;
    public string Name { get; set; }
}
public class B
{
    public int Id { get; set; }
    public string Name { get; set; }
}
