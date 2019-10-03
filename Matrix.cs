using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    public class Matrix
    {
        public int m, n;
        private double[] mrx;
        public bool sqrmrx = false;
        private static readonly double Epsilon = 1e-8;

        static bool IsZero(double value) { return Math.Abs(value) < Epsilon; }

        public Matrix(int i = 1, int j = 1)   //constructor.
        {
            m = i;
            n = j;
            if (m == n)
                sqrmrx = true;
            mrx = new double[m * n];
        }

        public void Inputdata()  //input_data.
        {
            Console.WriteLine();
            for (int i = 0; i < m * n; i++)
                mrx[i] = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine();
        }
        
        public Matrix Transpose()   //Transpose of this matrix
        {
            Matrix T = new Matrix(n, m);
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    T.mrx[j * T.n + i] = mrx[i * n + j];
            return T;
        }

        public Matrix Clone()     //return copy of matrix
        {
            Matrix temp = new Matrix(m, n);
            for (int i = 0; i < m * n; i++)
                temp.mrx[i] = mrx[i];
            return temp;

        }

        public void Clear()     //fill matrix with zero
        {
            for (int i = 0; i < m * n; i++)
                mrx[i] = 0;
        }
        public void ClrCol(int col)    //fill col with zero
        {
            if (col >= n || col < 0) throw new Exception("Wrong col index");
            for (int i = 0; i < m; i++)
                mrx[i * n + col] = 0;
        }
        public void ClrRow(int row)    //fill row with zero
        {
            if (row >= m || row < 0) throw new Exception("Wrong row index");
            for (int j = 0; j < n; j++)
                mrx[row * n + j] = 0;
        }

        public void RemoveRow(int row)    //Remove row from this matrix
        {
            if (row >= m || row < 0) throw new Exception("Wrong row index");
            Matrix t = new Matrix(m - 1, n);
            int i1 = 0;
            for (int i = 0; i < m; i++, i1++)
            {
                if (i == row) { i1--; continue; }
                for (int j = 0; j < n; j++)
                    t.mrx[i1 * t.n + j] = mrx[i * n + j];
            }
            n = t.n; m = t.m; mrx = t.mrx; sqrmrx = t.sqrmrx;
        }

        public void RemoveCol(int col)     //Remove col from this matrix
        {
            if (col >= n || col < 0) throw new Exception("Wrong col index");
            Matrix t = new Matrix(m, n - 1);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0, j1 = 0; j < n; j++, j1++)
                {
                    if (j == col) { j1--; continue; }
                    t.mrx[i * t.n + j1] = mrx[i * n + j];
                }
            }
            n = t.n; m = t.m; mrx = t.mrx; sqrmrx = t.sqrmrx;
        }

        public void Append(Matrix v)        //Append matrix v to right side of this matrix
        {
            Matrix t = new Matrix(Math.Max(m, v.m), n + v.n);
            for (int i = 0; i < t.m; i++)
            {
                for (int j = 0; j < t.n; j++)
                {
                    if (i >= m)
                    {
                        if (j >= n) { t.mrx[i * t.n + j] = v.mrx[i * v.n + j - n]; }
                        else { t.mrx[i * t.n + j] = 0; }
                    }
                    else if (i >= v.m)
                    {
                        if (j >= n) { t.mrx[i * t.n + j] = 0; }
                        else { t.mrx[i * t.n + j] = mrx[i * n + j]; }
                    }
                    else
                    {
                        if (j >= n) { t.mrx[i * t.n + j] = v.mrx[i * v.n + j - n]; }
                        else { t.mrx[i * t.n + j] = mrx[i * n + j]; }
                    }
                }
            }
            n = t.n; m = t.m; mrx = t.mrx; sqrmrx = t.sqrmrx;
        }

        public void Stack(Matrix v)        //Append matrix v to bottom of this matrix
        {
            Matrix t = new Matrix(m + v.m, Math.Max(n, v.n));
            for (int i = 0; i < t.m; i++)
            {
                for (int j = 0; j < t.n; j++)
                {
                    if (j >= v.n)
                    {
                        if (i >= m) { t.mrx[i * t.n + j] = 0; }
                        else { t.mrx[i * t.n + j] = mrx[i * n + j]; }
                    }
                    else if (j >= n)
                    {
                        if (i >= m) { t.mrx[i * t.n + j] = v.mrx[(i - m) * v.n + j]; }
                        else { t.mrx[i * t.n + j] = 0; }
                    }
                    else
                    {
                        if (i >= m) { t.mrx[i * t.n + j] = v.mrx[(i - m) * v.n + j]; }
                        else { t.mrx[i * t.n + j] = mrx[i * n + j]; }
                    }
                }
            }
            n = t.n; m = t.m; mrx = t.mrx; sqrmrx = t.sqrmrx;
        }

        public void Diagona(Matrix v)      //Append matrix v to right bottom of this matrix
        {
            Matrix t = new Matrix(m + v.m, n + v.n);
            for (int i = 0; i < t.m; i++)
            {
                for (int j = 0; j < t.n; j++)
                {
                    if (i < m && j < n) { t.mrx[i * t.n + j] = mrx[i * n + j]; }
                    else if (i >= m && j >= n) { t.mrx[i * t.n + j] = v.mrx[(i - m) * v.n + j - n]; }
                    else { t.mrx[i * t.n + j] = 0; }
                }
            }
            n = t.n; m = t.m; mrx = t.mrx; sqrmrx = t.sqrmrx;
        }

        public Matrix Invert()  //invert Matrix.
        {
            if (sqrmrx)
            {
                double det = Determinan();
                if (det != 0)
                {
                    Matrix temp = Clone();
                    Matrix Inverse = IdentityMatrix(m, n);

                    for (int j = 0; j < temp.n; j++)
                    {
                        int Pivot_row = Find_Pivot_row(ref temp, ref Inverse, j);
                        if (Pivot_row != j)
                        {
                            temp.SwapRow(j, Pivot_row);
                            Inverse.SwapRow(j, Pivot_row);
                        }
                        for (int i = 0; i < temp.m; i++)
                        {
                            if (i == j || temp.mrx[i * temp.n + j] == 0)
                                continue;
                            double t = -temp.mrx[i * temp.n + j];
                            Eliminate(ref temp, ref Inverse, j, i, t);
                        }
                    }
                    return Inverse;
                }
                else
                    throw new Exception("Determinan is zero");
            }
            throw new Exception("Non square matrix dont have inverse");
        }

        public double Determinan()    //return determinan. 
        {
            if (sqrmrx)
            {
                double det = 1;
                Matrix temp = Clone();

                int fixed_row = 0;
                for (int j = 0; j < n; j++)
                {
                    for (int i = fixed_row + 1; i < m; i++)
                    {
                        if (temp.mrx[fixed_row * n + j] == 0)
                        {
                            for (int x = fixed_row + 1; x < m; x++)
                                if (temp.mrx[x * n + j] != 0)
                                {
                                    temp.SwapRow(fixed_row, x);
                                    det *= -1;
                                    break;
                                }
                        }
                        if (temp.mrx[fixed_row * n + j] == 0)
                            return 0;

                        if (temp.mrx[i * n + j] == 0)
                            continue;
                        double t = temp.mrx[i * n + j] / temp.mrx[fixed_row * n + j];
                        for (int x = 0; x < n; x++)
                        {
                            temp.mrx[i * n + x] -= (temp.mrx[fixed_row * n + x] * t);
                            if (IsZero(temp.mrx[i * n + x])) temp.mrx[i * n + x] = 0;
                        }
                    }
                    fixed_row++;
                }
                for (int i = 0; i < m; i++)
                    det *= temp.mrx[i * n + i];

                return det;
            }
            throw new Exception("Non square matrix dont have determinan");
        }

        public void SwapRow(int row1, int row2)     //Swap row1 with row2
        {
            if (row1 < 0 || row2 < 0) throw new Exception("Wrong row index");
            double t;
            for (int x = 0; x < n; x++)
            {
                t = mrx[row1 * n + x];
                mrx[row1 * n + x] = mrx[row2 * n + x];
                mrx[row2 * n + x] = t;
            }
        }

        public void SwapCol(int col1, int col2)       //Swap col1 with col2
        {
            if (col1 < 0 || col2 < 0) throw new Exception("Wrong col index");
            double t;
            for (int x = 0; x < m; x++)
            {
                t = mrx[x * n + col1];
                mrx[x * n + col1] = mrx[x * n + col2];
                mrx[x * n + col2] = t;
            }
        }

        public override string ToString()                           // Function returns matrix as a string
        {
            string s = "";
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++) s += String.Format("{0,5:0.00}", mrx[i * n + j]) + " ";
                s += "\r\n";
            }
            return s;
        }

        public double this[int i, int j]  //Indexer.
        {
            get
            {
                return mrx[i * n + j];
            }
            set
            {
                mrx[i * n + j] = value;
            }

        }

        static private int Find_Pivot_row(ref Matrix input1, ref Matrix input2, int in_col_number)
        {
            int? Non_Ziro_row = null;
            for (int i = in_col_number; i < input1.m; i++)
            {
                if (input1.mrx[i * input1.n + in_col_number] == 1) return i;
                if (input1.mrx[i * input1.n + in_col_number] == -1)
                {
                    for (int x = 0; x < input1.n; x++)
                    {
                        input1.mrx[i * input1.n + x] *= -1;
                        input2.mrx[i * input2.n + x] *= -1;
                    }
                    return i;
                }
                if (input1.mrx[i * input1.n + in_col_number] != 0 && !Non_Ziro_row.HasValue) Non_Ziro_row = i;
            }
            if (input1.mrx[in_col_number * input1.n + in_col_number] == 0)
            {
                double t = input1.mrx[Non_Ziro_row.Value * input1.n + in_col_number];
                for (int x = 0; x < input1.n; x++)
                {
                    input1.mrx[Non_Ziro_row.Value * input1.n + x] /= t;
                    input2.mrx[Non_Ziro_row.Value * input2.n + x] /= t;
                }
                return Non_Ziro_row.Value;
            }
            double T = input1.mrx[in_col_number * input1.n + in_col_number];
            for (int x = 0; x < input1.n; x++)
            {
                input1.mrx[in_col_number * input1.n + x] /= T;
                input2.mrx[in_col_number * input2.n + x] /= T;
            }
            return in_col_number;
        }

        static private void Eliminate(ref Matrix input, ref Matrix inverse, int Fix_row, int row2, double t)
        {
            for (int x = 0; x < input.n; x++)
            {
                input.mrx[row2 * input.n + x] += input.mrx[Fix_row * input.n + x] * t;
                inverse.mrx[row2 * inverse.n + x] += inverse.mrx[Fix_row * inverse.n + x] * t;
                if (IsZero(input.mrx[row2 * input.n + x])) input.mrx[row2 * input.n + x] = 0;
                if (IsZero(inverse.mrx[row2 * inverse.n + x])) inverse.mrx[row2 * inverse.n + x] = 0;
            }
        }

        public static Matrix RandomMatrix(int iRows, int iCols, int dispersion)       // Function generates the random matrix
        {
            Random random = new Random();
            Matrix matrix = new Matrix(iRows, iCols);
            for (int i = 0; i < iRows * iCols; i++)
                matrix.mrx[i] = random.Next(-dispersion, dispersion);
            return matrix;
        }

        public static Matrix IdentityMatrix(int irows,int icols)
        {
            Matrix t = new Matrix(irows, icols);
            for (int i = 0; i < Math.Min(irows, icols); i++) 
                t.mrx[i * t.n + i] = 1;
            return t;
        }

        public static Matrix operator +(Matrix a, Matrix b)   //Overload + operator to add two matrix objects.
        {
            if (a.m == b.m && a.n == b.n)
            {
                int i;
                Matrix sum = new Matrix(a.m, a.n);
                for (i = 0; i < a.m * a.n; i++)
                    sum.mrx[i] = a.mrx[i] + b.mrx[i];
                return sum;
            }
            throw new Exception("Rows and cols must be same");
        }

        public static Matrix operator -(Matrix a, Matrix b)   //Overload - operator to Subtract two matrix objects.
        {
            if (a.m == b.m && a.n == b.n)
            {
                int i;
                Matrix sum = new Matrix(a.m, a.n);
                for (i = 0; i < a.m * a.n; i++)
                    sum.mrx[i] = a.mrx[i] - b.mrx[i];
                return sum;
            }
            throw new Exception("Rows and cols must be same");
        }

        public static Matrix operator *(Matrix a, Matrix b)   //Overload * operator to multiply two matrix objects.
        {
            if (a.n == b.m)
            {
                Matrix multi = new Matrix(a.m, b.n);
                for (int i = 0; i < a.m; i++)
                    for (int j = 0; j < b.n; j++)
                        for (int h = 0; h < a.n; h++)
                            multi.mrx[i * multi.n + j] += a.mrx[i * a.n + h] * b.mrx[h * b.n + j];
                return multi;
            }
            throw new Exception("Cols of matrix 1 and rows of matrix 2 must be same");
        }

        public static Matrix operator *(double k, Matrix b)   //Overload * operator to multiply double number in matrix objects.
        {
            for (int i = 0; i < b.m * b.n; i++)
                b.mrx[i] *= k;
            return b;
        }

        public static Matrix operator /(Matrix a, Matrix b)   //Overload / operator to division two matrix object.
        {
            b = b.Invert();
            return a * b;
        }
    }
}
