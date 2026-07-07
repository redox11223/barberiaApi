using barberiaApi;
using barberiaApi.models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Usuario> usuarios =
[
    new (){ Id = 1, FullName = "Miguel Angel", Email = "admin@gmail.com", Password = "123456", Role = "Admin", Phone = "987654321", IsActive = true },
    new (){ Id = 2, FullName = "Pedro Trillo", Email = "pedro@gmail.com", Password = "123456", Role = "User", Phone = "987654320", IsActive = true }
];

List<CategoriaServicio> categorias =
[
    new() { Id = 1, Name = "Cortes", Description = "Servicio de corte de cabello" },
    new() { Id = 2, Name = "Barba", Description = "Servicio de afeitado y diseño de barba" },
    new() { Id = 3, Name = "Cejas", Description = "Servicio de diseño de cejas" }
];

List<Servicio> servicios =
[
    new() { Id = 1, Name = "Corte Clásico", Description = "Corte clásico de cabello para caballeros", Price = 15.00m, Duration = 30, CategoryId = 1, IsActive = true },
    new() { Id = 2, Name = "Degradado", Description = "Fade limpio y preciso con acabado profesional", Price = 25.00m, Duration = 30, CategoryId = 1, IsActive = true },
    new() { Id = 3, Name = "Afeitado de barba", Description = "Afeitado de barba con navaja", Price = 10.00m, Duration = 20, CategoryId = 2, IsActive = true },
    new() { Id = 4, Name = "Diseño de cejas", Description = "Diseño de cejas con pinzas y cera", Price = 8.00m, Duration = 15, CategoryId = 3, IsActive = true }
];

var usuario=app.MapGroup("/usuarios");
var categoria=app.MapGroup("/categorias");
var servicio=app.MapGroup("/servicios");

//Usuarios
usuario.MapGet("/", () => usuarios);
usuario.MapGet("/{id}", (int id) => usuarios.FirstOrDefault(u => u.Id == id));
usuario.MapPut("/{id}", (int id, Usuario updatedUsuario) =>
{
    var usuario = usuarios.FirstOrDefault(u => u.Id == id);
    if (usuario is null)
    {
        return Results.NotFound();
    }

    usuario.FullName = updatedUsuario.FullName;
    usuario.Email = updatedUsuario.Email;
    usuario.Password = updatedUsuario.Password;
    usuario.Phone = updatedUsuario.Phone;

    return Results.Ok(usuario);
});

//Categorias
categoria.MapGet("/", () => categorias);
categoria.MapGet("/{id}", (int id) => categorias.FirstOrDefault(c => c.Id == id));
categoria.MapPost("/", (CategoriaServicio categoriaServicio) =>
{
    categoriaServicio.Id = categorias.Max(c => c.Id) + 1;
    categorias.Add(categoriaServicio);
    return Results.Created($"/categorias/{categoriaServicio.Id}", categoriaServicio);
});

//Servicios
servicio.MapGet("/", (int? categoryId) =>
{
    var result = servicios.AsEnumerable();

    if (categoryId.HasValue)
    {
        result = result.Where(s => s.CategoryId == categoryId.Value);
    }

    return result;
});
servicio.MapGet("/{id}", (int id) => servicios.FirstOrDefault(s => s.Id == id));
servicio.MapPost("/", (Servicio servicio) =>
{
    servicio.Id = servicios.Max(s => s.Id) + 1;
    servicios.Add(servicio);
    return Results.Created($"/servicios/{servicio.Id}", servicio);
});

app.MapPost("/login", (LoginDto loginDto) =>
{
    var usuario = usuarios.FirstOrDefault(u => u.Email == loginDto.Email && u.Password == loginDto.Password);
    if (usuario is null)
    {
        return Results.Unauthorized();
    }
    return Results.Ok(usuario);
});

app.MapPost("/registro", (RegistroDto registroDto) =>
{
    var usuarioExistente = usuarios.FirstOrDefault(u => u.Email == registroDto.Email);
    if (usuarioExistente is not null)
    {
        return Results.BadRequest("El correo electrónico ya está registrado.");
    }

    var nuevoUsuario = new Usuario{
        Id = usuarios.Max(u => u.Id) + 1,
        FullName = registroDto.Name,
        Email = registroDto.Email,
        Password = registroDto.Password,
        Role = "User",
        Phone = registroDto.PhoneNumber,
        IsActive = true
    };

    usuarios.Add(nuevoUsuario);
    return Results.Created($"/usuarios/{nuevoUsuario.Id}", nuevoUsuario);
});
app.Run();
