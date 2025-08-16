namespace trybe_hotel.Test.Test;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Diagnostics;
using System.Xml;
using System.IO;


public class RoomJson {
        public int RoomId { get; set; }
        public string? Name { get; set; }
        public int Capacity { get; set; }
        public string? Image { get; set; }
        public HotelJson? Hotel { get; set; }
}

public class TestReq06 : IClassFixture<WebApplicationFactory<Program>>
{
    public HttpClient _clientCityGet;

    public TestReq06(WebApplicationFactory<Program> factory)
    {
        //_factory = factory;
        _clientCityGet = factory.WithWebHostBuilder(builder => {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TrybeHotelContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ContextTest>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTestGetRoom");
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
                    appContext.Cities.Add(new City {CityId = 1, Name = "Manaus"});
                    appContext.Cities.Add(new City {CityId = 2, Name = "Palmas"});
                    appContext.SaveChanges();
                    appContext.Hotels.Add(new Hotel {HotelId = 1, Name = "Trybe Hotel Manaus", Address = "Address 1", CityId = 1});
                    appContext.Hotels.Add(new Hotel {HotelId = 2, Name = "Trybe Hotel Palmas", Address = "Address 2", CityId = 2});
                    appContext.Hotels.Add(new Hotel {HotelId = 3, Name = "Trybe Hotel Ponta Negra", Address = "Addres 3", CityId = 1});
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
   
    [Trait("Category", "6. Desenvolva o endpoint GET /room")]
    [Theory(DisplayName = "Será validado que a resposta será um status http 200")]
    [InlineData("/room/1")]
    public async Task TestRoomController(string url)
    {
        var response = await _clientCityGet.GetAsync(url);
        response.EnsureSuccessStatusCode();
    }

    [Trait("Category", "6. Desenvolva o endpoint GET /room")]
    [Theory(DisplayName = "Será validado que é possível listar todos os quartos de um hotel")]
    [InlineData("/room/1")]
    public async Task TestRoomControllerResponse(string url)
    {
        var response = await _clientCityGet.GetAsync(url);
        var responseString = await response.Content.ReadAsStringAsync();
        List<RoomJson> jsonResponseNotOrder = JsonConvert.DeserializeObject<List<RoomJson>>(responseString);
        List<RoomJson> jsonResponse = jsonResponseNotOrder.OrderBy(item => item.RoomId).ToList();

        Assert.Equal(1, jsonResponse[0].RoomId);
        Assert.Contains("Room 1", jsonResponse[0].Name);
        Assert.Equal(2, jsonResponse[0].Capacity);
        Assert.Contains("Image 1", jsonResponse[0].Image);
        Assert.Contains("Trybe Hotel Manaus", jsonResponse[0].Hotel.Name);
        Assert.Contains("Manaus", jsonResponse[0].Hotel.CityName);

        Assert.Equal(2, jsonResponse[1].RoomId);
        Assert.Contains("Room 2", jsonResponse[1].Name);
        Assert.Equal(3, jsonResponse[1].Capacity);
        Assert.Contains("Image 2", jsonResponse[1].Image);
        Assert.Contains("Trybe Hotel Manaus", jsonResponse[1].Hotel.Name);
        Assert.Contains("Manaus", jsonResponse[1].Hotel.CityName);

    }
}