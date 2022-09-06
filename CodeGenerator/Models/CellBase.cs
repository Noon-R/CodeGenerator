using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace app
{
    public abstract class CellBase
    {
        public SolidColorBrush RectFill => _RectFill;
        public Point Pos => _Pos;
        public float Size => _Size;

        private readonly SolidColorBrush _RectFill;
        private readonly Point _Pos;
        private readonly float _Size;
    }
}
