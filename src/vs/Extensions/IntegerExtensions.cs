namespace Luminous.Code.VisualStudio.Extensions.IntegerExtensions
{
    public static class IntegerExtensions
    {
        public static int ToInt(this int? instance)
            => (instance == null)
                ? 0
                : (int)instance;
    }
}
