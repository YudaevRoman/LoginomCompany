using Transaction;
using System.Collections.Generic;

namespace Cluster;

public class Cluster : ICluster<int, string>
{
    //public static HashSet<IAttribute<int, string>> D { get; private set; }

    public Dictionary<IAttribute<int, string>, int> Occ { get; private set; }
    public double N { get; private set; }
    public double S { get; private set; }
    public double W { get { return Occ.Count; } }

    Cluster()
    {
        N = 0;
        S = 0;
        Occ = new();
    }

    public double DeltaAdd(ITransaction<int, string> t, double r)
    {
        double newS = S + t.AttributeCount;
        double newW = W;
        foreach (IAttribute<int, string> attribute in t.Attributes)
        {
            if (!Occ.ContainsKey(attribute))
            {
                newW++;
            }
        }

        return (newS * (N + 1) / Math.Pow(newW, r)) - (S * N / Math.Pow(W, r));
    }
    public double DeltaRemove(ITransaction<int, string> t, double r)
    {
        double newS = S - t.AttributeCount;
        double newW = W;
        foreach (IAttribute<int, string> attribute in t.Attributes)
        {
            if (Occ[attribute] - 1 == 0)
            {
                newW--;
            }
        }

        return (S * N / Math.Pow(W, r)) - (newS * (N + 1) / Math.Pow(newW, r));
    }

    public void Add(ITransaction<int, string> t)
    {
        N++;
        S += t.AttributeCount;
        foreach (IAttribute<int, string> attribute in t.Attributes)
        {
            if (Occ.ContainsKey(attribute))
            {
                Occ[attribute]++;
            }
            else
            {
                Occ.Add(attribute, 1);
                //D.Add(attribute);
            }
        }
    }
    public void Remove(ITransaction<int, string> t)
    {
        N--;
        S -= t.AttributeCount;
        foreach (IAttribute<int, string> attribute in t.Attributes)
        {
            Occ[attribute]--;
            if (Occ[attribute] == 0)
            {
                Occ.Remove(attribute);
            }
        }
    }
}

