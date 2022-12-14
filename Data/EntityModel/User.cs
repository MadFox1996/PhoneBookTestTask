using System.ComponentModel.DataAnnotations;

namespace Data.EntityModel
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Login { get; set; }

        public byte[] Hash { get; set; }

        public byte[] Salt { get; set; }
    }
}
