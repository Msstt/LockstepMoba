using System;
using System.Collections.Generic;
using Google.Protobuf;

namespace Network {
    public enum MessageDef {
        echo_test_c2s = 1001,
        echo_test_s2c = 11001,
        frame_start_s2c = 11002,
        frame_input_c2s = 1003,
        frame_input_s2c = 11003,
        frame_reconnect_c2s = 1004,
        battle_start_s2c = 11004,
        select_champion_c2s = 1005,
        select_champion_s2c = 11005,
        test_input = 20001,
        skill_input = 20002,
        level_input = 20003,

    }
    
    public static class MessageMapping {
        public static readonly Dictionary<MessageDef, Type> type = new Dictionary<MessageDef, Type>() {
            { MessageDef.echo_test_c2s, typeof(echo_test_c2s) },
            { MessageDef.echo_test_s2c, typeof(echo_test_s2c) },
            { MessageDef.frame_start_s2c, typeof(frame_start_s2c) },
            { MessageDef.frame_input_c2s, typeof(frame_input_c2s) },
            { MessageDef.frame_input_s2c, typeof(frame_input_s2c) },
            { MessageDef.frame_reconnect_c2s, typeof(frame_reconnect_c2s) },
            { MessageDef.battle_start_s2c, typeof(battle_start_s2c) },
            { MessageDef.select_champion_c2s, typeof(select_champion_c2s) },
            { MessageDef.select_champion_s2c, typeof(select_champion_s2c) },
            { MessageDef.test_input, typeof(test_input) },
            { MessageDef.skill_input, typeof(skill_input) },
            { MessageDef.level_input, typeof(level_input) },

        };
    }

    public static class MessageParserDef {
        public static readonly Dictionary<MessageDef, MessageParser> Parsers = new Dictionary<MessageDef, MessageParser>() {
            { MessageDef.echo_test_c2s, echo_test_c2s.Parser },
            { MessageDef.echo_test_s2c, echo_test_s2c.Parser },
            { MessageDef.frame_start_s2c, frame_start_s2c.Parser },
            { MessageDef.frame_input_c2s, frame_input_c2s.Parser },
            { MessageDef.frame_input_s2c, frame_input_s2c.Parser },
            { MessageDef.frame_reconnect_c2s, frame_reconnect_c2s.Parser },
            { MessageDef.battle_start_s2c, battle_start_s2c.Parser },
            { MessageDef.select_champion_c2s, select_champion_c2s.Parser },
            { MessageDef.select_champion_s2c, select_champion_s2c.Parser },
            { MessageDef.test_input, test_input.Parser },
            { MessageDef.skill_input, skill_input.Parser },
            { MessageDef.level_input, level_input.Parser },

        };
    }
}
