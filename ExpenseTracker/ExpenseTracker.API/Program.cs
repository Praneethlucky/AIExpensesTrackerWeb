using AutoMapper;
using ExpenseTracker.API.Middleware;
using ExpenseTracker.BusinessLogic.Common;
using ExpenseTracker.BusinessLogic.Interfaces;
using ExpenseTracker.BusinessLogic.Mappings;
using ExpenseTracker.BusinessLogic.Services;
using ExpenseTracker.BusinessLogic.Services.Implementation;
using ExpenseTracker.BusinessLogic.Services.Interfaces;
using ExpenseTracker.Domain.Configurations;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Domain.Interfaces.Services;
using ExpenseTracker.Infrastructure.Interfaces;
using ExpenseTracker.Infrastructure.Persistence;
using ExpenseTracker.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDistributedMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddControllers().AddJsonOptions(o =>
 {
     o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
 });
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .Select(x => x.Value.Errors.First().ErrorMessage)
            .ToList();

        var response = new ApiResponse<object>
        {
            success = false,
            message = errors.FirstOrDefault() ?? "Validation error",
            errorCode = "VALIDATION_ERROR"
        };

        return new BadRequestObjectResult(response);
    };
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(50000); // short timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(MappingProfile).Assembly);
});
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ConnectionFactory>();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddScoped<IAIService, AIService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<IPaymentTypeRepository,PaymentTypeRepository>();
builder.Services.AddScoped<IPaymentTypeService,PaymentTypeService>();

builder.Services.AddScoped<IExpenseRepository,ExpenseRepository>();
builder.Services.AddScoped<IExpenseService,ExpenseService>();

builder.Services.AddScoped<IRecurringRuleRepository, RecurringRuleRepository>();
builder.Services.AddScoped<IRecurringRuleService, RecurringRuleService>();

builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IBillService, BillService>();

builder.Services.AddScoped<IBillOccurrenceRepository, BillOccurrenceRepository>();
builder.Services.AddScoped<IBillOccurrenceService, BillOccurrenceService>();

builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddScoped<IOccurrenceQueryRepository,OccurrenceQueryRepository>();
builder.Services.AddScoped<IOccurrenceQueryService,OccurrenceQueryService>();

builder.Services.AddScoped<IDebtRepository,DebtRepository>();
builder.Services.AddScoped<IDebtService,DebtService>();

builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwt.Issuer,
        ValidAudience = jwt.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt.Key))
    };
});

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API NAME 1.0.0.0"));

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
