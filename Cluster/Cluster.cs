using Transaction;
using System.Collections.Generic;

namespace Cluster;

public class Cluster<K, V> : ICluster<K, V>
{
    //public static HashSet<IAttribute<K, V>> D { get; private set; }

    public Dictionary<IAttribute<K, V>, int> Occ { get; private set; }
    public double N { get; private set; }
    public double S { get; private set; }
    public double W { get { return Occ.Count; } }

    public Cluster()
    {
        N = 0;
        S = 0;
        Occ = new();
    }

    public double DeltaAdd(ITransaction<K, V> t, double r)
    {
        double newS = S + t.AttributeCount;
        double newW = W;
        foreach (IAttribute<K, V> attribute in t.Attributes)
        {
            if (!Occ.ContainsKey(attribute))
            {
                newW++;
            }
        }

        double newG = (newS * (N + 1) / Math.Pow(newW, r));
        double G = (S * N / Math.Pow(W, r));

        if (double.IsNaN(G))
        {
            G = 0;
        }

        return newG - G;
    }
    public double DeltaRemove(ITransaction<K, V> t, double r)
    {
        double newS = S - t.AttributeCount;
        double newW = W;
        foreach (IAttribute<K, V> attribute in t.Attributes)
        {
            if (Occ[attribute] == 1)
            {
                newW--;
            }
        }

        double G = (S * N / Math.Pow(W, r));
        double newG = (newS * (N - 1) / Math.Pow(newW, r));

        if (double.IsNaN(newG))
        {
            newG = 0;
        }

        return G - newG;
    }

    public void Add(ITransaction<K, V> t)
    {
        N++;
        S += t.AttributeCount;
        foreach (IAttribute<K, V> attribute in t.Attributes)
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
    public void Remove(ITransaction<K, V> t)
    {
        N--;
        S -= t.AttributeCount;
        foreach (IAttribute<K, V> attribute in t.Attributes)
        {
            Occ[attribute]--;
            if (Occ[attribute] == 0)
            {
                Occ.Remove(attribute);
            }
        }
    }
}

