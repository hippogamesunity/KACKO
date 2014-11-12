using SimpleJSON;

namespace Assets.Scripts
{
    public static class JsonHelper
    {
        public static int GetInt(JSONNode node)
        {
            return GetInt(node.Value);
        }

        public static int GetInt(string value)
        {
            try
            {
                return (int)float.Parse(value.Replace(",", ".").Replace(" ", null));
            }
            catch
            {
                return 0;
            }
        }
    }
}