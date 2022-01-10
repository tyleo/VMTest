namespace ByteCode
{
    public enum Op : byte
    {
        NoOp,
        Push,
        Pop,
        Add,
        Subtract,
        Multiply,
        Divide,
        BranchIfLess,
        BranchIfGreaterOrEqual,
        Duplicate,
        Load,
        Store,
        End,
        Size,
    }
}
