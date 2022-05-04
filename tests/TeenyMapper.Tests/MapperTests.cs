namespace TeenyMapper.Tests;
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

public class MapperTests
{
    [Fact]
    public void Map_shouldMap_SameProperties()
    {
        var a = new A { Id = 1, Name = "abc" };
        var b =  Mapper.Map<B>(a);
        Assert.Equal(a.Id,b.Id);
        Assert.Equal(a.Name,b.Name);
    }

}