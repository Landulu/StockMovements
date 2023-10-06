using System;

namespace FnacDarty.JobInterview.Stock
{
    public class EanId  : IEquatable<EanId>
    {
        public string Value { get; }

        public EanId(string value)
        {
            Value = value;
        }

        
        //NB : J'utilise normalement des records pour manipuler des value objects,
        // mais vu les dépendances de ce projet que je n'ai pas préféré modifier, je réplique ici le comportement d'égalité
        // je suis conscient que c'est un peu overkill, mais je tenais à manipuler un value object pour mon id

        public bool Equals(EanId other)
        {
            if (other is null) return false;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            
            return Equals(obj as EanId);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        
        public static bool operator ==(EanId id1, EanId id2)
        {
            if (ReferenceEquals(id1, id2))
                return true;

            if (id1 is null || id2 is null)
                return false;

            return id1.Equals(id2);
        }

        public static bool operator !=(EanId id1, EanId id2)
        {
            return !(id1 == id2);
        }
    }
}