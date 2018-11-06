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

//                "x3x|xx5|xx2\n" +
//                "1xx|34x|xxx\n" +
//                "xx8|xxx|3xx\n" +
//
//                "5x9|xxx|x1x\n" +
//                "7xx|x1x|xx4\n" +
//                "x1x|xxx|2x5\n" +
//
//                "xx7|xxx|8xx\n" +
//                "xxx|x23|xx6\n" +
//                "8xx|6xx|x2x";
            
//                "xxx|x14|xx9\n" +
//                "4x5|xxx|32x\n" +
//                "xx9|x3x|41x\n" +
//
//                "xx2|64x|x7x\n" +
//                "xxx|x8x|xxx\n" +
//                "x6x|x29|5xx\n" +
//
//                "x54|x9x|1xx\n" +
//                "x93|xxx|8x5\n" +
//                "8xx|45x|xxx";
            
//                "6xx|7xx|x4x\n" +
//                "x35|xx6|xxx\n" +
//                "xxx|2xx|x3x\n" +
//
//                "9xx|x3x|xxx\n" +
//                "xx8|1xx|xx7\n" +
//                "x1x|xx5|2xx\n" +
//
//                "xxx|821|xx4\n" +
//                "7xx|xxx|x1x\n" +
//                "xx4|xxx|5xx";
            
                "xx6|7xx|1xx\n" +
                "x8x|x6x|xx3\n" +
                "9xx|4x1|x8x\n" +

                "xxx|x53|xxx\n" +
                "2xx|xxx|x4x\n" +
                "x7x|6xx|xx2\n" +

                "xxx|xx2|xx5\n" +
                "xx4|x9x|x1x\n" +
                "x9x|3xx|7xx";

            
            
            var sudoku = new Sudoku(s).Solve();
            Console.WriteLine(sudoku);
        }
    }
}