using Livet;
namespace CodeGenerator.Models
{
    public class StageDataModel : NotificationObject
        {
            private static readonly int MAX_SCALE = 30;
            private static readonly int MIN_SCALE = 1;

            public int Width => _Width;
            public int Height => _Height;
            public ClassGenerator.TargetDatas.StageCellData StageCellData => _StageCellData;
            public string StageName
            {
                get { return _StageName; }
                set {
                    if (!string.IsNullOrEmpty(value)) {
                        _StageName = value;
                        RaisePropertyChanged();
                    }
                }
            }

            private int _Width;
            private int _Height;
            private ClassGenerator.TargetDatas.StageCellData _StageCellData;
            private string _StageName;

            public StageDataModel() {
            //初期データの設定頑張って
            _StageCellData = new ClassGenerator.TargetDatas.StageCellData() { 
                _CellDatas = new int[,] {
                    {1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                },
                _FloorDatas = new int[,] {
                    {1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                },
                _DirDatas = new int[,] {
                    {1,1,1,1,1,1,1,1},
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                    {1,1,1,1,1,1,1,1 },
                },
            };

                _Width = _StageCellData._CellDatas.GetLength(1);
                _Height = _StageCellData._CellDatas.GetLength(0);
            }

            public void setStageWidth(int width) {
                if (width < MIN_SCALE) {
                    return;
                }
                if (width > MAX_SCALE) {
                    return;
                }

                _Width = width;
                RaisePropertyChanged(nameof(Width));
            }

            public void setStageHeight(int height)
            {
                if (height < MIN_SCALE)
                {
                    return;
                }
                if (height > MAX_SCALE)
                {
                    return;
                }

                _Height = height;
                RaisePropertyChanged(nameof(Height));
            }

            public void resetStageData() {
                _StageCellData._CellDatas = new int[Height, Width];
                _StageCellData._FloorDatas = new int[Height, Width];
                _StageCellData._DirDatas = new int[Height, Width];

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        _StageCellData._FloorDatas[i, j] = 1;
                        _StageCellData._DirDatas[i, j] = 2;
                    }
                }
            }

            public void changeStageData(int x, int y, int data)
            {
                if (x < 0 || _StageCellData._CellDatas.GetLength(1) <= x) {
                    return;
                }
                if (y < 0 || _StageCellData._CellDatas.GetLength(0) <= y)
                {
                    return;
                }
                _StageCellData._CellDatas[y, x] = data;
                RaisePropertyChanged(nameof(StageCellData));
            }
        }

    }
