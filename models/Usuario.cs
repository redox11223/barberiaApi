namespace barberiaApi;

public class Usuario{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? Role { get; set; }
    public required string Phone { get; set; }
    public bool IsActive { get; set; }

}