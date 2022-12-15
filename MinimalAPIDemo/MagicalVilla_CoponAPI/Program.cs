using AutoMapper;
using FluentValidation;
using MagicalVilla_CoponAPI;
using MagicalVilla_CoponAPI.Data;
using MagicalVilla_CoponAPI.models;
using MagicalVilla_CoponAPI.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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



app.MapPost("api/coupon", (IMapper _mapper, IValidator<CouponCreateDTO> _validator, ILogger<Program> _logger, [FromBody] CouponCreateDTO couponCreateDTO) =>
{
     _logger.Log(LogLevel.Information, "Post object");

    var validationResult = _validator.ValidateAsync(couponCreateDTO).GetAwaiter().GetResult();

    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors.FirstOrDefault().ToString());
    }
    else if (CouponStore.coupons.FirstOrDefault(c => c.Name.ToLower() == couponCreateDTO.Name.ToLower()) != null)
    {
        return Results.BadRequest("Coupon allready exists");
    }

    Coupon coupon = _mapper.Map<Coupon>(couponCreateDTO);



    coupon.Id = CouponStore.coupons.OrderByDescending(c => c.Id).FirstOrDefault().Id + 1;
 

    CouponStore.coupons.Add(coupon);

    return Results.CreatedAtRoute($"GetCoupon", new
    {
        Id = coupon.Id,
    } , 
    _mapper.Map<CouponDTO>(coupon));

    //return Results.Created($"/api/coupon/{model.Id}", model);

}).WithName("Add coupon").Accepts<CouponCreateDTO>("application/json").Produces<CouponDTO>(StatusCodes.Status201Created).Produces(StatusCodes.Status400BadRequest);


app.MapPut("api/coupon{id:int}", (IMapper _mapper, ILogger<Program> _logger, int id, [FromBody] CouponUpdateDTO model) =>
{
    _logger.Log(LogLevel.Information, $"Put object {id}");

    if(CouponStore.coupons.FirstOrDefault(c => c.Id == id) == null)
    {
        return Results.BadRequest("Coupon do not exists");
    }

    Coupon coupon = _mapper.Map<Coupon>(model);

    coupon.LastUpdate = DateTime.Now;

    CouponStore.coupons.RemoveAll(c => c.Id == id);
    CouponStore.coupons.Add(coupon);

    CouponUpDTO upDTO = _mapper.Map<CouponUpDTO>(coupon);

    return Results.Ok(upDTO);
}).WithName("UpdateCoupon").Produces<CouponUpDTO>(StatusCodes.Status200OK).Produces(StatusCodes.Status400BadRequest);


app.MapDelete("api/coupon{id:int}", (ILogger<Program> _logger, int id) =>
{
     _logger.Log(LogLevel.Information, $"Delete object {id}");

    CouponStore.coupons.RemoveAll(c => c.Id == id);

    return Results.Ok("Object deleted");
}).WithName("DeleteCoupon").Produces<String>(StatusCodes.Status200OK).Produces(StatusCodes.Status400BadRequest);;

app.UseHttpsRedirection();


app.Run();


