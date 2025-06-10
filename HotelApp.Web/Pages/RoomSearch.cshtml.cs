using HotelAppLibrary.Data;
using HotelAppLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelApp.Web.Pages
{
    public class RoomSearchModel : PageModel
    {
        private readonly IDatabaseData _db;

        [BindProperty(SupportsGet = true)]
        [DisplayName("Start Date")]
        //[Display(Name = "Start Date D", Prompt = "Start Date prompt d")]
        [DataType(DataType.Date)]
        //public DateOnly StartDate { get; set; } = DateOnly.FromDateTime( DateTime.Now.Date);
        //[CustomValidation(typeof(RoomSearchModel), "ValidateStartDate")]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;

        [BindProperty(SupportsGet = true)]
        [DisplayName("End Date")]
        //[Display(Name = "End Date D", Prompt = "End Date prompt d")]
        [DataType(DataType.Date)]
        //[CustomValidation(typeof(RoomSearchModel), "ValidateEndDate")]
        public DateTime EndDate { get; set; } = DateTime.Now.Date.AddDays(1);

        public List<RoomTypeModel> AvailableRoomTypes { get; set; }

        [BindProperty(SupportsGet = true)] public bool SearchEnabled { get; set; } = false;

        public RoomSearchModel(IDatabaseData db)
        {
            _db = db;
        }

        public static ValidationResult ValidateStartDate(DateTime startDate, ValidationContext context)
        {
            if (startDate.Date < DateTime.Now.Date)
            {
                return new ValidationResult("Start date cannot be in the past");
            }
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateEndDate(DateTime endDate, ValidationContext context)
        {
            var model = (RoomSearchModel)context.ObjectInstance;
            if (endDate.Date <= model.StartDate.Date)
            {
                return new ValidationResult("End date must be after start date");
            }
            return ValidationResult.Success;
        }

        public void OnGet()
        {
            if (SearchEnabled == true)
            {
                AvailableRoomTypes = _db.GetAvailableRoomTypes(
                    StartDate,
                    EndDate);
            }
        }

        public IActionResult OnPost()
        {
            ModelState.Clear();

            if (StartDate.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError(nameof(StartDate),
                    "Start date cannot be in the past");
            }

            if (EndDate.Date <= StartDate.Date)
            {
                ModelState.AddModelError(nameof(EndDate),
                    "End date must be after start date");
            }

            if (ModelState.IsValid is false)
            {
                return Page();
            }

            return RedirectToPage("/RoomSearch", new
            {
                SearchEnabled = true,
                StartDate = StartDate.ToString("yyyy-MM-dd"),
                EndDate = EndDate.ToString("yyyy-MM-dd")
            });

            //return RedirectToPage("BookRoom", new { model = "serialized",age=3 });
        }
    }
}