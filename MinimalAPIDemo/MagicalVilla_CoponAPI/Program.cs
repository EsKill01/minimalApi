using MagicalVilla_CoponAPI.Data;
using MagicalVilla_CoponAPI.models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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

app.MapGet("api/coupon", (ILogger<Program> _logger) =>
{
 _logger.Log(LogLevel.Information, "Get all");
 return Results.Ok(CouponStore.coupons);
}).WithName("GetAllCoupons").Produces<IEnumerable<Coupon>>(StatusCodes.Status200OK);

app.MapGet("api/coupon{id:int}", (ILogger<Program> _logger, int id) =>
{
     _logger.Log(LogLevel.Information, $"Get by id: {id}");
    Results.Ok(CouponStore.coupons.Where(c => c.Id == id));

}).WithName("GetCoupon").Produces<Coupon>(StatusCodes.Status200OK);

app.MapPost("api/coupon", (ILogger<Program> _logger, [FromBody]Coupon model) =>
{
     _logger.Log(LogLevel.Information, "Post object");

    if (model.Id != 0 || string.IsNullOrEmpty(model.Name))
    {
        return Results.BadRequest("Invalid Id or Coupon name");
    }
    else if (CouponStore.coupons.FirstOrDefault(c => c.Name.ToLower() == model.Name.ToLower()) != null)
    {
        return Results.BadRequest("Coupon allready exists");
    }



    model.Id = CouponStore.coupons.OrderByDescending(c => c.Id).FirstOrDefault().Id + 1;
    model.Created = DateTime.Now;

    CouponStore.coupons.Add(model);

    return Results.CreatedAtRoute($"GetCoupon", new
    {
        Id = model.Id,
    } , model);

    //return Results.Created($"/api/coupon/{model.Id}", model);

}).WithName("Add coupon").Accepts<Coupon>("application/json").Produces<Coupon>(StatusCodes.Status201Created).Produces(StatusCodes.Status400BadRequest);


app.MapPut("api/coupon{id:int}", (ILogger<Program> _logger, int id, [FromBody] Coupon model) =>
{
    _logger.Log(LogLevel.Information, $"Put object {id}");

    if (model.Id == 0 || string.IsNullOrEmpty(model.Name))
    {
        return Results.BadRequest("Invalid Id or Coupon name");
    }
    else if(CouponStore.coupons.FirstOrDefault(c => c.Id == id) == null)
    {
        return Results.BadRequest("Coupon do not exists");
    }

    model.LastUpdate = DateTime.Now;

    CouponStore.coupons.RemoveAll(c => c.Id == id);
    CouponStore.coupons.Add(model);
    return Results.Ok(model);
}).WithName("UpdateCoupon").Produces<Coupon>(StatusCodes.Status200OK).Produces(StatusCodes.Status400BadRequest);


app.MapDelete("api/coupon{id:int}", (ILogger<Program> _logger, int id) =>
{
     _logger.Log(LogLevel.Information, $"Delete object {id}");

    CouponStore.coupons.RemoveAll(c => c.Id == id);

    return Results.Ok("Object deleted");
}).WithName("DeleteCoupon").Produces<String>(StatusCodes.Status200OK).Produces(StatusCodes.Status400BadRequest);;

app.UseHttpsRedirection();


app.Run();


