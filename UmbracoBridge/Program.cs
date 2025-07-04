using UmbracoBridge.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<IUmbracoManagementService, UmbracoManagementService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.MapGet("/", context =>
{
    context.Response.Redirect("/scalar/index.html");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
