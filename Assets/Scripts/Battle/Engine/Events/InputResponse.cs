namespace BattleEvents
{
    public struct InputResponse
    {
        public Intention Intention { get; }
        public int Selection { get; }

        public InputResponse(Intention intention = Intention.None, int selection = -1)
        {
            Intention = intention;
            Selection = selection;
        }
    }
}
