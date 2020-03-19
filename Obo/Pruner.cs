namespace Obo
{
    public static class Pruner
    {
        public static void PruneLevels(DotNode node)
        {
            if (node.Status == Status.None) node.Status = Status.Pruned;
            foreach (DotNode childNode in node.Children) PruneLevels(childNode);
        }
    }
}