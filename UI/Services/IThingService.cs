using Control.Model;
using System.Threading.Tasks;

namespace UI.Services
{
    public interface IThingService
    {
        Task<ResponseModel> SendMessage(string method, int number, double value, string message = "");
    }
}