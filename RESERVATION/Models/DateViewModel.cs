using System.ComponentModel.DataAnnotations.Schema;

namespace RESERVATION.Models
{
    public class DateViewModel
    {
        public DateTime res_date { get; set; }
        public int coursem_id { get; set; }
    }
}
