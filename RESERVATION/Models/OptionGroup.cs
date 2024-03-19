using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace RESERVATION.Models
{
    public class OptionGroup
    {
        [Key]
        public int optionGroupId { get; set; }
        [DataType("nvarchar(15)")]
        [DisplayName("コース名")]
        public string optionGroupName { get; set; }
        [DisplayName("コース名")]
        public string selectionType { get; set; }
        [DisplayName("コース名")]
        public List<options> options { get; set; }

    }

    public class options
    {
        public int optionId { get; set; }
        public int courceId { get; set; }
        public string optionName { get; set; }
        public int price { get; set; }
        public string type { get; set; }
        public string alertmessage { get; set; }
    }
}
