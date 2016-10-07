namespace UnitTestTraining.Utilities.Math
{
    public static class Extensions
    {
        public static double Power(this int number, int power)
        {
            double result = 1;

            int absolutePower = System.Math.Abs(power);
            for (int i = 0; i < absolutePower; i++)
            {
                result *= number;
            }

            if (power < 0)
            {
                result = 1 / result;
            }

            return result;
        }
    }
}