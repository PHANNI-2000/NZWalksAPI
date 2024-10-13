using NZWalksAPI.Model.Domain;
using System.Threading.Tasks;

namespace NZWalksAPI.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
