using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Core.Identity;

namespace Core.Entities
{
    public class Specialization
    {

        public int Id { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Specialization Name is Required")]
        [DisplayName("Specialization Name")]
        public string Name { get; set; } = string.Empty;


        public ICollection<ApplicationUser> Doctors { get; set; } = new List<ApplicationUser>();




    }



}
