using HotChocolate;
using hotchocolate_bug.DB;
using hotchocolate_bug.Queries;
using Microsoft.EntityFrameworkCore;
[assembly: Module("BugTypes")]
// SeedDb
var factory = new ContextFactory();
var context = factory.CreateDbContext();
context.Database.EnsureCreated();

context.People.Add(new Person { Name = "Alice" });
context.People.Add(new Person { Name = "Bob" });
context.People.Add(new Person { Name = "Marc" });
context.People.Add(new Person { Name = "Will" });
context.People.Add(new Person { Name = "Michael" });
context.People.Add(new Person { Name = "Ayman" });
context.People.Add(new Person { Name = "Hans" });
context.People.Add(new Person { Name = "Nils" });

context.SaveChanges();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbContextFactory<AppDbContext>, ContextFactory>();
builder.Services
    .AddGraphQLServer()
    .AddBugTypes()
    .AddSorting()
;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("AllowAll");

app.MapGraphQL();
app.Run();
