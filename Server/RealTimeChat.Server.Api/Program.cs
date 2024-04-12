var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Tarayýcýlarýn güvenlik önlemlerinin baðlantý sorunu çýkarmasýný engellemek aracýlýðýyla ilgili adreslerden gelen istekleri kabul eden yapý.
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

// Yukarýda tanýmlanan CORS politikalarýný uygulayan middleware.
app.UseCors();

app.UseHttpsRedirection();

app.MapHub<ChatHub>("/chathub");

app.UseAuthorization();

app.MapControllers();

app.Run();