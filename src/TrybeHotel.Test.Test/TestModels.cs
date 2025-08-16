namespace trybe_hotel.Test.Test;

using Microsoft.EntityFrameworkCore;
using TrybeHotel.Models;

public class TestReq01
{

    [Trait("TrybeHotel", "1. Implemente as models da aplicação")]
    [Theory(DisplayName = "City deve conter chave primária")]
    [InlineData("CityId")]
    public void CityShouldContainProperPrimaryKey(string keyName)
    {
        var contextOptions = new DbContextOptionsBuilder<ContextTest>()
            .UseInMemoryDatabase("TrybeHotelContext")
            .Options;
        ContextTest testContext = new(contextOptions);
        DbSet<City> set = testContext.Set<City>();
        var property = set.EntityType.FindProperty(keyName);
        property.IsKey().Should().BeTrue();
    }

    [Trait("TrybeHotel", "1. Implemente as models da aplicação")]
    [Theory(DisplayName = "City deve conter as propriedades e tipos corretos")]
    [InlineData("CityId", typeof(int))]
    [InlineData("Name", typeof(string))]
    [InlineData("Hotels", typeof(IEnumerable<Hotel>))]
    public void CityShouldContainProperties(string propertyName, Type propertyType)
    {
        var propertyToCheck = typeof(City).GetProperty(propertyName);
        propertyToCheck.Should().NotBeNull();
        var propertyTypeName = propertyToCheck.PropertyType;
        propertyTypeName.Should().BeAssignableTo(propertyType);
    }

    [Trait("TrybeHotel", "1. Implemente as models da aplicação")]
    [Theory(DisplayName = "Hotel deve conter chave primária")]
    [InlineData("HotelId")]
    public void HotelShouldContainProperPrimaryKey(string keyName)
    {
        var contextOptions = new DbContextOptionsBuilder<ContextTest>()
            .UseInMemoryDatabase("TrybeHotelContext")
            .Options;
        ContextTest testContext = new(contextOptions);
        DbSet<Hotel> set = testContext.Set<Hotel>();
        var property = set.EntityType.FindProperty(keyName);
        property.IsKey().Should().BeTrue();
    }

    [Trait("TrybeHotel", "1. Implemente as models da aplicação")]
    [Theory(DisplayName = "Hotel deve conter as propriedades e tipos corretos")]
    [InlineData("HotelId", typeof(int))]
    [InlineData("Name", typeof(string))]
    [InlineData("Address", typeof(string))]
    [InlineData("CityId", typeof(int))]
    [InlineData("Rooms", typeof(IEnumerable<Room>))]
    public void HotelShouldContainProperties(string propertyName, Type propertyType)
    {
        var propertyToCheck = typeof(Hotel).GetProperty(propertyName);
        propertyToCheck.Should().NotBeNull();
        var propertyTypeName = propertyToCheck.PropertyType;
        propertyTypeName.Should().BeAssignableTo(propertyType);
    }

    [Trait("TrybeHotel", "1. Implemente as models da aplicação")]
    [Theory(DisplayName = "Room deve conter chave primária")]
    [InlineData("RoomId")]
    public void RoomShouldContainProperPrimaryKey(string keyName)
    {
        var contextOptions = new DbContextOptionsBuilder<ContextTest>()
            .UseInMemoryDatabase("TrybeHotelContext")
            .Options;
        ContextTest testContext = new(contextOptions);
        DbSet<Room> set = testContext.Set<Room>();
        var property = set.EntityType.FindProperty(keyName);
        property.IsKey().Should().BeTrue();
    }

    [Trait("TrybeHotel", "1. Implemente as models da aplicação")]
    [Theory(DisplayName = "Room deve conter as propriedades e tipos corretos")]
    [InlineData("RoomId", typeof(int))]
    [InlineData("Name", typeof(string))]
    [InlineData("Capacity", typeof(int))]
    [InlineData("Image", typeof(string))]
    [InlineData("HotelId", typeof(int))]
    public void RoomShouldContainProperties(string propertyName, Type propertyType)
    {
        var propertyToCheck = typeof(Room).GetProperty(propertyName);
        propertyToCheck.Should().NotBeNull();
        var propertyTypeName = propertyToCheck.PropertyType;
        propertyTypeName.Should().BeAssignableTo(propertyType);
    }



}