using API.Models;

namespace API.Interfaces
{
    public interface ICommentsRepository
    {
        Task<List<Comment>> GetAllAsync();
    }
}
