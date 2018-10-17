using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using ExtensionMethods;

namespace ExtensionMethods
{
    public static class EnumExtensions
    {
        public static long Mul(this IEnumerable<int> source)
        {
            long num1 = 1;
            foreach (var num2 in source)
                checked { num1 *= num2; }
            return num1;
        }

//        public static T Min<T>(this IEnumerable<T> source, Func<T, long> selector)
//        {
//            var enumerator = source.GetEnumerator();
//            T min = default(T);
//            foreach (var i in source)
//            {
//            }
//            
//            return min;
//        }
        
    }

}

namespace sudoku
{
    
    internal class CaseSudoku : List<int>
    {
        public CaseSudoku()
        {
        }

        public CaseSudoku(IEnumerable<int> collection) : base(collection)
        {
        }
    }
    
    public class Sudoku
    {
        private IList<CaseSudoku> _cases;
        
        private IList<IList<CaseSudoku>> _lists;
        private int _depth;

        public Sudoku(Sudoku sudoku, int depth)
        {

            _depth = depth;
            _cases = new List<CaseSudoku>();
            foreach (var sudokuCase in sudoku._cases)
            {
                if (sudokuCase.Count == 1)
                {
                    _cases.Add(sudokuCase); 
                }
                else
                {
                    var cloned = new CaseSudoku();
                    cloned.AddRange(sudokuCase);
                    _cases.Add(cloned);
                }
            }
        }
        
        
        public Sudoku(string grille = null)
        {
            _lists = null;
            
            _cases = new List<CaseSudoku>();
            for (var l = 0; l < 9; l++)
            {
                for (var c = 0; c < 9; c++)
                {
                    _cases.Add(new CaseSudoku());
                }
            }

            string[] lines = grille.Split('\n');
            for (var l = 0; l < 9; l++)
            {
                string line = lines[l].Replace("|", "");

                for (var c = 0; c < 9; c++)
                {
                    char k = line[c];
                    if (k >= '0' && k <= '9')
                    {
                        _cases[l*9+c].AddRange(new[] {k - '0'});
                    }
                    else
                    {
                        _cases[l*9+c].AddRange( new[] {1, 2, 3, 4, 5, 6, 7, 8, 9});
                    }
                }
            }
        }

        public override string ToString()
        {
            string str = "";

            for (var l = 0; l < 9; l++)
            {
                if (l % 3 == 0)
                {
                    str += "-------------\n";
                }

                for (var c = 0; c < 9; c++)
                {
                    if (c % 3 == 0)
                    {
                        str += "|";
                    }

                    var cc = _cases[9*l+c];
                    str += (cc.Count == 1) ? (char) (cc[0] + '0') : '*';
                }

                str += "|";

                str += "\n";
            }

            str += "-------------\n";

            return str;
        }

        private IList<IList<CaseSudoku>> GetLists()
        {
            if (_lists != null)
            {
                return _lists;
            }

            _lists = new List<IList<CaseSudoku>>();

            for (var l = 0; l < 9; l++)
            {
                var cc = new List<CaseSudoku>();
                for (var c = 0; c < 9; c++)
                {
                    cc.Add(_cases[l*9+c]);
                }
                
                _lists.Add(cc);
            }

            for (var c = 0; c < 9; c++)
            {
                var cc = new List<CaseSudoku>();
                for (var l = 0; l < 9; l++)
                {
                    cc.Add(_cases[l*9+c]);
                }
                
                _lists.Add(cc);
            }

            for (var i = 0; i < 9; i+=3)
            {
                for (var j = 0; j < 9; j+=3)
                {
                    var cc = new List<CaseSudoku>();

                    for (var l = 0; l < 3; l++)
                    {
                        for (var c = 0; c < 3; c++)
                        {
                            cc.Add(_cases[(i+l)*9+j+c]);
                        }
                    }
                    
                    _lists.Add(cc);
                }
            }
            
            return _lists;
        }

        enum Status
        {
            Solved, Unsolved
        }

