using Sqids;

namespace CommonTestUtilities.idEncryption
{
    public class IdEncripterBuilder
    {
        public static SqidsEncoder<long> Build()
        {
            return new SqidsEncoder<long>(new()
            {
                MinLength = 3,
                Alphabet = "R5atVsSOnC7Tv9oNAhciI683r142H"
            });
        }
    }
}
