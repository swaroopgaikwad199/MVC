
namespace REDTR.CODEMGR
{
    /// <summary>
    /// Grades As Defined by GS1 and ISO 15415 Specifications
    /// </summary>
    public enum GS12DGradeType
    {
        Decoded2DGrOverall,
        Decoded2DGrContrast,
        Decoded2DGrFixedPatternDamage,
        Decoded2DGr2DModulation,
        Decoded2DGrGridNonUniformity,
        Decoded2DGrNonUniformity,
        Decoded2DGrHorizontalGrowth,
        Decoded2DGrVerticalGrowth,
        Decoded2DGrUnusedEC,
        Decoded2DGrRefDecode,
        NONE
    }

    public class GS1Grade
    {
        public enum Grades
        {
            A,
            B,
            C,
            D,
            F
        }
        public const string DefaultGrade = "F";

        /// <summary>
        /// Returns the Valid GS1 Grade from String. Default In case of ERROR is 'C'
        /// </summary>
        /// <param name="gradeStr"></param>
        /// <returns></returns>
        public static char GetGrade(string gradeStr)
        {
            char GS12DGrade = DefaultGrade[0];
            if (string.IsNullOrEmpty(gradeStr) == false)
            {
                if (gradeStr[0] >= 'A' && gradeStr[0] <= 'F')
                    GS12DGrade = gradeStr[0];
            }
            return GS12DGrade;
        }
        public static string GetGradeStr(string gradeStr)
        {
            char GS12DGrade = GetGrade(gradeStr);
            return GS12DGrade.ToString();
        }

        public static int GetGradeIndex(char grade)
        {
            if (grade == 'A')
                return 0;
            else if (grade == 'B')
                return 1;
            else if (grade == 'C')
                return 2;
            else if (grade == 'D')
                return 3;
            else //if (grade == 'F')
                return 4;
        }
        public static char GetGrade(int gradeIndex)
        {
            if (gradeIndex == 0)
                return 'A';
            else if (gradeIndex == 1)
                return 'B';
            else if (gradeIndex == 2)
                return 'C';
            else if (gradeIndex == 3)
                return 'D';
            else //if (grade == 'F')
                return 'F';
        }
        // TBD: This should be removed when Old Job File usage is elliminated
        #region CAMERA OLD JOB FILE GRADING CODE
        /// <summary>
        /// Function returns the Human Readable value of Grade from its calculated value
        /// </summary>
        /// <param name="gradeType">Type of Grade</param>
        /// <param name="value">Image Processing Computed Value</param>
        /// <returns>Grade Value Between A~F</returns>
        public static string GetValuesGradeStr(GS12DGradeType gradeType, float value)
        {
            char grade = GetValuesGrade(gradeType, value);
            return grade.ToString();
        }

        // GRADE RANGE FOR CALCULATION
        //Grade	Contrast	FixedPatternDamage	2DModulation	GridNonUniformity	NonUniformity	Hori Growth	Vert Growth	UnusedEC	RefDecode
        //A	    1			4				    4				0.38				0.06			0.5			0.5			1			1
        //B	    0.7			3				    3				0.5					0.08			0.7			0.7			0.62		N/A
        //C	    0.55	    2					2				0.63				0.1				0.85		0.85		0.5			N/A
        //D	    0.4			1				    1				0.75				0.12			1			1			0.37		N/A
        //F	    0.2			0					0				100					1				100			100			0.25		0

        static int[] grRange2DGrOverall = { 4, 3, 2, 1, 0 };

        static float[] grRange2DGrContrast = { 1.0F, 0.7F, 0.55F, 0.4F, 0.2F };
        static int[] grRange2DGrFixedPatternDamage = { 4, 3, 2, 1, 0 };
        static int[] grRange2DGr2DModulation = { 4, 3, 2, 1, 0 };
        static float[] grRange2DGrGridNonUniformity = { 0.38F, 0.5F, 0.63F, 0.75F, 100.0F };
        static float[] grRange2DGrNonUniformity = { 0.06F, 0.08F, 0.10F, 0.12F, 1.00F };
        static float[] grRange2DGrUnusedEC = { 1.0F, 0.62F, 0.50F, 0.37F, 0.25F };
        static int[] grRange2DGrRefDecode = { 1, 1, 1, 1, 0 };