        public Sudoku Solve()
        {
            var c = Count();
            for (;;)
            {
                var newCount = _Solve();
                if (newCount == c)
                {
                    if (Check())
                    {
                        return this;
                    }

                    if (_cases.Any(cs => cs.Count == 0))
                    {
                        return null;
                    }

//                    Console.WriteLine(_cases.Where(cs => cs.Count >= 2 && cs.Count<=3).Select(cs => cs.Count).Mul());
//                    Console.WriteLine(string.Join(" x ", _cases.Where(cs => cs.Count > 1).Select(cs => cs.Count)));
//                    var counters = _cases.GroupBy(cs => cs.Count).ToDictionary(x => x.Key, x => x.AsEnumerable().Count());
//                    Console.WriteLine(string.Join(", ", counters.Select(x=> x.Key + ": " + x.Value)));
//                    Console.WriteLine(counters.Values.Sum());
//                    Console.WriteLine(_cases.Count(cs => cs.Count == 2));
                    Console.WriteLine(string.Join(", ", GetLists().Select(x => x.Count(xx=>xx.Count>1))));
                    var tuples = GetLists().Select(x =>
                        new Tuple<IList<CaseSudoku>, long>(x,
                            x.Where(xx => xx.Count > 1).Select(xx => xx.Count).Mul()));

                    var min = tuples.Min(cs => cs.Item2);
                    var first = tuples.Where(cs => cs.Item2 == min).First().Item1;
                    var els = first.Where(cs => cs.Count > 1).ToList();

                    var save = new List<CaseSudoku>();
                    foreach (var v in els)
                    {
                        save.Add(new CaseSudoku(v));
                        v.Clear();
                        v.Add(0);
                    }
                    
                    Console.WriteLine(string.Join(" | " , els.Select(cs=>string.Join(", ", cs))));


                    for (var i = 0; i < save.Count; i++)
                    {
                        els[i].Clear();
                        els[i].AddRange(save[i]);
                    }
                    
                    return null;
                    // la meilleure approche est de sélectionner que les alternatives binaires. 

                    if (_depth > 0)
                    {
                        return null;
                    }
                    var listBinary = _cases.Where(cs => cs.Count == 2).ToList();
                    if (listBinary.Count == 0)
                    {
//                        Console.WriteLine("No binary alternatives");
                        return null;
                    }
                    
                    var nbBinary = listBinary.Count;
                    var saveBinary = new List<CaseSudoku>();
                    listBinary.ForEach(l =>
                    {
                        saveBinary.Add(new CaseSudoku(l));
                        l.Clear();
                        l.Add(0);
                    });
                    
                    var nbIterations = 1 << nbBinary;
                    
                    Console.WriteLine(nbBinary + " :  " + _depth);

                    for (var i = 0; i < nbIterations; i++)
                    {
                        for (var b = 0; b < nbBinary; b++)
                        {
                            listBinary[b][0] = ((i & b) != 0) ? saveBinary[b][1] : saveBinary[b][0];
                        }
                        
                        var s = new Sudoku(this, _depth+1).Solve();
                        if (s != null)
                        {
                            return s;
                        }                        
                    }
                    
                    for (var i = 0; i < nbBinary; i++)
                    {
                        listBinary[i].Clear();
                        listBinary[i].AddRange(saveBinary[i]);
                    }
                }

                c = newCount;
            }
        }
        
        private int _Solve()
        {
            foreach (var list in GetLists())
            {
//                var unsolved = list.Where(c=>c.Count>1).ToList();
//                if (unsolved.Count == 0)
//                {
//                    continue;
//                }
//                var collect = list.Where(c => c.Count == 1).Select(c=>c[0]).ToList();
//                unsolved.ForEach(c=> c.RemoveAll(collect.Contains));

                var l = list.GroupBy(c => c.Count == 1 ? Status.Solved : Status.Unsolved).ToDictionary(x => x.Key, x => x.AsEnumerable().ToList());
                if (l.Count == 1)
                {
                    continue;
                }
                
                var collect = l[Status.Solved].Select(c=>c[0]).ToList();
                l[Status.Unsolved].ForEach(c=> c.RemoveAll(collect.Contains));
            }

            return Count();

        }

        public bool Check()
        {
            return _cases.All(c => c.Count == 1) 
                   && 
                   GetLists().All(list => list.Select(c => c[0]).Distinct().Count() == 9);
        }

        public int Count()
        {
            return _cases.Select(c => c.Count).Sum();
        }
    }
}