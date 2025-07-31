using FileTypeChecker.Extensions;
using FileTypeChecker.Types;

namespace RecipeBook.Application.Extensions
{
    public static class StreamImageExtensions
    {
        public static (bool isValidImage, string extension) ValidateAnGetImageExtension(this Stream stream)
        {
            var result = (false, string.Empty);

            if (stream.Is<PortableNetworkGraphic>())
                result = (true, NormalizeExtensions(PortableNetworkGraphic.TypeExtension));
            else if (stream.Is<JointPhotographicExpertsGroup>())
                result = (true, NormalizeExtensions(JointPhotographicExpertsGroup.TypeExtension));

            stream.Position = 0;

            return result;
        }

        private static string NormalizeExtensions(string extension)
        {
            return extension.StartsWith('.') ? extension : $".{extension}";
        }
    }
}
