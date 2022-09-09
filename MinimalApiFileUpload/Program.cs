var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/httpcontext/fileupload", (HttpContext ctx) =>
{
    if (!ctx.Request.Form.Files.Any())
        return Results.BadRequest("No files");

    foreach (var file in ctx.Request.Form.Files)
    {
        using (var stream = new FileStream(@"C:\temp\" + file.FileName, FileMode.Create))
        {
            file.CopyTo(stream);
        }
    }

    return Results.Ok("Files uploaded");
});

// This won't play nice with swagger
app.MapPost("/httprequest/fileupload", (HttpRequest req) =>
{
    if (!req.Form.Files.Any())
        return Results.BadRequest("No files");

    foreach(var file in req.Form.Files)
    {
        using(var stream = new FileStream(@"C:\temp\"+ file.FileName, FileMode.Create))
        {
            file.CopyTo(stream);
        }
    }

    return Results.Ok("Files uploaded");
});

app.Run();

