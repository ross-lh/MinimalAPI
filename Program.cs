using MinimalAPI.DTO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "");

app.MapPost("/login", (LoginDTO loginDTO) =>
{
    if (loginDTO.UserName == "admin" && loginDTO.Password == "1234")
        return Results.Ok("Login successful");
    else
        return Results.Unauthorized();
});

app.Run();
