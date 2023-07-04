using Cyberstar.Sprites;
using Cyberstar.UI.ViewFragments;

namespace Cyberstar.UI;

public struct SpriteAtlasGridView
{
    public DimensionsFragment Dimensions;
    public ViewBackgroundFragment Background;
    public SpriteAtlas SpriteAtlas;
    public int Columns;
    public int Rows;
    public int ColumnSpacing;
    public int RowSpacing;
    public Action<int, int> OnCellClick;

    public int CellWidth => (Dimensions.ContentWidth - ColumnSpacing * (Columns + 1)) / Columns;
    public int CellHeight => (Dimensions.ContentHeight - RowSpacing * (Rows + 1)) / Rows;
}