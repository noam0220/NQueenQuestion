using System.Drawing;

namespace NQueenQuestion {
    public class Solution {

        /// <summary>
        /// 解の座標の組
        /// </summary>
        public List<Point> Points { get; private set; }

        /// コンストラクタ
        /// <summary>
        /// </summary>
        /// <param name="points">解の座標の組</param>
        public Solution(List<Point>? points) {
            if(points == null) {
                throw new ArgumentNullException(nameof(points), "Points cannot be null.");
            }
            Points = new List<Point>();
            foreach(var point in points) {
                Points.Add(new Point(point.X, point.Y));
            }
        }
    }
}
