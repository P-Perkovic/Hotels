using Hotels.Domain.Models;
using Hotels.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Domain.Interfaces
{
    public interface IHotelService
    {
        Task<Hotel> GetById(int id);
        Task<IEnumerable<Hotel>> GetAll(PageQuery pageQuery);
        Task<IEnumerable<Hotel>> SearchByLocation(LocationQuery locationQuery);
        Task<Hotel> Upsert(Hotel hotel);
        Task<bool> Delete(int id);
    }
}
