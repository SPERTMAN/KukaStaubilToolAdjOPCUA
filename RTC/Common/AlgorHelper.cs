using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RTC.MainFrm;
using Model;
namespace Common
{
    public class AlgorHelper
    {


        /// <summary>
        /// 求圆心坐标以及半径的
        /// </summary>
        /// <param name="M"></param>
        public static double[] FitPlaneAndCalculateRadius(List<Point3D> Cr, out double radius)
        {
            int length = Cr.Count;
            double[,] M = new double[length, 3];
            for (int i = 0; i < length; i++)
            {
                M[i, 0] = Cr[i].X;
                M[i, 1] = Cr[i].Y;
                M[i, 2] = Cr[i].Z;
            }

            int num = M.GetLength(0);
            int dim = M.GetLength(1);

            //L1的矩阵
            double[,] L1 = new double[num, 1];
            for (int i = 0; i < num; i++)
            {
                L1[i, 0] = 1;
            }
            //M的转置*M
            double[,] MTM = MatrixMultiply(MatrixTranspose(M), M);
            //inv(M'*M）
            double[,] invMTM = MatrixInverse(MTM);
            //M' * L1
            double[,] MT_L1 = MatrixMultiply(MatrixTranspose(M), L1);
            //  A = inv(M'*M)*M' * L1;       % 求解平面法向量
            double[,] A = MatrixMultiply(invMTM, MT_L1);


            int _AutoStep = 0;
            double[,] B = new double[(num - 1) * num / 2, 3];
            for (int i = 1; i < num; i++)
            {
                for (int j = i + 1; j <= num; j++)
                {
                    _AutoStep++;

                    for (int k = 0; k < 3; k++)
                    {
                        B[_AutoStep - 1, k] = M[j - 1, k] - M[i - 1, k];
                    }
                }
            }

            //这相当于计算矩阵M中两行之间的平方和的差值再除以2。
            //向量L2中的每个元素都记录了矩阵M中两行之间的差值。
            double[] L2 = new double[(num - 1) * num / 2];
            _AutoStep = 0;
            for (int i = 1; i < num; i++)
            {
                for (int j = i + 1; j <= num; j++)
                {
                    _AutoStep++;
                    L2[_AutoStep - 1] = (Math.Pow(M[j - 1, 0], 2) + Math.Pow(M[j - 1, 1], 2) + Math.Pow(M[j - 1, 2], 2)
                        - Math.Pow(M[i - 1, 0], 2) - Math.Pow(M[i - 1, 1], 2) - Math.Pow(M[i - 1, 2], 2)) / 2;
                }
            }

            double[,] L2Matrix = new double[L2.Length, 1];
            for (int i = 0; i < L2.Length; i++)
            {
                L2Matrix[i, 0] = L2[i];
            }


            double[,] D = new double[4, 4];
            double[,] BTB = MatrixMultiply(MatrixTranspose(B), B); // 计算矩阵 B 的转置矩阵与 B 相乘的结果
            double[,] ATA = MatrixTranspose(A);
            // 将 BTB 的值赋给 D 的左上角3×3子矩阵
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    D[i, j] = BTB[i, j];
                }
            }
            // 将 A 的转置向量赋给 D 的第4行
            for (int i = 0; i < 3; i++)
            {
                D[3, i] = ATA[0, i];
            }
            // 将 A 的向量赋给 D 的第1到3列
            for (int i = 0; i < 3; i++)
            {
                D[i, 3] = A[i, 0];
            }


            double[,] BTranspose = MatrixTranspose(B); // 计算矩阵 B 的转置矩阵
            double[,] BTimesL2 = MatrixMultiply(BTranspose, L2Matrix); // 将矩阵 B 的转置与向量 L2 相乘

