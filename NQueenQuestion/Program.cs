using System.Diagnostics;
using System.Text;

public class Program {
    //N=4 → 2, N=5 → 10, N=6 → 4, N=7 → 40, N=8 → 92
    //【問題】if文をまとめて、処理を高速化しよう。
    //【問題】Nという変数を定義して、汎用化しよう。
    //【問題】数の組み合わせを生成するとき、1行に2個以上クイーンが配置される組み合わせを生成しないようにしよう。

    static List<int[]> numberCombinationsList = new List<int[]>();  //N個の数の組み合わせリスト
    static List<int[]> solutionList = new List<int[]>();            //解のリスト

    static void Main() {
        var sw = new Stopwatch();
        sw.Start();

        Console.WriteLine("N = 5のときのクイーンの配置");

        numberCombinationsList = Combination.Enumerate(Enumerable.Range(1, 5 * 5).ToArray(), 5, false).ToList();

        for(int combCount = 1; combCount < numberCombinationsList.Count; combCount++) {
            if(IsSettable(numberCombinationsList[combCount])) {
                solutionList.Add(numberCombinationsList[combCount]);
            }
        }

        PrintQueens(solutionList);

        sw.Stop();

        Console.WriteLine("経過時間: " + sw.Elapsed.ToString("hh':'mm':'ss'.'fff"));
    }

    //Nクイーンの配置として、適切かどうか判定する。
    private static bool IsSettable(int[] comb) {

        //p番目のクイーンが、何列目にあるかを保持する。
        var points = new Dictionary<int, int>();

        //p番目のクイーンが、p行目に配置されているかどうか判定する。
        for(int p = 1; p <= 5; p++) {
            if(!(comb[p - 1] > 5 * (p - 1) && comb[p - 1] <= 5 * p)) {
                return false;
            }
        }

        //q行目のクイーンを"Q"、r行目のクイーンを"R"とする。
        //"Q"、"R"(q < r)を選択し、"R"が"Q"と同じ列にないかを判定する。
        for(int q = 1; q <= 5; q++) {
            points.Add(q, comb[q - 1] % 5 > 0 ? comb[q - 1] % 5 : 5);
            for(int r = 1; r < q; r++) {
                if(points[r] == points[q]) {
                    return false;
                }
            }
        }

        //上と同様に、"R"の左斜め前に"Q"がないかを判定する。
        for(int q = 1; q <= 5; q++) {
            for(int r = 1; r < q; r++) {
                if(comb[q - 1] - comb[r - 1] == (q - r) * (5 + 1)) {
                    return false;
                }
            }
        }

        //上と同様に、"R"の右斜め前に"Q"がないかを判定する。
        for(int q = 1; q <= 5; q++) {
            for(int r = 1; r < q; r++) {
                if(comb[q - 1] - comb[r - 1] == (q - r) * (5 - 1)) {
                    return false;
                }
            }
        }

        return true;
    }

    //求めた配置を出力する。
    private static void PrintQueens(List<int[]> solutions) {

        var sbAllSolutions = new StringBuilder();

        for(var solutionIndex = 1; solutionIndex <= solutions.Count; solutionIndex++) {
            var sbSolution = new StringBuilder(new string('□', 5 * 5));

            var tmpSolution = solutions[solutionIndex - 1];

            for(var queenIndex = 1; queenIndex <= 5; queenIndex++) {
                sbSolution.Remove(tmpSolution[queenIndex - 1] - 1, 1).Insert(tmpSolution[queenIndex - 1] - 1, '■');
            }

            for(var column = 5; column >= 1; column--) {
                sbSolution.Insert(column * 5, '\n');
            }

            sbAllSolutions.Append(sbSolution.Insert(0, "Solution[" + solutionIndex + "] : \n").Append('\n'));
        }

        sbAllSolutions.Append("Total : " + solutionList.Count); //解の個数を表示

        Console.WriteLine(sbAllSolutions);
    }
}