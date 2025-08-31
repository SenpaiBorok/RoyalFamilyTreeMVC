using RoyalFamily.Models;

namespace RoyalFamily.Services
{
    public class FamilyTreeService
    {
        public FamilyNode Root { get; }

        public FamilyTreeService()
        {
            // Root monarch: Elizabeth II
            Root = new FamilyNode(new RoyalMember("Queen Elizabeth II", new DateOnly(1926, 4, 21), false));

            // Children of Elizabeth II
            var charles = Root.AddChild(new RoyalMember("King Charles III", new DateOnly(1948, 11, 14)));
            var anne = Root.AddChild(new RoyalMember("Anne, Princess Royal", new DateOnly(1950, 8, 15)));
            var andrew = Root.AddChild(new RoyalMember("Prince Andrew, Duke of York", new DateOnly(1960, 2, 19)));
            var edward = Root.AddChild(new RoyalMember("Prince Edward, Duke of Edinburgh", new DateOnly(1964, 3, 10)));

            // Charles’s line
            var william = charles.AddChild(new RoyalMember("William, Prince of Wales", new DateOnly(1982, 6, 21)));
            var harry = charles.AddChild(new RoyalMember("Harry, Duke of Sussex", new DateOnly(1984, 9, 15)));

            william.AddChild(new RoyalMember("Prince George of Wales", new DateOnly(2013, 7, 22)));
            william.AddChild(new RoyalMember("Princess Charlotte of Wales", new DateOnly(2015, 5, 2)));
            william.AddChild(new RoyalMember("Prince Louis of Wales", new DateOnly(2018, 4, 23)));

            harry.AddChild(new RoyalMember("Prince Archie of Sussex", new DateOnly(2019, 5, 6)));
            harry.AddChild(new RoyalMember("Princess Lilibet of Sussex", new DateOnly(2021, 6, 4)));

            // Andrew’s line
            var beatrice = andrew.AddChild(new RoyalMember("Princess Beatrice", new DateOnly(1988, 8, 8)));
            var eugenie = andrew.AddChild(new RoyalMember("Princess Eugenie", new DateOnly(1990, 3, 23)));

            beatrice.AddChild(new RoyalMember("Sienna Mapelli Mozzi", new DateOnly(2021, 9, 18)));
            eugenie.AddChild(new RoyalMember("August Brooksbank", new DateOnly(2021, 2, 9)));
            eugenie.AddChild(new RoyalMember("Ernest Brooksbank", new DateOnly(2023, 5, 30)));

            // Edward’s line
            edward.AddChild(new RoyalMember("Lady Louise Windsor", new DateOnly(2003, 11, 8)));
            edward.AddChild(new RoyalMember("James, Earl of Wessex", new DateOnly(2007, 12, 17)));

            // Anne’s line
            var peter = anne.AddChild(new RoyalMember("Peter Phillips", new DateOnly(1977, 11, 15)));
            var zara = anne.AddChild(new RoyalMember("Zara Tindall", new DateOnly(1981, 5, 15)));

            peter.AddChild(new RoyalMember("Savannah Phillips", new DateOnly(2010, 12, 29)));
            peter.AddChild(new RoyalMember("Isla Phillips", new DateOnly(2012, 3, 29)));

            zara.AddChild(new RoyalMember("Mia Tindall", new DateOnly(2014, 1, 17)));
            zara.AddChild(new RoyalMember("Lena Tindall", new DateOnly(2018, 6, 18)));
            zara.AddChild(new RoyalMember("Lucas Tindall", new DateOnly(2021, 3, 21)));
        }

        public FamilyNode? Find(string name) => Root.FindByName(name);

        public IEnumerable<FamilyNode> DepthFirstOrder(bool includeDeceased = false) =>
            Root.DFS().Where(n => includeDeceased || n.Member.IsAlive);

        public IEnumerable<FamilyNode> BreadthFirstOrder(bool includeDeceased = false) =>
            Root.BFS().Where(n => includeDeceased || n.Member.IsAlive);

        public IReadOnlyList<RoyalMember> GetSuccessionOrder() =>
            Root.DFS().Where(n => n != Root)
                      .Select(n => n.Member)
                      .Where(m => m.IsAlive)
                      .ToList();

        public int? GetPositionInLine(string name)
        {
            var order = GetSuccessionOrder();
            for (int i = 0; i < order.Count; i++)
                if (string.Equals(order[i].Name, name, StringComparison.OrdinalIgnoreCase))
                    return i + 1;
            return null;
        }

        public FamilyNode? AddChild(Guid parentId, string childName, DateOnly dob, bool isAlive)
        {
            var parent = Root.DFS().FirstOrDefault(n => n.Member.Id == parentId);
            if (parent == null) return null;
            return parent.AddChild(new RoyalMember(childName, dob, isAlive));
        }
    }
}
