using System;
using System.Windows.Media;

namespace app
{
    public class ColorSet {
            public SolidColorBrush Brush => _Brush;
            public int Type => _Type;

            private readonly SolidColorBrush _Brush;
            private readonly int _Type;

            public ColorSet(SolidColorBrush brush, int type)
            {
                _Brush = brush ?? throw new ArgumentNullException(nameof(brush));
                _Type = type;
            }
        }
    }
