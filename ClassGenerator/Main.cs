using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ClassGenerator.TextTamplate;

namespace ClassGenerator
{
    public class Entry
    {
        public static void Main() {
            SampleClassTemplate sample = new SampleClassTemplate();
            int[,] testData = new int[,] {
                { 1,2,1,2,1},
                { 2,3,2,3,2},
                { 3,4,3,4,3},
            };
            sample.setData(testData);

            using (StreamWriter sw = new StreamWriter("./test.cs", false, Encoding.UTF8))
            {
                sw.Write(sample.TransformText());
            }

        }
    }
}
