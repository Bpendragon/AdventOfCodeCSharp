using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace AdventOfCode.UserClasses
{
    class WeightedGraph
    {
        public List<Vertex> Vertices { get; set; }
        public List<WeightedEdge> Edges { get; set; }

        public WeightedGraph()
        {
            Vertices = new List<Vertex>();
        }

        public void AddVertex(Vertex v)
        {
            Vertices.Add(v);
        }


    }

    class Vertex:IComparable<Vertex>
    {
        string Name { get; set; }
        List<WeightedEdge> Edges = new List<WeightedEdge>();

        public int CompareTo([AllowNull] Vertex other)
        {
           return Name.CompareTo(other.Name);
        }
    }

    class WeightedEdge
    {
        Vertex end1;
        Vertex end2;
        int weight;  
    }
}
