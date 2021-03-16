using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quickstart.AspNetCore.Data.Entities
{

    public class TGUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public long? ChatId { get; set; }
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [ForeignKey("Order")]
        public long? LastOrderId { get; set; }
        public Order Order { get; set; }

        public bool IsAdmin { get; set; }
    }
}