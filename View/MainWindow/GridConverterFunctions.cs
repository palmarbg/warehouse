using System.Windows;

namespace View.Grid
{
    public static class GridConverterFunctions
    {
        /// <summary>
        /// The unit length of a tile displayed without zoom
        /// </summary>
        public static int unit = 30;

        /// <param name="zoom"></param>
        /// <returns>
        /// Number of rows/columns grouped together
        /// into one label
        /// </returns>
        public static double AmountOfNumbersInLabelGroup(double zoom)
        {
            double fun = Math.Floor((2.4 / zoom)) - 1;
            var groupedAmountInOneBlock = (fun - 1 <= 0.5) ? 1.0 : (fun - 2 <= 0.5) ? 2.0 : (fun - 5 <= 0.5) ? 5.0 : 10.0;
            return groupedAmountInOneBlock;
        }

        /// <param name="count">Row or column count</param>
        /// <param name="zoom"></param>
        /// <returns>
        /// Number of grouped labels needed to cover the whole map
        /// </returns>
        public static int NumberOfGroupedLabelsNeeded(int count, double zoom)
        {
            return System.Convert.ToInt32(Math.Ceiling(count / AmountOfNumbersInLabelGroup(zoom)));
        }

        /// <param name="zoom"></param>
        /// <returns>Length of a label group</returns>
        public static double GroupedLabelLength(double zoom)
        {
            return unit * zoom * AmountOfNumbersInLabelGroup(zoom);
        }

        /// <param name="zoom"></param>
        /// <returns>Amount of label groups that cover the whole screen both horizontally and vertically</returns>
        public static int NumberOfLabelGroupsOnScreen_Max(double zoom)
        {
            double size = Math.Max(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
            return (int)Math.Ceiling(size / GroupedLabelLength(zoom));
        }

        /// <param name="zoom"></param>
        /// <returns>Amount of label groups that cover the whole screen horizontally</returns>
        public static int NumberOfLabelGroupsOnScreen_Horizontal(double zoom)
        {
            double size = SystemParameters.PrimaryScreenWidth;
            return (int)Math.Ceiling(size / GroupedLabelLength(zoom));
        }

        /// <param name="zoom"></param>
        /// <returns>Amount of label groups that cover the whole screen vertically</returns>
        public static int NumberOfLabelGroupsOnScreen_Vertical(double zoom)
        {
            double size = SystemParameters.PrimaryScreenHeight;
            return (int)Math.Ceiling(size / GroupedLabelLength(zoom));
        }

        public static double MapLength(int count)
        {
            return 1.0 * count * unit;
        }

        /// <param name="rowCount"></param>
        /// <param name="columnCount"></param>
        /// <returns>The number of cells displayed as one block</returns>
        public static int AmountOfCellsInOneTile(int rowCount, int columnCount)
        {
            return Math.Max(rowCount / 500, columnCount / 500);
        }

        /// <param name="count1"></param>
        /// <param name="count2"></param>
        /// <returns>The number of lines in the first dimension</returns>
        public static int NumberOfLines(int count1, int count2)
        {
            int scale = AmountOfCellsInOneTile(count1, count2);
            return System.Convert.ToInt32(Math.Ceiling((double)count1 / scale));
        }


        /// <param name="offset"></param>
        /// <param name="zoom"></param>
        /// <returns>The number of grouped labels not displayed at the start</returns>
        public static int NumberOfLabelsToOmit(int offset, double zoom)
        {
            return (int)Math.Floor(offset / GridConverterFunctions.GroupedLabelLength(zoom));
        }

        /// <param name="offset"></param>
        /// <param name="zoom"></param>
        /// <returns>The offset after discarding the first labels</returns>
        public static int OmittedOffset(int offset, double zoom)
        {
            return offset - (int)Math.Round(GridConverterFunctions.GroupedLabelLength(zoom) * GridConverterFunctions.NumberOfLabelsToOmit(offset, zoom));
        }

        /// <param name="array"></param>
        /// <param name="length"></param>
        /// <returns>If array doesn't contain null or UnsetValue</returns>
        public static bool ValidateArray(object[] array, int? length = null)
        {
            if (length != null && array.Length != length)
                return false;
            return !array.Any(e => e == null || e == DependencyProperty.UnsetValue);
        }

    }
}
