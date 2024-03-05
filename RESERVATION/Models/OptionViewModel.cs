using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RESERVATION.Models
{
    public class OptionViewModel
    {
        public DateTime res_date { get; set; }
        public int coursem_id { get; set; }
        public int course_id { get; set; }
        public string option_id { get; set; }
        public int price { get; set; }
    }
}