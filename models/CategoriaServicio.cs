using System;

namespace barberiaApi;

public class CategoriaServicio
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
