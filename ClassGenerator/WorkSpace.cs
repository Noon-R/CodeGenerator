using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClassGenerator.TextTamplate
{

    public partial class SampleClassTemplate : SampleClassTemplateBase {


        int[,] _Data;

        public void setData(int[,] data) {
            _Data = data;

            
        }



    }


}