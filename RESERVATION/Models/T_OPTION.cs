using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RESERVATION.Models
{
    public class T_OPTION
    {
        [Key]
        public int OptionId { get; set; }
        [DisplayName("コースID")]
        public int courceId { get; set; }

        [DataType("nvarchar(15)")]
        [DisplayName("オプション名")]
        public string optionName { get; set; }
        [DisplayName("価格")]
        public int price { get; set; }
        [DataType("nvarchar(50)")]
        [DisplayName("メッセージ")]
        public string alertMessage { get; set; }

    }
}
