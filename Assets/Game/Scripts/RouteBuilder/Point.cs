namespace RouteBuilder {
    public class Point {
        public int x;
        public int y;
        public int index;    

        public Point(int _x, int _y) {
            x = _x;
            y = _y;
        }

        public Point(int _x, int _y, int _index) : this(_x, _y) {
            index = _index;
        }
    }
}
