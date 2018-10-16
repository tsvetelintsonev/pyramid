using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pyramid
{
    class Program
    {
        private static int[][] _pyramid;
        private static IList<Path> _paths { get; set; }

        static void Main(string[] args)
        {
            _paths = new List<Path>();
            ProcessPyramid();
            PrintResults();
        }

        private static void ProcessPyramid()
        {
            _pyramid = CreatePyramid();
            ResolvePaths(0, 0, _pyramid[0][0], new Path()); // root
        }

        private static void ResolvePaths(int columnIndex, int rowIndex, int value, Path path)
        {
            if (columnIndex + 1 == _pyramid.Length) columnIndex -= 1; // Index out of bound protection
            if (rowIndex + 1 == _pyramid.Length) rowIndex -= 1; // Index out of bound protection

            path.Values.Add(value);

            ResolveChildNode(columnIndex + 1, rowIndex, value, path); // Left child node
            ResolveChildNode(columnIndex + 1, rowIndex + 1, value, path); // Right child node

            if (path.Values.Count() == _pyramid.Count()) // save only the fully resolved paths
                _paths.Add(path);
        }

        private static void ResolveChildNode(int columnIndex, int rowIndex, int value, Path path)
        {
            if (IsEven(value) != IsEven(_pyramid[columnIndex][rowIndex]))
                ResolvePaths(columnIndex, rowIndex, _pyramid[columnIndex][rowIndex], new Path(path));
        }

        private static bool IsEven(int nodeValue)
        {
            return nodeValue % 2 == 0;
        }

        private static int[][] CreatePyramid()
        {
            string data = @"215
                            192 124
                            117 269 442
                            218 836 347 235
                            320 805 522 417 345
                            229 601 728 835 133 124
                            248 202 277 433 207 263 257
                            359 464 504 528 516 716 871 182
                            461 441 426 656 863 560 380 171 923
                            381 348 573 533 448 632 387 176 975 449
                            223 711 445 645 245 543 931 532 937 541 444
                            330 131 333 928 376 733 017 778 839 168 197 197
                            131 171 522 137 217 224 291 413 528 520 227 229 928
                            223 626 034 683 839 052 627 310 713 999 629 817 410 121
                            924 622 911 233 325 139 721 218 253 223 107 233 230 124 233";

            return data
                .Split('\n')
                .Select(row => row.Trim().Split(' '))
                .Select(nodes => nodes.Select(node => int.Parse(node)).ToArray()).ToArray();
        }

        private static void PrintResults()
        {
            Path pathWithMaxSum = _paths.OrderByDescending(path => path.Sum()).FirstOrDefault();
            Console.WriteLine($"Total paths resolved: {_paths.Count()}");
            Console.WriteLine($"Path with max sum: {pathWithMaxSum}");
            Console.WriteLine($"Max sum: {_paths.Max(path => path.Sum())}");
            Console.ReadLine();
        }
    }

    internal class Path
    {
        internal Path(Path path) { Values = path.Values.ToList(); }
        internal Path() { Values = new List<int>(); }
        public List<int> Values { get; private set; }
        public override string ToString() => string.Join("->", Values);
        public int Sum() => Values.Sum();
    }
}
