using System;
using System.Collections.Generic;

namespace Network {
    public static class NetworkDef {
        public static readonly List<Type> Dispatcher = new List<Type>() {
            typeof(TestMsgDispatcher),
        };
    }
}
