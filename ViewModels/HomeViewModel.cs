using Microsoft.AspNetCore.Mvc.Rendering;

namespace TutorBridge.ViewModels
{
    public class HomeViewModel
    {
        public List<Tutor> FeaturedTutors { get; set; }

        public IEnumerable<SelectListItem> Subjects { get; set; }

        public HomeViewModel(List<Tutor> featuredTutors, IEnumerable<SelectListItem> subjects)
        {
            FeaturedTutors = featuredTutors;
            Subjects = subjects;
        }
    }
}