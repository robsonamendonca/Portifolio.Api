using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Portifolio.Api;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

//connection
builder.Services.AddSqlServer<PortifolioContext>(builder.Configuration.GetConnectionString("ServerConnection"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Portifolio API",
        Description = "ASP.NET Core Web API Portifolio",
        Contact = new OpenApiContact
        {
            Name = "Minha redes sociais",
            Url = new Uri("https://about.me/robsonamendonca")
        }
    });
    options.EnableAnnotations();
});

    
//cors
builder.Services.AddCors();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseCors(p=>p
.AllowAnyHeader()
.AllowAnyOrigin()
.AllowAnyMethod());


app.MapPost("/contacts", async  (PortifolioContext context, Contact contact)=> {
    await context.Contacts.AddAsync(contact);
    await context.SaveChangesAsync();

    return Results.Ok(contact);

})
.WithDescription("Inserir dados de contatos")
.WithMetadata(new SwaggerOperationAttribute("Inserir", "Inserir dados de contatos"))
.WithOpenApi();

app.MapGet("/contacts", async (PortifolioContext context) => {
    var contacts = await context.Contacts.ToListAsync();

    return Results.Ok(contacts);

})
.WithDescription("Listar dados de contatos")
.WithMetadata(new SwaggerOperationAttribute("Listar", "Listar dados de contatos"))
.WithOpenApi();



app.Run();

public class Contact
{
    public Guid Id {get;set;}
    public string? Name {get;set;}
    public string? Email {get;set;}
    public string? Subject {get;set;}
    public DateTime Date {get;set;}

}