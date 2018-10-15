using System;
using System.Collections.Generic;
using System.Linq;
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
        
    }

}

namespace sudoku
{
    
    internal class CaseSudoku : List<int>
    {
    }
    
    public class Sudoku
    {
        private IList<CaseSudoku> _cases;
        
        private IList<IList<CaseSudoku>> _lists;

        public Sudoku(Sudoku sudoku)
        {
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

                    Console.WriteLine(_cases.Where(cs => cs.Count >= 2 && cs.Count<=3).Select(cs => cs.Count).Mul());
                    Console.WriteLine(string.Join(" x ", _cases.Where(cs => cs.Count > 1).Select(cs => cs.Count)));
                    var counters = _cases.GroupBy(cs => cs.Count).ToDictionary(x => x.Key, x => x.AsEnumerable().Count());
                    Console.WriteLine(string.Join(", ", counters.Select(x=> x.Key + ": " + x.Value)));
                    Console.WriteLine(counters.Values.Sum());
                    Console.WriteLine(_cases.Count(cs => cs.Count == 2));
                    return null;
                    
                    var unsolvedCaseSudoku = _cases.FirstOrDefault(cs => cs.Count > 1);
                    if (unsolvedCaseSudoku == null || unsolvedCaseSudoku.Count==0)
                    {
                        return null;
                    }

                    var index = _cases.IndexOf(unsolvedCaseSudoku);
                    if (index == -1)
                    {
                        Console.WriteLine("Erreur");
                        throw new Exception("Erreur");
                    }

                    var forcedCaseSudoku = new CaseSudoku();
                    _cases[index] = forcedCaseSudoku;
                    forcedCaseSudoku.Add(0);
                    
                    foreach (var n in unsolvedCaseSudoku)
                    {
                        forcedCaseSudoku[0] = n;
                        _lists = null;
//                        Console.WriteLine(forcedCaseSudoku.Count);
//                        Console.WriteLine(string.Join(", ", _cases.Select(cc=>cc.Count)));
                        var check = Check();
                        Sudoku s = new Sudoku(this).Solve();
                        if (s != null)
                        {
                            return s;
                        }
                    }

                    _cases[index] = unsolvedCaseSudoku;
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