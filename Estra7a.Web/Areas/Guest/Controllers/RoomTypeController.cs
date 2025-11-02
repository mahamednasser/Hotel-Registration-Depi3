using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.Models.Models;
using Estra7a.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Estra7a.Web.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class RoomTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public IActionResult AddType([FromBody]AddRoomTypeViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newType = new RoomType
            {
                Name = viewModel.RoomTypeName,
                Description = viewModel.RoomTypeDescription,
                //Capacity = viewModel.Capacity
            };
            _unitOfWork.RoomType.Add(newType);
            _unitOfWork.save();

            return Json(new { success = true, id = newType.RoomTypeId, name = newType.Name });

        }
    }
}
