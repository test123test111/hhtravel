using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.Site.Models
{
    public interface ITopImageModel
    {
        ImageInfo TopImage { get; set; }
    }
}