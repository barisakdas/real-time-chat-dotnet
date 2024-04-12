var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Taray�c�lar�n g�venlik �nlemlerinin ba�lant� sorunu ��karmas�n� engellemek arac�l���yla ilgili adreslerden gelen istekleri kabul eden yap�.
builder.Services.AddCors(action =>
{
    action.AddDefaultPolicy(policy =>
    {
        policy
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowed(origin => true);
    });
});

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Yukar�da tan�mlanan CORS politikalar�n� uygulayan middleware.
app.UseCors();

app.UseHttpsRedirection();

app.MapHub<ChatHub>("/chathub");

app.UseAuthorization();

app.MapControllers();

app.Run();