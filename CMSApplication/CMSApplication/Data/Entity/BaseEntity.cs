using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSApplication.Data.Entity
{
    public class BaseEntity<TKey> : BaseEntity where TKey : struct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }



    }

    public interface IBaseEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime? ModifiedAt { get; set; }
    }

    public abstract class BaseEntity : IBaseEntity
    {
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? ModifiedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
