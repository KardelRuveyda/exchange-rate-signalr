// Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add ConfigureServices içeriði
builder.Services.AddControllers();
builder.Services.AddSingleton<CurrencyService>();
builder.Services.AddSignalR();
builder.Services.AddCors();

var app = builder.Build();

// Add Configure içeriði
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseWebSockets();
app.UseHttpsRedirection();

app.UseRouting();

// global cors policy
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials            

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<CurrencyHub>("/currencyHub");
});

app.Run();
