namespace CinemaApi.Repository.Interfaces;

public interface IDashboardRepository
{
    Task<int> CountActiveMoviesAsync();
    Task<int> CountActiveRoomsAsync();
    Task<int> CountAvailableRoomsAsync();
}