            int numRows = BTimesL2.GetLength(0);
            int numCols = BTimesL2.GetLength(1);
            double[,] L3 = new double[numRows + 1, numCols];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    L3[i, j] = BTimesL2[i, j];
                }
            }

            L3[numRows, numCols - 1] = 1; // 在最后一行最后一个元素位置添加1


            // 计算 D' 的逆矩阵
            double[,] DTranspose = MatrixTranspose(D); // 计算矩阵 D的转置矩阵
            double[,] invDTD = MatrixInverse(DTranspose);

            // 计算 C = inv(D') * L3
            double[,] MT_C = MatrixMultiply(invDTD, L3);
            //表示该平面距离其圆心所有点到圆心的距离平方的和。
            double dsqSum = 0;
            //处理点集分别计算该点到已拟合的圆心位置的偏移量并更新 dsqSum 的值。
            for (var i = 0; i < num; i++)
            {
                var dx = M[i, 0] - MT_C[0, 0];
                var dy = M[i, 1] - MT_C[1, 0];
                var dz = M[i, 2] - MT_C[2, 0];
                dsqSum += dx * dx + dy * dy + dz * dz;
            }
            //利用所有计算出的点到圆心距离平方的和除以点数
            //n得到平均距离然后开放平方根即可获得该平面的半径大小。
            radius = Math.Sqrt(dsqSum / num);
            double[] result = new double[3];
            result[0] = MT_C[0, 0];//x
            result[1] = MT_C[1, 0];//y
            result[2] = MT_C[2, 0];//z
            return result;
        }
        /// <summary>
        /// 矩阵的转置
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        static double[,] MatrixTranspose(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            double[,] result = new double[cols, rows];
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    result[i, j] = matrix[j, i];
                }
            }
            return result;
        }

        /// <summary>
        /// 矩阵相乘
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        static double[,] MatrixMultiply(double[,] matrix1, double[,] matrix2)
        {
            int rows1 = matrix1.GetLength(0);
            int cols1 = matrix1.GetLength(1);
            int rows2 = matrix2.GetLength(0);
            int cols2 = matrix2.GetLength(1);
            if (cols1 != rows2)
            {
                throw new ArgumentException("The number of columns in matrix1 must be equal to the number of rows in matrix2.");
            }
            double[,] result = new double[rows1, cols2];
            for (int i = 0; i < rows1; i++)
            {
                for (int j = 0; j < cols2; j++)
                {
                    for (int k = 0; k < cols1; k++)
                    {
                        result[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 矩阵的伪逆
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        static double[,] MatrixInverse(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[,] result;
            if (n == 2)
            {
                double det = matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
                if (det == 0)
                {
                    throw new ArgumentException("The matrix is singular and cannot be inverted.");
                }
                double[,] inverse = {
                    { matrix[1, 1] / det, -matrix[0, 1] / det },
                    { -matrix[1, 0] / det, matrix[0, 0] / det }
                };
                result = inverse;
            }
            else
            {
                double det = MatrixDeterminant(matrix);
                if (det == 0)
                {
                    throw new ArgumentException("The matrix is singular and cannot be inverted.");
                }
                double[,] adjugate = MatrixAdjugate(matrix);
                result = MatrixScalarMultiply(adjugate, 1 / det);
            }
            return result;
        }

        static double MatrixDeterminant(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            if (n == 2)
            {
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
            }
            double determinant = 0;
            for (int i = 0; i < n; i++)
            {
                double subDeterminant = matrix[0, i] * MatrixDeterminant(MatrixMinor(matrix, 0, i));
                if (i % 2 == 0)
                {
                    determinant += subDeterminant;
                }
                else
                {
                    determinant -= subDeterminant;
                }
            }
            return determinant;
        }

        static double[,] MatrixAdjugate(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[,] adjugate = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double subDeterminant = MatrixDeterminant(MatrixMinor(matrix, i, j));
                    if ((i + j) % 2 == 0)
                    {
                        adjugate[j, i] = subDeterminant;
                    }
                    else
                    {
                        adjugate[j, i] = -subDeterminant;
                    }
                }
            }
            return adjugate;
        }

        static double[,] MatrixMinor(double[,] matrix, int row, int col)
        {
            int n = matrix.GetLength(0);
            double[,] minor = new double[n - 1, n - 1];
            int minorRow = 0;
            for (int i = 0; i < n; i++)
            {
                if (i == row)
                {
                    continue;
                }
                int minorCol = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j == col)
                    {
                        continue;
                    }
                    minor[minorRow, minorCol] = matrix[i, j];
                    minorCol++;
                }
                minorRow++;
            }
            return minor;
        }

        static double[,] MatrixScalarMultiply(double[,] matrix, double scalar)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            double[,] result = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = matrix[i, j] * scalar;
                }
            }
            return result;
        }



    }
}
