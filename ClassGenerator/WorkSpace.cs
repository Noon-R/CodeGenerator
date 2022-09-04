using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;


namespace ClassGenerator.TextTamplate
{

    public partial class SampleClassTemplate : SampleClassTemplateBase {


        int[,] _Data;

        public void setData(int[,] data) {
            _Data = data;
        }

        public void ExportDataClass( string relativePath, string fileName)
        {
            if (!Directory.Exists(relativePath))
            {
                Directory.CreateDirectory(relativePath);
            }

            StringBuilder outPutPathBuilder = new StringBuilder();

            outPutPathBuilder.Append(relativePath);
            outPutPathBuilder.Append("/");
            outPutPathBuilder.Append(fileName);
            outPutPathBuilder.Append(".cs");

            using (StreamWriter sw = new StreamWriter(outPutPathBuilder.ToString(), false, Encoding.UTF8))
            {
                sw.Write(TransformText());
            }
        }


        private void WriteArray(int[,] data) {
            ClearIndent();
            PushIndent("                ");
            Write(@"{");
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    Write($"{data[i, j]},");
                }
                Write(@"},");
            }
        }
    }


}