using Cyberstar.Engine.Logging;
using Cyberstar.Strings;
using Raylib_cs;

namespace Cyberstar.Engine.Model3d;

public static class Extensions
{
    public static unsafe bool TryFindBone(this Model self, ReadOnlySpan<char> boneName, out int boneIndex)
    {
        Span<char> readBoneName = stackalloc char[32];
        for (var i = 0; i < self.boneCount; i++)
        {
            var len = readBoneName.FillFromCString(self.bones[i].name);
            if (StringHelper.Compare(boneName, self.bones[i].name, len))
            {
                boneIndex = i;
                return true;
            }
        }

        boneIndex = -1;
        return false;
    }

    public static unsafe void ListBones(this Model self)
    {
        var boneBase = self.bones;
        var bone = boneBase;
        Span<char> str = stackalloc char[128];
        for (var i = 0; i < self.boneCount; i++)
        {
            var chars = str.FillFromCString(bone[i].name);
            Log.Debug($">>> Bone Info: {str.Slice(0, chars)}");
        }
    }
}