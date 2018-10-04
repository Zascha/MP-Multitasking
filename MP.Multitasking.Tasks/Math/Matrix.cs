namespace MP.Multitasking.Tasks.Math
{
    public class MatrixSizeParams
    {
        public int RowsNumber { get; set; }

        public int ColumnsNumber { get; set; }
    }

    public class Matrix<T>
    {
        public MatrixSizeParams Size { get; set; }

        public T[][] Values { get; set; }

    }
}
