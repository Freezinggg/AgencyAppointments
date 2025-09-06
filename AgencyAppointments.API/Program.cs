
using AgencyAppointments.Application.DTO.Setting;
using AgencyAppointments.Application.Interfaces.Booking;
using AgencyAppointments.Application.Interfaces.DaysOff;
using AgencyAppointments.Application.Interfaces.Setting;
using AgencyAppointments.Application.Services.Booking;
using AgencyAppointments.Application.Services.DaysOff;
using AgencyAppointments.Application.Services.Setting;
using AgencyAppointments.Infrastructure.Persistance;
using AgencyAppointments.Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace AgencyAppointments.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer(); 
            builder.Services.AddSwaggerGen();           

            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();

            builder.Services.AddScoped<IDaysOffService, DaysOffService>();
            builder.Services.AddScoped<IDaysOffRepository, DaysOffRepository>();

            builder.Services.AddScoped<ISettingRepository, SettingRepository>();
            builder.Services.AddScoped<ISettingService, SettingService>();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            var app = builder.Build();

            //Initial seed for 'Setting'
            using (var scope = app.Services.CreateScope())
            {
                var settingService = scope.ServiceProvider.GetRequiredService<ISettingService>();
                var exist = await settingService.GetByKeyAsync("MAX_BOOKING_COUNT_DAILY");
                if (exist.Data == null)
                {
                    await settingService.CreateAsync(new SettingDTO { Key = "MAX_BOOKING_COUNT_DAILY", Value = 2 });
                }
            }


            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.MapOpenApi();
            //}

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            

            app.Run();
        }
    }
}
