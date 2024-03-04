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
        public string date { get; set; }
        [DisplayName("時間")]
        public int time { get; set; }
        [DisplayName("コース名")]
        public string courceName { get; set; }
        [DisplayName("オプション名")]
        public string optionName { get; set; }
        [DisplayName("価格")]
        public int price { get; set; }
        [DataType("nvarchar(50)")]
        [DisplayName("メッセージ")]

        public string alertMessage { get; set; }
        [DisplayName("日付")]
        public string update { get; set; }
    }
}
