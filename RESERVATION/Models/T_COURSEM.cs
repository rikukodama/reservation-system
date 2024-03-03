
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RESERVATION.Models
{
    public class T_COURSEM
    {
        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("価格")]
        public string Name { get; set; }
        [DisplayName("価格")]
        public string alertMessage { get; set; }

    }
}
