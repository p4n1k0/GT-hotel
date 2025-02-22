using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 6. Desenvolva o endpoint GET /room/:hotelId
        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            return _context.Rooms.Where(room => room.HotelId == HotelId)
            .Select(room => new RoomDto
            {
                RoomId = room.RoomId,
                Name = room.Name,
                Capacity = room.Capacity,
                Image = room.Image,
                Hotel = new HotelDto
                {
                    HotelId = room.Hotel!.HotelId,
                    Name = room.Hotel.Name,
                    Address = room.Hotel.Address,
                    CityId = room.Hotel.CityId,
                    CityName = room.Hotel.City!.Name,
                }
            }).ToList();
        }

        // 7. Desenvolva o endpoint POST /room
        public RoomDto AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();

            return _context.Rooms.Where(r => r.Name == room.Name)
            .Select(r => new RoomDto
            {
                RoomId = r.RoomId,
                Name = r.Name,
                Capacity = r.Capacity,
                Image = r.Image,
                Hotel = new HotelDto
                {
                    HotelId = r.Hotel!.HotelId,
                    Name = r.Hotel.Name,
                    Address = r.Hotel.Address,
                    CityId = r.Hotel.CityId,
                    CityName = r.Hotel.City!.Name,
                }
            }).FirstOrDefault()!;
        }

        // 8. Desenvolva o endpoint DELETE /room/:roomId
        public void DeleteRoom(int RoomId)
        {
            _context.Rooms.Remove(_context.Rooms
            .Where(room => room.RoomId == RoomId).FirstOrDefault()!);
            _context.SaveChanges();
        }
    }
}
