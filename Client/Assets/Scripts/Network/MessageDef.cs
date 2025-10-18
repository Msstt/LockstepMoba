using System.Collections.Generic;
using Google.Protobuf;

namespace Network {
    public enum MessageDef {
        echo_test_c2s = 10001,
        echo_test_s2c = 20001,

    }

    public static class MessageParserDef {
        public static readonly Dictionary<MessageDef, MessageParser> Parsers = new Dictionary<MessageDef, MessageParser>() {
            { MessageDef.echo_test_c2s, echo_test_c2s.Parser },
            { MessageDef.echo_test_s2c, echo_test_s2c.Parser },

        };
    }
}
