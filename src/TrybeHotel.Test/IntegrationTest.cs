namespace TrybeHotel.Test;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;

public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    public HttpClient _clientTest;

    public IntegrationTest(WebApplicationFactory<Program> factory)
    {
        //_factory = factory;
        _clientTest = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TrybeHotelContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ContextTest>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTestDatabase");
                });
                services.AddScoped<ITrybeHotelContext, ContextTest>();
                services.AddScoped<ICityRepository, CityRepository>();
                services.AddScoped<IHotelRepository, HotelRepository>();
                services.AddScoped<IRoomRepository, RoomRepository>();
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                using (var appContext = scope.ServiceProvider.GetRequiredService<ContextTest>())
                {
                    appContext.Database.EnsureCreated();
                    appContext.Database.EnsureDeleted();
                    appContext.Database.EnsureCreated();
                    appContext.Cities.Add(new City { CityId = 1, Name = "Manaus" });
                    appContext.Cities.Add(new City { CityId = 2, Name = "Palmas" });
                    appContext.SaveChanges();
                    appContext.Hotels.Add(new Hotel { HotelId = 1, Name = "Trybe Hotel Manaus", Address = "Address 1", CityId = 1 });
                    appContext.Hotels.Add(new Hotel { HotelId = 2, Name = "Trybe Hotel Palmas", Address = "Address 2", CityId = 2 });
                    appContext.Hotels.Add(new Hotel { HotelId = 3, Name = "Trybe Hotel Ponta Negra", Address = "Addres 3", CityId = 1 });
                    appContext.SaveChanges();
                    appContext.Rooms.Add(new Room { RoomId = 1, Name = "Room 1", Capacity = 2, Image = "Image 1", HotelId = 1 });
                    appContext.Rooms.Add(new Room { RoomId = 2, Name = "Room 2", Capacity = 3, Image = "Image 2", HotelId = 1 });
                    appContext.Rooms.Add(new Room { RoomId = 3, Name = "Room 3", Capacity = 4, Image = "Image 3", HotelId = 1 });
                    appContext.Rooms.Add(new Room { RoomId = 4, Name = "Room 4", Capacity = 2, Image = "Image 4", HotelId = 2 });
                    appContext.Rooms.Add(new Room { RoomId = 5, Name = "Room 5", Capacity = 3, Image = "Image 5", HotelId = 2 });
                    appContext.Rooms.Add(new Room { RoomId = 6, Name = "Room 6", Capacity = 4, Image = "Image 6", HotelId = 2 });
                    appContext.Rooms.Add(new Room { RoomId = 7, Name = "Room 7", Capacity = 2, Image = "Image 7", HotelId = 3 });
                    appContext.Rooms.Add(new Room { RoomId = 8, Name = "Room 8", Capacity = 3, Image = "Image 8", HotelId = 3 });
                    appContext.Rooms.Add(new Room { RoomId = 9, Name = "Room 9", Capacity = 4, Image = "Image 9", HotelId = 3 });
                    appContext.SaveChanges();
                }
            });
        }).CreateClient();
    }

    [Theory]
    [InlineData("/city", HttpStatusCode.OK)]
    [InlineData("/hotel", HttpStatusCode.OK)]
    public async Task TestCityGet(string url, HttpStatusCode expectedStatusCode)
    {
        var response = (await _clientTest.GetAsync(url)).EnsureSuccessStatusCode();
        Assert.Equal(expectedStatusCode, response.StatusCode);
    }

    [Theory]
    [InlineData("/room/1", HttpStatusCode.OK)]
    public async Task TestGetRoomById(string url, HttpStatusCode expectedStatusCode)
    {
        var response = (await _clientTest.GetAsync(url)).EnsureSuccessStatusCode();
        Assert.Equal(expectedStatusCode, response.StatusCode);
    }

    [Theory]
    [InlineData("/city", "Rio de Janeiro", HttpStatusCode.Created)]
    public async Task TestPostCity(string url, string cityName, HttpStatusCode expectedStatusCode)
    {
        var response = (await _clientTest.PostAsync(url, new StringContent(
            JsonConvert.SerializeObject(new { Name = cityName }),
            Encoding.UTF8, "application/json"
        ))).EnsureSuccessStatusCode();
        Assert.Equal(expectedStatusCode, response.StatusCode);
    }

    [Theory]
    [InlineData("/hotel", "Trybe Hotel S�o Paulo", "Address 4", 2, HttpStatusCode.Created)]
    public async Task TestPostHotel(string url, string hotelName, string address, int cityId, HttpStatusCode expectedStatusCode)
    {
        var response = (await _clientTest.PostAsync(url, new StringContent(
            JsonConvert.SerializeObject(new { Name = hotelName, Address = address, CityId = cityId }),
            Encoding.UTF8, "application/json"
        ))).EnsureSuccessStatusCode();
        Assert.Equal(expectedStatusCode, response.StatusCode);
    }

    [Theory]
    [InlineData("/room", "Room 10", 5, "Image 10", 2, HttpStatusCode.Created)]
    public async Task TestPostRoom(string url, string roomName, int capacity, string image, int hotelId, HttpStatusCode expectedStatusCode)
    {
        var response = (await _clientTest.PostAsync(url, new StringContent(
            JsonConvert.SerializeObject(new { Name = roomName, Capacity = capacity, Image = image, HotelId = hotelId }),
            Encoding.UTF8, "application/json"
        ))).EnsureSuccessStatusCode();
        Assert.Equal(expectedStatusCode, response.StatusCode);
    }
}
