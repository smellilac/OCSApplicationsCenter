var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IApplicationHandler, ApplicationHandler>();
builder.Services.AddScoped<MainModelValidator>();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(string), StatusCodes.Status400BadRequest));
})
.ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errorMessage = string.Join("\n", context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        return new BadRequestObjectResult(errorMessage);
    };
});
builder.Services.AddScoped<ApplicationRepository>();
 builder.Services.AddDbContext<PostgreContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"));
});
using (var serviceScope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<PostgreContext>();
    dbContext.Database.EnsureCreated();
}
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


