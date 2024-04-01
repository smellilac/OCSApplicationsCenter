var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<ApplicationsModelValidator>();


builder.Services.AddDbContext<UnSubmittedAppsDb>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteUnsubmitted"));
});

builder.Services.AddDbContext<UsersContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SqliteUsers"));
});

    builder.Services.AddDbContext<SubmittedAppsDb>(options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("SqliteSubmitted"));
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var dbUnSubmitted = scope.ServiceProvider.GetRequiredService<UnSubmittedAppsDb>();
    dbUnSubmitted.Database.EnsureCreated();

    var dbUsers = scope.ServiceProvider.GetRequiredService<UsersContext>();
    dbUsers.Database.EnsureCreated();

    var dbSubmitted = scope.ServiceProvider.GetRequiredService<SubmittedAppsDb>();
    dbSubmitted.Database.EnsureCreated();
}

app.MapGet("/applications/{id}", async (Guid id, IApplicationRepository repository) => 
    await repository.GetApplicationByIdAsync(id) is ApplicationsModel app
    ? Results.Ok(app)
    : Results.NotFound());

app.MapGet("/applications", async ([FromQuery] DateTime? submittedAfter, 
    [FromQuery] DateTime? unsubmittedOlder, IApplicationRepository repository, 
    HttpContext context) =>
{
    if (submittedAfter.HasValue)
    {
        var applications = await repository.
            GetApplicationsSubmittedAfterDateAsync(submittedAfter.Value);
        return Results.Ok(applications);
    }
    else if (unsubmittedOlder.HasValue)
    {
        var draftApplications = await repository
            .GetDraftApplicationsAfterDateAsync(unsubmittedOlder.Value);
        return Results.Ok(draftApplications);
    }

    else
    {
        return Results.BadRequest("Invalid parameters");
    }
});

app.MapGet("/users/{userId}/currentapplication", async (Guid userId, 
    IApplicationRepository repository) =>
    await repository.GetDraftApplicationForUserAsync(userId) is ApplicationsModel app
    ? Results.Ok(app)
    : Results.NotFound());


app.MapGet("/applications/{id}/submit", async (Guid id, IApplicationRepository repository) =>
{
    var app = await repository.SubmitApplicationForReviewAsync(id);
    await repository.SaveAsyncUnsubmitted();
    await repository.SaveAsync();
    return Results.Ok(app);
});

app.MapGet("/activities", async (IApplicationRepository repository) =>
{
    var activityTypes = await repository.GetActivityTypesAsync();
    return Results.Ok(activityTypes);
});

app.MapPost("/applications", async ([FromBody] ApplicationsModel app, IApplicationRepository repository) =>
{
    await repository.CreateApplicationAsync(app);
    await repository.SaveAsyncUnsubmitted();
     return Results.Created("/applications", app);
});

app.MapPut("/applications/{id}", async ([FromBody] ApplicationsModel app, 
    IApplicationRepository repository, [FromRoute] Guid id) =>
{
    Guid requestId = id;
    await repository.UpdateApplicationAsync(app, requestId);
    await repository.SaveAsyncUnsubmitted();
    return Results.Ok(app);
});

app.MapDelete("/applications/{appId}", async (Guid appId, IApplicationRepository repository) =>
{
    await repository.DeleteApplicationAsync(appId);
    await repository.SaveAsync();
    return Results.Ok();
});

app.UseHttpsRedirection();
app.Run();

