using System.Numerics;
using Raylib_cs;

namespace Cyberstar.Extensions.Raylib;

public static class RectangleExtensions
{
    public static bool Contains(this Rectangle self, float x, float y) => self.x <= x && self.x + self.width >= x && self.y <= y && self.y + self.height >= y;
    public static bool Contains(this Rectangle self, Vector2 pos) => Contains(self, pos.X, pos.Y);
}