using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RESERVATION.Models
{
    public class T_COURSE
    {
        [Key]
        public int courceId { get; set; }
        [DataType("nvarchar(15)")]
        [DisplayName("コース名")]
        public string courceName { get; set; }
        [DisplayName("最小数")]
        public int limitMinNum { get; set; }
        [DisplayName("最小数")]
        public int limitMaxNum { get; set; }
        [DisplayName("価格")]
        public int price { get; set; }
        [DataType("nvarchar(50)")]
        [DisplayName("メッセージ")]
        public string alertMessage { get; set; }
        [DisplayName("")]
        public string date { get; set; }
        
    }
}