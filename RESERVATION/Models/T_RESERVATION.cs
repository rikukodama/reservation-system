using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RESERVATION.Models
{
    public class T_RESERVATION
    {
        [Key]
        public int reservationId { get; set; }
        [DataType("nvarchar(15)")]
        [DisplayName("日付")]
        public DateTime date { get; set; }
        [DisplayName("時間")]
        public int coursem_id { get; set; }
        [DisplayName("コース名")]
        public int cource_id { get; set; }
        [DisplayName("オプション名")]
        public string option_id { get; set; }
        [DisplayName("価格")]
        public int price { get; set; }

        [DisplayName("価格")]
        public string username { get; set; }
        [DisplayName("価格")]
        public int phonenumber { get; set; }
        [DisplayName("価格")]
        public string mail { get; set; }
        [DisplayName("日付")]
        public DateTime update { get; set; }
    }
}
