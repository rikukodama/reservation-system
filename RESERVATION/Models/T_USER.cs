using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RESERVATION.Models
{
    public class T_USER
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("名前")]
        public string Name { get; set; }
        [DisplayName("住所")]
        public string Address { get; set; }
        [DisplayName("連絡先")]
        public string Contact { get; set; }
        [DisplayName("誕生日")]
        public string Birthday { get; set; }
        [DisplayName("性別")]
        public string Gender { get; set; }
        [DisplayName("利用履歴")]
        public string History { get; set; }


    }
}