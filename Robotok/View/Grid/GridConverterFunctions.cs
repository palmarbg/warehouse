using System.Windows;

namespace Robotok.View.Grid
{
    public static class GridConverterFunctions
    {
        /// <summary>
        /// The unit length of a cell displayed without zoom
        /// </summary>
        public static int unit = 30;

        /// <param name="zoom"></param>
        /// <returns>
        /// The number of cells grouped together
        /// into one label
        /// </returns>
        public static double AmountOfNumbersInOneLabel(double zoom)
        {
            double fun = Math.Floor((2.4 / zoom)) - 1;
            var groupedAmountInOneBlock = (fun - 1 <= 0.5) ? 1.0 : (fun - 2 <= 0.5) ? 2.0 : (fun - 5 <= 0.5) ? 5.0 : 10.0;
            return groupedAmountInOneBlock;
        }

        /// <param name="count"></param>
        /// <param name="zoom"></param>
        /// <returns>
        /// The number of grouped labels to cover the whole map
        /// </returns>
        public static int NumberOfLabels(int count, double zoom)
        {
            return System.Convert.ToInt32(Math.Ceiling(count / AmountOfNumbersInOneLabel(zoom)));
        }

        public static double LabelLength(double zoom)
        {
            return unit * zoom * AmountOfNumbersInOneLabel(zoom);
        }

        public static int NumberOfLabelsOnScreen(double zoom)
        {
            double size = Math.Max(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
            return (int)Math.Ceiling(size / LabelLength(zoom));
        }

        public static double MapLength(int count)
        {
            return 1.0 * count * unit;
        }

        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <returns>The number of cells displayed as one block</returns>
        public static int AmountOfCellsInOneBlock(int rowCount, int columnCount)
        {
            return Math.Max(rowCount / 500, columnCount / 500);
        }

        /// <param name="count1"></param>
        /// <param name="count2"></param>
        /// <returns>The number of lines in the first dimension</returns>
        public static int NumberOfLines(int count1, int count2)
        {
            int scale = AmountOfCellsInOneBlock(count1, count2);
            return System.Convert.ToInt32(Math.Ceiling((double)count1 / scale));
        }


        /// <param name="offset"></param>
        /// <param name="zoom"></param>
        /// <returns>The number of grouped labels not displayed at the start</returns>
        public static int NumberOfLabelsToOmit(int offset, double zoom)
        {
            return (int)Math.Floor(offset / GridConverterFunctions.LabelLength(zoom));
        }

        /// <param name="offset"></param>
        /// <param name="zoom"></param>
        /// <returns>The offset after discarding the first labels</returns>
        public static int OmittedOffset(int offset, double zoom)
        {
            return offset - (int)Math.Round(GridConverterFunctions.LabelLength(zoom) * GridConverterFunctions.NumberOfLabelsToOmit(offset, zoom));
        }
        



        /// <param name="array"></param>
        /// <param name="length"></param>
        /// <returns>If array doesn't contain null or UnsetValue</returns>
        public static bool ValidateArray(object[] array, int? length=null)
        {
            if(length != null && array.Length != length)
                return false;
            return !array.Any(e => e == null || e == DependencyProperty.UnsetValue);
        }

    }
}
