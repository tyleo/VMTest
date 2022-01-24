using NLua;

namespace LuaVM
{
    public sealed class CLuaVM
    {
        private readonly object[] arg = new object[1];
        private readonly LuaFunction fibonacci;

        public void Prepare(int value) => arg[0] = value;

        public int Fibonacci() => (int)(long)fibonacci.Call(arg)[0];

        public CLuaVM()
        {
            var lua = new Lua();
            lua.DoString(@"
                function fibonacci (iterations)
                    local iterations = iterations - 1
                    local current = 0
                    local next = 1
                    for i = 0, iterations do
                        local lastCurrent = current
                        current = next
                        next = lastCurrent + current
                    end
                    return current
                end
            ");
            fibonacci = (LuaFunction)lua["fibonacci"];
        }
    }
}
