using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public abstract class Entity<TId>(TId id) {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TId Id { get; set; } = id;
    
    public override bool Equals(object obj) {
        if (obj is not Entity<TId> other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        if (Id.Equals(default) || other.Id.Equals(default)) return false;
        return Id.Equals(other.Id);
    }
    
    public static bool operator ==(Entity<TId> a, Entity<TId> b) {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }
    
    public static bool operator !=(Entity<TId> a, Entity<TId> b) => !(a == b);
    
    public override int GetHashCode() => Id.GetHashCode();
}