using MagicalVilla_CoponAPI.Data;
using MagicalVilla_CoponAPI.models;
using Microsoft.AspNetCore.Http.HttpResults;

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

//app.MapPost("api/coupon", (Coupon model) =>
//{
//    CouponStore.coupons.Add(model);
//    return Results.Ok("Object created");
//});


app.UseHttpsRedirection();


app.Run();


