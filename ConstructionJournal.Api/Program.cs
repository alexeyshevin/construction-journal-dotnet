using ConstructionJournal.Api.Data;
using ConstructionJournal.Api.Dtos;
using ConstructionJournal.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5080", "https://localhost:7080")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/api/work-types", async (AppDbContext db) =>
{
    return await db.WorkTypes
        .OrderBy(x => x.Name)
        .Select(x => new WorkTypeDto(x.Id, x.Name!, x.Unit!))
        .ToListAsync();
});

app.MapGet("/api/work-logs", async (
    AppDbContext db,
    DateTime? dateFrom,
    DateTime? dateTo,
    string? sort) =>
{
    var query = db.WorkLogs
        .Include(x => x.WorkType)
        .AsQueryable();

    if (dateFrom.HasValue)
        query = query.Where(x => x.WorkDate >= dateFrom.Value.Date);

    if (dateTo.HasValue)
        query = query.Where(x => x.WorkDate <= dateTo.Value.Date.AddDays(1).AddTicks(-1));

    query = sort == "asc"
        ? query.OrderBy(x => x.WorkDate)
        : query.OrderByDescending(x => x.WorkDate);

    return await query.Select(x => new WorkLogDto(
        x.Id,
        x.WorkDate,
        x.Amount,
        x.Unit!,
        x.PerformerName!,
        x.WorkTypeId,
        new WorkTypeDto(x.WorkType!.Id, x.WorkType.Name!, x.WorkType.Unit!),
        x.CreatedAt,
        x.UpdatedAt
    )).ToListAsync();
});

app.MapPost("/api/work-logs", async (CreateWorkLogDto dto, AppDbContext db) =>
{
    var workType = await db.WorkTypes.FindAsync(dto.WorkTypeId);

    if (workType is null)
        return Results.BadRequest("Вид работ не найден");

    var entity = new WorkLog
    {
        Id = Guid.NewGuid(),
        WorkDate = dto.WorkDate.Date,
        WorkTypeId = dto.WorkTypeId,
        Amount = dto.Amount,
        Unit = string.IsNullOrWhiteSpace(dto.Unit) ? workType.Unit : dto.Unit,
        PerformerName = dto.PerformerName,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    db.WorkLogs.Add(entity);
    await db.SaveChangesAsync();

    return Results.Created($"/api/work-logs/{entity.Id}", entity);
});

app.MapPatch("/api/work-logs/{id:guid}", async (Guid id, UpdateWorkLogDto dto, AppDbContext db) =>
{
    var entity = await db.WorkLogs.FindAsync(id);

    if (entity is null)
        return Results.NotFound("Запись журнала не найдена");

    var workType = await db.WorkTypes.FindAsync(dto.WorkTypeId);

    if (workType is null)
        return Results.BadRequest("Вид работ не найден");

    entity.WorkDate = dto.WorkDate.Date;
    entity.WorkTypeId = dto.WorkTypeId;
    entity.Amount = dto.Amount;
    entity.Unit = string.IsNullOrWhiteSpace(dto.Unit) ? workType.Unit : dto.Unit;
    entity.PerformerName = dto.PerformerName;
    entity.UpdatedAt = DateTime.UtcNow;

    await db.SaveChangesAsync();

    return Results.Ok(entity);
});

app.MapDelete("/api/work-logs/{id:guid}", async (Guid id, AppDbContext db) =>
{
    var entity = await db.WorkLogs.FindAsync(id);

    if (entity is null)
        return Results.NotFound("Запись журнала не найдена");

    db.WorkLogs.Remove(entity);
    await db.SaveChangesAsync();

    return Results.Ok(new { success = true });
});

app.Run();