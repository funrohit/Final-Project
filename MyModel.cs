using System.ComponentModel.DataAnnotations;

namespace coretest.Models
{
    public class MyModel
    {
        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
         public string Email { get; set; }
        [Required]
        public string City { get; set; }
  
     

    }
}