        /// <summary>
        /// Function returns the Human Readable value of Grade from its calculated value
        /// </summary>
        /// <param name="gradeType">Type of Grade</param>
        /// <param name="value">Image Processing Computed Value</param>
        /// <returns>Grade Value Between A~F</returns>
        public static char GetValuesGrade(GS12DGradeType gradeType, float value)
        {
            const int A = 0, B = 1, C = 2, D = 3, F = 4;

            char GS12DGrade = ' ';
            switch (gradeType)
            {
                case GS12DGradeType.Decoded2DGrOverall:
                    {
                        int[] grRange = { 4, 3, 2, 1, 0 };
                        if (value == grRange[A]) GS12DGrade = 'A';
                        else if (value == grRange[B]) GS12DGrade = 'B';
                        else if (value == grRange[C]) GS12DGrade = 'C';
                        else if (value == grRange[D]) GS12DGrade = 'D';
                        else if (value == grRange[F]) GS12DGrade = 'F';
                        break;
                    }
                case GS12DGradeType.Decoded2DGrContrast:
                    {
                        float[] grRange = { 1.0F, 0.7F, 0.55F, 0.4F, 0.2F };
                        if (grRange[B] <= value && value <= grRange[A]) GS12DGrade = 'A';       //Grade A (4.0): 0.70 ≤ x < 1.00
                        else if (grRange[C] <= value && value < grRange[B]) GS12DGrade = 'B';   //Grade B (3.0): 0.55 ≤ x < 0.70
                        else if (grRange[D] <= value && value < grRange[C]) GS12DGrade = 'C';   //Grade C (2.0): 0.40 ≤ x < 0.55
                        else if (grRange[F] <= value && value < grRange[D]) GS12DGrade = 'D';   //Grade D (1.0): 0.20 ≤ x < 0.40
                        else if (0.00 <= value && value < grRange[F]) GS12DGrade = 'F';         //Grade F (0.0): 0.00 ≤ x < 0.20
                        break;
                    }
                case GS12DGradeType.Decoded2DGrFixedPatternDamage:
                    {
                        int[] grRange = { 4, 3, 2, 1, 0 };
                        if (value == grRange[A]) GS12DGrade = 'A';
                        else if (value == grRange[B]) GS12DGrade = 'B';
                        else if (value == grRange[C]) GS12DGrade = 'C';
                        else if (value == grRange[D]) GS12DGrade = 'D';   //'D';
                        else if (value == grRange[F]) GS12DGrade = 'F';   //'F';
                        break;
                    }
                case GS12DGradeType.Decoded2DGr2DModulation:
                    {
                        int[] grRange = { 4, 3, 2, 1, 0 };
                        if (value == grRange[A]) GS12DGrade = 'A';
                        else if (value == grRange[B]) GS12DGrade = 'B';
                        else if (value == grRange[C]) GS12DGrade = 'C';
                        else if (value == grRange[D]) GS12DGrade = 'D';   //'D';
                        else if (value == grRange[F]) GS12DGrade = 'F';   //'F';
                        break;
                    }
                case GS12DGradeType.Decoded2DGrGridNonUniformity:
                    {
                        float[] grRange = { 0.38F, 0.5F, 0.63F, 0.75F, 100.0F };
                        if (value <= grRange[A]) GS12DGrade = 'A';   //Grade A (4.0): x ≤ 0.38
                        else if (value <= grRange[B]) GS12DGrade = 'B';//Grade B (3.0): 0.38 < x ≤ 0.50
                        else if (value <= grRange[C]) GS12DGrade = 'C';//Grade C (2.0): 0.50 < x ≤ 0.63
                        else if (value <= grRange[D]) GS12DGrade = 'D';//Grade D (1.0): 0.63 < x ≤ 0.75
                        else if (value > grRange[F]) GS12DGrade = 'F';//Grade F (0.0): x > 0.75
                        break;
                    }
                case GS12DGradeType.Decoded2DGrNonUniformity:
                    {
                        float[] grRange = { 0.06F, 0.08F, 0.10F, 0.12F, 1.00F };
                        if (value >= 0.00 && value < grRange[A]) GS12DGrade = 'A';//Grade A (4.0): 0.00 ≤ x ≤ 0.06 
                        else if (value >= grRange[A] && value < grRange[B]) GS12DGrade = 'B';//Grade B (3.0): 0.06 < x ≤ 0.08 
                        else if (value >= grRange[B] && value < grRange[C]) GS12DGrade = 'C';//Grade C (2.0): 0.08 < x ≤ 0.10 
                        else if (value >= grRange[C] && value < grRange[D]) GS12DGrade = 'D';//Grade D (1.0): 0.10 < x ≤ 0.12 
                        else if (value >= grRange[D] && value < grRange[F]) GS12DGrade = 'F';//Grade F (0.0): 0.12 < x ≤ 1.00 
                        break;
                    }
                case GS12DGradeType.Decoded2DGrUnusedEC:
                    {
                        float[] grRange = { 1.0F, 0.62F, 0.50F, 0.37F, 0.25F };
                        if (value >= grRange[B] && value <= grRange[A]) GS12DGrade = 'A';//Grade A (4.0): 0.62 ≤ x ≤  1.00 
                        else if (value >= grRange[C] && value < grRange[B]) GS12DGrade = 'B';//Grade B (3.0): 0.50 ≤ x < 0.62 
                        else if (value >= grRange[D] && value < grRange[C]) GS12DGrade = 'C';//Grade C (2.0): 0.37 ≤ x < 0.50 
                        else if (value >= grRange[F] && value < grRange[D]) GS12DGrade = 'D';//Grade D (1.0): 0.25 ≤ x < 0.37 
                        else if (value >= 0.00 && value < grRange[F]) GS12DGrade = 'F';//Grade F (0.0): 0.00 ≤ x < 0.25 
                        break;
                    }
                case GS12DGradeType.Decoded2DGrRefDecode:
                    {
                        int[] grRange = { 1, 1, 1, 1, 0 };
                        if (value == grRange[A]) GS12DGrade = 'A';//Grade A (4.0): 1
                        else if (value == grRange[F]) GS12DGrade = 'F';//Grade F (0.0): 0
                        //Grade B (3.0): N/A    //Grade C (2.0): N/A    //Grade D (1.0): N/A
                        break;
                    }
            }
            return GS12DGrade;
        }

