namespace RestorantApp.Entity.Entities.Common;

public class BaseEntity
{
    public int Id { get; set; }
}
public class AuditAble : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
