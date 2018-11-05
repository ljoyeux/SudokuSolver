using System;
using System.Collections.Generic;
using System.Linq;

namespace sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string s =
//                "xxx|4x5|xx1\n" +
//                "6x5|198|xxx\n" +
//                "xx9|xx7|x5x\n" +
//
//                "x6x|8xx|517\n" +
//                "x7x|xxx|x9x\n" +
//                "581|xx3|x4x\n" +
//
//                "x9x|2xx|8xx\n" +
//                "xxx|519|4x2\n" +
//                "2xx|7x6|xxx\n";

//                "x72|xx5|4xx\n" +
//                "6xx|x2x|xxx\n" +
//                "4xx|x3x|x5x\n" +
//
//                "x58|xx2|xx1\n" +
//                "x6x|xxx|x8x\n" +
//                "7xx|5xx|23x\n" +
//
//                "x4x|x9x|xx5\n" +
//                "xxx|x1x|xx4\n" +
//                "xx7|4xx|92x";

                "x3x|xx5|xx2\n" +
                "1xx|34x|xxx\n" +
                "xx8|xxx|3xx\n" +

                "5x9|xxx|x1x\n" +
                "7xx|x1x|xx4\n" +
                "x1x|xxx|2x5\n" +

                "xx7|xxx|8xx\n" +
                "xxx|x23|xx6\n" +
                "8xx|6xx|x2x";

            List<int> els = new List<int> (new[] {1, 2, 3, 4, 5, 6, 7, 8, 9});
            var groupBy = els.GroupBy(c => c > 5 ? 0 : 1);

            var enumerators = groupBy.ToDictionary(x => x.Key, x => new List<int>(x.AsEnumerable()));
            Console.WriteLine(string.Join(", ", enumerators[0]));
            Console.WriteLine(string.Join(", ", enumerators[1]));

//            List<int> collect = new List<int>(new[] {3, 4});
//            els.RemoveAll(collect.Contains);
//            Console.WriteLine(string.Join(",", els));
            
            var sudoku = new Sudoku(s);
            
            Console.WriteLine(sudoku.Solve());
            Console.WriteLine(sudoku);
        }
    }
}