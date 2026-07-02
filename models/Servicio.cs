using System;

namespace barberiaApi;

public class Servicio
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Duration { get; set; }
    public int CategoryId { get; set; }
    public bool IsActive { get; set; }
}
