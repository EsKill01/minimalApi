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

app.MapGet("api/coupon",() => Results.Ok(CouponStore.coupons));

app.MapGet("api/coupon{id:int}",(int id) => Results.Ok(CouponStore.coupons.Where(c => c.Id == id)));

app.MapPost("api/coupon", ([FromBody]Coupon model) =>
{
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
    return Results.Ok(model);
});

app.MapPut("api/coupon{id:int}", (int id, [FromBody] Coupon model) =>
{
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
});

app.MapDelete("api/coupon{id:int}", (int id) =>
{
    CouponStore.coupons.RemoveAll(c => c.Id == id);

    return Results.Ok("Object deleated");
});

app.UseHttpsRedirection();


app.Run();


