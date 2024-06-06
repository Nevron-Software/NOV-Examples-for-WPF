using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Formulas;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
    public class NFormulasExample : NExampleBase
    {
        #region Constructors

        public NFormulasExample()
        {
        }
        static NFormulasExample()
        {
            NFormulasExampleSchema = NSchema.Create(typeof(NFormulasExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            m_FormulaCalculator = new NFormulaCalculator();

            NStackPanel stack = new NStackPanel();
            NDockLayout.SetDockArea(stack, ENDockArea.Center);

            m_InputTextBox = new NTextBox();
            stack.Add(m_InputTextBox);

            NStackPanel hstack = new NStackPanel();
            hstack.Direction = ENHVDirection.LeftToRight;
            stack.Add(hstack);

            NButton evaluateButton = new NButton("Evaluate");
            evaluateButton.Click += new Function<NEventArgs>(OnEvaluateButtonClick);
            hstack.Add(evaluateButton);

            NButton evaluateAllButton = new NButton("Evaluate All");
            evaluateAllButton.Click += new Function<NEventArgs>(OnEvaluateAllButtonClick);
            hstack.Add(evaluateAllButton);

            m_ResultTextBox = new NTextBox();
            stack.Add(m_ResultTextBox);

            return stack;
        }
        protected override NWidget CreateExampleControls()
        {
            NTreeView testsView = CreateTestsTreeView();
            return new NGroupBox("Predefined Examples", testsView);
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
Demonstrates the strong support for Formulas. Formula expressions can be assigned to any DOM Element property.
</p>
";
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Creates a tree view to navigate through the predefined formula examples
        /// </summary>
        /// <returns></returns>
        NTreeView CreateTestsTreeView()
        {
            NTreeViewItem categoryItem, folderItem;

            NTreeView treeView = new NTreeView();
            m_TestsTreeView = treeView;

            #region Operators

            folderItem = new NTreeViewItem("Operators");
            treeView.Items.Add(folderItem);

            #region Arithmetic Operators

            categoryItem = new NTreeViewItem("Arithmetic");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[] {
                "+10",
                "-10",
                "-ARRAY(10, 12)",
                "10 ^ 2",
                "ARRAY(10, 12) ^ 2",
                "10 * 2",
                "ARRAY(10, 12) * 2",
                "10 / 2",
                "ARRAY(10, 12) / 2",
                "10 + 2",
                "ARRAY(10, 12) + 2",
                "ARRAY(10, 12) + ARRAY(12, 23)",
                "10 + \"Nevron\"",
                "10 - 2",
                "ARRAY(10, 12) - 2",
                "ARRAY(10, 12) - ARRAY(12, 23)"
            });

            #endregion

            #region Comparision Operators

            categoryItem = new NTreeViewItem("Comparision");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[] {
               "10 > 2",
               "10 < 2",
               "10 >= 2",
               "10 >= 10",
               "10 <= 2",
               "10 <= 10",
               "10 == 2",
               "10 != 2"
            });

            #endregion

            #region Logical operators

            categoryItem = new NTreeViewItem("Logical");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[] {
               "true && false",
               "true || false",
               "!true"
            });

            #endregion

            #region Bitwise operators

            categoryItem = new NTreeViewItem("Bitwise");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[] {
               "7 & 2",
               "5 | 3",
               "~1"
            });

            #endregion

            #region Assignment operators

            categoryItem = new NTreeViewItem("Assignment");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[] {
                "a=5; b=3; a+b;",
                "a=5; a+=3",
                "a=5; a-=3"
            });

            #endregion

            #endregion

            #region Functions

            folderItem = new NTreeViewItem("Functions");
            treeView.Items.Add(folderItem);

            #region Bitwise 

            categoryItem = new NTreeViewItem("Bitwise");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[]{
                   "BITAND(7,2)",
                   "BITNOT(1)",
                   "BITOR(5,3)",
                   "BITXOR(5,3)",
            });

            #endregion

            #region Logical

            categoryItem = new NTreeViewItem("Logical");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[]{
                "AND(true, false)",
                "AND(true, false)",
                "IF(true, 2, 10)",
                "IF(false, 2, 10)",
                "NOT(true)",
                "OR(true, false)",
            });

            #endregion

            #region Mathematical

            categoryItem = new NTreeViewItem("Mathematical");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[]{
               "ABS(-2.5)",
               "CEILING(1.7)",
               "CEILING(1.7, 0.25)",
               "FLOOR(1.7)",
               "FLOOR(1.7, 0.25)",
               "INT(1.2)",
               "INT(-1.2)",
               "INTUP(1.2)",
               "INTUP(-1.2)",
               "LN(10)",
               "LOG10(10)",
               "MAGNITUDE(3, 4)",
               "MAX(1, 3, 2)",
               "MIN(1, 3, 2)",
               "MOD(5, 1.4)",
               "MOD(5, -1.4)",
               "POW(10, 2)",
               "ROUND(123.654,2)",
               "ROUND(123.654,0)",
               "ROUND(123.654,-1)",
               "SIGN(-10)",
               "SIGN(0)",
               "SQRT(4)",
               "SUM(1,2,3)",
               "TRUNC(123.654,2)",
               "TRUNC(123.654,0)",
               "TRUNC(123.654,-1)",
           });

            #endregion

            #region Statistical

            categoryItem = new NTreeViewItem("Statistical");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[]{

                "BETADIST(2,8,10,TRUE,1,3)",  // 0.6854706
                "BETADIST(2,8,10,FALSE,1,3)", // 1.4837646
                "BETAINV(0.685470581,8,10,1,3)", // 2

                "BINOMDIST(6, 10, 0.5, FALSE)", // 0.2050781

                "CHISQDIST(0.5,1,TRUE)",  // 0.52049988
                "CHISQDIST(2,3,FALSE)",  // 0.20755375

                "CHISQINV(0.93,1)",  // 3.283020286
                "CHISQINV(0.6,2)",  // 1.832581464

                "EXPONDIST(0.2,10,TRUE)", // 0.86466472
                "EXPONDIST(0.2,10,FALSE)", // 1.35335283 
                "EXPONINV(0.86466472, 10)", //  0.2

                "FDIST(15.2069,6,7,TRUE)", // 0.99
                "FDIST(15.2069,6,7,FALSE)", // 0.000220332 
                "FINV(0.01,6,4)", // 0.10930991

                "GAMMADIST(10.00001131,9,2,FALSE)", // 0.032639
                "GAMMADIST(10.00001131,9,2,TRUE)", //  0.068094
                "GAMMAINV(0.068094,9,2)", //  10.0000112

                "HYPGEOMDIST(1,4,8,20,TRUE)", // 0.4654
                "HYPGEOMDIST(1,4,8,20,FALSE)", // 0.3633

                "LOGNORMDIST(4,3.5,1.2,TRUE)", // 0.0390836
                "LOGNORMDIST(4,3.5,1.2,FALSE)", // 0.0176176
                "LOGNORMINV(0.039084, 3.5, 1.2)", // 4.0000252

                "NEGBINOMDIST(10,5,0.25,TRUE)", // 0.3135141
                "NEGBINOMDIST(10,5,0.25,FALSE)", // 0.0550487

                "NORMDIST(42,40,1.5,TRUE)", // 0.9087888
                "NORMDIST(42,40,1.5,FALSE)", // 0.10934
                "NORMINV(0.6, 5, 2 )", // 5.506694206

                "POISSONDIST(2,5,TRUE)", // 0.124652
                "POISSONDIST(2,5,FALSE)", // 0.084224

                "TDIST(60,1,TRUE)", // 0.99469533
                "TDIST(8,3,FALSE)", // 0.00073691

                "TINV(0.75,2)", // 0.8164966
                "TINV2T(0.546449,60)", // 0.606533

                "WEIBULLDIST(105,20,100,TRUE)", // 0.929581
                "WEIBULLDIST(105,20,100,FALSE)", // 0.035589

                "CORRELATION(ARRAY(3,2,4,5,6), ARRAY(9,7,12,15,17))", // 0.997

                "COVARIANCE(ARRAY(3,2,4,5,6), ARRAY(9,7,12,15,17), true)", //  5.2
                "COVARIANCE(ARRAY(2, 4, 8), ARRAY(5, 11, 12), false)", // 9.666666667

                "STDDEV(ARRAY(1345,1301,1368,1322,1310,1370,1318,1350,1303,1299), true)", // 26.05455814 
                "STDDEV(ARRAY(1345,1301,1368,1322,1310,1370,1318,1350,1303,1299), false)", // 27.46392

                "VARIANCE(ARRAY(1345,1301,1368,1322,1310,1370,1318,1350,1303,1299), true)", // 678,84
                "VARIANCE(ARRAY(1345,1301,1368,1322,1310,1370,1318,1350,1303,1299), false)", // 754.2667

                "LINEST(ARRAY(1, 9, 5, 7), ARRAY(0, 4, 2, 3) ,FALSE)", // 2.31 
                "LINEST(ARRAY(1, 9, 5, 7), ARRAY(0, 4, 2, 3) ,TRUE)", // { 2, 1 }

                "LOGEST(ARRAY(33100,47300,69000,102000,150000,220000), ARRAY(11,12,13,14,15,16))", // 1.46328, 495.305
                "LOGEST(ARRAY(33100,47300,69000,102000,150000,220000), ARRAY(11,12,13,14,15,16), FALSE)", // 2.300393, 1

                "RSQ(ARRAY(2,3,9,1,8,7,5), ARRAY(6,5,11,7,5,4,4))" // 0.05795
            });

            #endregion

            #region Text

            categoryItem = new NTreeViewItem("Text");
            folderItem.Items.Add(categoryItem);
            CreateTestItems(categoryItem, new string[]{
                "CHAR(9)",
                "LEN(\"Hello World\")",
                "LOWER(\"Hello World\")",
                "STRSAME(\"Hello\", \"hello\")",
                "STRSAME(\"Hello\", \"hello\", true)",
                "TRIM(\" Hello World \")",
                "UPPER(\"Hello World\")",
                "INDEX(0,\"Hello;World\")",
            });

            #endregion

            #region Trigonometrical

            NTreeViewItem trigonometrical = new NTreeViewItem("Trigonometrical");
            folderItem.Items.Add(trigonometrical);
            CreateTestItems(trigonometrical, new string[]{
                "ACOS(0)",
                "ANG360(1.4 + 2 * PI())",
                "ASIN(1)",
                "ATAN2(1,1)",
                "ATAN2(1,SQRT(3))",
                "ATAN(1)",
                "COS(0)",
                "COSH(PI()/4)",
                "PI()",
                "SIN(0)",
                "SINH(PI()/4)",
                "TAN(PI()/4)",
                "TANH(-PI()/4)"
            });

            #endregion

            #region Type

            NTreeViewItem type = new NTreeViewItem("Type");
            folderItem.Items.Add(type);
            CreateTestItems(type, new string[]{
               "EMPTY()",
               "ISARRAY(ARRAY(10,20))",
               "ISARRAY(10)",
               "ISBOOL(true)",
               "ISBOOL(false)",
               "ISBOOL(\"true\")",
               "ISDATETIME(10)",
               "ISDATETIME(DATETIME(2008,9,15))",
               "ISDEFAULT(TODATETIME(\"1-1-0001 0:0:0\"))",
               "ISEMPTY(EMPTY())",
               "ISEMPTY(true)",
               "ISMEASURE(10[mm])",
               "ISMEASURE(10)",
               "ISNUM(10)",
               "ISNUM(true)",
               "ISSTR(true)",
               "ISSTR(\"hello world\")",
               "TOBOOL(\"false\")",
               "TOBOOL(\"true\")",
               "TOBOOL(\"hello\")",
               "TODATETIME(\"2008-09-15 09:30:41.770\")",
               "TONUM(true)",
               "TONUM(\"10\")",
               "TONUM(\"hello\")",
               "TOSTR(10)"
            });

            #endregion

            #endregion

            treeView.ExpandAll(true);
            treeView.SelectedPathChanged += new Function<NValueChangeEventArgs>(OnTestsTreeViewSelectedPathChanged);

            return treeView;
        }
        void CreateTestItems(NTreeViewItem parentItem, string[] formulas)
        {
            for (int i = 0; i < formulas.Length; i++)
            {
                parentItem.Items.Add(CreateTestItem(formulas[i]));
            }
        }
        NTreeViewItem CreateTestItem(string formula)
        {
            NTreeViewItem item = new NTreeViewItem(formula);
            item.Tag = formula;
            return item;
        }

        void OnTestsTreeViewSelectedPathChanged(NValueChangeEventArgs arg)
        {
            NTreeViewItem item = m_TestsTreeView.SelectedItem;
            if (item == null || item.Tag == null)
                return;

            m_InputTextBox.Text = item.Tag.ToString();
            EvaluateFormula();
        }
        void OnEvaluateButtonClick(NEventArgs arg)
        {
            EvaluateFormula();
        }
        void OnEvaluateAllButtonClick(NEventArgs arg)
        {
            NList<string> tests = new NList<string>();

            INIterator<NNode> it = m_TestsTreeView.GetSubtreeIterator();
            while (it.MoveNext())
            {
                NTreeViewItem item = it.Current as NTreeViewItem;
                if (item == null || item.Tag == null || !(item.Tag is string))
                    continue;

                tests.Add((string)item.Tag);
            }

            NStopwatch stopwatch = new NStopwatch();

            stopwatch.Start();
            int itcount = 10000;
            for (int j = 0; j < itcount; j++)
            {
                for (int i = 0; i < tests.Count; i++)
                {
                    try
                    {
                        m_FormulaCalculator.Formula = tests[i];
                        m_FormulaCalculator.Evaluate();
                    }
                    catch (Exception ex)
                    {
                        m_ResultTextBox.Text = "Failed on test: " + tests[i] + ". Error was: " + ex.Message;
                        m_InputTextBox.Text = tests[i];
                        return;
                    }
                }
            }
            stopwatch.Stop();

            int ms = stopwatch.ElapsedMilliseconds;
            m_ResultTextBox.Text = tests.Count + " tests performed " + itcount + " times in: " + ms + " milliseconds.";
        }

        void EvaluateFormula()
        {
            try
            {
                m_FormulaCalculator.Formula = m_InputTextBox.Text;
                NVariant result = m_FormulaCalculator.Evaluate();
                m_ResultTextBox.Text = result.ToString();
            }
            catch (Exception ex)
            {
                m_ResultTextBox.Text = ex.Message;
            }
        }

        #endregion

        #region Fields

        NTextBox m_InputTextBox;
        NTextBox m_ResultTextBox;
        NTreeView m_TestsTreeView;
        NFormulaCalculator m_FormulaCalculator;

        #endregion

        #region Schema

        public static readonly NSchema NFormulasExampleSchema;

        #endregion
    }
}