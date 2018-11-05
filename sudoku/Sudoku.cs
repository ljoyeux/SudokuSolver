using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using ExtensionMethods;

namespace ExtensionMethods
{
    public static class EnumExtensions
    {
        public static long? Mul(this IEnumerable<int> source)
        {
            long? num1 = null;
            foreach (var num2 in source)
                checked { num1 = (num1!=null) ? num1*num2 : num2; }
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

                    return null;
                    
//                    var tuples = GetLists().Select(x =>
//                        new Tuple<IList<CaseSudoku>, long?>(x,
//                            x.Count(xx => xx.Count > 1)));
//                    
//                    Console.WriteLine("nb " + string.Join("," , tuples.Select(xx=>xx.Item2)));
//                    if (!tuples.Any())
//                    {
//                        return null;
//                    }
//
//                    var min = tuples.Max(cs => cs.Item2);
//                    var first = tuples.First(cs => cs.Item2 == min).Item1;
//                    foreach (var list in tuples)
//                    {
//                        Console.WriteLine(list.Item2);
//                        var els = list.Item1.Where(cs => cs.Count > 1).ToList();
//                        var alternatives = Iterate(els);
//                        Console.WriteLine(string.Join(" | " , els.Select(cs=>string.Join(", ", cs))));
//                        var merged = Merge(alternatives);
//                        Console.WriteLine(string.Join(" | ", merged.Select(m=>string.Join(", ", m))));
//                        Console.WriteLine("----------------");
//                    }
//
//                    return null;
//                    var els = first.Where(cs => cs.Count > 1).ToList();
//                    var alternatives = Iterate(els);
//                    Console.WriteLine("depth : " + _depth);
//                    Console.WriteLine(string.Join(" | " , els.Select(cs=>string.Join(", ", cs))));
////                    Console.WriteLine(string.Join("|", alternatives.Select(i => string.Join(",", i))));
//
//                    var merged = Merge(alternatives);
//                    Console.WriteLine(string.Join(" | ", merged.Select(m=>string.Join(", ", m))));
                    
//                    var save = new List<CaseSudoku>();
//                    foreach (var v in els)
//                    {
//                        save.Add(new CaseSudoku(v));
//                        v.Clear();
//                        v.Add(0);
//                    }
//
//                    foreach (var entry in alternatives)
//                    {
//                        for (var i = 0; i < entry.Length; i++)
//                        {
//                            els[i][0] = entry[i];
//                        } 
//                        
//                        var s = new Sudoku(this, _depth+1).Solve();
//                        if (s!=null)
//                        {
//                            return s;
//                        }
//                    }
//
//                    for (var i = 0; i < save.Count; i++)
//                    {
//                        els[i].Clear();
//                        els[i].AddRange(save[i]);
//                    }
//                    
//                    return null;
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

            foreach (var list in GetLists())
            {
                var notSolved = list.Where(cs => cs.Count > 1).ToList();
                if (!notSolved.Any())
                {
                    continue;
                }
                var alternatives = Iterate(notSolved);
                var merged = Merge(alternatives);

                for (var k = 0; k < notSolved.Count; k++)
                {
                    if (notSolved[k].Count <= merged[k].Count)
                    {
                        continue;
                    }
                    
                    notSolved[k].Clear();
                    notSolved[k].AddRange(merged[k]);
                }
                
            }
            
            return Count();

        }

        private bool Check()
        {
            return _cases.All(c => c.Count == 1) 
                   && 
                   GetLists().All(list => list.Select(c => c[0]).Distinct().Count() == 9);
        }

        private int Count()
        {
            return _cases.Select(c => c.Count).Sum();
        }


        private static List<int[]> Iterate(IReadOnlyList<CaseSudoku> els)
        {
            var limits = els.Select(e => e.Count).ToArray();
            var nbEls = els.Count;
            var counters = new int[nbEls];

            var list = new List<int[]>();
            var used = new int[nbEls];
            for (;;)
            {
                var ok = true;
                for (var k = 0; k < nbEls; k++)
                {

                    var v = els[k][counters[k]];
                    if (k != 0)
                    {
                        for (var l = 0; l < k; l++)
                        {
                            if (used[l] == v)
                            {
                                ok = false;
                                break;
                            }
                        }    
                    }

                    if (!ok)
                    {
                        break;
                    }

                    used[k] = v;
                }

                if (ok)
                {
                    list.Add(used);
                    used = new int[nbEls];
                }

                if (Inc(counters, limits))
                {
                    break;
                }
            }

            return list;
        }

        private List<List<int>> Merge(IReadOnlyList<int[]> list)
        {
            var merged = new List<List<int>>();
            
            foreach (var ints in list)
            {
                if (merged.Count == 0)
                {
                    for (var k = 0; k < ints.Length; k++)
                    {
                        merged.Add(new List<int>());
                    }
                }
                
                for (var k = 0; k < ints.Length; k++)
                {
                    if (!merged[k].Contains(ints[k]))
                    {
                        merged[k].Add(ints[k]);
                    }
                }
            }

            return merged;
        }
        
        private static bool Inc(IList<int> counters, IReadOnlyList<int> limits)
        {
            var nbEls = counters.Count;
            
            var k = 0 ;                
            for (; k < nbEls; k++)
            {
                if (++counters[k] >= limits[k])
                {
                    counters[k] = 0;
                }
                else
                {
                    break;
                }
            }

            return k == nbEls;
            
        }
    }
}