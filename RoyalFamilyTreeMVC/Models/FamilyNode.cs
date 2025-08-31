using System.Collections.ObjectModel;

namespace RoyalFamily.Models
{
    public class FamilyNode
    {
        public RoyalMember Member { get; set; }
        public FamilyNode? Parent { get; set; }
        public Collection<FamilyNode> Children { get; } = new();

        public FamilyNode(RoyalMember member)
        {
            Member = member;
        }

        public FamilyNode AddChild(RoyalMember child)
        {
            var node = new FamilyNode(child) { Parent = this };
            Children.Add(node);

            // Keep children sorted by birth date
            var ordered = Children.OrderBy(c => c.Member.DateOfBirth).ToList();
            Children.Clear();
            foreach (var c in ordered) Children.Add(c);

            return node;
        }

        public IEnumerable<FamilyNode> DFS()
        {
            yield return this;
            foreach (var child in Children)
                foreach (var d in child.DFS())
                    yield return d;
        }

        public IEnumerable<FamilyNode> BFS()
        {
            var q = new Queue<FamilyNode>();
            q.Enqueue(this);
            while (q.Count > 0)
            {
                var n = q.Dequeue();
                yield return n;
                foreach (var c in n.Children)
                    q.Enqueue(c);
            }
        }

        public FamilyNode? FindByName(string name) =>
            DFS().FirstOrDefault(n => string.Equals(n.Member.Name, name,
                StringComparison.OrdinalIgnoreCase));
    }
}
