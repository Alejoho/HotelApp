using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Web.Pages
{
    public class BookRoomModel : PageModel
    {
        private readonly IDatabaseData _db;
        [BindProperty(SupportsGet = true)] public DateTime StartDate { get; set; }

        [BindProperty(SupportsGet = true)] public DateTime EndDate { get; set; }

        public RoomTypeModel RoomType { get; set; }

        [BindProperty]
        [Display(Name = "First Name", Prompt = "Insert your First Name.")]
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [BindProperty]
        [Display(Name = "Last Name", Prompt = "Insert your Last Name.")]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        public BookRoomModel(IDatabaseData db)
        {
            _db = db;
        }

        public void OnGet(int roomTypeId)
        {
            if (roomTypeId > 0)
            {
                RoomType = _db.GetRoomTypeById(roomTypeId);
            }
        }

        public string DaysStaying()
        {
            int days = (EndDate - StartDate).Days;
            return days is 1 ? $"{days} night" : $"{days} nights";
        }

        public IActionResult OnPost(int roomTypeId)
        {
            if (ModelState.IsValid == false)
            {
                RoomType = _db.GetRoomTypeById(roomTypeId);
                return Page();
            }

            _db.BookGuest(FirstName, LastName, StartDate, EndDate, roomTypeId);

            return RedirectToPage("Confirmation");
        }
    }
}