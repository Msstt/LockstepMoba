using System.Collections.Generic;
using Google.Protobuf;

namespace Network {
    public enum MessageDef {
        test_c2s = 1,

    }

    public static class MessageParserDef {
        public static readonly Dictionary<MessageDef, MessageParser> Parsers = new Dictionary<MessageDef, MessageParser>() {
            { MessageDef.test_c2s, test_c2s.Parser },

        };
    }
}
