public struct Shield
{
    public int Charges { get; }
    public Element Element { get; }

    public Shield(int charges = 0, Element element = Element.None)
    {
        Charges = charges;
        Element = element;
    }
}
