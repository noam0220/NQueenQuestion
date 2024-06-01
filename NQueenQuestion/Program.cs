using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace NQueenQuestion {

    /// <summary>
    /// メイン処理を行うクラス
    /// </summary>
    public class Program {
        //解の個数
        //N=4 → 2, N=5 → 10, N=6 → 4, N=7 → 40, N=8 → 92
        //【問題1】Nという変数を定義して、汎用化しよう。
        //【問題2】IsMatch()を簡略化して、処理を効率化しよう。
        //【問題3】PrintSolution()を簡略化して、処理を効率化しよう。
        //【問題4】回転させたとき、左右反転させたときに重複する組合わせを出力させないようにしよう。

        //クイーンの個数
        //const int N = 5;

        //解のリスト
        static List<Solution> solutionList = new();

        static void Main() {
            var sw = new Stopwatch();
            sw.Start();

            Console.WriteLine("N = 5のときのクイーンの配置");

            //全ての座標の組合わせを生成する。
            var numberCombinationsList = CreateAllCombinations();

            //適当な配置かどうかチェックする。
            foreach(var numComb in numberCombinationsList) {
                if(IsMatch(numComb)) {
                    solutionList.Add(numComb);
                }
            }

            //解を出力する。
            PrintSolution(solutionList);

            sw.Stop();

            Console.WriteLine("経過時間: " + sw.Elapsed.ToString("hh':'mm':'ss'.'fff"));
        }

        /// <summary>
        /// 全ての座標の組み合わせを生成する。(1行に2個以上クイーンが存在する組は省く。)
        /// </summary>
        private static List<Solution> CreateAllCombinations() {

            var allCombs = new List<Solution>();
            var disit = new int[5];
            var tmpPoints = new Point[5];

            //iiを5進法で表し、その数に応じて各座標のY座標の値を設定する。
            for(var ii = 0; ii < (int)Math.Pow(5, 5); ++ii) {
                for(var tmpRow = 1; tmpRow <= 5; tmpRow++) {

                    //商を求める。
                    var quotient = ii / (int)Math.Pow(5, 5 - tmpRow);
                    disit[tmpRow - 1] = quotient % 5 + 1;

                    //各行の座標を設定する。
                    tmpPoints[tmpRow - 1].X = disit[tmpRow - 1];
                    tmpPoints[tmpRow - 1].Y = tmpRow;
                }
                allCombs.Add(new Solution(tmpPoints.ToList()));
            }

            return allCombs;
        }

        /// <summary>
        /// 座標の組が条件を満たすか判定する。
        /// </summary>
        /// <param name="solution">座標の組</param>
        /// <returns></returns>
        public static bool IsMatch(Solution solution) {

            //Pointsの中から2つの座標を選んで、適当かどうかチェックする。(N > q > r >= 0)
            var solArray = solution.Points.ToArray();
            for(var former = 0; former <= 5 - 1; former++) {
                for(var latter = 0; latter <= 5 - 1; latter++) {
                    if(latter != former) {
                        var diff = former - latter;
                        if(solArray[former].X == solArray[latter].X           //同じ列に存在しないかチェック
                        || solArray[former].X == solArray[latter].X - diff    //左斜め前に存在しないかチェック
                        || solArray[former].X == solArray[latter].X + diff    //右斜め前存在しにないかチェック
                        ) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 全ての解を出力する。
        /// </summary>
        /// <param name="solutions">解の組</param>
        public static void PrintSolution(List<Solution> solutions) {

            //全ての要素をまとめる。
            var sbAllSolutions = new StringBuilder();

            var solIndex = 1;
            foreach(var solution in solutions) {
                var sbSolution = new StringBuilder(new string('□', 5 * 5));

                for (var tmpRow = 1; tmpRow < 5 + 1; tmpRow++){
                    for (var tmpCol = 1; tmpCol < 5 + 1; tmpCol++) {

                        //solutionの座標の中に、(tmpRow, tmpCol)が存在していたら'□'を'■'に変更する。
                        if(ExistPoint(solution, tmpRow, tmpCol)){
                            var target = (tmpRow - 1) * 5 + tmpCol - 1;
                            sbSolution.Remove(target, 1).Insert(target, '■');
                        }
                    }
                }

                //行末で改行する。
                for(var row = 5; row > 0; row--) {
                    sbSolution.Insert(row * 5, '\n');
                }

                //1つの解を末尾に追記する。
                sbAllSolutions.Append(sbSolution.Insert(0, "Solution[" + solIndex + "] : \n").Append('\n'));
                solIndex++;
            }

            //解の個数を表示する。
            sbAllSolutions.Append("Total : " + solutions.Count);

            Console.WriteLine(sbAllSolutions);
        }

        //solの座標の中に、特定の座標が存在するかどうかを判定する。
        private static bool ExistPoint(Solution sol, int tRow, int tCol) {
            var existFlg = false;
            foreach(var point in sol.Points) {
                if(point.Y == tRow && point.X == tCol) {
                    existFlg = true;
                    break;
                }
            }
            return existFlg;
        } 
    }
}