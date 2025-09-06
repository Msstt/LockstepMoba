using UnityEngine.UI;

namespace Framework.Network {
    public struct Message {
        public MessageDef msgId;
        public byte[] data;

        public string ToString() {
            string str = string.Format("ID: {0}, Data: ", msgId);
            for (int i = 0; i < data.Length; ++i) {
                str += string.Format("{0:X2}", data[i]);
            }
            return str;
        }
    }
}