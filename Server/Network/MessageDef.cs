using System.Collections.Generic;
using Google.Protobuf;

namespace Network {
    public enum MessageDef {
        Test = 1,
    }

    public static class MessageParserDef {
        public static readonly Dictionary<MessageDef, MessageParser> Parsers = new Dictionary<MessageDef, MessageParser>() {
            { MessageDef.Test, TestMsg.Parser },
        };
    }
}