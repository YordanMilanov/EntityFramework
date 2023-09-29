using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastFood.Web.ViewModels.Positions;

namespace FastFood.Services.Data
{
    public interface IPositionsService
    {
        Task CreateAsync(CreatePositionInputModel inputModel);

        Task<IEnumerable<PositionsAllViewModel>> GetAllAsync();
    }
}