        /// <summary>
        /// Function returns the Image Processing Computed Value from its Human Readable value of Grade 
        /// </summary>
        /// <param name="gradeType">Type of Grade</param>
        /// <param name="grade">Grade Value Between A~F</param>
        /// <returns>Image Processing Computed Value</returns>
        public static float GetGradeValue(GS12DGradeType gradeType, char grade)
        {
            int grIndex = GetGradeIndex(grade);
            float grVal = 4;
            switch (gradeType)
            {
                case GS12DGradeType.Decoded2DGrOverall:
                    {
                        int[] values = { 4, 3, 2, 1, 0 };
                        grVal = values[grIndex];
                        break;
                    }
                case GS12DGradeType.Decoded2DGrFixedPatternDamage:
                    {
                        int[] values = { 4, 3, 2, 1, 0 };
                        grVal = values[grIndex];
                        break;
                    }
                case GS12DGradeType.Decoded2DGr2DModulation:
                    {
                        int[] values = { 4, 3, 2, 1, 0 };
                        grVal = values[grIndex];
                        break;
                    }
                case GS12DGradeType.Decoded2DGrGridNonUniformity:
                    {
                        double[] values = { 0.38, 0.5, 0.63, 0.75, 100 };
                        grVal = (float)values[grIndex];
                        break;
                    }
                case GS12DGradeType.Decoded2DGrContrast:
                    {
                        double[] values = { 1, 0.7, 0.55, 0.4, 0.2 };
                        grVal = (float)values[grIndex];
                        break;
                    }
                case GS12DGradeType.Decoded2DGrRefDecode:
                    {
                        double[] values = { 1, 1, 1, 1, 0 };
                        grVal = (float)values[grIndex];
                        break;
                    }
            }
            return grVal;
        }
        #endregion CAMERA OLD JOB FILE GRADING CODE
    }
}
