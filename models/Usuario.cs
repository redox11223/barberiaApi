namespace barberiaApi;

public record class Usuario(
    int Id,
    string FullName,
    string Email,
    string Password,
    string Role,
    string Phone,
    bool IsActive
)
{

}
