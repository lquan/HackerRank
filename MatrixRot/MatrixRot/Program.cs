using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MatrixRot
{
  class Program
  {
    /// <summary>
    /// Loads the matrix and the shift value R as described in the input specification
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="M"></param>
    /// <param name="N"></param>
    /// <param name="R"></param>
    /// <param name="matrix"></param>
    static void LoadInput(string filename, out int M, out int N, out int R, out int[,] matrix)
    {
      M = 0;
      N = 0;
      R = 0;
      int lineCount = 0;
      matrix = null;
      foreach (var line in File.ReadLines(filename))
      {
        string[] splitted = line.Split();

        if (lineCount == 0)
        {
          M = int.Parse(splitted[0]);
          N = int.Parse(splitted[1]);
          R = int.Parse(splitted[2]);
          matrix = new int[M, N];
        }
        else
        {
          System.Diagnostics.Debug.Assert(splitted.Length == N);
          for (int i = 0; i < N; i++)
          {
            matrix[lineCount - 1, i] = int.Parse(splitted[i]);
          }
        }

        lineCount++;
      }
    }

    /// <summary>
    /// Constructs the rotated matrix
    /// </summary>
    /// <param name="inputMatrix"></param>
    /// <param name="R"></param>
    /// <param name="outputMatrix"></param>
    static void BuildRotatedMatrix(int[,] inputMatrix, int R, out int[,] outputMatrix)
    {
      int M = inputMatrix.GetLength(0);
      int N = inputMatrix.GetLength(1);
      outputMatrix = new int[M, N];

      for (int level = 0; level < Math.Min(M, N) / 2; level++)
      {
        var circleCoordinates = GetCircleCoordinates(inputMatrix, level);
        var circleValues = circleCoordinates.Select(x => inputMatrix[x.Item1, x.Item2]);
        var circleValuesRotated = Rotated(circleValues, R);

        foreach (var it in circleCoordinates.Zip(circleValuesRotated, (first, second) => new { Coordinate = first, Value = second }))
        {
          outputMatrix[it.Coordinate.Item1, it.Coordinate.Item2] = it.Value;
        }
      }
    }

    /// <summary>
    /// Solves the problem instance given by the file
    /// </summary>
    /// <param name="filename"></param>
    static void Solve(string filename)
    {
      int M, N, R;
      int[,] inputMatrix, outputMatrix;
      LoadInput(filename, out M, out N, out R, out inputMatrix);
      BuildRotatedMatrix(inputMatrix, R, out outputMatrix);
      PrintMatrix(outputMatrix);
    }

    static void LoadInput(out int M, out int N, out int R, out int[,] matrix)
    {
      M = 0;
      N = 0;
      R = 0;

      string line = System.Console.ReadLine();
      string[] splitted = line.Split();
      M = int.Parse(splitted[0]);
      N = int.Parse(splitted[1]);
      R = int.Parse(splitted[2]);

      matrix = new int[M, N];

      for (int i = 0; i < M; i++)
      {
        line = System.Console.ReadLine();
        splitted = line.Split();
        for (int j = 0; j < N; j++)
        {
          matrix[i, j] = int.Parse(splitted[j]);
        }
      }
      
    }


    /// <summary>
    /// Solves a problem instance given in STDIN
    /// </summary>
    static void Solve()
    {
      int M, N, R;
      int[,] inputMatrix, outputMatrix;
      LoadInput(out M, out N, out R, out inputMatrix);
      BuildRotatedMatrix(inputMatrix, R, out outputMatrix);
      PrintMatrix(outputMatrix);
    }


    static void Main(string[] args)
    {
      //Solve(@"input\input0.txt");
      Solve();
      System.Console.ReadLine();
    }

    /// <summary>
    /// Returns the values shifted R times anti-clockwise
    /// </summary>
    /// <param name="array"></param>
    /// <param name="R"></param>
    /// <returns></returns>
    static IEnumerable<int> Rotated(IEnumerable<int> input, int R)
    {
      int[] array = input.ToArray();

      int start = R % array.Length;
      for (int i = start; i < array.Length; i++)
      {
        yield return array[i];
      }
      for (int i = 0; i < start; i++)
      {
        yield return array[i];
      }
    }

    /// <summary>
    /// Returns the coordinates in the matrix starting at element (start,start) in a circular (clockwise) fashion
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    static IEnumerable<Tuple<int, int>> GetCircleCoordinates(int[,] matrix, int start)
    {
      int M = matrix.GetLength(0);
      int N = matrix.GetLength(1);

      //left up to right up 
      for (int i = start; i < N - start; i++)
      {
        yield return Tuple.Create(start, i);
      }

      //right upper to right down
      for (int i = start + 1; i < M - start; i++)
      {
        yield return Tuple.Create(i, N - start - 1);
      }

      //right down to left down
      for (int i = N - start - 2; i >= start; i--)
      {
        yield return Tuple.Create(M - start - 1, i);
      }

      //left down to left up
      for (int i = M - start - 2; i > start; i--)
      {
        yield return Tuple.Create(i, start);
      }
    }


    static void PrintMatrix(int[,] array)
    {
      int M = array.GetLength(0);
      int N = array.GetLength(1);

      for (int i = 0; i < M; i++)
      {
        for (int j = 0; j < N; j++)
        {
          System.Console.Write(array[i, j] + " ");
        }
        System.Console.WriteLine();
      }
    }

  }
}
