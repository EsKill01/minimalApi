using AutoMapper;
using FluentValidation;
using MagicalVilla_CoponAPI;
using MagicalVilla_CoponAPI.Data;
using MagicalVilla_CoponAPI.models;
using MagicalVilla_CoponAPI.Models;
using MagicalVilla_CoponAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("api/coupon", (ApplicationDbContext _db, ILogger<Program> _logger) =>
{
    ApiResponse apiResponse = new ApiResponse();
    _logger.Log(LogLevel.Information, "Get all");

    apiResponse.Result = _db.Coupons;


    return Results.Ok(apiResponse);
}).WithName("GetAllCoupons").Produces<ApiResponse>(StatusCodes.Status200OK);

app.MapGet("api/coupon/{id:int}", async (ApplicationDbContext _db, ILogger<Program> _logger, int id) =>
{
    ApiResponse apiResponse= new ApiResponse();

    _logger.Log(LogLevel.Information, $"Get by id: {id}");

    Coupon coupon = await _db.Coupons.FirstOrDefaultAsync(c => c.Id == id);

    if (coupon == null)
    {
        apiResponse.IsSuccess = false;
        apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
        apiResponse.ErrorMessages.Add("Object dont exists");

        return Results.BadRequest(apiResponse);
    }

    apiResponse.Result = coupon;

    return Results.Ok(apiResponse);

}).WithName("GetCoupon").Produces<ApiResponse>(StatusCodes.Status200OK);

app.MapPost("api/coupon", async (ApplicationDbContext _db, IMapper _mapper, 
    IValidator<CouponCreateDTO> _validator, 
    ILogger<Program> _logger, 
    [FromBody] CouponCreateDTO couponCreateDTO) =>
{
    ApiResponse apiResponse = new ApiResponse(); 
    _logger.Log(LogLevel.Information, "Post object");

    var validationResult = await _validator.ValidateAsync(couponCreateDTO);

    if (!validationResult.IsValid)
    {
        apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
        apiResponse.IsSuccess = false;
        apiResponse.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ErrorMessage);
        return Results.BadRequest(apiResponse);
    }
    else if (_db.Coupons.FirstOrDefault(c => c.Name.ToLower() == couponCreateDTO.Name.ToLower()) != null)
    {
        apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
        apiResponse.IsSuccess = false;
        apiResponse.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ErrorMessage);
        return Results.BadRequest("Coupon allready exists");
    }

    Coupon coupon = _mapper.Map<Coupon>(couponCreateDTO);

    await _db.Coupons.AddAsync(coupon);
    await _db.SaveChangesAsync();


    apiResponse.Result = coupon;

    return Results.CreatedAtRoute($"GetCoupon", new
    {
        Id = coupon.Id,
    },
    apiResponse);

    //return Results.Created($"/api/coupon/{model.Id}", model);
}).WithName("Add coupon").Accepts<CouponCreateDTO>("application/json").Produces<ApiResponse>(StatusCodes.Status201Created).Produces(StatusCodes.Status400BadRequest);

app.MapPut("api/coupon/{id:int}", async (ApplicationDbContext _db, IValidator<CouponUpdateDTO> _validator, IMapper _mapper, ILogger<Program> _logger, int id, [FromBody] CouponUpdateDTO model) =>
{
    ApiResponse apiResponse = new();
    _logger.Log(LogLevel.Information, $"Put object {id}");

    var couponToUpdate = await _db.Coupons.FirstOrDefaultAsync(c => c.Id == id);

    if (couponToUpdate == null)
    {
        apiResponse.IsSuccess = false;
        apiResponse.ErrorMessages.Add("Coupon do not exists");
        apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
        return Results.BadRequest(apiResponse);
    }

    var validationResult = await _validator.ValidateAsync(model);

    if (!validationResult.IsValid)
    {
        apiResponse.IsSuccess = false;
        apiResponse.ErrorMessages.Add($"Model invalid: {validationResult.Errors.FirstOrDefault().ErrorMessage}");
        apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;

        return Results.BadRequest(apiResponse);
    }

    couponToUpdate.Percent = model.Percent;
    couponToUpdate.IsActive = model.IsActive;
    couponToUpdate.Name = model.Name;
    couponToUpdate.LastUpdate = DateTime.Now;

    //_db.Coupons.Update(_mapper.Map<Coupon>(model));

    await _db.SaveChangesAsync();




    CouponUpDTO upDTO = _mapper.Map<CouponUpDTO>(couponToUpdate);

    apiResponse.Result = upDTO;


    return Results.Ok(apiResponse);
}).WithName("UpdateCoupon").Produces<ApiResponse>(StatusCodes.Status200OK).Produces(StatusCodes.Status400BadRequest);

app.MapDelete("api/coupon/{id:int}", async (ApplicationDbContext _db, ILogger<Program> _logger, int id) =>
{
    ApiResponse apiResponse = new ApiResponse();
    _logger.Log(LogLevel.Information, $"Delete object {id}");

    Coupon coupon = await _db.Coupons.FirstOrDefaultAsync(c => c.Id == id);

    if(coupon == null)
    {
        apiResponse.IsSuccess = false;
        apiResponse.ErrorMessages.Add("Object do not exits");
        apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;

        return Results.BadRequest(apiResponse);
    }

    apiResponse.Result = "Object deleted";

    _db.Coupons.Remove(coupon);

    await _db.SaveChangesAsync();

    return Results.Ok(apiResponse);

}).WithName("DeleteCoupon").Produces<ApiResponse>(StatusCodes.Status200OK).Produces(StatusCodes.Status400BadRequest); ;

app.UseHttpsRedirection();

app.Run();