using System;
using System.Collections.Generic;

namespace Navmesh {
    public class Layer {
        
        private NavmeshSurface data;

        private Connection connection;

        public Layer(NavmeshSurface data) {
            this.data = data;
            connection = new Connection(data);
        }

        public bool Init() {
            if (!CheckData()) return false;
            if (!connection.Init()) return false;
            return true;
        }

        private bool CheckData() {
            if (data.indices.Count % 3 != 0) {
                Log.Error("NavmeshSurface indices count error");
                return false;
            }
            return true;
        }
    }
}