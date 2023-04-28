namespace CHR.MenuControllers
{
    public class GlobalKeyCodeOverride
    {
        public readonly int priority;
        public int overrides;

        public GlobalKeyCodeOverride(int priority)
        {
            this.priority = priority;
            overrides = 1;
        }
    }
}