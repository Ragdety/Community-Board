using System.ComponentModel.DataAnnotations;

namespace CommunityBoard.Core.Enums
{
    public enum AnnouncementType
    {
        [Display(Name = "Sale")]
        Sale,

        [Display(Name = "Club")]
        Club,

        [Display(Name = "School Event")]
        SchoolEvent,

        [Display(Name = "Tutoring Offer")]
        TutoringOffer,

        [Display(Name = "Job Offer")]
        JobOffer,

        [Display(Name = "Other")]
        Other
    }
}
