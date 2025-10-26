using System.Collections.Generic;
using Google.Protobuf;

namespace Network {
    public enum MessageDef {
        echo_test_c2s = 1001,
        echo_test_s2c = 11001,
        frame_start_s2c = 11002,
        frame_input_c2s = 1003,
        frame_input_s2c = 11003,
        battle_start_s2c = 11004,
        test_input = 20001,

    }

    public static class MessageParserDef {
        public static readonly Dictionary<MessageDef, MessageParser> Parsers = new Dictionary<MessageDef, MessageParser>() {
            { MessageDef.echo_test_c2s, echo_test_c2s.Parser },
            { MessageDef.echo_test_s2c, echo_test_s2c.Parser },
            { MessageDef.frame_start_s2c, frame_start_s2c.Parser },
            { MessageDef.frame_input_c2s, frame_input_c2s.Parser },
            { MessageDef.frame_input_s2c, frame_input_s2c.Parser },
            { MessageDef.battle_start_s2c, battle_start_s2c.Parser },
            { MessageDef.test_input, test_input.Parser },

        };
    }
}
